<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'
import { useApi } from '@/composables/useApi'

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

// Alerts: use the shared app alerts only

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

// استخدام نظام التنبيهات الجديد "نمط خالد" على مستوى التطبيق
import { useAppAlerts } from '@/composables/useAppAlerts'

const { success: showSuccess, error: showError, warning: showWarning, info: showInfo } = useAppAlerts()

// استخدام i18n للترجمة
const { t, te, locale } = useI18n()

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
    
    const response = await useApi()(`/Office/filter?${params}`, {
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
    showError(t('pages.offices.errorLoadingOffices'), {
      timeout: 0,
      clickToDismiss: true
    })
    offices.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadRegions = async () => {
  regionsLoading.value = true
  try {
    const response = await useApi()('/Region/filter?PageSize=100', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    regions.value = response.data.items || []
  } catch (error) {
    console.error('Error loading regions:', error)
    showError(t('pages.offices.errorLoadingRegions'), {
      timeout: 0,
      clickToDismiss: true
    })
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
    showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
      timeout: 5000,
      clickToDismiss: true
    })
    return
  }

  try {
    const response = await useApi()('/Office', {
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
        
        showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
          timeout: 5000,
          clickToDismiss: true
        })
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء إضافة المكتب' : 'Error adding office')
        showError(errorMsg, {
          timeout: 0,
          clickToDismiss: true
        })
      }
      return
    }
    
    dialog.value = false
    resetNewOffice()
    loadOffices()
    showSuccess(locale.value === 'ar' ? 'تم إضافة المكتب بنجاح' : 'Office added successfully', {
      timeout: 4000,
      clickToDismiss: true
    })
  } catch (error) {
    console.error('Error adding office:', error)
    showError(locale.value === 'ar' ? 'حدث خطأ أثناء إضافة المكتب' : 'Error adding office', {
      timeout: 0,
      clickToDismiss: true
    })
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
    showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
      timeout: 5000,
      clickToDismiss: true
    })
    return
  }

  try {
    const response = await useApi()(`/Office/${editOffice.value.id}`, {
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
        
        showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
          timeout: 5000,
          clickToDismiss: true
        })
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء تحديث المكتب' : 'Error updating office')
        showError(errorMsg, {
          timeout: 0,
          clickToDismiss: true
        })
      }
      return
    }
    
    editDialog.value = false
    loadOffices()
    showSuccess(locale.value === 'ar' ? 'تم تحديث المكتب بنجاح' : 'Office updated successfully', {
      timeout: 4000,
      clickToDismiss: true
    })
  } catch (error) {
    console.error('Error updating office:', error)
    showError(locale.value === 'ar' ? 'حدث خطأ أثناء تحديث المكتب' : 'Error updating office', {
      timeout: 0,
      clickToDismiss: true
    })
  }
}

