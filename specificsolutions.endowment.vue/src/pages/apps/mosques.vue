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
const loading = ref(false)
const citiesLoading = ref(false)
const regionsLoading = ref(false)
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

// Expansion panel state for add/edit forms
const addPanel = ref(0)
const editPanel = ref(0)

const newMosque = ref<NewMosque>({
  name: '',
  address: '',
  cityId: '',
  regionId: '',
  imamName: '',
  capacity: 0,
  description: '',
})

const editMosque = ref<Mosque>({
  id: 0,
  name: '',
  address: '',
  cityId: '',
  regionId: '',
  imamName: '',
  capacity: 0,
  description: '',
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
    const response = await $api('/Mosque', {
      method: 'POST',
      body: {
        name: newMosque.value.name,
        address: newMosque.value.address,
        cityId: newMosque.value.cityId,
        regionId: newMosque.value.regionId,
        imamName: newMosque.value.imamName,
        capacity: newMosque.value.capacity,
        description: newMosque.value.description,
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
    const response = await $api(`/Mosque/${editMosque.value.id}`, {
      method: 'PUT',
      body: {
        id: editMosque.value.id,
        name: editMosque.value.name,
        address: editMosque.value.address,
        cityId: editMosque.value.cityId,
        regionId: editMosque.value.regionId,
        imamName: editMosque.value.imamName,
        capacity: editMosque.value.capacity,
        description: editMosque.value.description,
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
    const response = await $api(`/Mosque/${selectedMosque.value.id}`, {
      method: 'DELETE',
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المسجد'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
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
  loadCities()
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

    <!-- Add Mosque Dialog using VExpansionPanels -->
    <VDialog
      v-model="dialog"
      max-width="800px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مسجد جديد</VCardTitle>
        <VCardText>
          <VExpansionPanels v-model="addPanel">
            <!-- Basic Information Panel -->
            <VExpansionPanel>
              <VExpansionPanelTitle>
                <VIcon icon="mdi-information" class="me-2" />
                المعلومات الأساسية
              </VExpansionPanelTitle>
              <VExpansionPanelText>
                <VForm @submit.prevent="addMosque">
                  <VRow>
                    <VCol cols="12">
                      <VTextField
                        v-model="newMosque.name"
                        label="اسم المسجد"
                        variant="outlined"
                        required
                        :rules="[v => !!v || 'اسم المسجد مطلوب']"
                        prepend-inner-icon="mdi-mosque"
                      />
                    </VCol>
                    <VCol cols="12">
                      <VTextField
                        v-model="newMosque.address"
                        label="العنوان"
                        variant="outlined"
                        required
                        :rules="[v => !!v || 'العنوان مطلوب']"
                        prepend-inner-icon="mdi-map-marker"
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
                  </VRow>
                </VForm>
              </VExpansionPanelText>
            </VExpansionPanel>

            <!-- Additional Information Panel -->
            <VExpansionPanel>
              <VExpansionPanelTitle>
                <VIcon icon="mdi-details" class="me-2" />
                معلومات إضافية
              </VExpansionPanelTitle>
              <VExpansionPanelText>
                <VRow>
                  <VCol cols="12" md="6">
                    <VTextField
                      v-model="newMosque.imamName"
                      label="اسم الإمام"
                      variant="outlined"
                      placeholder="أدخل اسم الإمام..."
                      prepend-inner-icon="mdi-account"
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
                      prepend-inner-icon="mdi-account-group"
                    />
                  </VCol>
                  <VCol cols="12">
                    <VTextarea
                      v-model="newMosque.description"
                      label="الوصف"
                      variant="outlined"
                      rows="3"
                      placeholder="أدخل وصف المسجد..."
                      prepend-inner-icon="mdi-text"
                    />
                  </VCol>
                </VRow>
              </VExpansionPanelText>
            </VExpansionPanel>
          </VExpansionPanels>
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

    <!-- Edit Mosque Dialog using VExpansionPanels -->
    <VDialog
      v-model="editDialog"
      max-width="800px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المسجد</VCardTitle>
        <VCardText>
          <VExpansionPanels v-model="editPanel">
            <!-- Basic Information Panel -->
            <VExpansionPanel>
              <VExpansionPanelTitle>
                <VIcon icon="mdi-information" class="me-2" />
                المعلومات الأساسية
              </VExpansionPanelTitle>
              <VExpansionPanelText>
                <VForm @submit.prevent="updateMosque">
                  <VRow>
                    <VCol cols="12">
                      <VTextField
                        v-model="editMosque.name"
                        label="اسم المسجد"
                        variant="outlined"
                        required
                        :rules="[v => !!v || 'اسم المسجد مطلوب']"
                        prepend-inner-icon="mdi-mosque"
                      />
                    </VCol>
                    <VCol cols="12">
                      <VTextField
                        v-model="editMosque.address"
                        label="العنوان"
                        variant="outlined"
                        required
                        :rules="[v => !!v || 'العنوان مطلوب']"
                        prepend-inner-icon="mdi-map-marker"
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
                  </VRow>
                </VForm>
              </VExpansionPanelText>
            </VExpansionPanel>

            <!-- Additional Information Panel -->
            <VExpansionPanel>
              <VExpansionPanelTitle>
                <VIcon icon="mdi-details" class="me-2" />
                معلومات إضافية
              </VExpansionPanelTitle>
              <VExpansionPanelText>
                <VRow>
                  <VCol cols="12" md="6">
                    <VTextField
                      v-model="editMosque.imamName"
                      label="اسم الإمام"
                      variant="outlined"
                      placeholder="أدخل اسم الإمام..."
                      prepend-inner-icon="mdi-account"
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
                      prepend-inner-icon="mdi-account-group"
                    />
                  </VCol>
                  <VCol cols="12">
                    <VTextarea
                      v-model="editMosque.description"
                      label="الوصف"
                      variant="outlined"
                      rows="3"
                      placeholder="أدخل وصف المسجد..."
                      prepend-inner-icon="mdi-text"
                    />
                  </VCol>
                </VRow>
              </VExpansionPanelText>
            </VExpansionPanel>
          </VExpansionPanels>
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

