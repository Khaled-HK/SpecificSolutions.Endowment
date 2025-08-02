<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('pages.maintenanceRequests.title') }}</span>
      <VBtn color="primary" @click="dialog = true">{{ t('pages.maintenanceRequests.addRequest') }}</VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable :headers="headers" :items="maintenanceRequests" :loading="loading" class="text-no-wrap">
        <template #item.actions="{ item }">
          <VBtn size="small" color="primary" variant="text" @click="openEditDialog(item.raw)">{{ t('common.edit') }}</VBtn>
          <VBtn size="small" color="error" variant="text" @click="openDeleteDialog(item.raw)">{{ t('common.delete') }}</VBtn>
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
              :error="validationState.errors.title" 
              :error-messages="validationState.errors.title" 
              @blur="setFieldTouched('title')" 
            />
          </VCol>
          <VCol cols="12">
            <VTextarea 
              v-model="newRequest.description" 
              :label="t('pages.maintenanceRequests.description')" 
              :error="validationState.errors.description" 
              :error-messages="validationState.errors.description" 
              @blur="setFieldTouched('description')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.priority" 
              :label="t('pages.maintenanceRequests.priority')" 
              :error="validationState.errors.priority" 
              :error-messages="validationState.errors.priority" 
              @blur="setFieldTouched('priority')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.location" 
              :label="t('pages.maintenanceRequests.location')" 
              :error="validationState.errors.location" 
              :error-messages="validationState.errors.location" 
              @blur="setFieldTouched('location')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.referenceNumber" 
              :label="t('pages.maintenanceRequests.referenceNumber')" 
              :error="validationState.errors.referenceNumber" 
              :error-messages="validationState.errors.referenceNumber" 
              @blur="setFieldTouched('referenceNumber')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.requestStatus" 
              :label="t('pages.maintenanceRequests.requestStatus')" 
              :error="validationState.errors.requestStatus" 
              :error-messages="validationState.errors.requestStatus" 
              @blur="setFieldTouched('requestStatus')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.maintenanceType" 
              :label="t('pages.maintenanceRequests.maintenanceType')" 
              :error="validationState.errors.maintenanceType" 
              :error-messages="validationState.errors.maintenanceType" 
              @blur="setFieldTouched('maintenanceType')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model.number="newRequest.estimatedCost" 
              :label="t('pages.maintenanceRequests.estimatedCost')" 
              type="number" 
              :error="validationState.errors.estimatedCost" 
              :error-messages="validationState.errors.estimatedCost" 
              @blur="setFieldTouched('estimatedCost')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.expectedStartDate" 
              :label="t('pages.maintenanceRequests.expectedStartDate')" 
              type="date" 
              :error="validationState.errors.expectedStartDate" 
              :error-messages="validationState.errors.expectedStartDate" 
              @blur="setFieldTouched('expectedStartDate')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="newRequest.expectedEndDate" 
              :label="t('pages.maintenanceRequests.expectedEndDate')" 
              type="date" 
              :error="validationState.errors.expectedEndDate" 
              :error-messages="validationState.errors.expectedEndDate" 
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
              :error="validationState.errors.title" 
              :error-messages="validationState.errors.title" 
              @blur="setFieldTouched('title')" 
            />
          </VCol>
          <VCol cols="12">
            <VTextarea 
              v-model="editRequest.description" 
              :label="t('pages.maintenanceRequests.description')" 
              :error="validationState.errors.description" 
              :error-messages="validationState.errors.description" 
              @blur="setFieldTouched('description')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.priority" 
              :label="t('pages.maintenanceRequests.priority')" 
              :error="validationState.errors.priority" 
              :error-messages="validationState.errors.priority" 
              @blur="setFieldTouched('priority')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.location" 
              :label="t('pages.maintenanceRequests.location')" 
              :error="validationState.errors.location" 
              :error-messages="validationState.errors.location" 
              @blur="setFieldTouched('location')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.referenceNumber" 
              :label="t('pages.maintenanceRequests.referenceNumber')" 
              :error="validationState.errors.referenceNumber" 
              :error-messages="validationState.errors.referenceNumber" 
              @blur="setFieldTouched('referenceNumber')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.requestStatus" 
              :label="t('pages.maintenanceRequests.requestStatus')" 
              :error="validationState.errors.requestStatus" 
              :error-messages="validationState.errors.requestStatus" 
              @blur="setFieldTouched('requestStatus')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.maintenanceType" 
              :label="t('pages.maintenanceRequests.maintenanceType')" 
              :error="validationState.errors.maintenanceType" 
              :error-messages="validationState.errors.maintenanceType" 
              @blur="setFieldTouched('maintenanceType')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model.number="editRequest.estimatedCost" 
              :label="t('pages.maintenanceRequests.estimatedCost')" 
              type="number" 
              :error="validationState.errors.estimatedCost" 
              :error-messages="validationState.errors.estimatedCost" 
              @blur="setFieldTouched('estimatedCost')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.expectedStartDate" 
              :label="t('pages.maintenanceRequests.expectedStartDate')" 
              type="date" 
              :error="validationState.errors.expectedStartDate" 
              :error-messages="validationState.errors.expectedStartDate" 
              @blur="setFieldTouched('expectedStartDate')" 
            />
          </VCol>
          <VCol cols="6">
            <VTextField 
              v-model="editRequest.expectedEndDate" 
              :label="t('pages.maintenanceRequests.expectedEndDate')" 
              type="date" 
              :error="validationState.errors.expectedEndDate" 
              :error-messages="validationState.errors.expectedEndDate" 
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

const maintenanceRequests = ref([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedRequest = ref(null)

const newRequest = ref({
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

const editRequest = ref({
  id: null,
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
    const response = await $api('/MaintenanceRequests/GetMaintenanceRequests', {
      headers: { 'Accept-Language': locale.value }
    })
    maintenanceRequests.value = response
  } catch (error) {
    console.error('Error loading maintenance requests:', error)
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
    await $api('/MaintenanceRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: { 'Accept-Language': locale.value }
    })
    dialog.value = false
    resetNewRequest()
    await loadMaintenanceRequests()
  } catch (error) {
    console.error('Error adding maintenance request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        MaintenanceType: 'maintenanceType',
        EstimatedCost: 'estimatedCost',
        ExpectedStartDate: 'expectedStartDate',
        ExpectedEndDate: 'expectedEndDate',
      })
    }
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
    await $api(`/MaintenanceRequests/${editRequest.value.id}`, {
      method: 'PUT',
      body: editRequest.value,
      headers: { 'Accept-Language': locale.value }
    })
    editDialog.value = false
    await loadMaintenanceRequests()
  } catch (error) {
    console.error('Error updating maintenance request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        MaintenanceType: 'maintenanceType',
        EstimatedCost: 'estimatedCost',
        ExpectedStartDate: 'expectedStartDate',
        ExpectedEndDate: 'expectedEndDate',
      })
    }
  }
}

const deleteMaintenanceRequest = async () => {
  try {
    await $api(`/MaintenanceRequests/${selectedRequest.value.id}`, {
      method: 'DELETE',
      headers: { 'Accept-Language': locale.value }
    })
    deleteDialog.value = false
    selectedRequest.value = null
    await loadMaintenanceRequests()
  } catch (error) {
    console.error('Error deleting maintenance request:', error)
  }
}

const openEditDialog = (request) => {
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

const openDeleteDialog = (request) => {
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