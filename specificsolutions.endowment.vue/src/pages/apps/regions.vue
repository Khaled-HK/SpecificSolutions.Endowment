<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'
import { useApi } from '@/composables/useApi'

// Define interfaces for better type safety
interface Region {
  id: number
  key: string
  value: string
  name: string
  description: string
  country: string
  cityId: string
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

const { t, te } = useI18n()

// استخدام نظام التحقق الجديد "نمط خالد"
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

// استخدام نظام التنبيهات الموحد على مستوى التطبيق
import { useAppAlerts } from '@/composables/useAppAlerts'

const { success: showSuccess, error: showError, warning: showWarning, info: showInfo } = useAppAlerts()

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
  country: '',
  cityId: '',
})



// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using dynamic i18n translations "نمط خالد"
const headers = computed(() => [
  {
    title: t('tableHeaders.regions.name'),
    key: 'name',
  },
  {
    title: t('tableHeaders.regions.country'),
    key: 'country',
  },
  {
    title: t('tableHeaders.regions.actions'),
    key: 'actions',
    sortable: false,
  },
])

// Get API instance
const api = useApi()

const loadRegions = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await api(`/Region/filter?${params}`)
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
    console.error('❌ خطأ في تحميل المناطق:', error)
    showError(t('pages.regions.errorLoading') || 'حدث خطأ أثناء تحميل المناطق', { timeout: 0, clickToDismiss: true })
    regions.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadCities = async () => {
  citiesLoading.value = true
  try {
    const response = await api('/City/filter?PageSize=100')
    cities.value = response.data.items || []
    
    // Extract unique countries from cities for searchable dropdown
    const uniqueCountries = [...new Set(cities.value.map(city => city.country).filter(Boolean))]
    countries.value = uniqueCountries.sort()
  } catch (error) {
    console.error('❌ خطأ في تحميل المدن:', error)
    showError(t('pages.cities.errorLoading') ?? 'حدث خطأ أثناء تحميل المدن', { timeout: 0, clickToDismiss: true })
  } finally {
    citiesLoading.value = false
  }
}



