<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface Mosque {
  id: number
  name: string
  address: string
  cityId?: string
  cityName?: string
  regionId?: string
  regionName?: string
  imamName?: string
  capacity?: number
  description?: string
}

interface NewMosque {
  name: string
  address: string
  cityId: string
  regionId: string
  imamName: string
  capacity: number
  description: string
  // Required Building fields
  fileNumber: string
  definition: string
  classification: string
  officeId: string
  unit: string
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
  userId: string
  servicesSpecialNeeds: boolean
  specialEntranceWomen: boolean
  picturePath: string
  landDonorName: string
  prayerCapacity: string
  sourceFunds: string
  mosqueDefinition: string
  mosqueClassification: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'Mosque',
  },
})

const mosques = ref<Mosque[]>([])
const cities = ref<any[]>([])
const regions = ref<any[]>([])
const offices = ref<any[]>([])
const loading = ref(false)
const citiesLoading = ref(false)
const regionsLoading = ref(false)
const officesLoading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref<'success' | 'error' | 'warning' | 'info'>('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedMosque = ref<Mosque | null>(null)
const selectedRows = ref<Mosque[]>([])
const search = ref('')

const newMosque = ref<NewMosque>({
  name: '',
  address: '',
  cityId: '',
  regionId: '',
  imamName: '',
  capacity: 0,
  description: '',
  // Required Building fields
  fileNumber: '',
  definition: '',
  classification: '',
  officeId: '',
  unit: '',
  nearestLandmark: '',
  constructionDate: '',
  openingDate: '',
  mapLocation: '',
  totalLandArea: 0,
  totalCoveredArea: 0,
  numberOfFloors: 1,
  electricityMeter: '',
  alternativeEnergySource: '',
  waterSource: '',
  sanitation: '',
  briefDescription: '',
  userId: 'admin', // Default user ID
  servicesSpecialNeeds: false,
  specialEntranceWomen: false,
  picturePath: '',
  landDonorName: '',
  prayerCapacity: '',
  sourceFunds: 'Endowment',
  mosqueDefinition: 'Regular',
  mosqueClassification: 'Active'
})

const editMosque = ref<Mosque & {
  officeId?: string
  fileNumber?: string
  definition?: string
  classification?: string
  unit?: string
  nearestLandmark?: string
  constructionDate?: string
  openingDate?: string
  mapLocation?: string
  totalLandArea?: number
  totalCoveredArea?: number
  numberOfFloors?: number
  electricityMeter?: string
  alternativeEnergySource?: string
  waterSource?: string
  sanitation?: string
  briefDescription?: string
  userId?: string
  servicesSpecialNeeds?: boolean
  specialEntranceWomen?: boolean
  picturePath?: string
  landDonorName?: string
  prayerCapacity?: string
  sourceFunds?: string
  mosqueDefinition?: string
  mosqueClassification?: string
}>({
  id: 0,
  name: '',
  address: '',
  cityId: '',
  regionId: '',
  imamName: '',
  capacity: 0,
  description: '',
  officeId: '',
  fileNumber: '',
  definition: '',
  classification: '',
  unit: '',
  nearestLandmark: '',
  constructionDate: '',
  openingDate: '',
  mapLocation: '',
  totalLandArea: 0,
  totalCoveredArea: 0,
  numberOfFloors: 1,
  electricityMeter: '',
  alternativeEnergySource: '',
  waterSource: '',
  sanitation: '',
  briefDescription: '',
  userId: 'admin',
  servicesSpecialNeeds: false,
  specialEntranceWomen: false,
  picturePath: '',
  landDonorName: '',
  prayerCapacity: '',
  sourceFunds: 'Endowment',
  mosqueDefinition: 'Regular',
  mosqueClassification: 'Active'
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
    title: 'المسجد',
    key: 'name',
  },
  {
    title: 'العنوان',
    key: 'address',
  },
  {
    title: 'المدينة',
    key: 'cityName',
  },
  {
    title: 'المنطقة',
    key: 'regionName',
  },
  {
    title: 'الإمام',
    key: 'imamName',
  },
  {
    title: 'السعة',
    key: 'capacity',
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

const loadCities = async () => {
  citiesLoading.value = true
  try {
    const response = await $api('/City/filter?PageSize=100')
    cities.value = response.data.items || []
  } catch (error) {
    console.error('Error loading cities:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المدن'
    alertType.value = 'error'
    showAlert.value = true
  } finally {
    citiesLoading.value = false
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

const addMosque = async () => {
  try {
    loading.value = true
    
    // Ensure we have valid office and region IDs
    let officeId = newMosque.value.officeId
    let regionId = newMosque.value.regionId
    
    // If no office selected, use the first available office
    if (!officeId && offices.value.length > 0) {
      officeId = offices.value[0].id
    }
    
    // If no region selected, use the first available region
    if (!regionId && regions.value.length > 0) {
      regionId = regions.value[0].id
    }
    
    // If still no valid IDs, show error
    if (!officeId || !regionId) {
      showAlert.value = true
      alertMessage.value = 'يرجى التأكد من وجود مكاتب ومناطق في النظام'
      alertType.value = 'error'
      return
    }
    
    const mosqueData = {
      name: newMosque.value.name,
      fileNumber: newMosque.value.fileNumber || `MOSQUE-${Date.now()}`,
      definition: newMosque.value.definition || newMosque.value.description,
      classification: newMosque.value.classification || 'Mosque',
      officeId: officeId,
      unit: newMosque.value.unit || 'Unit 1',
      regionId: regionId,
      nearestLandmark: newMosque.value.nearestLandmark || newMosque.value.address,
      constructionDate: newMosque.value.constructionDate || new Date().toISOString().split('T')[0],
      openingDate: newMosque.value.openingDate || new Date().toISOString().split('T')[0],
      mapLocation: newMosque.value.mapLocation || '0,0',
      totalLandArea: newMosque.value.totalLandArea || 100,
      totalCoveredArea: newMosque.value.totalCoveredArea || 80,
      numberOfFloors: newMosque.value.numberOfFloors || 1,
      electricityMeter: newMosque.value.electricityMeter || 'METER-001',
      alternativeEnergySource: newMosque.value.alternativeEnergySource || 'Solar',
      waterSource: newMosque.value.waterSource || 'Municipal',
      sanitation: newMosque.value.sanitation || 'Connected',
      briefDescription: newMosque.value.briefDescription || newMosque.value.description,
      userId: newMosque.value.userId || 'admin',
      servicesSpecialNeeds: newMosque.value.servicesSpecialNeeds || false,
      specialEntranceWomen: newMosque.value.specialEntranceWomen || false,
      picturePath: newMosque.value.picturePath || '',
      landDonorName: newMosque.value.landDonorName || 'Anonymous',
      prayerCapacity: newMosque.value.prayerCapacity || newMosque.value.capacity.toString(),
      sourceFunds: newMosque.value.sourceFunds || 'Endowment',
      mosqueDefinition: newMosque.value.mosqueDefinition || 'Regular',
      mosqueClassification: newMosque.value.mosqueClassification || 'Active'
    }

    console.log('Adding mosque with data:', mosqueData)
    console.log('Available offices:', offices.value)
    console.log('Available regions:', regions.value)
    
    const response = await $api('/api/Mosque', {
      method: 'POST',
      body: mosqueData
    })

    console.log('Add response:', response)

    if (response.isSuccess) {
      showAlert.value = true
      alertMessage.value = 'تم إضافة المسجد بنجاح'
      alertType.value = 'success'
      
      // Reset form
      newMosque.value = {
        name: '',
        address: '',
        cityId: '',
        regionId: '',
        imamName: '',
        capacity: 0,
        description: '',
        fileNumber: '',
        definition: '',
        classification: '',
        officeId: '',
        unit: '',
        nearestLandmark: '',
        constructionDate: '',
        openingDate: '',
        mapLocation: '',
        totalLandArea: 0,
        totalCoveredArea: 0,
        numberOfFloors: 1,
        electricityMeter: '',
        alternativeEnergySource: '',
        waterSource: '',
        sanitation: '',
        briefDescription: '',
        userId: 'admin',
        servicesSpecialNeeds: false,
        specialEntranceWomen: false,
        picturePath: '',
        landDonorName: '',
        prayerCapacity: '',
        sourceFunds: 'Endowment',
        mosqueDefinition: 'Regular',
        mosqueClassification: 'Active'
      }
      
      // Refresh data
      await loadMosques()
      await loadCities()
      await loadRegions()
      
      // Close dialog
      dialog.value = false
    } else {
      showAlert.value = true
      alertMessage.value = response.message || 'حدث خطأ أثناء إضافة المسجد'
      alertType.value = 'error'
    }
  } catch (error) {
    console.error('Error adding mosque:', error)
    showAlert.value = true
    alertMessage.value = 'حدث خطأ في الشبكة أثناء إضافة المسجد'
    alertType.value = 'error'
  } finally {
    loading.value = false
  }
}

const updateMosque = async () => {
  try {
    const response = await $api(`/Mosque/${editMosque.value.id}`, {
      method: 'PUT',
      body: {
        id: editMosque.value.id,
        name: editMosque.value.name,
        address: editMosque.value.address,
        cityId: editMosque.value.cityId,
        regionId: editMosque.value.regionId,
        officeId: editMosque.value.officeId,
        imamName: editMosque.value.imamName,
        capacity: editMosque.value.capacity,
        description: editMosque.value.description,
        fileNumber: editMosque.value.fileNumber,
        definition: editMosque.value.definition,
        classification: editMosque.value.classification,
        unit: editMosque.value.unit,
        nearestLandmark: editMosque.value.nearestLandmark,
        constructionDate: editMosque.value.constructionDate,
        openingDate: editMosque.value.openingDate,
        mapLocation: editMosque.value.mapLocation,
        totalLandArea: editMosque.value.totalLandArea,
        totalCoveredArea: editMosque.value.totalCoveredArea,
        numberOfFloors: editMosque.value.numberOfFloors,
        electricityMeter: editMosque.value.electricityMeter,
        alternativeEnergySource: editMosque.value.alternativeEnergySource,
        waterSource: editMosque.value.waterSource,
        sanitation: editMosque.value.sanitation,
        briefDescription: editMosque.value.briefDescription,
        userId: editMosque.value.userId,
        servicesSpecialNeeds: editMosque.value.servicesSpecialNeeds,
        specialEntranceWomen: editMosque.value.specialEntranceWomen,
        picturePath: editMosque.value.picturePath,
        landDonorName: editMosque.value.landDonorName,
        prayerCapacity: editMosque.value.prayerCapacity,
        sourceFunds: editMosque.value.sourceFunds,
        mosqueDefinition: editMosque.value.mosqueDefinition,
        mosqueClassification: editMosque.value.mosqueClassification
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
    console.log('Attempting to delete mosque:', selectedMosque.value.id)
    const response = await $api(`/Mosque/${selectedMosque.value.id}`, {
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
      $api(`/Mosque/${mosque.id}`, { method: 'DELETE' })
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
  editMosque.value = { ...mosque }
  editDialog.value = true
}

const openDeleteDialog = (mosque: Mosque) => {
  selectedMosque.value = mosque
  deleteDialog.value = true
}

const resetNewMosque = () => {
  newMosque.value = {
    name: '',
    address: '',
    cityId: '',
    regionId: '',
    imamName: '',
    capacity: 0,
    description: '',
    fileNumber: '',
    definition: '',
    classification: '',
    officeId: '',
    unit: '',
    nearestLandmark: '',
    constructionDate: '',
    openingDate: '',
    mapLocation: '',
    totalLandArea: 0,
    totalCoveredArea: 0,
    numberOfFloors: 1,
    electricityMeter: '',
    alternativeEnergySource: '',
    waterSource: '',
    sanitation: '',
    briefDescription: '',
    userId: 'admin',
    servicesSpecialNeeds: false,
    specialEntranceWomen: false,
    picturePath: '',
    landDonorName: '',
    prayerCapacity: '',
    sourceFunds: 'Endowment',
    mosqueDefinition: 'Regular',
    mosqueClassification: 'Active'
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

const fetchOffices = async () => {
  try {
    officesLoading.value = true
    console.log('Fetching offices...')
    const response = await $api('/api/Office')
    console.log('Offices response:', response)
    if (response.isSuccess) {
      offices.value = response.data || []
      console.log('Loaded offices:', offices.value)
    } else {
      console.error('Failed to fetch offices:', response.message)
    }
  } catch (error) {
    console.error('Error fetching offices:', error)
  } finally {
    officesLoading.value = false
  }
}

const fetchRegions = async () => {
  try {
    regionsLoading.value = true
    console.log('Fetching regions...')
    const response = await $api('/api/Region')
    console.log('Regions response:', response)
    if (response.isSuccess) {
      regions.value = response.data || []
      console.log('Loaded regions:', regions.value)
    } else {
      console.error('Failed to fetch regions:', response.message)
    }
  } catch (error) {
    console.error('Error fetching regions:', error)
  } finally {
    regionsLoading.value = false
  }
}

onMounted(async () => {
  await Promise.all([
    loadMosques(),
    loadCities(),
    fetchOffices(),
    fetchRegions()
  ])
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

          <!-- Address using ready-made template -->
          <template #item.address="{ item }">
            <span class="text-medium-emphasis">{{ item.address || 'لا يوجد عنوان' }}</span>
          </template>

          <!-- City using ready-made template -->
          <template #item.cityName="{ item }">
            <VChip
              v-if="item.cityName"
              color="secondary"
              variant="tonal"
              size="small"
            >
              {{ item.cityName }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد مدينة
            </span>
          </template>

          <!-- Region using ready-made template -->
          <template #item.regionName="{ item }">
            <VChip
              v-if="item.regionName"
              color="info"
              variant="tonal"
              size="small"
            >
              {{ item.regionName }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد منطقة
            </span>
          </template>

          <!-- Imam using ready-made template -->
          <template #item.imamName="{ item }">
            <span class="text-medium-emphasis">{{ item.imamName || 'غير محدد' }}</span>
          </template>

          <!-- Capacity using ready-made template -->
          <template #item.capacity="{ item }">
            <VChip
              v-if="item.capacity"
              color="success"
              variant="tonal"
              size="small"
            >
              {{ item.capacity }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              غير محدد
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
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مسجد جديد</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addMosque">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newMosque.name"
                  label="اسم المسجد"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newMosque.address"
                  label="العنوان"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'العنوان مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="newMosque.cityId"
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
                  prepend-inner-icon="mdi-map-marker"
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
                  v-model="newMosque.imamName"
                  label="اسم الإمام"
                  variant="outlined"
                  placeholder="أدخل اسم الإمام..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.capacity"
                  label="السعة"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل السعة..."
                  min="0"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.fileNumber"
                  label="رقم الملف"
                  variant="outlined"
                  placeholder="أدخل رقم الملف..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.unit"
                  label="الوحدة"
                  variant="outlined"
                  placeholder="أدخل الوحدة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.nearestLandmark"
                  label="أقرب معلم"
                  variant="outlined"
                  placeholder="أدخل أقرب معلم..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.mapLocation"
                  label="موقع الخريطة"
                  variant="outlined"
                  placeholder="أدخل إحداثيات الموقع..."
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
                <VTextField
                  v-model="newMosque.openingDate"
                  label="تاريخ الافتتاح"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.totalLandArea"
                  label="إجمالي مساحة الأرض (م²)"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل مساحة الأرض..."
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.totalCoveredArea"
                  label="إجمالي المساحة المغطاة (م²)"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل المساحة المغطاة..."
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.numberOfFloors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل عدد الطوابق..."
                  min="1"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.electricityMeter"
                  label="عداد الكهرباء"
                  variant="outlined"
                  placeholder="أدخل رقم عداد الكهرباء..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.alternativeEnergySource"
                  label="مصدر الطاقة البديل"
                  variant="outlined"
                  placeholder="مثل: الطاقة الشمسية..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.waterSource"
                  label="مصدر المياه"
                  variant="outlined"
                  placeholder="مثل: البلدية، بئر..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.sanitation"
                  label="الصرف الصحي"
                  variant="outlined"
                  placeholder="مثل: متصل بالشبكة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.landDonorName"
                  label="اسم متبرع الأرض"
                  variant="outlined"
                  placeholder="أدخل اسم متبرع الأرض..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.prayerCapacity"
                  label="سعة الصلاة"
                  variant="outlined"
                  placeholder="أدخل سعة الصلاة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.sourceFunds"
                  label="مصدر الأموال"
                  variant="outlined"
                  :items="[
                    { title: 'الأوقاف', value: 'Endowment' },
                    { title: 'تبرعات', value: 'Donations' },
                    { title: 'حكومي', value: 'Government' },
                    { title: 'أخرى', value: 'Other' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.mosqueDefinition"
                  label="تعريف المسجد"
                  variant="outlined"
                  :items="[
                    { title: 'عادي', value: 'Regular' },
                    { title: 'جامع', value: 'Friday' },
                    { title: 'مصلى', value: 'Prayer' },
                    { title: 'مؤقت', value: 'Temporary' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newMosque.mosqueClassification"
                  label="تصنيف المسجد"
                  variant="outlined"
                  :items="[
                    { title: 'نشط', value: 'Active' },
                    { title: 'قيد الإنشاء', value: 'UnderConstruction' },
                    { title: 'مغلق', value: 'Closed' },
                    { title: 'قيد الصيانة', value: 'UnderMaintenance' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.picturePath"
                  label="مسار الصورة"
                  variant="outlined"
                  placeholder="أدخل مسار الصورة..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newMosque.definition"
                  label="التعريف"
                  variant="outlined"
                  rows="2"
                  placeholder="أدخل تعريف المسجد..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newMosque.briefDescription"
                  label="الوصف المختصر"
                  variant="outlined"
                  rows="2"
                  placeholder="أدخل وصف مختصر..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newMosque.description"
                  label="الوصف التفصيلي"
                  variant="outlined"
                  rows="3"
                  placeholder="أدخل وصف تفصيلي للمسجد..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VCheckbox
                  v-model="newMosque.servicesSpecialNeeds"
                  label="خدمات ذوي الاحتياجات الخاصة"
                  color="primary"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VCheckbox
                  v-model="newMosque.specialEntranceWomen"
                  label="مدخل خاص للنساء"
                  color="primary"
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
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المسجد</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateMosque">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editMosque.name"
                  label="اسم المسجد"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المسجد مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editMosque.address"
                  label="العنوان"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'العنوان مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VAutocomplete
                  v-model="editMosque.cityId"
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
                  prepend-inner-icon="mdi-map-marker"
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
                  v-model="editMosque.imamName"
                  label="اسم الإمام"
                  variant="outlined"
                  placeholder="أدخل اسم الإمام..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.capacity"
                  label="السعة"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل السعة..."
                  min="0"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.fileNumber"
                  label="رقم الملف"
                  variant="outlined"
                  placeholder="أدخل رقم الملف..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.unit"
                  label="الوحدة"
                  variant="outlined"
                  placeholder="أدخل الوحدة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.nearestLandmark"
                  label="أقرب معلم"
                  variant="outlined"
                  placeholder="أدخل أقرب معلم..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.mapLocation"
                  label="موقع الخريطة"
                  variant="outlined"
                  placeholder="أدخل إحداثيات الموقع..."
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
                <VTextField
                  v-model="editMosque.openingDate"
                  label="تاريخ الافتتاح"
                  variant="outlined"
                  type="date"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.totalLandArea"
                  label="إجمالي مساحة الأرض (م²)"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل مساحة الأرض..."
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.totalCoveredArea"
                  label="إجمالي المساحة المغطاة (م²)"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل المساحة المغطاة..."
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.numberOfFloors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  placeholder="أدخل عدد الطوابق..."
                  min="1"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.electricityMeter"
                  label="عداد الكهرباء"
                  variant="outlined"
                  placeholder="أدخل رقم عداد الكهرباء..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.alternativeEnergySource"
                  label="مصدر الطاقة البديل"
                  variant="outlined"
                  placeholder="مثل: الطاقة الشمسية..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.waterSource"
                  label="مصدر المياه"
                  variant="outlined"
                  placeholder="مثل: البلدية، بئر..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.sanitation"
                  label="الصرف الصحي"
                  variant="outlined"
                  placeholder="مثل: متصل بالشبكة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.landDonorName"
                  label="اسم متبرع الأرض"
                  variant="outlined"
                  placeholder="أدخل اسم متبرع الأرض..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.prayerCapacity"
                  label="سعة الصلاة"
                  variant="outlined"
                  placeholder="أدخل سعة الصلاة..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.sourceFunds"
                  label="مصدر الأموال"
                  variant="outlined"
                  :items="[
                    { title: 'الأوقاف', value: 'Endowment' },
                    { title: 'تبرعات', value: 'Donations' },
                    { title: 'حكومي', value: 'Government' },
                    { title: 'أخرى', value: 'Other' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.mosqueDefinition"
                  label="تعريف المسجد"
                  variant="outlined"
                  :items="[
                    { title: 'عادي', value: 'Regular' },
                    { title: 'جامع', value: 'Friday' },
                    { title: 'مصلى', value: 'Prayer' },
                    { title: 'مؤقت', value: 'Temporary' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editMosque.mosqueClassification"
                  label="تصنيف المسجد"
                  variant="outlined"
                  :items="[
                    { title: 'نشط', value: 'Active' },
                    { title: 'قيد الإنشاء', value: 'UnderConstruction' },
                    { title: 'مغلق', value: 'Closed' },
                    { title: 'قيد الصيانة', value: 'UnderMaintenance' }
                  ]"
                  item-title="title"
                  item-value="value"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.picturePath"
                  label="مسار الصورة"
                  variant="outlined"
                  placeholder="أدخل مسار الصورة..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editMosque.definition"
                  label="التعريف"
                  variant="outlined"
                  rows="2"
                  placeholder="أدخل تعريف المسجد..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editMosque.briefDescription"
                  label="الوصف المختصر"
                  variant="outlined"
                  rows="2"
                  placeholder="أدخل وصف مختصر..."
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editMosque.description"
                  label="الوصف التفصيلي"
                  variant="outlined"
                  rows="3"
                  placeholder="أدخل وصف تفصيلي للمسجد..."
                />
              </VCol>
              <VCol cols="12" md="6">
                <VCheckbox
                  v-model="editMosque.servicesSpecialNeeds"
                  label="خدمات ذوي الاحتياجات الخاصة"
                  color="primary"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VCheckbox
                  v-model="editMosque.specialEntranceWomen"
                  label="مدخل خاص للنساء"
                  color="primary"
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
          هل أنت متأكد من حذف المسجد "<strong>{{ selectedMosque?.name }}</strong>"؟
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

