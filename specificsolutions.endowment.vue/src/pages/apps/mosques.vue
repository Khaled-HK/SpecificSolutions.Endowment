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
  picturePath: string
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
  picturePath: string
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

// دالة للحصول على التاريخ الحالي بصيغة YYYY-MM-DD
const getCurrentDate = (): string => {
  const today = new Date();
  return today.toISOString().split('T')[0];
}

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref<'success' | 'error' | 'warning' | 'info'>('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const buildingDetailsDialog = ref(false)
const addBuildingDetailDialog = ref(false)
const selectedMosque = ref<Mosque | null>(null)
const selectedRows = ref<Mosque[]>([])
const search = ref('')

// Building Details variables
const buildingDetails = ref<any[]>([])
const buildingDetailsLoading = ref(false)
const buildingDetailsSearch = ref('')
const addBuildingDetailForm = ref()
const editBuildingDetailDialog = ref(false)
const editBuildingDetailForm = ref()
const editingBuildingDetail = ref({
  id: '',
  name: '',
  floors: 1,
  withinMosqueArea: true,
  buildingCategory: 'Endowment'
})
const newBuildingDetail = ref({
  name: '',
  floors: 1,
  withinMosqueArea: true,
  buildingCategory: 'Endowment'
})

const buildingCategoryOptions = [
  { value: 'Facility', label: 'منشأة' },
  { value: 'Endowment', label: 'وقف' }
]

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
  openingDate: getCurrentDate(), // تعبئة التاريخ الحالي
  constructionDate: getCurrentDate(), // تعبئة التاريخ الحالي
  mosqueDefinition: 0, // تغيير من 1 إلى 0
  mosqueClassification: 0, // تغيير من 1 إلى 0
  landDonorName: '',
  prayerCapacity: '',
  sourceFunds: 0, // تغيير من 1 إلى 0
  servicesSpecialNeeds: false,
  specialEntranceWomen: false,
  picturePath: '',
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
  picturePath: '',
})

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Data table headers
const headers = [
  { title: 'اسم المسجد', key: 'mosqueName', sortable: true },
  { title: 'رقم الملف', key: 'fileNumber', sortable: true },
  { title: 'المنطقة', key: 'region', sortable: true },
  { title: 'المكتب', key: 'office', sortable: true },
  { title: 'الإجراءات', key: 'actions', sortable: false, width: '120px' },
]

// Building Details table headers
const buildingDetailHeaders = [
  { title: 'اسم المبنى', key: 'name', sortable: true },
  { title: 'عدد الطوابق', key: 'floors', sortable: true },
  { title: 'نوع المبنى', key: 'buildingCategory', sortable: true },
  { title: 'الموقع', key: 'withinMosqueArea', sortable: true },
  { title: 'الإجراءات', key: 'actions', sortable: false, width: '80px' },
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
    // معالجة التواريخ - إجبارية
    const processDate = (dateString: string): string => {
      if (!dateString || dateString.trim() === '') {
        // إذا كان التاريخ فارغ، استخدم التاريخ الحالي
        return getCurrentDate();
      }
      try {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) {
          // إذا كان التاريخ غير صحيح، استخدم التاريخ الحالي
          return getCurrentDate();
        }
        return date.toISOString().split('T')[0]; // YYYY-MM-DD
      } catch {
        // في حالة الخطأ، استخدم التاريخ الحالي
        return getCurrentDate();
      }
    };

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
        openingDate: processDate(newMosque.value.openingDate),
        constructionDate: processDate(newMosque.value.constructionDate),
        mosqueDefinition: newMosque.value.mosqueDefinition,
        mosqueClassification: newMosque.value.mosqueClassification,
        landDonorName: newMosque.value.landDonorName,
        prayerCapacity: newMosque.value.prayerCapacity,
        sourceFunds: newMosque.value.sourceFunds,
        servicesSpecialNeeds: newMosque.value.servicesSpecialNeeds,
        specialEntranceWomen: newMosque.value.specialEntranceWomen,
        picturePath: newMosque.value.picturePath,
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
    // معالجة التواريخ - إجبارية
    const processDate = (dateString: string): string => {
      if (!dateString || dateString.trim() === '') {
        // إذا كان التاريخ فارغ، استخدم التاريخ الحالي
        return getCurrentDate();
      }
      try {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) {
          // إذا كان التاريخ غير صحيح، استخدم التاريخ الحالي
          return getCurrentDate();
        }
        return date.toISOString().split('T')[0]; // YYYY-MM-DD
      } catch {
        // في حالة الخطأ، استخدم التاريخ الحالي
        return getCurrentDate();
      }
    };

    // طباعة البيانات المرسلة للتأكد
    console.log('البيانات المرسلة للتحديث:', {
      id: selectedMosque.value?.mosqueID,
      name: editMosque.value.name,
      regionId: editMosque.value.regionId,
      officeId: editMosque.value.officeId,
      openingDate: processDate(editMosque.value.openingDate),
      constructionDate: processDate(editMosque.value.constructionDate),
      // ... باقي الحقول
    });
    
    // طباعة الـ ID المرسل للتأكد
    console.log('ID المرسل:', selectedMosque.value?.mosqueID);
    console.log('نوع الـ ID:', typeof selectedMosque.value?.mosqueID);
    
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
        openingDate: processDate(editMosque.value.openingDate),
        constructionDate: processDate(editMosque.value.constructionDate),
        mosqueDefinition: editMosque.value.mosqueDefinition,
        mosqueClassification: editMosque.value.mosqueClassification,
        landDonorName: editMosque.value.landDonorName,
        prayerCapacity: editMosque.value.prayerCapacity,
        sourceFunds: editMosque.value.sourceFunds,
        servicesSpecialNeeds: editMosque.value.servicesSpecialNeeds,
        specialEntranceWomen: editMosque.value.specialEntranceWomen,
        picturePath: editMosque.value.picturePath,
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

