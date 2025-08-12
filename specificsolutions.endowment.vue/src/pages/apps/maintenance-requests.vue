<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('pages.maintenanceRequests.title') }}</span>
      <VBtn color="primary" @click="dialog = true">{{ t('pages.maintenanceRequests.addRequest') }}</VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable :headers="headers" :items="maintenanceRequests" :loading="loading" class="text-no-wrap" :items-per-page="options.itemsPerPage" :page="options.page">
        <template #item.actions="{ item }: { item: any }">
          <VBtn size="small" color="primary" variant="text" @click="openEditDialog(item.raw)">{{ t('common.edit') }}</VBtn>
          <VBtn size="small" color="error" variant="text" @click="openDeleteDialog(item.raw)">{{ t('common.delete') }}</VBtn>
        </template>
        <template #bottom>
          <VCardText class="pt-2">
            <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
              <VSelect
                v-model="options.itemsPerPage"
                :items="[5, 10, 25, 50, 100]"
                :label="t('common.itemsPerPage')"
                variant="underlined"
                style="max-inline-size: 8rem;min-inline-size: 5rem;"
              />

              <VPagination
                v-model="options.page"
                :total-visible="$vuetify.display.smAndDown ? 3 : 5"
                :length="Math.ceil(totalItems / options.itemsPerPage) || 1"
              />
            </div>
          </VCardText>
        </template>
      </VDataTable>
    </VCardText>
  </VCard>

  <!-- Add Maintenance Request Dialog -->
  <VDialog v-model="dialog" max-width="800px">
    <VCard>
      <VCardTitle>{{ t('pages.maintenanceRequests.addNewRequest') }}</VCardTitle>
      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField 
              v-model="newRequest.title" 
              :label="t('pages.maintenanceRequests.title')" 
              :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title" 
              :error-messages="validationState.errors.title || []" 
              @blur="setFieldTouched('title')" 
            />
          </VCol>
          <VCol cols="12">
            <VTextarea 
              v-model="newRequest.description" 
              :label="t('pages.maintenanceRequests.description')" 
              :error="validationState.errors.description && validationState.errors.description.length > 0 && validationState.touched.description" 
              :error-messages="validationState.errors.description || []" 
              @blur="setFieldTouched('description')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.priority" 
              :label="t('pages.maintenanceRequests.priority')" 
              :error="validationState.errors.priority && validationState.errors.priority.length > 0 && validationState.touched.priority" 
              :error-messages="validationState.errors.priority || []" 
              @blur="setFieldTouched('priority')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.location" 
              :label="t('pages.maintenanceRequests.location')" 
              :error="validationState.errors.location && validationState.errors.location.length > 0 && validationState.touched.location" 
              :error-messages="validationState.errors.location || []" 
              @blur="setFieldTouched('location')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.referenceNumber" 
              :label="t('pages.maintenanceRequests.referenceNumber')" 
              :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber" 
              :error-messages="validationState.errors.referenceNumber || []" 
              @blur="setFieldTouched('referenceNumber')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.requestStatus" 
              :label="t('pages.maintenanceRequests.requestStatus')" 
              :error="validationState.errors.requestStatus && validationState.errors.requestStatus.length > 0 && validationState.touched.requestStatus" 
              :error-messages="validationState.errors.requestStatus || []" 
              @blur="setFieldTouched('requestStatus')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.maintenanceType" 
              :label="t('pages.maintenanceRequests.maintenanceType')" 
              :error="validationState.errors.maintenanceType && validationState.errors.maintenanceType.length > 0 && validationState.touched.maintenanceType" 
              :error-messages="validationState.errors.maintenanceType || []" 
              @blur="setFieldTouched('maintenanceType')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model.number="newRequest.estimatedCost" 
              :label="t('pages.maintenanceRequests.estimatedCost')" 
              type="number" 
              :error="validationState.errors.estimatedCost && validationState.errors.estimatedCost.length > 0 && validationState.touched.estimatedCost" 
              :error-messages="validationState.errors.estimatedCost || []" 
              @blur="setFieldTouched('estimatedCost')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.expectedStartDate" 
              :label="t('pages.maintenanceRequests.expectedStartDate')" 
              type="date" 
              :error="validationState.errors.expectedStartDate && validationState.errors.expectedStartDate.length > 0 && validationState.touched.expectedStartDate" 
              :error-messages="validationState.errors.expectedStartDate || []" 
              @blur="setFieldTouched('expectedStartDate')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.expectedEndDate" 
              :label="t('pages.maintenanceRequests.expectedEndDate')" 
              type="date" 
              :error="validationState.errors.expectedEndDate && validationState.errors.expectedEndDate.length > 0 && validationState.touched.expectedEndDate" 
              :error-messages="validationState.errors.expectedEndDate || []" 
              @blur="setFieldTouched('expectedEndDate')" 
            />
          </VCol>
        </VRow>
      </VCardText>
      <VCardActions>
        <VSpacer />
        <VBtn color="grey-darken-1" variant="text" @click="dialog = false">{{ t('common.cancel') }}</VBtn>
        <VBtn color="primary" :disabled="hasErrors" @click="addMaintenanceRequest">{{ t('common.save') }}</VBtn>
      </VCardActions>
    </VCard>
  </VDialog>

  <!-- Edit Maintenance Request Dialog -->
  <VDialog v-model="editDialog" max-width="800px">
    <VCard>
      <VCardTitle>{{ t('pages.maintenanceRequests.editRequest') }}</VCardTitle>
      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField 
              v-model="editRequest.title" 
              :label="t('pages.maintenanceRequests.title')" 
              :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title" 
              :error-messages="validationState.errors.title || []" 
              @blur="setFieldTouched('title')" 
            />
          </VCol>
          <VCol cols="12">
            <VTextarea 
              v-model="editRequest.description" 
              :label="t('pages.maintenanceRequests.description')" 
              :error="validationState.errors.description && validationState.errors.description.length > 0 && validationState.touched.description" 
              :error-messages="validationState.errors.description || []" 
              @blur="setFieldTouched('description')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.priority" 
              :label="t('pages.maintenanceRequests.priority')" 
              :error="validationState.errors.priority && validationState.errors.priority.length > 0 && validationState.touched.priority" 
              :error-messages="validationState.errors.priority || []" 
              @blur="setFieldTouched('priority')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.location" 
              :label="t('pages.maintenanceRequests.location')" 
              :error="validationState.errors.location && validationState.errors.location.length > 0 && validationState.touched.location" 
              :error-messages="validationState.errors.location || []" 
              @blur="setFieldTouched('location')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.referenceNumber" 
              :label="t('pages.maintenanceRequests.referenceNumber')" 
              :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber" 
              :error-messages="validationState.errors.referenceNumber || []" 
              @blur="setFieldTouched('referenceNumber')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.requestStatus" 
              :label="t('pages.maintenanceRequests.requestStatus')" 
              :error="validationState.errors.requestStatus && validationState.errors.requestStatus.length > 0 && validationState.touched.requestStatus" 
              :error-messages="validationState.errors.requestStatus || []" 
              @blur="setFieldTouched('requestStatus')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.maintenanceType" 
              :label="t('pages.maintenanceRequests.maintenanceType')" 
              :error="validationState.errors.maintenanceType && validationState.errors.maintenanceType.length > 0 && validationState.touched.maintenanceType" 
              :error-messages="validationState.errors.maintenanceType || []" 
              @blur="setFieldTouched('maintenanceType')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model.number="editRequest.estimatedCost" 
              :label="t('pages.maintenanceRequests.estimatedCost')" 
              type="number" 
              :error="validationState.errors.estimatedCost && validationState.errors.estimatedCost.length > 0 && validationState.touched.estimatedCost" 
              :error-messages="validationState.errors.estimatedCost || []" 
              @blur="setFieldTouched('estimatedCost')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.expectedStartDate" 
              :label="t('pages.maintenanceRequests.expectedStartDate')" 
              type="date" 
              :error="validationState.errors.expectedStartDate && validationState.errors.expectedStartDate.length > 0 && validationState.touched.expectedStartDate" 
              :error-messages="validationState.errors.expectedStartDate || []" 
              @blur="setFieldTouched('expectedStartDate')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.expectedEndDate" 
              :label="t('pages.maintenanceRequests.expectedEndDate')" 
              type="date" 
              :error="validationState.errors.expectedEndDate && validationState.errors.expectedEndDate.length > 0 && validationState.touched.expectedEndDate" 
              :error-messages="validationState.errors.expectedEndDate || []" 
              @blur="setFieldTouched('expectedEndDate')" 
            />
          </VCol>
        </VRow>
      </VCardText>
      <VCardActions>
        <VSpacer />
        <VBtn color="grey-darken-1" variant="text" @click="editDialog = false">{{ t('common.cancel') }}</VBtn>
        <VBtn color="primary" :disabled="hasErrors" @click="updateMaintenanceRequest">{{ t('common.update') }}</VBtn>
      </VCardActions>
    </VCard>
  </VDialog>

  <!-- Delete Confirmation Dialog -->
  <VDialog v-model="deleteDialog" max-width="400px">
    <VCard>
      <VCardTitle>{{ t('pages.maintenanceRequests.confirmDelete') }}</VCardTitle>
      <VCardText>{{ t('pages.maintenanceRequests.deleteConfirmation') }}</VCardText>
      <VCardActions>
        <VSpacer />
        <VBtn color="grey-darken-1" variant="text" @click="deleteDialog = false">{{ t('common.cancel') }}</VBtn>
        <VBtn color="error" @click="deleteMaintenanceRequest">{{ t('common.delete') }}</VBtn>
      </VCardActions>
    </VCard>
  </VDialog>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'

