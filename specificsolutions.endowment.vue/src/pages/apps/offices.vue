<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

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
    title: 'المكتب',
    key: 'name',
  },
  {
    title: 'المنطقة',
    key: 'regionName',
  },
  {
    title: 'رقم الهاتف',
    key: 'phoneNumber',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadOffices = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Office/filter?${params}`)
    console.log('API Response:', response)
    console.log('Offices data:', response.data)
    console.log('Offices items:', response.data.items)
    
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
    alertMessage.value = 'حدث خطأ أثناء تحميل المكاتب'
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

const addOffice = async () => {
  try {
    const response = await $api('/Office', {
      method: 'POST',
      body: {
        name: newOffice.value.name,
        location: newOffice.value.location,
        phoneNumber: newOffice.value.phoneNumber,
        regionId: newOffice.value.regionId,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة المكتب'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    dialog.value = false
    resetNewOffice()
    loadOffices()
    alertMessage.value = 'تم إضافة المكتب بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding office:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة المكتب'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateOffice = async () => {
  try {
    const response = await $api(`/Office/${editOffice.value.id}`, {
      method: 'PUT',
      body: {
        id: editOffice.value.id,
        name: editOffice.value.name,
        location: editOffice.value.location || '',
        phoneNumber: editOffice.value.phoneNumber,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تحديث المكتب'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    editDialog.value = false
    loadOffices()
    alertMessage.value = 'تم تحديث المكتب بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating office:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث المكتب'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteOffice = async () => {
  if (!selectedOffice.value) return
  
  try {
    console.log('Attempting to delete office:', selectedOffice.value.id)
    const response = await $api(`/Office/${selectedOffice.value.id}`, {
      method: 'DELETE',
    })
    
    console.log('Delete response:', response)
    console.log('Response data:', response.data)
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المكتب'
      console.log('API returned error:', errorMsg)
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      deleteDialog.value = false
      return
    }
    
    // If we reach here, the deletion was successful
    console.log('Office deleted successfully')
    deleteDialog.value = false
    loadOffices()
    alertMessage.value = 'تم حذف المكتب بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting office:', error)
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
    const deletePromises = selectedRows.value.map(office => 
      $api(`/Office/${office.id}`, { method: 'DELETE' })
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
    loadOffices()
    alertMessage.value = 'تم حذف المكاتب المحددة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected offices:', error)
    // ofetch doesn't throw for HTTP errors, so this is likely a network error
    alertMessage.value = 'حدث خطأ في الاتصال بالخادم'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (office: Office) => {
  console.log('Opening edit dialog for office:', office)
  editOffice.value = { 
    ...office,
    location: office.location || office.Location || '', // Handle both cases
    phoneNumber: office.phoneNumber || office.PhoneNumber || '',
    name: office.name || office.Name || ''
  }
  console.log('Edit office data:', editOffice.value)
  editDialog.value = true
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
        <span class="text-h5">إدارة المكاتب</span>
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
            إضافة مكتب
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
              placeholder="البحث في المكاتب..."
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
              لا توجد منطقة
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
              لا يوجد رقم هاتف
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

    <!-- Add Office Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مكتب جديد</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.name"
                  label="اسم المكتب"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المكتب مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.location"
                  label="الموقع"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'الموقع مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newOffice.phoneNumber"
                  label="رقم الهاتف"
                  variant="outlined"
                  required
                  placeholder="091-1234567 أو 021-1234567"
                  :rules="[
                    v => !!v || 'رقم الهاتف مطلوب',
                    v => /^(09[1-5]|02[1-9])-?\d{7}$/.test(v) || 'يجب أن يكون رقم الهاتف بتنسيق ليبي صحيح (مثال: 091-1234567)'
                  ]"
                  prepend-inner-icon="mdi-phone"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newOffice.regionId"
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
            @click="addOffice"
          >
            حفظ
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
        <VCardTitle class="text-h6">تعديل المكتب</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateOffice">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.name"
                  label="اسم المكتب"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المكتب مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.location"
                  label="الموقع"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'الموقع مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editOffice.phoneNumber"
                  label="رقم الهاتف"
                  variant="outlined"
                  required
                  placeholder="091-1234567 أو 021-1234567"
                  :rules="[
                    v => !!v || 'رقم الهاتف مطلوب',
                    v => /^(09[1-5]|02[1-9])-?\d{7}$/.test(v) || 'يجب أن يكون رقم الهاتف بتنسيق ليبي صحيح (مثال: 091-1234567)'
                  ]"
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
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="updateOffice"
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
          هل أنت متأكد من حذف المكتب "<strong>{{ selectedOffice?.name }}</strong>"؟
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
            @click="deleteOffice"
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