<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface Mosque {
  mosqueID: string
  mosqueName: string
  fileNumber: string
  mosqueDefinition: string
  mosqueClassification: string
  office: string
  unit: string
  region: string
  nearestLandmark: string
  constructionDate: string
  openingDate: string
  mapLocation: string
  totalLandArea: number
  totalCoveredArea: number
  numberOfFloors: number
  electricityMeter: string
  alternativeEnergySource: string
  waterSource: string
  sanitation: string
  briefDescription: string
}

interface NewMosque {
  name: string
  regionId: string
  officeId: string
  fileNumber: string
  definition: string
  classification: string
  unit: string
  nearestLandmark: string
  mapLocation: string
  sanitation: string
  electricityMeter: string
  alternativeEnergySource: string
  waterSource: string
  briefDescription: string
  totalCoveredArea: number
  totalLandArea: number
  numberOfFloors: number
  openingDate: string
  constructionDate: string
  mosqueDefinition: number
  mosqueClassification: number
  landDonorName: string
  prayerCapacity: string
  sourceFunds: number
  servicesSpecialNeeds: boolean
  specialEntranceWomen: boolean
}

definePage({
  meta: {
    action: 'View',
    subject: 'Mosque',
  },
})

const mosques = ref<Mosque[]>([])
const regions = ref<any[]>([])
const offices = ref<any[]>([])
const loading = ref(false)
const regionsLoading = ref(false)
const officesLoading = ref(false)
const totalItems = ref(0)

// Enum options
const mosqueDefinitions = [
  { value: 0, label: 'لا شيء' },
  { value: 1, label: 'جمعة' },
  { value: 2, label: 'أوقات' },
  { value: 3, label: 'مسجد مركزي' },
  { value: 4, label: 'قاعة صلاة عامة' },
]

const mosqueClassifications = [
  { value: 0, label: 'لا شيء' },
  { value: 1, label: 'عام' },
  { value: 2, label: 'وطني' },
]

const sourceFundsOptions = [
  { value: 0, label: 'منظمة' },
  { value: 1, label: 'محسنين' },
]

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedMosque = ref<Mosque | null>(null)
const selectedRows = ref<Mosque[]>([])
const search = ref('')

const newMosque = ref<NewMosque>({
  name: '',
  regionId: '',
  officeId: '',
  fileNumber: '',
  definition: '',
  classification: '',
  unit: '',
  nearestLandmark: '',
  mapLocation: '',
  sanitation: '',
  electricityMeter: '',
  alternativeEnergySource: '',
  waterSource: '',
  briefDescription: '',
  totalCoveredArea: 0,
  totalLandArea: 0,
  numberOfFloors: 1,
  openingDate: '',
  constructionDate: '',
  mosqueDefinition: 1,
  mosqueClassification: 1,
  landDonorName: '',
  prayerCapacity: '',
  sourceFunds: 1,
  servicesSpecialNeeds: false,
  specialEntranceWomen: false,
})