definePage({
  meta: {
    action: 'View',
    subject: 'MaintenanceRequest',
  },
})

const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  addError,
} = useFormValidation()

const { t, locale } = useI18n()
const api = useApi()

interface MaintenanceRequestRow {
  id: string
  title: string
  description: string
  priority: string
  location: string
  referenceNumber: string
  requestStatus: string
  maintenanceType: string
  estimatedCost: number
  expectedStartDate: string
  expectedEndDate: string
}

const maintenanceRequests = ref<MaintenanceRequestRow[]>([])
const loading = ref(false)
const totalItems = ref(0)
const options = ref({ page: 1, itemsPerPage: 10 })
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedRequest = ref<MaintenanceRequestRow | null>(null)

const newRequest = ref<Omit<MaintenanceRequestRow, 'id'>>({
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
  maintenanceType: '',
  estimatedCost: 0,
  expectedStartDate: '',
  expectedEndDate: '',
})

const editRequest = ref<MaintenanceRequestRow>({
  id: '',
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
  maintenanceType: '',
  estimatedCost: 0,
  expectedStartDate: '',
  expectedEndDate: '',
})

const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.maintenanceRequests.title'), key: 'title' },
  { title: t('tableHeaders.maintenanceRequests.description'), key: 'description' },
  { title: t('tableHeaders.maintenanceRequests.priority'), key: 'priority' },
  { title: t('tableHeaders.maintenanceRequests.location'), key: 'location' },
  { title: t('tableHeaders.maintenanceRequests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.maintenanceRequests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.maintenanceRequests.maintenanceType'), key: 'maintenanceType' },
  { title: t('tableHeaders.maintenanceRequests.estimatedCost'), key: 'estimatedCost' },
  { title: t('tableHeaders.maintenanceRequests.expectedStartDate'), key: 'expectedStartDate' },
  { title: t('tableHeaders.maintenanceRequests.expectedEndDate'), key: 'expectedEndDate' },
  { title: t('tableHeaders.maintenanceRequests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.maintenanceRequests.actions'), key: 'actions', sortable: false },
])

