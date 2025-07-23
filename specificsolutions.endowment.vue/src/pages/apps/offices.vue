<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'

// Define interfaces for better type safety
interface Office {
  id: number
  key: string
  value: string
  name: string
  phoneNumber: string
  location?: string
  regionId?: string
  regionName?: string
}

interface NewOffice {
  name: string
  location: string
  phoneNumber: string
  regionId: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'Office',
  },
})

const offices = ref<Office[]>([])
const regions = ref<any[]>([])
const loading = ref(false)
const regionsLoading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref<'success' | 'error' | 'warning' | 'info'>('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedOffice = ref<Office | null>(null)
const selectedRows = ref<Office[]>([])
const search = ref('')

const newOffice = ref<NewOffice>({
  name: '',
  location: '',
  phoneNumber: '',
  regionId: '',
})

const editOffice = ref<Office>({
  id: 0,
  key: '',
  value: '',
  name: '',
  phoneNumber: '',
  location: '',
  regionId: '',
})

// استخدام نظام التحقق من النماذج
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
  addError,
} = useFormValidation()

// استخدام i18n للترجمة
const { t, locale } = useI18n()

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using i18n translations
const headers = computed(() => [
  {
    title: t('tableHeaders.offices.name'),
    key: 'name',
  },
  {
    title: t('tableHeaders.offices.regionName'),
    key: 'regionName',
  },
  {
    title: t('tableHeaders.offices.phoneNumber'),
    key: 'phoneNumber',
  },
  {
    title: t('tableHeaders.offices.actions'),
    key: 'actions',
    sortable: false,
  },
])

