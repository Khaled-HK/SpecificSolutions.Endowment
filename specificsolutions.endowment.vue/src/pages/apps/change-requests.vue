<template>
  <VCard>
    <VCardTitle class="d-flex justify-space-between align-center">
      <span>{{ t('pages.changeRequests.title') }}</span>
      <VBtn
        color="primary"
        @click="openAddDialog"
      >
        {{ t('pages.changeRequests.addChangeRequest') }}
      </VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable
        :headers="headers"
        :items="changeRequests"
        :loading="loading"
        class="text-no-wrap"
      >
        <template #item.actions="{ item }">
          <VBtn
            icon="mdi-pencil"
            size="small"
            color="primary"
            variant="text"
            @click="openEditDialog(item.raw)"
          />
          <VBtn
            icon="mdi-delete"
            size="small"
            color="error"
            variant="text"
            @click="deleteChangeRequest(item.raw.id)"
          />
        </template>
      </VDataTable>
    </VCardText>

    <!-- Add/Edit Dialog -->
    <VDialog
      v-model="dialog"
      max-width="800px"
    >
      <VCard>
        <VCardTitle>
          {{ isEditing ? t('pages.changeRequests.editChangeRequest') : t('pages.changeRequests.addNewChangeRequest') }}
        </VCardTitle>

        <VCardText>
          <VRow>
            <VCol cols="12" md="6">
              <VTextField
                v-model="newRequest.title"
                :label="t('pages.changeRequests.title')"
                :error="hasErrors('title')"
                :error-messages="getErrors('title')"
                @blur="setFieldTouched('title')"
              />
            </VCol>
            <VCol cols="12" md="6">
              <VTextField
                v-model="newRequest.referenceNumber"
                :label="t('pages.changeRequests.referenceNumber')"
                :error="hasErrors('referenceNumber')"
                :error-messages="getErrors('referenceNumber')"
                @blur="setFieldTouched('referenceNumber')"
              />
            </VCol>
            <VCol cols="12" md="6">
              <VSelect
                v-model="newRequest.priority"
                :items="priorityOptions"
                :label="t('pages.changeRequests.priority')"
                :error="hasErrors('priority')"
                :error-messages="getErrors('priority')"
                @blur="setFieldTouched('priority')"
              />
            </VCol>
            <VCol cols="12" md="6">
              <VSelect
                v-model="newRequest.requestStatus"
                :items="statusOptions"
                :label="t('pages.changeRequests.requestStatus')"
                :error="hasErrors('requestStatus')"
                :error-messages="getErrors('requestStatus')"
                @blur="setFieldTouched('requestStatus')"
              />
            </VCol>
            <VCol cols="12" md="6">
              <VTextField
                v-model="newRequest.currentType"
                :label="t('pages.changeRequests.currentType')"
                :error="hasErrors('currentType')"
                :error-messages="getErrors('currentType')"
                @blur="setFieldTouched('currentType')"
              />
            </VCol>
            <VCol cols="12" md="6">
              <VTextField
                v-model="newRequest.newType"
                :label="t('pages.changeRequests.newType')"
                :error="hasErrors('newType')"
                :error-messages="getErrors('newType')"
                @blur="setFieldTouched('newType')"
              />
            </VCol>
            <VCol cols="12">
              <VTextField
                v-model="newRequest.location"
                :label="t('pages.changeRequests.location')"
                :error="hasErrors('location')"
                :error-messages="getErrors('location')"
                @blur="setFieldTouched('location')"
              />
            </VCol>
            <VCol cols="12">
              <VTextarea
                v-model="newRequest.description"
                :label="t('pages.changeRequests.description')"
                :error="hasErrors('description')"
                :error-messages="getErrors('description')"
                @blur="setFieldTouched('description')"
                rows="3"
              />
            </VCol>
            <VCol cols="12">
              <VTextarea
                v-model="newRequest.reason"
                :label="t('pages.changeRequests.reason')"
                :error="hasErrors('reason')"
                :error-messages="getErrors('reason')"
                @blur="setFieldTouched('reason')"
                rows="3"
              />
            </VCol>
          </VRow>
        </VCardText>

        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey"
            variant="text"
            @click="closeDialog"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            v-if="!isEditing"
            color="primary"
            :disabled="hasErrors"
            @click="addChangeRequest"
          >
            {{ t('common.save') }}
          </VBtn>
          <VBtn
            v-else
            color="primary"
            :disabled="hasErrors"
            @click="updateChangeRequest"
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
        <VCardTitle>{{ t('pages.changeRequests.confirmDelete') }}</VCardTitle>
        <VCardText>{{ t('pages.changeRequests.deleteConfirmation') }}</VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey"
            variant="text"
            @click="deleteDialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="error"
            @click="confirmDelete"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { t, locale } = useI18n()
const api = useApi()

// Form validation
const {
  validationState,
  clearErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
  addError,
  hasErrors,
  getErrors,
  setErrorsFromResponse
} = useFormValidation()