const openEditDialog = async (mosque: Mosque) => {
  // طباعة بيانات المسجد القادمة من API
  console.log('بيانات المسجد من API:', mosque);
  
  // تأكد من تحميل القوائم قبل التعديل
  if (regions.value.length === 0) await loadRegions();
  if (offices.value.length === 0) await loadOffices();

  // البحث عن regionId بناءً على اسم المنطقة
  const regionId = regions.value.find(r => r.name === mosque.region)?.id || '';
  
  // البحث عن officeId بناءً على اسم المكتب
  const officeId = offices.value.find(o => o.name === mosque.office)?.id || '';
  
  console.log('تم العثور على:', { regionId, officeId, regionName: mosque.region, officeName: mosque.office });

  // تحويل التواريخ من ISO string إلى YYYY-MM-DD
  const formatDate = (dateString: string | null | undefined): string => {
    if (!dateString) return '';
    try {
      const date = new Date(dateString);
      return date.toISOString().split('T')[0]; // YYYY-MM-DD
    } catch {
      return '';
    }
  };

  // تعبئة بيانات المسجد للتعديل
  editMosque.value = {
    name: mosque.mosqueName,
    regionId: regionId,
    officeId: officeId,
    fileNumber: mosque.fileNumber,
    definition: mosque.definition || '',
    classification: mosque.classification || '',
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
    openingDate: formatDate(mosque.openingDate),
    constructionDate: formatDate(mosque.constructionDate),
    mosqueDefinition: mosque.mosqueDefinition || 0, // استخدام القيمة من API أو 0 كافتراضي
    mosqueClassification: mosque.mosqueClassification || 0, // استخدام القيمة من API أو 0 كافتراضي
    landDonorName: mosque.landDonorName || '',
    prayerCapacity: mosque.prayerCapacity || '',
    sourceFunds: mosque.sourceFunds || 0, // استخدام القيمة من API أو 0 كافتراضي
    servicesSpecialNeeds: mosque.servicesSpecialNeeds || false,
    specialEntranceWomen: mosque.specialEntranceWomen || false,
    picturePath: mosque.picturePath || '',
  };
  selectedMosque.value = mosque;
  editDialog.value = true;
  // طباعة القيم للتأكد
  console.log('editMosque.value عند التعديل:', editMosque.value);
}

const openBuildingDetailsDialog = async (mosque: Mosque) => {
  selectedMosque.value = mosque
  buildingDetailsDialog.value = true
  await loadBuildingDetails(mosque.mosqueID)
}

const reloadBuildingDetails = async () => {
  if (selectedMosque.value) {
    await loadBuildingDetails(selectedMosque.value.mosqueID)
  }
}