const loadOffices = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Office/filter?${params}`, {
      headers: {
        'Accept-Language': locale.value
      }
    })
    offices.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadOffices() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading offices:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء تحميل المكاتب' : 'Error loading offices'
    alertType.value = 'error'
    showAlert.value = true
    offices.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadRegions = async () => {
  regionsLoading.value = true
  try {
    const response = await $api('/Region/filter?PageSize=100', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    regions.value = response.data.items || []
  } catch (error) {
    console.error('Error loading regions:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء تحميل المناطق' : 'Error loading regions'
    alertType.value = 'error'
    showAlert.value = true
  } finally {
    regionsLoading.value = false
  }
}

const addOffice = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من اسم المكتب
  if (!validateRequired(newOffice.value.name, 'name', locale.value === 'ar' ? 'اسم المكتب مطلوب' : 'Office name is required')) {
    isValid = false
  } else if (!validateLength(newOffice.value.name, 'name', 1, 100, locale.value === 'ar' ? 'اسم المكتب يجب أن يكون بين 1 و 100 حرف' : 'Office name must be between 1 and 100 characters')) {
    isValid = false
  }
  
  // التحقق من الموقع
  if (!validateRequired(newOffice.value.location, 'location', locale.value === 'ar' ? 'الموقع مطلوب' : 'Location is required')) {
    isValid = false
  } else if (!validateLength(newOffice.value.location, 'location', 1, 200, locale.value === 'ar' ? 'الموقع يجب أن يكون بين 1 و 200 حرف' : 'Location must be between 1 and 200 characters')) {
    isValid = false
  }
  
  // التحقق من رقم الهاتف
  if (!validateRequired(newOffice.value.phoneNumber, 'phoneNumber', locale.value === 'ar' ? 'رقم الهاتف مطلوب' : 'Phone number is required')) {
    isValid = false
  } else {
    const phoneRegex = /^(09[1-5]|02[1-9])-?\d{7}$/
    if (newOffice.value.phoneNumber && !phoneRegex.test(newOffice.value.phoneNumber)) {
      addError('phoneNumber', locale.value === 'ar' ? 'يجب أن يكون رقم الهاتف بتنسيق ليبي صحيح (مثال: 091-1234567)' : 'Phone number must be in valid Libyan format (e.g., 091-1234567)')
      isValid = false
    }
  }
  
  // التحقق من المنطقة
  if (!validateRequired(newOffice.value.regionId, 'regionId', locale.value === 'ar' ? 'المنطقة مطلوبة' : 'Region is required')) {
    isValid = false
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('name')
  setFieldTouched('location')
  setFieldTouched('phoneNumber')
  setFieldTouched('regionId')
  
  if (!isValid) {
    return
  }

  try {
    const response = await $api('/Office', {
      method: 'POST',
      body: {
        name: newOffice.value.name.trim(),
        location: newOffice.value.location.trim(),
        phoneNumber: newOffice.value.phoneNumber.trim(),
        regionId: newOffice.value.regionId,
      },
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
        // تعيين جميع الحقول كملموسة لعرض الأخطاء
        setFieldTouched('name')
        setFieldTouched('location')
        setFieldTouched('phoneNumber')
        setFieldTouched('regionId')
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء إضافة المكتب' : 'Error adding office')
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    dialog.value = false
    resetNewOffice()
    loadOffices()
    alertMessage.value = locale.value === 'ar' ? 'تم إضافة المكتب بنجاح' : 'Office added successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding office:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء إضافة المكتب' : 'Error adding office'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateOffice = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من اسم المكتب
  if (!validateRequired(editOffice.value.name, 'name', locale.value === 'ar' ? 'اسم المكتب مطلوب' : 'Office name is required')) {
    isValid = false
  } else if (!validateLength(editOffice.value.name, 'name', 1, 100, locale.value === 'ar' ? 'اسم المكتب يجب أن يكون بين 1 و 100 حرف' : 'Office name must be between 1 and 100 characters')) {
    isValid = false
  }
  
  // التحقق من الموقع
  if (!validateRequired(editOffice.value.location, 'location', locale.value === 'ar' ? 'الموقع مطلوب' : 'Location is required')) {
    isValid = false
  } else if (editOffice.value.location && !validateLength(editOffice.value.location, 'location', 1, 200, locale.value === 'ar' ? 'الموقع يجب أن يكون بين 1 و 200 حرف' : 'Location must be between 1 and 200 characters')) {
    isValid = false
  }
  
  // التحقق من رقم الهاتف
  if (!validateRequired(editOffice.value.phoneNumber, 'phoneNumber', locale.value === 'ar' ? 'رقم الهاتف مطلوب' : 'Phone number is required')) {
    isValid = false
  } else {
    const phoneRegex = /^(09[1-5]|02[1-9])-?\d{7}$/
    if (editOffice.value.phoneNumber && !phoneRegex.test(editOffice.value.phoneNumber)) {
      addError('phoneNumber', locale.value === 'ar' ? 'يجب أن يكون رقم الهاتف بتنسيق ليبي صحيح (مثال: 091-1234567)' : 'Phone number must be in valid Libyan format (e.g., 091-1234567)')
      isValid = false
    }
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('name')
  setFieldTouched('location')
  setFieldTouched('phoneNumber')
  
  if (!isValid) {
    return
  }

  try {
    const response = await $api(`/Office/${editOffice.value.id}`, {
      method: 'PUT',
      body: {
        id: editOffice.value.id,
        name: editOffice.value.name.trim(),
        location: editOffice.value.location?.trim() || '',
        phoneNumber: editOffice.value.phoneNumber.trim(),
      },
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
        // تعيين جميع الحقول كملموسة لعرض الأخطاء
        setFieldTouched('name')
        setFieldTouched('location')
        setFieldTouched('phoneNumber')
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء تحديث المكتب' : 'Error updating office')
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    editDialog.value = false
    loadOffices()
    alertMessage.value = locale.value === 'ar' ? 'تم تحديث المكتب بنجاح' : 'Office updated successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating office:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء تحديث المكتب' : 'Error updating office'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteOffice = async () => {
  if (!selectedOffice.value) return
  
  try {
    const response = await $api(`/Office/${selectedOffice.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || (locale.value === 'ar' ? 'حدث خطأ أثناء حذف المكتب' : 'Error deleting office')
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    deleteDialog.value = false
    loadOffices()
    alertMessage.value = locale.value === 'ar' ? 'تم حذف المكتب بنجاح' : 'Office deleted successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting office:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ في الاتصال بالخادم' : 'Network connection error'
    alertType.value = 'error'
    showAlert.value = true
    deleteDialog.value = false
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(office => 
      $api(`/Office/${office.id}`, { 
        method: 'DELETE',
        headers: {
          'Accept-Language': locale.value
        }
      })
    )
    const responses = await Promise.all(deletePromises)
    
    // Check if any operation failed - response comes directly
    const failedOperations = responses.filter((response: any) => 
      response && response.isSuccess === false
    )
    
    if (failedOperations.length > 0) {
      const errorMessages = failedOperations.map((response: any) => 
        response?.message || response?.errors?.[0]?.errorMessage || (locale.value === 'ar' ? 'حدث خطأ أثناء العملية' : 'Operation error')
      )
      const errorMsg = locale.value === 'ar' 
        ? `فشل في حذف ${failedOperations.length} عنصر: ${errorMessages.join(', ')}`
        : `Failed to delete ${failedOperations.length} items: ${errorMessages.join(', ')}`
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    // If we reach here, all deletions were successful
    selectedRows.value = []
    loadOffices()
    alertMessage.value = locale.value === 'ar' ? 'تم حذف المكاتب المحددة بنجاح' : 'Selected offices deleted successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected offices:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ في الاتصال بالخادم' : 'Network connection error'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (office: Office) => {
  editOffice.value = { 
    ...office,
    location: office.location || '',
    phoneNumber: office.phoneNumber || '',
    name: office.name || ''
  }
  editDialog.value = true
  // مسح أخطاء التحقق عند فتح النافذة
  clearErrors()
}

const openDeleteDialog = (office: Office) => {
  selectedOffice.value = office
  deleteDialog.value = true
}

const resetNewOffice = () => {
  newOffice.value = {
    name: '',
    location: '',
    phoneNumber: '',
    regionId: '',
  }
  // مسح أخطاء التحقق
  clearErrors()
}

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadOffices()
}, { immediate: false })