const editMosque = ref<NewMosque>({
  name: '',
  regionId: '',
  officeId: '',
  fileNumber: '',
  definition: '',
  classification: '',
  unit: '',
  nearestLandmark: '',
  mapLocation: '',
  sanitation: '',
  electricityMeter: '',
  alternativeEnergySource: '',
  waterSource: '',
  briefDescription: '',
  totalCoveredArea: 0,
  totalLandArea: 0,
  numberOfFloors: 1,
  openingDate: '',
  constructionDate: '',
  mosqueDefinition: 1,
  mosqueClassification: 1,
  landDonorName: '',
  prayerCapacity: '',
  sourceFunds: 1,
  servicesSpecialNeeds: false,
  specialEntranceWomen: false,
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
    title: 'اسم المسجد',
    key: 'mosqueName',
  },
  {
    title: 'رقم الملف',
    key: 'fileNumber',
  },
  {
    title: 'المنطقة',
    key: 'region',
  },
  {
    title: 'المكتب',
    key: 'office',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadMosques = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Mosque/filter?${params}`)
    mosques.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadMosques() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading mosques:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المساجد'
    alertType.value = 'error'
    showAlert.value = true
    mosques.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadRegions = async () => {
  regionsLoading.value = true
  try {
    const response = await $api('/Region/filter?PageSize=100')
    regions.value = response.data.items || []
  } catch (error) {
    console.error('Error loading regions:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المناطق'
    alertType.value = 'error'
    showAlert.value = true
  } finally {
    regionsLoading.value = false
  }
}

const loadOffices = async () => {
  officesLoading.value = true
  try {
    const response = await $api('/Office/filter?PageSize=100')
    offices.value = response.data.items || []
  } catch (error) {
    console.error('Error loading offices:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المكاتب'
    alertType.value = 'error'
    showAlert.value = true
  } finally {
    officesLoading.value = false
  }
}

const addMosque = async () => {
  try {
    const response = await $api('/Mosque', {
      method: 'POST',
      body: {
        name: newMosque.value.name,
        regionId: newMosque.value.regionId,
        officeId: newMosque.value.officeId,
        fileNumber: newMosque.value.fileNumber,
        definition: newMosque.value.definition,
        classification: newMosque.value.classification,
        unit: newMosque.value.unit,
        nearestLandmark: newMosque.value.nearestLandmark,
        mapLocation: newMosque.value.mapLocation,
        sanitation: newMosque.value.sanitation,
        electricityMeter: newMosque.value.electricityMeter,
        alternativeEnergySource: newMosque.value.alternativeEnergySource,
        waterSource: newMosque.value.waterSource,
        briefDescription: newMosque.value.briefDescription,
        totalCoveredArea: newMosque.value.totalCoveredArea,
        totalLandArea: newMosque.value.totalLandArea,
        numberOfFloors: newMosque.value.numberOfFloors,
        openingDate: newMosque.value.openingDate,
        constructionDate: newMosque.value.constructionDate,
        mosqueDefinition: newMosque.value.mosqueDefinition,
        mosqueClassification: newMosque.value.mosqueClassification,
        landDonorName: newMosque.value.landDonorName,
        prayerCapacity: newMosque.value.prayerCapacity,
        sourceFunds: newMosque.value.sourceFunds,
        servicesSpecialNeeds: newMosque.value.servicesSpecialNeeds,
        specialEntranceWomen: newMosque.value.specialEntranceWomen,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة المسجد'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    dialog.value = false
    resetNewMosque()
    loadMosques()
    alertMessage.value = 'تم إضافة المسجد بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding mosque:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة المسجد'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateMosque = async () => {
  try {
    const response = await $api(`/Mosque/${selectedMosque.value?.mosqueID}`, {
      method: 'PUT',
      body: {
        id: selectedMosque.value?.mosqueID,
        name: editMosque.value.name,
        regionId: editMosque.value.regionId,
        officeId: editMosque.value.officeId,
        fileNumber: editMosque.value.fileNumber,
        definition: editMosque.value.definition,
        classification: editMosque.value.classification,
        unit: editMosque.value.unit,
        nearestLandmark: editMosque.value.nearestLandmark,
        mapLocation: editMosque.value.mapLocation,
        sanitation: editMosque.value.sanitation,
        electricityMeter: editMosque.value.electricityMeter,
        alternativeEnergySource: editMosque.value.alternativeEnergySource,
        waterSource: editMosque.value.waterSource,
        briefDescription: editMosque.value.briefDescription,
        totalCoveredArea: editMosque.value.totalCoveredArea,
        totalLandArea: editMosque.value.totalLandArea,
        numberOfFloors: editMosque.value.numberOfFloors,
        openingDate: editMosque.value.openingDate,
        constructionDate: editMosque.value.constructionDate,
        mosqueDefinition: editMosque.value.mosqueDefinition,
        mosqueClassification: editMosque.value.mosqueClassification,
        landDonorName: editMosque.value.landDonorName,
        prayerCapacity: editMosque.value.prayerCapacity,
        sourceFunds: editMosque.value.sourceFunds,
        servicesSpecialNeeds: editMosque.value.servicesSpecialNeeds,
        specialEntranceWomen: editMosque.value.specialEntranceWomen,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تحديث المسجد'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    editDialog.value = false
    loadMosques()
    alertMessage.value = 'تم تحديث المسجد بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating mosque:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث المسجد'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteMosque = async () => {
  if (!selectedMosque.value) return
  
  try {
    console.log('Attempting to delete mosque:', selectedMosque.value.mosqueID)
    const response = await $api(`/Mosque/${selectedMosque.value.mosqueID}`, {
      method: 'DELETE',
    })
    
    console.log('Delete response:', response)
    console.log('Response data:', response.data)
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المسجد'
      console.log('API returned error:', errorMsg)
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    console.log('Mosque deleted successfully')
    deleteDialog.value = false
    loadMosques()
    alertMessage.value = 'تم حذف المسجد بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting mosque:', error)
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
    const deletePromises = selectedRows.value.map(mosque => 
      $api(`/Mosque/${mosque.mosqueID}`, { method: 'DELETE' })
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
    loadMosques()
    alertMessage.value = 'تم حذف المساجد المحددة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected mosques:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = 'حدث خطأ في الاتصال بالخادم'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (mosque: Mosque) => {
  editMosque.value = {
    name: mosque.mosqueName,
    regionId: '',
    officeId: '',
    fileNumber: mosque.fileNumber,
    definition: '',
    classification: '',
    unit: mosque.unit,
    nearestLandmark: mosque.nearestLandmark,
    mapLocation: mosque.mapLocation,
    sanitation: mosque.sanitation,
    electricityMeter: mosque.electricityMeter,
    alternativeEnergySource: mosque.alternativeEnergySource,
    waterSource: mosque.waterSource,
    briefDescription: mosque.briefDescription,
    totalCoveredArea: mosque.totalCoveredArea,
    totalLandArea: mosque.totalLandArea,
    numberOfFloors: mosque.numberOfFloors,
    openingDate: mosque.openingDate,
    constructionDate: mosque.constructionDate,
    mosqueDefinition: 1,
    mosqueClassification: 1,
    landDonorName: '',
    prayerCapacity: '',
    sourceFunds: 1,
    servicesSpecialNeeds: false,
    specialEntranceWomen: false,
  }
  selectedMosque.value = mosque
  editDialog.value = true
}

const openDeleteDialog = (mosque: Mosque) => {
  selectedMosque.value = mosque
  deleteDialog.value = true
}

const resetNewMosque = () => {
  newMosque.value = {
    name: '',
    regionId: '',
    officeId: '',
    fileNumber: '',
    definition: '',
    classification: '',
    unit: '',
    nearestLandmark: '',
    mapLocation: '',
    sanitation: '',
    electricityMeter: '',
    alternativeEnergySource: '',
    waterSource: '',
    briefDescription: '',
    totalCoveredArea: 0,
    totalLandArea: 0,
    numberOfFloors: 1,
    openingDate: '',
    constructionDate: '',
    mosqueDefinition: 1,
    mosqueClassification: 1,
    landDonorName: '',
    prayerCapacity: '',
    sourceFunds: 1,
    servicesSpecialNeeds: false,
    specialEntranceWomen: false,
  }
}

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadMosques()
}, { immediate: false })

// Watch for pagination changes
watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadMosques()
}, { immediate: false })

onMounted(() => {
  loadMosques()
  loadRegions()
  loadOffices()
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
        <span class="text-h5">إدارة المساجد</span>
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
            إضافة مسجد
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
              placeholder="البحث في المساجد..."
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
          :items="mosques"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          :options="options"
          class="text-no-wrap"
          show-select
          v-model="selectedRows"
        >
          <!-- Mosque name using ready-made template -->
          <template #item.mosqueName="{ item }">
            <div class="d-flex align-center">
              <VAvatar
                size="32"
                color="primary"
                variant="tonal"
              >
                {{ item.mosqueName.charAt(0).toUpperCase() }}
              </VAvatar>
              <div class="d-flex flex-column ms-3">
                <span class="d-block font-weight-medium text-truncate text-high-emphasis">{{ item.mosqueName }}</span>
                <small class="text-medium-emphasis">{{ item.briefDescription || 'لا يوجد وصف' }}</small>
              </div>
            </div>
          </template>

          <!-- File number using ready-made template -->
          <template #item.fileNumber="{ item }">
            <VChip
              v-if="item.fileNumber"
              color="secondary"
              variant="tonal"
              size="small"
            >
              {{ item.fileNumber }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا يوجد رقم ملف
            </span>
          </template>

          <!-- Region using ready-made template -->
          <template #item.region="{ item }">
            <VChip
              v-if="item.region"
              color="info"
              variant="tonal"
              size="small"
            >
              {{ item.region }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد منطقة
            </span>
          </template>

          <!-- Office using ready-made template -->
          <template #item.office="{ item }">
            <VChip
              v-if="item.office"
              color="success"
              variant="tonal"
              size="small"
            >
              {{ item.office }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا يوجد مكتب
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

    <!-- Add Mosque Dialog -->
    <VDialog
      v-model="dialog"
      max-width="800px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مسجد جديد</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addMosque">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.name"
                  label="اسم المسجد"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.fileNumber"
                  label="رقم الملف"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'رقم الملف مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="newMosque.regionId"
                  label="المنطقة"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  no-data-text="لا توجد مناطق متاحة"
                  required
                  :rules="[v => !!v || 'المنطقة مطلوبة']"
                  prepend-inner-icon="mdi-map"
                  placeholder="اختر المنطقة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="newMosque.officeId"
                  label="المكتب"
                  variant="outlined"
                  :items="offices"
                  item-title="name"
                  item-value="id"
                  :loading="officesLoading"
                  clearable
                  no-data-text="لا توجد مكاتب متاحة"
                  required
                  :rules="[v => !!v || 'المكتب مطلوب']"
                  prepend-inner-icon="mdi-office-building"
                  placeholder="اختر المكتب..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.unit"
                  label="الوحدة"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.nearestLandmark"
                  label="أقرب معلم"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.mapLocation"
                  label="موقع الخريطة"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.electricityMeter"
                  label="عداد الكهرباء"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.alternativeEnergySource"
                  label="مصدر الطاقة البديل"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.waterSource"
                  label="مصدر المياه"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.sanitation"
                  label="الصرف الصحي"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.totalLandArea"
                  label="إجمالي مساحة الأرض"
                  variant="outlined"
                  type="number"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.totalCoveredArea"
                  label="إجمالي المساحة المغطاة"
                  variant="outlined"
                  type="number"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.numberOfFloors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  min="1"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.openingDate"
                  label="تاريخ الافتتاح"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.constructionDate"
                  label="تاريخ البناء"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.mosqueDefinition"
                  label="تعريف المسجد"
                  variant="outlined"
                  :items="mosqueDefinitions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'تعريف المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.mosqueClassification"
                  label="تصنيف المسجد"
                  variant="outlined"
                  :items="mosqueClassifications"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'تصنيف المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.sourceFunds"
                  label="مصدر التمويل"
                  variant="outlined"
                  :items="sourceFundsOptions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'مصدر التمويل مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newMosque.briefDescription"
                  label="وصف مختصر"
                  variant="outlined"
                  rows="3"
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
            @click="addMosque"
          >
            حفظ
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Mosque Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="800px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المسجد</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateMosque">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.name"
                  label="اسم المسجد"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.fileNumber"
                  label="رقم الملف"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'رقم الملف مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="editMosque.regionId"
                  label="المنطقة"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  no-data-text="لا توجد مناطق متاحة"
                  required
                  :rules="[v => !!v || 'المنطقة مطلوبة']"
                  prepend-inner-icon="mdi-map"
                  placeholder="اختر المنطقة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="editMosque.officeId"
                  label="المكتب"
                  variant="outlined"
                  :items="offices"
                  item-title="name"
                  item-value="id"
                  :loading="officesLoading"
                  clearable
                  no-data-text="لا توجد مكاتب متاحة"
                  required
                  :rules="[v => !!v || 'المكتب مطلوب']"
                  prepend-inner-icon="mdi-office-building"
                  placeholder="اختر المكتب..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.unit"
                  label="الوحدة"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.nearestLandmark"
                  label="أقرب معلم"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.mapLocation"
                  label="موقع الخريطة"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.electricityMeter"
                  label="عداد الكهرباء"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.alternativeEnergySource"
                  label="مصدر الطاقة البديل"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.waterSource"
                  label="مصدر المياه"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.sanitation"
                  label="الصرف الصحي"
                  variant="outlined"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.totalLandArea"
                  label="إجمالي مساحة الأرض"
                  variant="outlined"
                  type="number"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.totalCoveredArea"
                  label="إجمالي المساحة المغطاة"
                  variant="outlined"
                  type="number"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.numberOfFloors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  min="1"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.openingDate"
                  label="تاريخ الافتتاح"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.constructionDate"
                  label="تاريخ البناء"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.mosqueDefinition"
                  label="تعريف المسجد"
                  variant="outlined"
                  :items="mosqueDefinitions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'تعريف المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.mosqueClassification"
                  label="تصنيف المسجد"
                  variant="outlined"
                  :items="mosqueClassifications"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'تصنيف المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.sourceFunds"
                  label="مصدر التمويل"
                  variant="outlined"
                  :items="sourceFundsOptions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => v !== null || 'مصدر التمويل مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editMosque.briefDescription"
                  label="وصف مختصر"
                  variant="outlined"
                  rows="3"
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
            @click="updateMosque"
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
          هل أنت متأكد من حذف المسجد "<strong>{{ selectedMosque?.mosqueName }}</strong>"؟
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
            @click="deleteMosque"
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