const deleteOffice = async () => {
  if (!selectedOffice.value) return
  
  try {
    const response = await useApi()(`/Office/${selectedOffice.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || (locale.value === 'ar' ? 'حدث خطأ أثناء حذف المكتب' : 'Error deleting office')
      showError(errorMsg, {
        timeout: 0,
        clickToDismiss: true
      })
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    deleteDialog.value = false
    loadOffices()
    showSuccess(locale.value === 'ar' ? 'تم حذف المكتب بنجاح' : 'Office deleted successfully', {
      timeout: 4000,
      clickToDismiss: true
    })
  } catch (error) {
    console.error('Error deleting office:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    showError(locale.value === 'ar' ? 'حدث خطأ في الاتصال بالخادم' : 'Network connection error', {
      timeout: 0,
      clickToDismiss: true
    })
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(office => 
      useApi()(`/Office/${office.id}`, { 
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
      showError(errorMsg, {
        timeout: 0,
        clickToDismiss: true
      })
      return
    }
    
    // If we reach here, all deletions were successful
    selectedRows.value = []
    loadOffices()
    showSuccess(locale.value === 'ar' ? 'تم حذف المكاتب المحددة بنجاح' : 'Selected offices deleted successfully', {
      timeout: 4000,
      clickToDismiss: true
    })
  } catch (error) {
    console.error('Error deleting selected offices:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    showError(locale.value === 'ar' ? 'حدث خطأ في الاتصال بالخادم' : 'Network connection error', {
      timeout: 0,
      clickToDismiss: true
    })
  }
}

const openEditDialog = (office: Office) => {
  clearErrors() // Clear previous validation errors
  editOffice.value = { 
    ...office,
    location: office.location || '',
    phoneNumber: office.phoneNumber || '',
    name: office.name || ''
  }
  editDialog.value = true
}

const openDeleteDialog = (office: Office) => {
  clearErrors() // Clear any validation errors
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
  clearErrors() // Clear validation errors
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

    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center pa-6">
        <span class="text-h5">{{ t('pages.offices.title') }}</span>
        <div class="d-flex gap-2">
          <VBtn
            v-if="selectedRows.length > 0"
            color="error"
            variant="outlined"
            @click="() => { clearErrors(); deleteSelectedRows(); }"
          >
            {{ t('pages.offices.deleteSelected', { count: selectedRows.length }) }}
          </VBtn>
          <VBtn
            color="primary"
            @click="() => { clearErrors(); resetNewOffice(); dialog = true; }"
          >
            {{ t('pages.offices.addOffice') }}
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
              :placeholder="t('pages.offices.searchPlaceholder')"
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
              {{ t('pages.offices.noRegion') }}
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
              {{ t('pages.offices.noPhone') }}
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
                  :label="t('pages.offices.itemsPerPage')"
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
        <VCardTitle class="text-h6">{{ t('pages.offices.addNewOffice') }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.name"
                  :label="t('pages.offices.officeName')"
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
                  :label="t('pages.offices.location')"
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
                  :label="t('pages.offices.phoneNumber')"
                  variant="outlined"
                  required
                  :placeholder="'091-1234567 / 021-1234567'"
                  :error="validationState.errors.phoneNumber && validationState.errors.phoneNumber.length > 0 && validationState.touched.phoneNumber"
                  :error-messages="validationState.errors.phoneNumber || []"
                  @blur="setFieldTouched('phoneNumber')"
                  prepend-inner-icon="mdi-phone"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newOffice.regionId"
                  :label="t('pages.offices.region')"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  :no-data-text="t('pages.offices.noRegionsAvailable')"
                  required
                  :error="validationState.errors.regionId && validationState.errors.regionId.length > 0 && validationState.touched.regionId"
                  :error-messages="validationState.errors.regionId || []"
                  @blur="setFieldTouched('regionId')"
                  prepend-inner-icon="mdi-map-marker"
                  :placeholder="t('pages.offices.selectRegionPlaceholder')"
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
            @click="() => { clearErrors(); dialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="() => { clearErrors(); addOffice(); }"
          >
            {{ t('common.save') }}
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
        <VCardTitle class="text-h6">{{ t('pages.offices.editOffice') }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.name"
                  :label="t('pages.offices.officeName')"
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
                  :label="t('pages.offices.location')"
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
                  :label="t('pages.offices.phoneNumber')"
                  variant="outlined"
                  required
                  :placeholder="'091-1234567 / 021-1234567'"
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
            @click="() => { clearErrors(); editDialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            :disabled="hasErrors"
            @click="() => { clearErrors(); updateOffice(); }"
          >
            {{ t('common.update') }}
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
        <VCardTitle class="text-h6">{{ t('pages.offices.confirmDelete') }}</VCardTitle>
        <VCardText>
          {{ t('pages.offices.deleteConfirmation', { name: selectedOffice?.name }) }}
          <br>
          <span class="text-error">{{ t('pages.offices.deleteWarning') }}</span>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="() => { clearErrors(); deleteDialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="error"
            variant="flat"
            @click="() => { clearErrors(); deleteOffice(); }"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* Using template styling */
</style> 