const addRegion = async () => {
  // Clear previous errors
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من اسم المنطقة
  if (!validateRequired(newRegion.value.name, 'name', 'اسم المنطقة مطلوب')) {
    isValid = false
  } else if (!validateLength(newRegion.value.name, 'name', 1, 100, 'اسم المنطقة يجب أن يكون بين 1 و 100 حرف')) {
    isValid = false
  }
  
  // التحقق من الدولة
  if (!validateRequired(newRegion.value.country, 'country', 'الدولة مطلوبة')) {
    isValid = false
  } else if (!validateLength(newRegion.value.country, 'country', 1, 100, 'اسم الدولة يجب أن يكون بين 1 و 100 حرف')) {
    isValid = false
  }
  
  // التحقق من المدينة
  if (!validateRequired(newRegion.value.cityId, 'cityId', 'المدينة مطلوبة')) {
    isValid = false
  }
  
  // إذا كان هناك أخطاء في التحقق، لا تتابع
  if (!isValid) {
    // Mark all fields as touched to ensure errors show immediately
    setFieldTouched('name')
    setFieldTouched('country')
    setFieldTouched('cityId')
    showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    return
  }

  try {
    const response = await api('/Region', {
      method: 'POST',
      body: {
        name: newRegion.value.name,
        country: newRegion.value.country,
        cityId: newRegion.value.cityId,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response, 'add')
        
        // Mark fields as touched to show errors
        setFieldTouched('name')
        setFieldTouched('cityId')
        setFieldTouched('country')
        
        // إظهار رسالة للمستخدم باستخدام النظام الجديد
        showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
      } else {
        const errorMsg = response.message || t('pages.regions.errorAdd') || 'حدث خطأ أثناء إضافة المنطقة'
        showError(errorMsg, { timeout: 0, clickToDismiss: true })
      }
      return
    }
    
    dialog.value = false
    resetNewRegion()
    loadRegions()
    showSuccess(t('pages.regions.successAdd') ?? 'تم إضافة المنطقة بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (error: any) {
    console.error('❌ خطأ في الشبكة أو في الخادم:', error)
    
    // التحقق من أن الخطأ يحتوي على أخطاء FluentValidation
    if (error?.data?.errors && Array.isArray(error.data.errors)) {
      setErrorsFromResponse(error.data, 'add')
      
      // Mark fields as touched to show errors
      setFieldTouched('name')
      setFieldTouched('cityId')
      setFieldTouched('country')
      
      showWarning(t('validation.fixHighlighted') || '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    } else if (error?.data?.message) {
      showError(error.data.message, { timeout: 0, clickToDismiss: true })
    } else {
      showError(t('pages.regions.errorAdd') || 'حدث خطأ أثناء إضافة المنطقة', { timeout: 0, clickToDismiss: true })
    }
  }
}

const updateRegion = async () => {
  // Clear previous errors
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من اسم المنطقة
  if (!validateRequired(editRegion.value.name, 'editName', 'اسم المنطقة مطلوب')) {
    isValid = false
  } else if (!validateLength(editRegion.value.name, 'editName', 1, 100, 'اسم المنطقة يجب أن يكون بين 1 و 100 حرف')) {
    isValid = false
  }
  
  // التحقق من الدولة
  if (!validateRequired(editRegion.value.country, 'editCountry', 'الدولة مطلوبة')) {
    isValid = false
  } else if (!validateLength(editRegion.value.country, 'editCountry', 1, 100, 'اسم الدولة يجب أن يكون بين 1 و 100 حرف')) {
    isValid = false
  }
  
  // التحقق من المدينة
  if (!validateRequired(editRegion.value.cityId, 'editCityId', 'المدينة مطلوبة')) {
    isValid = false
  }
  
  // إذا كان هناك أخطاء في التحقق، لا تتابع
  if (!isValid) {
    // Mark all fields as touched to ensure errors show immediately
    setFieldTouched('editName')
    setFieldTouched('editCountry')
    setFieldTouched('editCityId')
    showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    return
  }

  try {
    const response = await api(`/Region/${editRegion.value.id}`, {
      method: 'PUT',
      body: {
        id: editRegion.value.id,
        name: editRegion.value.name,
        country: editRegion.value.country,
        cityId: editRegion.value.cityId,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response, 'edit')
        
        // Mark fields as touched to show errors
        setFieldTouched('editName')
        setFieldTouched('editCityId')
        setFieldTouched('editCountry')
        
        // إظهار رسالة للمستخدم باستخدام النظام الجديد
        showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
      } else {
        const errorMsg = response.message || t('pages.regions.errorUpdate') || 'حدث خطأ أثناء تحديث المنطقة'
        showError(errorMsg, { timeout: 0, clickToDismiss: true })
      }
      return
    }
    
    editDialog.value = false
    loadRegions()
    showSuccess(t('pages.regions.successUpdate') || 'تم تحديث المنطقة بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (error: any) {
    console.error('❌ خطأ في الشبكة أو في الخادم:', error)
    
    // التحقق من أن الخطأ يحتوي على أخطاء FluentValidation
    if (error?.data?.errors && Array.isArray(error.data.errors)) {
      setErrorsFromResponse(error.data, 'edit')
      
      // Mark fields as touched to show errors
      setFieldTouched('editName')
      setFieldTouched('editCityId')
      setFieldTouched('editCountry')
      
      showWarning(t('validation.fixHighlighted') || '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    } else if (error?.data?.message) {
      showError(error.data.message, { timeout: 0, clickToDismiss: true })
    } else {
      showError(t('pages.regions.errorUpdate') || 'حدث خطأ أثناء تحديث المنطقة', { timeout: 0, clickToDismiss: true })
    }
  }
}

const deleteRegion = async () => {
  if (!selectedRegion.value) return
  
  try {
    console.log('Attempting to delete region:', selectedRegion.value.id)
    const response = await api(`/Region/${selectedRegion.value.id}`, {
      method: 'DELETE',
    })
    
    console.log('Delete response:', response)
    console.log('Response data:', response.data)
    
    // Check if the response indicates success - response comes directly, not in response.data
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || t('pages.regions.errorDelete') || 'حدث خطأ أثناء حذف المنطقة'
      console.log('API returned error:', errorMsg)
      showError(errorMsg, { timeout: 0, clickToDismiss: true })
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    console.log('Region deleted successfully')
    deleteDialog.value = false
    loadRegions()
    showSuccess(t('pages.regions.successDelete') || 'تم حذف المنطقة بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (error: any) {
    console.error('❌ خطأ في الشبكة أو في الخادم:', error)
    
    // التحقق من أن الخطأ يحتوي على أخطاء FluentValidation
    if (error?.data?.errors && Array.isArray(error.data.errors)) {
      setErrorsFromResponse(error.data, 'edit')
      
      // Mark fields as touched to show errors
      setFieldTouched('editName')
      setFieldTouched('editCityId')
      setFieldTouched('editCountry')
      
      showWarning(t('validation.fixHighlighted') || '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    } else if (error?.data?.message) {
      showError(error.data.message, { timeout: 0, clickToDismiss: true })
    } else {
      showError(t('pages.regions.errorDelete') || 'حدث خطأ أثناء حذف المنطقة', { timeout: 0, clickToDismiss: true })
    }
    deleteDialog.value = false
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(region => 
      api(`/Region/${region.id}`, { method: 'DELETE' })
    )
    const responses = await Promise.all(deletePromises)
    
    // Check if any operation failed - response comes directly
    const failedOperations = responses.filter((response: any) => 
      response && response.isSuccess === false
    )
    
    if (failedOperations.length > 0) {
      const errorMessages = failedOperations.map((response: any) => 
        response?.message || response?.errors?.[0]?.errorMessage || (t('common.operationError') || 'حدث خطأ أثناء العملية')
      )
      const errorMsg = t('pages.regions.deleteSelectedError', { count: failedOperations.length, details: errorMessages.join(', ') }) || `فشل في حذف ${failedOperations.length} عنصر: ${errorMessages.join(', ')}`
      showError(errorMsg, { timeout: 0, clickToDismiss: true })
      return
    }
    
    // If we reach here, all deletions were successful
    selectedRows.value = []
    loadRegions()
    showSuccess(t('pages.regions.successDeleteSelected') || 'تم حذف المناطق المحددة بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (error: any) {
    console.error('❌ خطأ في الشبكة أو في الخادم:', error)
    
    // التحقق من أن الخطأ يحتوي على أخطاء FluentValidation
    if (error?.data?.errors && Array.isArray(error.data.errors)) {
      setErrorsFromResponse(error.data, 'edit')
      
      // Mark fields as touched to show errors
      setFieldTouched('editName')
      setFieldTouched('editCityId')
      setFieldTouched('editCountry')
      
      showWarning(t('validation.fixHighlighted') || '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    } else if (error?.data?.message) {
      showError(error.data.message, { timeout: 0, clickToDismiss: true })
    } else {
      showError(t('pages.regions.errorDeleteSelected') || 'حدث خطأ أثناء حذف المناطق المحددة', { timeout: 0, clickToDismiss: true })
    }
  }
}

const openEditDialog = (region: Region) => {
  clearErrors() // Clear previous validation errors
  editRegion.value = { 
    ...region, 
    country: region.country || region.description || '' 
  }
  editDialog.value = true
}

const openDeleteDialog = (region: Region) => {
  clearErrors() // Clear any validation errors
  selectedRegion.value = region
  deleteDialog.value = true
}

const resetNewRegion = () => {
  newRegion.value = {
    name: '',
    country: '',
    cityId: '',
  }
  clearErrors()
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
            @click="() => { clearErrors(); resetNewRegion(); dialog = true; }"
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
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newRegion.name"
                  label="اسم المنطقة"
                  variant="outlined"
                  required
                  :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
                  :error-messages="validationState.errors.name || []"
                  @blur="setFieldTouched('name')"
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
                  :error="validationState.errors.cityId && validationState.errors.cityId.length > 0 && validationState.touched.cityId"
                  :error-messages="validationState.errors.cityId || []"
                  @blur="setFieldTouched('cityId')"
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
                  :error="validationState.errors.country && validationState.errors.country.length > 0 && validationState.touched.country"
                  :error-messages="validationState.errors.country || []"
                  @blur="setFieldTouched('country')"
                  prepend-inner-icon="mdi-flag"
                  placeholder="ابحث عن دولة..."
                  hide-no-data
                />
              </VCol>
            </VRow>
          </VContainer>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="() => { clearErrors(); dialog = false; }"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="() => { clearErrors(); addRegion(); }"
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
                  :error="validationState.errors.editName && validationState.errors.editName.length > 0 && validationState.touched.editName"
                  :error-messages="validationState.errors.editName || []"
                  @blur="setFieldTouched('editName')"
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
                  :error="validationState.errors.editCityId && validationState.errors.editCityId.length > 0 && validationState.touched.editCityId"
                  :error-messages="validationState.errors.editCityId || []"
                  @blur="setFieldTouched('editCityId')"
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
                  :error="validationState.errors.editCountry && validationState.errors.editCountry.length > 0 && validationState.touched.editCountry"
                  :error-messages="validationState.errors.editCountry || []"
                  @blur="setFieldTouched('editCountry')"
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
            @click="() => { clearErrors(); editDialog = false; }"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="() => { clearErrors(); updateRegion(); }"
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
            @click="() => { clearErrors(); deleteDialog = false; }"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="error"
            variant="flat"
            @click="() => { clearErrors(); deleteRegion(); }"
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

