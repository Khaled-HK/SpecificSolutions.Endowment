<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'NeedsRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'

const { t } = useI18n()

// Data
const needsRequests = ref([])
const loading = ref(false)
const dialog = ref(false)
const isEdit = ref(false)
const selectedRequest = ref(null)

// Form data
const newRequest = ref({
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
  needsType: '',
  estimatedCost: '',
  provider: '',
})

// Form validation
const { errors, clearErrors, setFieldTouched, validateRequired, validateEmail } = useFormValidation()

// Priority options
const priorityOptions = [
  { title: t('high'), value: 'high' },
  { title: t('medium'), value: 'medium' },
  { title: t('low'), value: 'low' },
]

// Status options
const statusOptions = [
  { title: t('pending'), value: 'pending' },
  { title: t('approved'), value: 'approved' },
  { title: t('rejected'), value: 'rejected' },
  { title: t('inProgress'), value: 'inProgress' },
  { title: t('completed'), value: 'completed' },
]

// Needs type options
const needsTypeOptions = [
  { title: t('equipment'), value: 'equipment' },
  { title: t('furniture'), value: 'furniture' },
  { title: t('supplies'), value: 'supplies' },
  { title: t('maintenance'), value: 'maintenance' },
  { title: t('other'), value: 'other' },
]

// API functions
const api = useApi()

const fetchNeedsRequests = async () => {
  try {
    loading.value = true
    const response = await api('/NeedsRequests')
    needsRequests.value = response.data || []
  } catch (error) {
    console.error('Error fetching needs requests:', error)
  } finally {
    loading.value = false
  }
}

const addNeedsRequest = async () => {
  clearErrors()

  let isValid = true

  if (!validateRequired(newRequest.value.title, 'title', t('titleRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.description, 'description', t('descriptionRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.priority, 'priority', t('priorityRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.location, 'location', t('locationRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.referenceNumber, 'referenceNumber', t('referenceNumberRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.requestStatus, 'requestStatus', t('requestStatusRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.needsType, 'needsType', t('needsTypeRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.estimatedCost, 'estimatedCost', t('estimatedCostRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.provider, 'provider', t('providerRequired'))) {
    isValid = false
  }

  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('needsType')
  setFieldTouched('estimatedCost')
  setFieldTouched('provider')

  if (!isValid) return

  try {
    loading.value = true

    if (isEdit.value) {
      await api(`/NeedsRequests/${selectedRequest.value.id}`, {
        method: 'PUT',
        body: newRequest.value,
      })
    } else {
      await api('/NeedsRequests', {
        method: 'POST',
        body: newRequest.value,
      })
    }

    await fetchNeedsRequests()
    dialog.value = false
    resetForm()
  } catch (error) {
    console.error('Error saving needs request:', error)
    if (error.data?.errors) {
      Object.keys(error.data.errors).forEach(field => {
        errors.value[field] = error.data.errors[field]
      })
    }
  } finally {
    loading.value = false
  }
}

const editRequest = (request) => {
  isEdit.value = true
  selectedRequest.value = request
  newRequest.value = { ...request }
  dialog.value = true
}

const deleteRequest = async (id) => {
  if (confirm(t('confirmDelete'))) {
    try {
      await api(`/NeedsRequests/${id}`, {
        method: 'DELETE',
      })
      await fetchNeedsRequests()
    } catch (error) {
      console.error('Error deleting needs request:', error)
    }
  }
}

const resetForm = () => {
  newRequest.value = {
    title: '',
    description: '',
    priority: '',
    location: '',
    referenceNumber: '',
    requestStatus: '',
    needsType: '',
    estimatedCost: '',
    provider: '',
  }
  isEdit.value = false
  selectedRequest.value = null
  clearErrors()
}

const openDialog = () => {
  resetForm()
  dialog.value = true
}

onMounted(() => {
  fetchNeedsRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('needsRequests') }}</span>
      <VBtn
        color="primary"
        prepend-icon="tabler-plus"
        @click="openDialog"
      >
        {{ t('addNeedsRequest') }}
      </VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable
        :headers="[
          { title: t('title'), key: 'title' },
          { title: t('description'), key: 'description' },
          { title: t('priority'), key: 'priority' },
          { title: t('location'), key: 'location' },
          { title: t('referenceNumber'), key: 'referenceNumber' },
          { title: t('requestStatus'), key: 'requestStatus' },
          { title: t('needsType'), key: 'needsType' },
          { title: t('estimatedCost'), key: 'estimatedCost' },
          { title: t('provider'), key: 'provider' },
          { title: t('actions'), key: 'actions', sortable: false },
        ]"
        :items="needsRequests"
        :loading="loading"
      >
        <template #item.actions="{ item }">
          <VBtn
            icon
            variant="text"
            size="small"
            color="primary"
            @click="editRequest(item.raw)"
          >
            <VIcon icon="tabler-edit" />
          </VBtn>
          <VBtn
            icon
            variant="text"
            size="small"
            color="error"
            @click="deleteRequest(item.raw.id)"
          >
            <VIcon icon="tabler-trash" />
          </VBtn>
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
          {{ isEdit ? t('editNeedsRequest') : t('addNeedsRequest') }}
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="addNeedsRequest">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.title"
                  :label="t('title')"
                  :error-messages="errors.title"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.description"
                  :label="t('description')"
                  :error-messages="errors.description"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.priority"
                  :items="priorityOptions"
                  :label="t('priority')"
                  :error-messages="errors.priority"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.location"
                  :label="t('location')"
                  :error-messages="errors.location"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.referenceNumber"
                  :label="t('referenceNumber')"
                  :error-messages="errors.referenceNumber"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.requestStatus"
                  :items="statusOptions"
                  :label="t('requestStatus')"
                  :error-messages="errors.requestStatus"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.needsType"
                  :items="needsTypeOptions"
                  :label="t('needsType')"
                  :error-messages="errors.needsType"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.estimatedCost"
                  :label="t('estimatedCost')"
                  :error-messages="errors.estimatedCost"
                  type="number"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.provider"
                  :label="t('provider')"
                  :error-messages="errors.provider"
                  required
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
            {{ t('cancel') }}
          </VBtn>
          <VBtn
            color="primary"
            :loading="loading"
            :disabled="Object.keys(errors).length > 0"
            @click="addNeedsRequest"
          >
            {{ isEdit ? t('update') : t('save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 