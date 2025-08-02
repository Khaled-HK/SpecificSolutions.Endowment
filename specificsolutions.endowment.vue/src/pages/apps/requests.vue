<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'

definePage({
  meta: {
    action: 'View',
    subject: 'Request',
  },
})

// استخدام نظام التحقق
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

// استخدام i18n للترجمة
const { t, locale } = useI18n()

// Get API instance
const api = useApi()

const requests = ref([])
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
})

const editRequest = ref({
  id: null,
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
})

// Headers ديناميكية مع دعم الترجمة
const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.requests.title'), key: 'title' },
  { title: t('tableHeaders.requests.description'), key: 'description' },
  { title: t('tableHeaders.requests.priority'), key: 'priority' },
  { title: t('tableHeaders.requests.location'), key: 'location' },
  { title: t('tableHeaders.requests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.requests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.requests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.requests.actions'), key: 'actions', sortable: false },
])

const loadRequests = async () => {
  loading.value = true
  try {
    const response = await api('/Requests/requests', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    requests.value = response.data || []
  } catch (error) {
    console.error('Error loading requests:', error)
  } finally {
    loading.value = false
  }
}

const addRequest = async () => {
  clearErrors()
  
  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('requestType')
  setFieldTouched('priority')
  
  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.requests.title') }))
  }
  
  if (!validateRequired(newRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.requests.description') }))
  }
  
  if (!validateRequired(newRequest.value.requestType, 'requestType')) {
    addError('requestType', t('validation.required', { field: t('pages.requests.requestType') }))
  }
  
  if (!validateRequired(newRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.requests.priority') }))
  }
  
  if (hasErrors.value) {
    return
  }
  
  try {
    await api('/Requests', {
      method: 'POST',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    dialog.value = false
    resetNewRequest()
    await loadRequests()
  } catch (error) {
    console.error('Error adding request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        RequestType: 'requestType',
        Priority: 'priority',
      })
    }
  }
}

const updateRequest = async () => {
  clearErrors()
  
  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  
  if (!validateRequired(editRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.requests.title') }))
  }
  
  if (!validateRequired(editRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.requests.description') }))
  }
  
  if (!validateRequired(editRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.requests.priority') }))
  }
  
  if (!validateRequired(editRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.requests.location') }))
  }
  
  if (!validateRequired(editRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.requests.referenceNumber') }))
  }
  
  if (!validateRequired(editRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.requests.requestStatus') }))
  }
  
  if (hasErrors.value) {
    return
  }
  
  try {
    await api(`/Requests/${editRequest.value.id}`, {
      method: 'PUT',
      body: editRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    editDialog.value = false
    await loadRequests()
  } catch (error) {
    console.error('Error updating request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
      })
    }
  }
}

const deleteRequest = async () => {
  try {
    await api(`/Requests/${selectedRequest.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    deleteDialog.value = false
    selectedRequest.value = null
    await loadRequests()
  } catch (error) {
    console.error('Error deleting request:', error)
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
  }
}

onMounted(() => {
  loadRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('pages.requests.title') }}</span>
      <VBtn
        color="primary"
        @click="dialog = true"
      >
        {{ t('pages.requests.addRequest') }}
      </VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable
        :headers="headers"
        :items="requests"
        :loading="loading"
        class="text-no-wrap"
      >
        <template #item.actions="{ item }">
          <VBtn
            size="small"
            color="primary"
            variant="text"
            @click="openEditDialog(item.raw)"
          >
            {{ t('common.edit') }}
          </VBtn>
          <VBtn
            size="small"
            color="error"
            variant="text"
            @click="openDeleteDialog(item.raw)"
          >
            {{ t('common.delete') }}
          </VBtn>
        </template>
      </VDataTable>
    </VCardText>
  </VCard>

  <!-- Add Request Dialog -->
  <VDialog
    v-model="dialog"
    max-width="600px"
  >
    <VCard>
      <VCardTitle>
        {{ t('pages.requests.addNewRequest') }}
      </VCardTitle>
      
      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField
              v-model="newRequest.title"
              :label="t('pages.requests.title')"
              :error="validationState.errors.title"
              :error-messages="validationState.errors.title"
              @blur="setFieldTouched('title')"
            />
          </VCol>
          
          <VCol cols="12">
            <VTextarea
              v-model="newRequest.description"
              :label="t('pages.requests.description')"
              :error="validationState.errors.description"
              :error-messages="validationState.errors.description"
              @blur="setFieldTouched('description')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="newRequest.priority"
              :label="t('pages.requests.priority')"
              :error="validationState.errors.priority"
              :error-messages="validationState.errors.priority"
              @blur="setFieldTouched('priority')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="newRequest.location"
              :label="t('pages.requests.location')"
              :error="validationState.errors.location"
              :error-messages="validationState.errors.location"
              @blur="setFieldTouched('location')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="newRequest.referenceNumber"
              :label="t('pages.requests.referenceNumber')"
              :error="validationState.errors.referenceNumber"
              :error-messages="validationState.errors.referenceNumber"
              @blur="setFieldTouched('referenceNumber')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="newRequest.requestStatus"
              :label="t('pages.requests.requestStatus')"
              :error="validationState.errors.requestStatus"
              :error-messages="validationState.errors.requestStatus"
              @blur="setFieldTouched('requestStatus')"
            />
          </VCol>
        </VRow>
      </VCardText>
      
      <VCardActions>
        <VSpacer />
        <VBtn
          color="grey-darken-1"
          variant="text"
          @click="dialog = false"
        >
          {{ t('common.cancel') }}
        </VBtn>
        <VBtn
          color="primary"
          :disabled="hasErrors"
          @click="addRequest"
        >
          {{ t('common.save') }}
        </VBtn>
      </VCardActions>
    </VCard>
  </VDialog>

  <!-- Edit Request Dialog -->
  <VDialog
    v-model="editDialog"
    max-width="600px"
  >
    <VCard>
      <VCardTitle>
        {{ t('pages.requests.editRequest') }}
      </VCardTitle>
      
      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField
              v-model="editRequest.title"
              :label="t('pages.requests.title')"
              :error="validationState.errors.title"
              :error-messages="validationState.errors.title"
              @blur="setFieldTouched('title')"
            />
          </VCol>
          
          <VCol cols="12">
            <VTextarea
              v-model="editRequest.description"
              :label="t('pages.requests.description')"
              :error="validationState.errors.description"
              :error-messages="validationState.errors.description"
              @blur="setFieldTouched('description')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="editRequest.priority"
              :label="t('pages.requests.priority')"
              :error="validationState.errors.priority"
              :error-messages="validationState.errors.priority"
              @blur="setFieldTouched('priority')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="editRequest.location"
              :label="t('pages.requests.location')"
              :error="validationState.errors.location"
              :error-messages="validationState.errors.location"
              @blur="setFieldTouched('location')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="editRequest.referenceNumber"
              :label="t('pages.requests.referenceNumber')"
              :error="validationState.errors.referenceNumber"
              :error-messages="validationState.errors.referenceNumber"
              @blur="setFieldTouched('referenceNumber')"
            />
          </VCol>
          
          <VCol cols="6">
            <VTextField
              v-model="editRequest.requestStatus"
              :label="t('pages.requests.requestStatus')"
              :error="validationState.errors.requestStatus"
              :error-messages="validationState.errors.requestStatus"
              @blur="setFieldTouched('requestStatus')"
            />
          </VCol>
        </VRow>
      </VCardText>
      
      <VCardActions>
        <VSpacer />
        <VBtn
          color="grey-darken-1"
          variant="text"
          @click="editDialog = false"
        >
          {{ t('common.cancel') }}
        </VBtn>
        <VBtn
          color="primary"
          :disabled="hasErrors"
          @click="updateRequest"
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
      <VCardTitle>
        {{ t('pages.requests.confirmDelete') }}
      </VCardTitle>
      
      <VCardText>
        {{ t('pages.requests.deleteConfirmation') }}
      </VCardText>
      
      <VCardActions>
        <VSpacer />
        <VBtn
          color="grey-darken-1"
          variant="text"
          @click="deleteDialog = false"
        >
          {{ t('common.cancel') }}
        </VBtn>
        <VBtn
          color="error"
          @click="deleteRequest"
        >
          {{ t('common.delete') }}
        </VBtn>
      </VCardActions>
    </VCard>
  </VDialog>
</template>

<style scoped>
.v-field--error {
  border-color: rgb(var(--v-theme-error)) !important;
}

.v-messages__message {
  color: rgb(var(--v-theme-error)) !important;
}
</style> 