const loadBuildingDetails = async (mosqueId: string) => {
  buildingDetailsLoading.value = true
  try {
    // نحتاج إلى الحصول على BuildingId من Mosque أولاً
    const mosqueResponse = await $api(`/Mosque/${mosqueId}`)
    if (mosqueResponse && mosqueResponse.data) {
      const buildingId = mosqueResponse.data.buildingId
      if (buildingId) {
        // استخدام filter endpoint مع BuildingId
        const response = await $api(`/BuildingDetail/filter?BuildingId=${buildingId}&PageSize=100&PageNumber=1`)
        if (response && response.data) {
          buildingDetails.value = response.data.items || []
        } else {
          buildingDetails.value = []
        }
      } else {
        buildingDetails.value = []
      }
    } else {
      buildingDetails.value = []
    }
  } catch (error) {
    console.error('Error loading building details:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل تفاصيل المبنى'
    alertType.value = 'error'
    showAlert.value = true
    buildingDetails.value = []
  } finally {
    buildingDetailsLoading.value = false
  }
}

const openDeleteDialog = (mosque: Mosque) => {
  selectedMosque.value = mosque
  deleteDialog.value = true
}

const openAddDialog = () => {
  // تعبئة التاريخ الحالي تلقائياً
  newMosque.value.openingDate = getCurrentDate()
  newMosque.value.constructionDate = getCurrentDate()
  dialog.value = true
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
    openingDate: getCurrentDate(), // تعبئة التاريخ الحالي
    constructionDate: getCurrentDate(), // تعبئة التاريخ الحالي
    mosqueDefinition: 0,
    mosqueClassification: 0,
    landDonorName: '',
    prayerCapacity: '',
    sourceFunds: 0,
    servicesSpecialNeeds: false,
    specialEntranceWomen: false,
    picturePath: '',
  }
}