// Data
const changeRequests = ref([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const isEditing = ref(false)
const selectedId = ref(null)

// Form data
const newRequest = ref({
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
  currentType: '',
  newType: '',
  reason: ''
})

// Options
const priorityOptions = [
  { title: t('pages.changeRequests.priorityLow'), value: 'Low' },
  { title: t('pages.changeRequests.priorityMedium'), value: 'Medium' },
  { title: t('pages.changeRequests.priorityHigh'), value: 'High' }
]

const statusOptions = [
  { title: t('pages.changeRequests.statusPending'), value: 'Pending' },
  { title: t('pages.changeRequests.statusApproved'), value: 'Approved' },
  { title: t('pages.changeRequests.statusRejected'), value: 'Rejected' },
  { title: t('pages.changeRequests.statusInProgress'), value: 'InProgress' }
]

// Computed headers
const headers = computed(() => [
  { title: t('tableHeaders.changeRequests.title'), key: 'title' },
  { title: t('tableHeaders.changeRequests.currentType'), key: 'currentType' },
  { title: t('tableHeaders.changeRequests.newType'), key: 'newType' },
  { title: t('tableHeaders.changeRequests.priority'), key: 'priority' },
  { title: t('tableHeaders.changeRequests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.changeRequests.location'), key: 'location' },
  { title: t('tableHeaders.changeRequests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.changeRequests.actions'), key: 'actions', sortable: false }
])

// Methods
const loadChangeRequests = async () => {
  loading.value = true
  try {
    const response = await api('/ChangeOfPathRequests/filter', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    if (response && response.data) {
      changeRequests.value = response.data.items || response.data
    }
  } catch (error) {
    console.error('Error loading change requests:', error)
  } finally {
    loading.value = false
  }
}

const openAddDialog = () => {
  isEditing.value = false
  resetNewRequest()
  clearErrors()
  dialog.value = true
}

const openEditDialog = (item: any) => {
  isEditing.value = true
  selectedId.value = item.id
  newRequest.value = { ...item }
  clearErrors()
  dialog.value = true
}

const closeDialog = () => {
  dialog.value = false
  resetNewRequest()
  clearErrors()
}

const resetNewRequest = () => {
  newRequest.value = {
    title: '',
    description: '',
    priority: '',
    location: '',
    referenceNumber: '',
    requestStatus: '',
    currentType: '',
    newType: '',
    reason: ''
  }
}

const addChangeRequest = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('currentType')
  setFieldTouched('newType')
  setFieldTouched('reason')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.changeRequests.title') }))
  }
  if (!validateRequired(newRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.changeRequests.description') }))
  }
  if (!validateRequired(newRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.changeRequests.priority') }))
  }
  if (!validateRequired(newRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.changeRequests.location') }))
  }
  if (!validateRequired(newRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.changeRequests.referenceNumber') }))
  }
  if (!validateRequired(newRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.changeRequests.requestStatus') }))
  }
  if (!validateRequired(newRequest.value.currentType, 'currentType')) {
    addError('currentType', t('validation.required', { field: t('pages.changeRequests.currentType') }))
  }
  if (!validateRequired(newRequest.value.newType, 'newType')) {
    addError('newType', t('validation.required', { field: t('pages.changeRequests.newType') }))
  }
  if (!validateRequired(newRequest.value.reason, 'reason')) {
    addError('reason', t('validation.required', { field: t('pages.changeRequests.reason') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api('/ChangeOfPathRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewRequest()
    await loadChangeRequests()
  } catch (error) {
    console.error('Error adding change request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        CurrentType: 'currentType',
        NewType: 'newType',
        Reason: 'reason'
      })
    }
  }
}

const updateChangeRequest = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('currentType')
  setFieldTouched('newType')
  setFieldTouched('reason')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.changeRequests.title') }))
  }
  if (!validateRequired(newRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.changeRequests.description') }))
  }
  if (!validateRequired(newRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.changeRequests.priority') }))
  }
  if (!validateRequired(newRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.changeRequests.location') }))
  }
  if (!validateRequired(newRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.changeRequests.referenceNumber') }))
  }
  if (!validateRequired(newRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.changeRequests.requestStatus') }))
  }
  if (!validateRequired(newRequest.value.currentType, 'currentType')) {
    addError('currentType', t('validation.required', { field: t('pages.changeRequests.currentType') }))
  }
  if (!validateRequired(newRequest.value.newType, 'newType')) {
    addError('newType', t('validation.required', { field: t('pages.changeRequests.newType') }))
  }
  if (!validateRequired(newRequest.value.reason, 'reason')) {
    addError('reason', t('validation.required', { field: t('pages.changeRequests.reason') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api(`/ChangeOfPathRequests/${selectedId.value}`, {
      method: 'PUT',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewRequest()
    await loadChangeRequests()
  } catch (error) {
    console.error('Error updating change request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        CurrentType: 'currentType',
        NewType: 'newType',
        Reason: 'reason'
      })
    }
  }
}

const deleteChangeRequest = (id: string) => {
  selectedId.value = id
  deleteDialog.value = true
}

const confirmDelete = async () => {
  try {
    await api(`/ChangeOfPathRequests/${selectedId.value}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })

    deleteDialog.value = false
    await loadChangeRequests()
  } catch (error) {
    console.error('Error deleting change request:', error)
  }
}

// Load data on mount
onMounted(() => {
  loadChangeRequests()
})
</script>

<style scoped>
.v-data-table :deep(.v-field--error) {
  border-color: rgb(var(--v-theme-error)) !important;
}

.v-data-table :deep(.v-field--error .v-field__outline) {
  border-color: rgb(var(--v-theme-error)) !important;
}
</style> 