// Watch for pagination changes
watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadOffices()
}, { immediate: false })

onMounted(() => {
  loadOffices()
  loadRegions()
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
        <span class="text-h5">{{ locale === 'ar' ? 'إدارة المكاتب' : 'Offices Management' }}</span>
        <div class="d-flex gap-2">
          <VBtn
            v-if="selectedRows.length > 0"
            color="error"
            variant="outlined"
            @click="deleteSelectedRows"
          >
            {{ locale === 'ar' ? `حذف المحدد (${selectedRows.length})` : `Delete Selected (${selectedRows.length})` }}
          </VBtn>
          <VBtn
            color="primary"
            @click="dialog = true"
          >
            {{ locale === 'ar' ? 'إضافة مكتب' : 'Add Office' }}
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
              :placeholder="locale === 'ar' ? 'البحث في المكاتب...' : 'Search offices...'"
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
          :items="offices"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          :options="options"
          class="text-no-wrap"
          show-select
          v-model="selectedRows"
        >
          <!-- Office name using ready-made template -->
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

          <!-- Region using ready-made template -->
          <template #item.regionName="{ item }">
            <VChip
              v-if="item.regionName"
              color="secondary"
              variant="tonal"
              size="small"
            >
              {{ item.regionName }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              {{ locale === 'ar' ? 'لا توجد منطقة' : 'No region' }}
            </span>
          </template>

          <!-- Phone Number using ready-made template -->
          <template #item.phoneNumber="{ item }">
            <VChip
              v-if="item.phoneNumber"
              color="success"
              variant="tonal"
              size="small"
            >
              {{ item.phoneNumber }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              {{ locale === 'ar' ? 'لا يوجد رقم هاتف' : 'No phone number' }}
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
                  :label="locale === 'ar' ? 'عناصر في الصفحة:' : 'Items per page:'"
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

    <!-- Add Office Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">{{ locale === 'ar' ? 'إضافة مكتب جديد' : 'Add New Office' }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.name"
                  :label="locale === 'ar' ? 'اسم المكتب' : 'Office Name'"
                  variant="outlined"
                  required
                  :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
                  :error-messages="validationState.errors.name || []"
                  @blur="setFieldTouched('name')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.location"
                  :label="locale === 'ar' ? 'الموقع' : 'Location'"
                  variant="outlined"
                  required
                  :error="validationState.errors.location && validationState.errors.location.length > 0 && validationState.touched.location"
                  :error-messages="validationState.errors.location || []"
                  @blur="setFieldTouched('location')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.phoneNumber"
                  :label="locale === 'ar' ? 'رقم الهاتف' : 'Phone Number'"
                  variant="outlined"
                  required
                  :placeholder="locale === 'ar' ? '091-1234567 أو 021-1234567' : '091-1234567 or 021-1234567'"
                  :error="validationState.errors.phoneNumber && validationState.errors.phoneNumber.length > 0 && validationState.touched.phoneNumber"
                  :error-messages="validationState.errors.phoneNumber || []"
                  @blur="setFieldTouched('phoneNumber')"
                  prepend-inner-icon="mdi-phone"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newOffice.regionId"
                  :label="locale === 'ar' ? 'المنطقة' : 'Region'"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  :no-data-text="locale === 'ar' ? 'لا توجد مناطق متاحة' : 'No regions available'"
                  required
                  :error="validationState.errors.regionId && validationState.errors.regionId.length > 0 && validationState.touched.regionId"
                  :error-messages="validationState.errors.regionId || []"
                  @blur="setFieldTouched('regionId')"
                  prepend-inner-icon="mdi-map-marker"
                  :placeholder="locale === 'ar' ? 'اختر المنطقة...' : 'Select region...'"
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
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            :disabled="hasErrors"
            @click="addOffice"
          >
            {{ locale === 'ar' ? 'حفظ' : 'Save' }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Office Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">{{ locale === 'ar' ? 'تعديل المكتب' : 'Edit Office' }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.name"
                  :label="locale === 'ar' ? 'اسم المكتب' : 'Office Name'"
                  variant="outlined"
                  required
                  :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
                  :error-messages="validationState.errors.name || []"
                  @blur="setFieldTouched('name')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.location"
                  :label="locale === 'ar' ? 'الموقع' : 'Location'"
                  variant="outlined"
                  required
                  :error="validationState.errors.location && validationState.errors.location.length > 0 && validationState.touched.location"
                  :error-messages="validationState.errors.location || []"
                  @blur="setFieldTouched('location')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.phoneNumber"
                  :label="locale === 'ar' ? 'رقم الهاتف' : 'Phone Number'"
                  variant="outlined"
                  required
                  :placeholder="locale === 'ar' ? '091-1234567 أو 021-1234567' : '091-1234567 or 021-1234567'"
                  :error="validationState.errors.phoneNumber && validationState.errors.phoneNumber.length > 0 && validationState.touched.phoneNumber"
                  :error-messages="validationState.errors.phoneNumber || []"
                  @blur="setFieldTouched('phoneNumber')"
                  prepend-inner-icon="mdi-phone"
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
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            :disabled="hasErrors"
            @click="updateOffice"
          >
            {{ locale === 'ar' ? 'تحديث' : 'Update' }}
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
        <VCardTitle class="text-h6">{{ locale === 'ar' ? 'تأكيد الحذف' : 'Confirm Delete' }}</VCardTitle>
        <VCardText>
          {{ locale === 'ar' ? `هل أنت متأكد من حذف المكتب "${selectedOffice?.name}"؟` : `Are you sure you want to delete the office "${selectedOffice?.name}"?` }}
          <br>
          <span class="text-error">{{ locale === 'ar' ? 'لا يمكن التراجع عن هذا الإجراء.' : 'This action cannot be undone.' }}</span>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="error"
            variant="flat"
            @click="deleteOffice"
          >
            {{ locale === 'ar' ? 'حذف' : 'Delete' }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* Using template styling */
</style> 