const addBuildingDetail = async () => {
  if (!addBuildingDetailForm.value) return

  const isValid = await addBuildingDetailForm.value.validate()
  if (!isValid) {
    alertMessage.value = 'يرجى ملء جميع الحقول المطلوبة'
    alertType.value = 'warning'
    showAlert.value = true
    return
  }

  try {
    // نحتاج إلى الحصول على BuildingId من Mosque أولاً
    const mosqueResponse = await $api(`/Mosque/${selectedMosque.value?.mosqueID}`)
    if (!mosqueResponse || !mosqueResponse.data) {
      alertMessage.value = 'لم يتم العثور على المسجد'
      alertType.value = 'error'
      showAlert.value = true
      return
    }

    const buildingId = mosqueResponse.data.buildingId
    if (!buildingId) {
      alertMessage.value = 'لم يتم العثور على المبنى المرتبط بالمسجد'
      alertType.value = 'error'
      showAlert.value = true
      return
    }

    const response = await $api('/BuildingDetail', {
      method: 'POST',
      body: {
        name: newBuildingDetail.value.name,
        buildingId: buildingId, // استخدام BuildingId
        floors: newBuildingDetail.value.floors,
        withinMosqueArea: newBuildingDetail.value.withinMosqueArea,
        buildingCategory: newBuildingDetail.value.buildingCategory
      },
    })
    
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة تفصيل المبنى'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    // Close popup dialog and reload data
    closeAddBuildingDetailDialog()
    
    // Reload building details
    await reloadBuildingDetails()
    
    alertMessage.value = 'تم إضافة تفصيل المبنى بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding building detail:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة تفصيل المبنى'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteBuildingDetail = async (buildingDetailId: string) => {
  try {
    const response = await $api(`/BuildingDetail/${buildingDetailId}`, {
      method: 'DELETE',
    })
    
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف تفصيل المبنى'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    if (selectedMosque.value) {
      await reloadBuildingDetails()
    }
    
    alertMessage.value = 'تم حذف تفصيل المبنى بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting building detail:', error)
    alertMessage.value = 'حدث خطأ أثناء حذف تفصيل المبنى'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openAddBuildingDetailDialog = () => {
  addBuildingDetailDialog.value = true
}

const openEditBuildingDetailDialog = (buildingDetail: any) => {
  editingBuildingDetail.value = {
    id: buildingDetail.id,
    name: buildingDetail.name,
    floors: parseInt(buildingDetail.floors) || 1, // Ensure proper number conversion
    withinMosqueArea: buildingDetail.withinMosqueArea,
    buildingCategory: buildingDetail.buildingCategory
  }
  console.log('Editing building detail - Original item:', buildingDetail)
  console.log('Editing building detail - Floors value:', buildingDetail.floors, 'Type:', typeof buildingDetail.floors)
  console.log('Editing building detail - Form after population:', editingBuildingDetail.value)
  editBuildingDetailDialog.value = true
}

const closeEditBuildingDetailDialog = () => {
  editBuildingDetailDialog.value = false
  // Reset form
  editingBuildingDetail.value = {
    id: '',
    name: '',
    floors: 1,
    withinMosqueArea: true,
    buildingCategory: 'Endowment'
  }
}

const updateBuildingDetail = async () => {
  try {
    const response = await $api(`/BuildingDetail/${editingBuildingDetail.value.id}`, {
      method: 'PUT',
      body: {
        id: editingBuildingDetail.value.id,
        name: editingBuildingDetail.value.name,
        floors: editingBuildingDetail.value.floors,
        withinMosqueArea: editingBuildingDetail.value.withinMosqueArea,
        buildingCategory: editingBuildingDetail.value.buildingCategory
      },
    })
    
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تعديل تفصيل المبنى'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    // Close popup dialog and reload data
    closeEditBuildingDetailDialog()
    
    // Reload building details
    await reloadBuildingDetails()
    
    alertMessage.value = 'تم تعديل تفصيل المبنى بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating building detail:', error)
    alertMessage.value = 'حدث خطأ أثناء تعديل تفصيل المبنى'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const closeAddBuildingDetailDialog = () => {
  addBuildingDetailDialog.value = false
  // Reset form
  newBuildingDetail.value = {
    name: '',
    floors: 1,
    withinMosqueArea: true,
    buildingCategory: 'Endowment'
  }
}

const searchBuildingDetails = async () => {
  if (!selectedMosque.value) return
  
  buildingDetailsLoading.value = true
  try {
    const mosqueResponse = await $api(`/Mosque/${selectedMosque.value.mosqueID}`)
    if (mosqueResponse && mosqueResponse.data) {
      const buildingId = mosqueResponse.data.buildingId
      if (buildingId) {
        // استخدام filter endpoint مع BuildingId و SearchTerm
        const response = await $api(`/BuildingDetail/filter?BuildingId=${buildingId}&SearchTerm=${buildingDetailsSearch.value}&PageSize=100&PageNumber=1`)
        if (response && response.data) {
          buildingDetails.value = response.data.items || []
        } else {
          buildingDetails.value = []
        }
      } else {
        buildingDetails.value = []
      }
    } else {
      buildingDetails.value = []
    }
  } catch (error) {
    console.error('Error searching building details:', error)
    alertMessage.value = 'حدث خطأ أثناء البحث في تفاصيل المبنى'
    alertType.value = 'error'
    showAlert.value = true
    buildingDetails.value = []
  } finally {
    buildingDetailsLoading.value = false
  }
}

const clearBuildingDetailsSearch = async () => {
  buildingDetailsSearch.value = ''
  await reloadBuildingDetails()
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
            @click="openAddDialog"
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
              <VTooltip text="إدارة تفاصيل المبنى (منزل الإمام، المصلى، دورة المياه...)" location="top">
                <template #activator="{ props }">
                  <IconBtn @click="openBuildingDetailsDialog(item)" color="info" v-bind="props">
                    <VIcon icon="tabler-building" />
                  </IconBtn>
                </template>
              </VTooltip>
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
                  required
                  :rules="[v => !!v || 'تاريخ الافتتاح مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newMosque.constructionDate"
                  label="تاريخ البناء"
                  variant="outlined"
                  type="date"
                  required
                  :rules="[v => !!v || 'تاريخ البناء مطلوب']"
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
                  required
                  :rules="[v => !!v || 'تاريخ الافتتاح مطلوب']"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editMosque.constructionDate"
                  label="تاريخ البناء"
                  variant="outlined"
                  type="date"
                  required
                  :rules="[v => !!v || 'تاريخ البناء مطلوب']"
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

    <!-- Building Details Dialog -->
    <VDialog
      v-model="buildingDetailsDialog"
      max-width="1000px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">
          تفاصيل المبنى - {{ selectedMosque?.mosqueName }}
        </VCardTitle>
        <VCardText>
          <!-- Building Details Table -->
          <VCard variant="outlined">
            <VCardTitle class="text-h6 d-flex justify-space-between align-center">
              تفاصيل المبنى الحالية
              <VBtn
                color="primary"
                variant="flat"
                size="small"
                @click="openAddBuildingDetailDialog"
                prepend-icon="tabler-plus"
                :disabled="buildingDetailsLoading"
              >
                إضافة تفصيل جديد
              </VBtn>
            </VCardTitle>
            <VCardText>
              <!-- Search Bar -->
              <VRow class="mb-4">
                <VCol cols="12" md="6">
                  <VTextField
                    v-model="buildingDetailsSearch"
                    label="البحث في تفاصيل المبنى"
                    variant="outlined"
                    density="compact"
                    prepend-inner-icon="tabler-search"
                    clearable
                    @update:model-value="searchBuildingDetails"
                    @click:clear="clearBuildingDetailsSearch"
                    placeholder="ابحث عن اسم المبنى..."
                  />
                </VCol>
                <VCol cols="12" md="6" class="d-flex align-center">
                  <VChip
                    color="info"
                    variant="tonal"
                    size="small"
                    class="me-2"
                  >
                    {{ buildingDetails.length }} تفصيل
                  </VChip>
                </VCol>
              </VRow>
              <VDataTable
                :headers="buildingDetailHeaders"
                :items="buildingDetails"
                :loading="buildingDetailsLoading"
                :items-per-page="10"
                class="text-no-wrap"
                no-data-text="لا توجد تفاصيل مبنى"
              >
                <template #no-data>
                  <div class="text-center py-8">
                    <VIcon icon="tabler-building-off" size="48" color="grey" class="mb-4" />
                    <div class="text-h6 text-medium-emphasis mb-2">لا توجد تفاصيل مبنى</div>
                    <div class="text-body-2 text-medium-emphasis mb-4">
                      لم يتم إضافة أي تفاصيل مبنى لهذا المسجد بعد
                    </div>
                    <VBtn
                      color="primary"
                      variant="outlined"
                      size="small"
                      @click="openAddBuildingDetailDialog"
                      prepend-icon="tabler-plus"
                    >
                      إضافة أول تفصيل
                    </VBtn>
                  </div>
                </template>
                <template #item.name="{ item }">
                  <div class="d-flex align-center">
                    <VAvatar
                      size="32"
                      color="info"
                      variant="tonal"
                    >
                      {{ item.name.charAt(0).toUpperCase() }}
                    </VAvatar>
                    <div class="d-flex flex-column ms-3">
                      <span class="d-block font-weight-medium text-truncate text-high-emphasis">{{ item.name }}</span>
                      <small class="text-medium-emphasis">{{ item.floors }} طابق</small>
                    </div>
                  </div>
                </template>
                <template #item.buildingCategory="{ item }">
                  <VChip
                    :color="item.buildingCategory === 'Facility' ? 'success' : 'warning'"
                    variant="tonal"
                    size="small"
                  >
                    {{ item.buildingCategory === 'Facility' ? 'منشأة' : 'وقف' }}
                  </VChip>
                </template>
                <template #item.withinMosqueArea="{ item }">
                  <VChip
                    :color="item.withinMosqueArea ? 'success' : 'error'"
                    variant="tonal"
                    size="small"
                  >
                    {{ item.withinMosqueArea ? 'داخل المسجد' : 'خارج المسجد' }}
                  </VChip>
                </template>
                <template #item.actions="{ item }">
                  <div class="d-flex gap-1">
                    <IconBtn @click="openEditBuildingDetailDialog(item)" color="primary">
                      <VIcon icon="tabler-edit" />
                    </IconBtn>
                    <IconBtn @click="deleteBuildingDetail(item.id)" color="error">
                      <VIcon icon="tabler-trash" />
                    </IconBtn>
                  </div>
                </template>
              </VDataTable>
            </VCardText>
          </VCard>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="buildingDetailsDialog = false"
          >
            إغلاق
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Add Building Detail Popup Dialog -->
    <VDialog
      v-model="addBuildingDetailDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6 d-flex align-center">
          <VIcon icon="tabler-building-plus" class="me-2" />
          إضافة تفصيل مبنى جديد
          <VSpacer />
          <IconBtn @click="closeAddBuildingDetailDialog" size="small">
            <VIcon icon="tabler-x" />
          </IconBtn>
        </VCardTitle>
        <VCardSubtitle v-if="selectedMosque" class="text-medium-emphasis">
          للمسجد: {{ selectedMosque.mosqueName }}
        </VCardSubtitle>
        <VCardText>
          <VForm @submit.prevent="addBuildingDetail" ref="addBuildingDetailForm">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newBuildingDetail.name"
                  label="اسم المبنى"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المبنى مطلوب']"
                  placeholder="مثال: منزل الإمام، المصلى، دورة المياه..."
                  prepend-inner-icon="tabler-building"
                  clearable
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newBuildingDetail.floors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  min="1"
                  max="50"
                  required
                  :rules="[v => !!v || 'عدد الطوابق مطلوب', v => v > 0 || 'عدد الطوابق يجب أن يكون أكبر من صفر']"
                  prepend-inner-icon="tabler-layers"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newBuildingDetail.buildingCategory"
                  label="نوع المبنى"
                  variant="outlined"
                  :items="buildingCategoryOptions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => !!v || 'نوع المبنى مطلوب']"
                  prepend-inner-icon="tabler-category"
                />
              </VCol>
              <VCol cols="12">
                <VCheckbox
                  v-model="newBuildingDetail.withinMosqueArea"
                  label="داخل مساحة المسجد"
                  hide-details
                  color="primary"
                  class="mt-2"
                />
                <VCardText class="text-caption text-medium-emphasis pa-0 mt-1">
                  <VIcon icon="tabler-info-circle" size="small" class="me-1" />
                  إذا كان المبنى داخل حدود المسجد أم خارجه
                </VCardText>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="closeAddBuildingDetailDialog"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="addBuildingDetail"
            :loading="buildingDetailsLoading"
            prepend-icon="tabler-plus"
          >
            إضافة
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Building Detail Popup Dialog -->
    <VDialog
      v-model="editBuildingDetailDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6 d-flex align-center">
          <VIcon icon="tabler-building-edit" class="me-2" />
          تعديل تفصيل مبنى
          <VSpacer />
          <IconBtn @click="closeEditBuildingDetailDialog" size="small">
            <VIcon icon="tabler-x" />
          </IconBtn>
        </VCardTitle>
        <VCardSubtitle v-if="selectedMosque" class="text-medium-emphasis">
          للمسجد: {{ selectedMosque.mosqueName }}
        </VCardSubtitle>
        <VCardText>
          <VForm @submit.prevent="updateBuildingDetail" ref="editBuildingDetailForm">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editingBuildingDetail.name"
                  label="اسم المبنى"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المبنى مطلوب']"
                  placeholder="مثال: منزل الإمام، المصلى، دورة المياه..."
                  prepend-inner-icon="tabler-building"
                  clearable
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="editingBuildingDetail.floors"
                  label="عدد الطوابق"
                  variant="outlined"
                  type="number"
                  min="1"
                  max="50"
                  required
                  :rules="[v => !!v || 'عدد الطوابق مطلوب', v => v > 0 || 'عدد الطوابق يجب أن يكون أكبر من صفر']"
                  prepend-inner-icon="tabler-layers"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="editingBuildingDetail.buildingCategory"
                  label="نوع المبنى"
                  variant="outlined"
                  :items="buildingCategoryOptions"
                  item-title="label"
                  item-value="value"
                  required
                  :rules="[v => !!v || 'نوع المبنى مطلوب']"
                  prepend-inner-icon="tabler-category"
                />
              </VCol>
              <VCol cols="12">
                <VCheckbox
                  v-model="editingBuildingDetail.withinMosqueArea"
                  label="داخل مساحة المسجد"
                  hide-details
                  color="primary"
                  class="mt-2"
                />
                <VCardText class="text-caption text-medium-emphasis pa-0 mt-1">
                  <VIcon icon="tabler-info-circle" size="small" class="me-1" />
                  إذا كان المبنى داخل حدود المسجد أم خارجه
                </VCardText>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="closeEditBuildingDetailDialog"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="updateBuildingDetail"
            :loading="buildingDetailsLoading"
            prepend-icon="tabler-check"
          >
            تحديث
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* Using template styling */
</style> 