const loadMaintenanceRequests = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: String(options.value.page),
      PageSize: String(options.value.itemsPerPage)
    }).toString()
    const response = await api(`/MaintenanceRequest/filter?${params}`, {
      headers: { 'Accept-Language': locale.value }
    })
    maintenanceRequests.value = response.data?.items || []
    totalItems.value = response.data?.totalCount || 0
  } catch (error) {
    console.error('Error loading maintenance requests:', error)
    // تنبيه موحّد عند الفشل
    const { error: appError } = useAppAlerts()
    appError(t('pages.maintenanceRequests.errorLoadingRequests') || 'حدث خطأ أثناء تحميل طلبات الصيانة', { timeout: 0, clickToDismiss: true })
  } finally {
    loading.value = false
  }
}

const addMaintenanceRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('maintenanceType')
  setFieldTouched('estimatedCost')
  setFieldTouched('expectedStartDate')
  setFieldTouched('expectedEndDate')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.maintenanceRequests.title') }))
  }
  if (!validateRequired(newRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.maintenanceRequests.description') }))
  }
  if (!validateRequired(newRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.maintenanceRequests.priority') }))
  }
  if (!validateRequired(newRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.maintenanceRequests.location') }))
  }
  if (!validateRequired(newRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.maintenanceRequests.referenceNumber') }))
  }
  if (!validateRequired(newRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.maintenanceRequests.requestStatus') }))
  }
  if (!validateRequired(newRequest.value.maintenanceType, 'maintenanceType')) {
    addError('maintenanceType', t('validation.required', { field: t('pages.maintenanceRequests.maintenanceType') }))
  }
  if (!validateRequired(newRequest.value.expectedStartDate, 'expectedStartDate')) {
    addError('expectedStartDate', t('validation.required', { field: t('pages.maintenanceRequests.expectedStartDate') }))
  }
  if (!validateRequired(newRequest.value.expectedEndDate, 'expectedEndDate')) {
    addError('expectedEndDate', t('validation.required', { field: t('pages.maintenanceRequests.expectedEndDate') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api('/MaintenanceRequest', {
      method: 'POST',
      body: newRequest.value,
      headers: { 'Accept-Language': locale.value }
    })
    dialog.value = false
    resetNewRequest()
    await loadMaintenanceRequests()
    const { success } = useAppAlerts()
    success(t('pages.maintenanceRequests.successAdd') || 'تم إضافة الطلب بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (e: any) {
    console.error('Error adding maintenance request:', e)
    if (e.response?.data) {
      setErrorsFromResponse(e.response.data, 'add')
    }
    const { error: appError } = useAppAlerts()
    appError(t('pages.maintenanceRequests.errorAdd') || 'حدث خطأ أثناء إضافة الطلب', { timeout: 0, clickToDismiss: true })
  }
}

const updateMaintenanceRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('maintenanceType')
  setFieldTouched('estimatedCost')
  setFieldTouched('expectedStartDate')
  setFieldTouched('expectedEndDate')

  if (!validateRequired(editRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.maintenanceRequests.title') }))
  }
  if (!validateRequired(editRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.maintenanceRequests.description') }))
  }
  if (!validateRequired(editRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.maintenanceRequests.priority') }))
  }
  if (!validateRequired(editRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.maintenanceRequests.location') }))
  }
  if (!validateRequired(editRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.maintenanceRequests.referenceNumber') }))
  }
  if (!validateRequired(editRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.maintenanceRequests.requestStatus') }))
  }
  if (!validateRequired(editRequest.value.maintenanceType, 'maintenanceType')) {
    addError('maintenanceType', t('validation.required', { field: t('pages.maintenanceRequests.maintenanceType') }))
  }
  if (!validateRequired(editRequest.value.expectedStartDate, 'expectedStartDate')) {
    addError('expectedStartDate', t('validation.required', { field: t('pages.maintenanceRequests.expectedStartDate') }))
  }
  if (!validateRequired(editRequest.value.expectedEndDate, 'expectedEndDate')) {
    addError('expectedEndDate', t('validation.required', { field: t('pages.maintenanceRequests.expectedEndDate') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api(`/MaintenanceRequest/${editRequest.value.id}`, {
      method: 'PUT',
      body: editRequest.value,
      headers: { 'Accept-Language': locale.value }
    })
    editDialog.value = false
    await loadMaintenanceRequests()
    const { success } = useAppAlerts()
    success(t('pages.maintenanceRequests.successUpdate') || 'تم تحديث الطلب بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (e: any) {
    console.error('Error updating maintenance request:', e)
    if (e.response?.data) {
      setErrorsFromResponse(e.response.data, 'edit')
    }
    const { error: appError } = useAppAlerts()
    appError(t('pages.maintenanceRequests.errorUpdate') || 'حدث خطأ أثناء تحديث الطلب', { timeout: 0, clickToDismiss: true })
  }
}

const deleteMaintenanceRequest = async () => {
  try {
    if (!selectedRequest.value) return
    await api(`/MaintenanceRequest/${selectedRequest.value.id}`, {
      method: 'DELETE',
      headers: { 'Accept-Language': locale.value }
    })
    deleteDialog.value = false
    selectedRequest.value = null
    await loadMaintenanceRequests()
    const { success } = useAppAlerts()
    success(t('pages.maintenanceRequests.successDelete') || 'تم حذف الطلب بنجاح', { timeout: 4000, clickToDismiss: true })
  } catch (error) {
    console.error('Error deleting maintenance request:', error)
    const { error: appError } = useAppAlerts()
    appError(t('pages.maintenanceRequests.errorDelete') || 'حدث خطأ أثناء حذف الطلب', { timeout: 0, clickToDismiss: true })
  }
}

const openEditDialog = (request: any) => {
  clearErrors()
  editRequest.value = {
    id: request.id,
    title: request.title,
    description: request.description,
    priority: request.priority,
    location: request.location,
    referenceNumber: request.referenceNumber,
    requestStatus: request.requestStatus,
    maintenanceType: request.maintenanceType,
    estimatedCost: request.estimatedCost,
    expectedStartDate: request.expectedStartDate,
    expectedEndDate: request.expectedEndDate,
  }
  editDialog.value = true
}

const openDeleteDialog = (request: any) => {
  selectedRequest.value = request
  deleteDialog.value = true
}

const resetNewRequest = () => {
  clearErrors()
  newRequest.value = {
    title: '',
    description: '',
    priority: '',
    location: '',
    referenceNumber: '',
    requestStatus: '',
    maintenanceType: '',
    estimatedCost: 0,
    expectedStartDate: '',
    expectedEndDate: '',
  }
}

watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadMaintenanceRequests()
})

onMounted(() => {
  loadMaintenanceRequests()
})
</script>

<style scoped>
.v-field--error {
  border-color: rgb(var(--v-theme-error)) !important;
}
.v-messages__message {
  color: rgb(var(--v-theme-error)) !important;
}
</style> 