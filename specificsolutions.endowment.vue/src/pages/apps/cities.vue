<script setup lang="ts">
import { ref, onMounted, watch, reactive, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'

// Define interfaces for better type safety
interface City {
  id: number
  name: string
  country?: string
}

interface NewCity {
  name: string
  country: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'City',
  },
})

const { t } = useI18n()

// استخدام نظام التحقق الجديد
const {
  validationState,
  addError,
  removeError,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  hasFieldError,
  getFieldErrors,
  getFirstFieldError,
  setFieldTouched,
  setFieldDirty,
  shouldShowFieldError,
  validateRequired,
  validateEmail,
  validateLength,
} = useFormValidation()

const cities = ref<City[]>([])
const loading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref<'success' | 'error' | 'warning' | 'info'>('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedCity = ref<City | null>(null)
const selectedRows = ref<City[]>([])
const search = ref('')

const newCity = reactive<NewCity>({
  name: '',
  country: '',
})

const editCity = reactive<City>({
  id: 0,
  name: '',
  country: '',
})

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using dynamic i18n translations
const headers = computed(() => [
  {
    title: t('tableHeaders.cities.name'),
    key: 'name',
  },
  {
    title: t('tableHeaders.cities.country'),
    key: 'country',
  },
  {
    title: t('tableHeaders.cities.actions'),
    key: 'actions',
    sortable: false,
  },
])

const loadCities = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/City/filter?${params}`)
    cities.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadCities() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading cities:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المدن'
    alertType.value = 'error'
    showAlert.value = true
    cities.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const addCity = async () => {
  // Clear previous errors
  clearErrors()
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(newCity.name, 'name', 'اسم المدينة مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(newCity.country, 'country', 'الدولة مطلوبة')) {
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await $api('/City', {
      method: 'POST',
      body: {
        name: newCity.name,
        country: newCity.country,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else {
        const errorMsg = response.message || 'حدث خطأ أثناء إضافة المدينة'
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    dialog.value = false
    resetNewCity()
    loadCities()
    alertMessage.value = 'تم إضافة المدينة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding city:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة المدينة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateCity = async () => {
  // Clear previous errors
  clearErrors()
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(editCity.name, 'editName', 'اسم المدينة مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(editCity.country, 'editCountry', 'الدولة مطلوبة')) {
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await $api(`/City/${editCity.id}`, {
      method: 'PUT',
      body: {
        id: editCity.id,
        name: editCity.name,
        country: editCity.country,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else {
        const errorMsg = response.message || 'حدث خطأ أثناء تحديث المدينة'
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    editDialog.value = false
    loadCities()
    alertMessage.value = 'تم تحديث المدينة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating city:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث المدينة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteCity = async () => {
  if (!selectedCity.value) return
  
  try {
    console.log('Attempting to delete city:', selectedCity.value.id)
    const response = await $api(`/City/${selectedCity.value.id}`, {
      method: 'DELETE',
    })
    
    console.log('Delete response:', response)
    console.log('Response data:', response.data)
    
    // Check if the response indicates success - response comes directly, not in response.data
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المدينة'
      console.log('API returned error:', errorMsg)
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    console.log('City deleted successfully')
    deleteDialog.value = false
    loadCities()
    alertMessage.value = 'تم حذف المدينة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting city:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = 'حدث خطأ في الاتصال بالخادم'
    alertType.value = 'error'
    showAlert.value = true
    deleteDialog.value = false
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(city => 
      $api(`/City/${city.id}`, { method: 'DELETE' })
    )
    const responses = await Promise.all(deletePromises)
    
    // Check if any operation failed - response comes directly
    const failedOperations = responses.filter((response: any) => 
      response && response.isSuccess === false
    )
    
    if (failedOperations.length > 0) {
      const errorMessages = failedOperations.map((response: any) => 
        response?.message || response?.errors?.[0]?.errorMessage || 'حدث خطأ أثناء العملية'
      )
      const errorMsg = `فشل في حذف ${failedOperations.length} عنصر: ${errorMessages.join(', ')}`
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    // If we reach here, all deletions were successful
    selectedRows.value = []
    loadCities()
    alertMessage.value = 'تم حذف المدن المحددة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected cities:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = 'حدث خطأ في الاتصال بالخادم'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (city: City) => {
  clearErrors() // Clear previous validation errors
  editCity.id = city.id // Ensure id is set for update
  editCity.name = city.name
  editCity.country = city.country || ''
  editDialog.value = true
}

const openDeleteDialog = (city: City) => {
  clearErrors() // Clear any validation errors
  selectedCity.value = city
  deleteDialog.value = true
}

const resetNewCity = () => {
  newCity.name = ''
  newCity.country = ''
  clearErrors() // Clear validation errors
}

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadCities()
}, { immediate: false })

// Watch for pagination changes
watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadCities()
}, { immediate: false })

onMounted(() => {
  loadCities()
})
</script>

<template>
  <div>
    <!-- Simple VAlert - just like template examples -->
    <VAlert
      v-model="showAlert"
      :type="alertType"
      variant="tonal"
      closable
      class="mb-4"
    >
      {{ alertMessage }}
    </VAlert>

    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center pa-6">
        <span class="text-h5">إدارة المدن</span>
        <div class="d-flex gap-2">
          <VBtn
            v-if="selectedRows.length > 0"
            color="error"
            variant="outlined"
            @click="deleteSelectedRows"
          >
            حذف المحدد ({{ selectedRows.length }})
          </VBtn>
          <VBtn
            color="primary"
            @click="() => { clearErrors(); dialog = true; }"
          >
            إضافة مدينة
          </VBtn>
        </div>
      </VCardTitle>

      <VDivider />

      <VCardText>
        <!-- Search Bar using ready-made template -->
        <VRow>
          <VCol
            cols="12"
            offset-md="8"
            md="4"
          >
            <VTextField
              v-model="search"
              placeholder="البحث في المدن..."
              prepend-inner-icon="mdi-magnify"
              single-line
              hide-details
              dense
              outlined
            />
          </VCol>
        </VRow>

        <!-- Data Table using ready-made template -->
        <VDataTable
          :headers="headers"
          :items="cities"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          :options="options"
          class="text-no-wrap"
          show-select
          v-model="selectedRows"
        >
          <!-- City name using ready-made template -->
          <template #item.name="{ item }">
            <div class="d-flex align-center">
              <VAvatar
                size="32"
                color="primary"
                variant="tonal"
              >
                {{ item.name.charAt(0).toUpperCase() }}
              </VAvatar>
              <div class="d-flex flex-column ms-3">
                <span class="d-block font-weight-medium text-truncate text-high-emphasis">{{ item.name }}</span>
              </div>
            </div>
          </template>

          <!-- Country using ready-made template -->
          <template #item.country="{ item }">
            <VChip
              v-if="item.country"
              color="info"
              variant="tonal"
              size="small"
            >
              {{ item.country }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد دولة
            </span>
          </template>

          <!-- Actions using template icons -->
          <template #item.actions="{ item }">
            <div class="d-flex gap-1">
              <IconBtn @click="openEditDialog(item)">
                <VIcon icon="tabler-edit" />
              </IconBtn>
              <IconBtn @click="openDeleteDialog(item)">
                <VIcon icon="tabler-trash" />
              </IconBtn>
            </div>
          </template>

          <!-- External pagination using ready-made template -->
          <template #bottom>
            <VCardText class="pt-2">
              <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
                <VSelect
                  v-model="options.itemsPerPage"
                  :items="[5, 10, 25, 50, 100]"
                  label="عناصر في الصفحة:"
                  variant="underlined"
                  style="max-inline-size: 8rem;min-inline-size: 5rem;"
                />

                <VPagination
                  v-model="options.page"
                  :total-visible="$vuetify.display.smAndDown ? 3 : 5"
                  :length="Math.ceil(totalItems / options.itemsPerPage)"
                />
              </div>
            </VCardText>
          </template>
        </VDataTable>
      </VCardText>
    </VCard>

    <!-- Add City Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مدينة جديدة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addCity">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newCity.name"
                  label="اسم المدينة"
                  variant="outlined"
                  required
                  :error="shouldShowFieldError('name')"
                  :error-messages="getFieldErrors('name')"
                  @blur="setFieldTouched('name')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newCity.country"
                  label="الدولة"
                  variant="outlined"
                  required
                  :error="shouldShowFieldError('country')"
                  :error-messages="getFieldErrors('country')"
                  @blur="setFieldTouched('country')"
                />
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="dialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            :disabled="hasErrors"
            @click="addCity"
          >
            حفظ
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit City Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المدينة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateCity">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editCity.name"
                  label="اسم المدينة"
                  variant="outlined"
                  required
                  :error="shouldShowFieldError('editName')"
                  :error-messages="getFieldErrors('editName')"
                  @blur="setFieldTouched('editName')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editCity.country"
                  label="الدولة"
                  variant="outlined"
                  required
                  :error="shouldShowFieldError('editCountry')"
                  :error-messages="getFieldErrors('editCountry')"
                  @blur="setFieldTouched('editCountry')"
                />
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="editDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            :disabled="hasErrors"
            @click="updateCity"
          >
            تحديث
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Delete Confirmation Dialog -->
    <VDialog
      v-model="deleteDialog"
      max-width="400px"
    >
      <VCard>
        <VCardTitle class="text-h6">تأكيد الحذف</VCardTitle>
        <VCardText>
          هل أنت متأكد من حذف المدينة "<strong>{{ selectedCity?.name }}</strong>"؟
          <br>
          <span class="text-error">لا يمكن التراجع عن هذا الإجراء.</span>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="error"
            variant="flat"
            @click="deleteCity"
          >
            حذف
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* Using template styling */
</style>

