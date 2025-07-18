<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface Region {
  id: number
  key: string
  value: string
  name: string
  description: string
  country?: string
  cityId?: string
}

interface NewRegion {
  name: string
  country: string
  cityId: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'Region',
  },
})

const regions = ref<Region[]>([])
const cities = ref<any[]>([])
const countries = ref<string[]>([])
const loading = ref(false)
const citiesLoading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedRegion = ref<Region | null>(null)
const selectedRows = ref<Region[]>([])
const search = ref('')

const newRegion = ref<NewRegion>({
  name: '',
  country: '',
  cityId: '',
})

const editRegion = ref<Region>({
  id: 0,
  key: '',
  value: '',
  name: '',
  description: '',
  cityId: '',
})

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using the ready-made template structure
const headers = [
  {
    title: 'المنطقة',
    key: 'name',
  },
  {
    title: 'الدولة',
    key: 'country',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadRegions = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Region/filter?${params}`)
    regions.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadRegions() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading regions:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المناطق'
    alertType.value = 'error'
    showAlert.value = true
    regions.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadCities = async () => {
  citiesLoading.value = true
  try {
    const response = await $api('/City/filter?PageSize=100')
    cities.value = response.data.items || []
    
    // Extract unique countries from cities for searchable dropdown
    const uniqueCountries = [...new Set(cities.value.map(city => city.country).filter(Boolean))]
    countries.value = uniqueCountries.sort()
  } catch (error) {
    console.error('Error loading cities:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المدن'
    alertType.value = 'error'
    showAlert.value = true
  } finally {
    citiesLoading.value = false
  }
}

const addRegion = async () => {
  try {
    const response = await $api('/Region', {
      method: 'POST',
      body: {
        name: newRegion.value.name,
        country: newRegion.value.country,
        cityId: newRegion.value.cityId,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة المنطقة'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    dialog.value = false
    resetNewRegion()
    loadRegions()
    alertMessage.value = 'تم إضافة المنطقة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding region:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة المنطقة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateRegion = async () => {
  try {
    const response = await $api(`/Region/${editRegion.value.id}`, {
      method: 'PUT',
      body: {
        id: editRegion.value.id,
        name: editRegion.value.name,
        country: editRegion.value.country || editRegion.value.description,
        cityId: editRegion.value.cityId,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تحديث المنطقة'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    editDialog.value = false
    loadRegions()
    alertMessage.value = 'تم تحديث المنطقة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating region:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث المنطقة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteRegion = async () => {
  if (!selectedRegion.value) return
  
  try {
    console.log('Attempting to delete region:', selectedRegion.value.id)
    const response = await $api(`/Region/${selectedRegion.value.id}`, {
      method: 'DELETE',
    })
    
    console.log('Delete response:', response)
    console.log('Response data:', response.data)
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المنطقة'
      console.log('API returned error:', errorMsg)
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    console.log('Region deleted successfully')
    deleteDialog.value = false
    loadRegions()
    alertMessage.value = 'تم حذف المنطقة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting region:', error)
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
    const deletePromises = selectedRows.value.map(region => 
      $api(`/Region/${region.id}`, { method: 'DELETE' })
    )
    const responses = await Promise.all(deletePromises)
    
    // Check if any operation failed - response comes directly
    const failedOperations = responses.filter((response) => 
      response && response.isSuccess === false
    )
    
    if (failedOperations.length > 0) {
      const errorMessages = failedOperations.map((response) => 
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
    loadRegions()
    alertMessage.value = 'تم حذف المناطق المحددة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected regions:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = 'حدث خطأ في الاتصال بالخادم'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (region: Region) => {
  editRegion.value = { 
    ...region, 
    country: region.country || region.description || '' 
  }
  editDialog.value = true
}

const openDeleteDialog = (region: Region) => {
  selectedRegion.value = region
  deleteDialog.value = true
}

const resetNewRegion = () => {
  newRegion.value = {
    name: '',
    country: '',
    cityId: '',
  }
}

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadRegions()
}, { immediate: false })

// Watch for pagination changes
watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadRegions()
}, { immediate: false })

onMounted(() => {
  loadRegions()
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
        <span class="text-h5">إدارة المناطق</span>
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
            @click="dialog = true"
          >
            إضافة منطقة
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
              placeholder="البحث في المناطق..."
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
          :items="regions"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          :options="options"
          class="text-no-wrap"
          show-select
          v-model="selectedRows"
        >
          <!-- Region name using ready-made template -->
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
                <small class="text-medium-emphasis">{{ item.description || 'لا يوجد وصف' }}</small>
              </div>
            </div>
          </template>

          <!-- Country using ready-made template -->
          <template #item.country="{ item }">
            <VChip
              v-if="item.country"
              color="secondary"
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

    <!-- Add Region Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة منطقة جديدة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addRegion">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newRegion.name"
                  label="اسم المنطقة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المنطقة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newRegion.cityId"
                  label="المدينة"
                  variant="outlined"
                  :items="cities"
                  item-title="name"
                  item-value="id"
                  :loading="citiesLoading"
                  clearable
                  no-data-text="لا توجد مدن متاحة"
                  required
                  :rules="[v => !!v || 'المدينة مطلوبة']"
                  prepend-inner-icon="mdi-city"
                  placeholder="اختر المدينة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newRegion.country"
                  label="الدولة"
                  variant="outlined"
                  :items="countries"
                  :loading="citiesLoading"
                  clearable
                  no-data-text="لا توجد دول متاحة"
                  required
                  :rules="[v => !!v || 'الدولة مطلوبة']"
                  prepend-inner-icon="mdi-magnify"
                  placeholder="ابحث عن دولة..."
                  hide-no-data
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
            @click="addRegion"
          >
            حفظ
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Region Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المنطقة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateRegion">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editRegion.name"
                  label="اسم المنطقة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المنطقة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="editRegion.cityId"
                  label="المدينة"
                  variant="outlined"
                  :items="cities"
                  item-title="name"
                  item-value="id"
                  :loading="citiesLoading"
                  clearable
                  no-data-text="لا توجد مدن متاحة"
                  required
                  :rules="[v => !!v || 'المدينة مطلوبة']"
                  prepend-inner-icon="mdi-city"
                  placeholder="اختر المدينة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="editRegion.country"
                  label="الدولة"
                  variant="outlined"
                  :items="countries"
                  :loading="citiesLoading"
                  clearable
                  no-data-text="لا توجد دول متاحة"
                  required
                  :rules="[v => !!v || 'الدولة مطلوبة']"
                  prepend-inner-icon="mdi-magnify"
                  placeholder="ابحث عن دولة..."
                  hide-no-data
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
            @click="updateRegion"
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
          هل أنت متأكد من حذف المنطقة "<strong>{{ selectedRegion?.value }}</strong>"؟
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
            @click="deleteRegion"
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

