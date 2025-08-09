<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'DemolitionRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { t } = useI18n()

// Data
const demolitionRequests = ref([])
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
  buildingType: '',
  demolitionReason: '',
  estimatedCost: '',
  safetyMeasures: '',
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

// Building type options
const buildingTypeOptions = [
  { title: t('mosque'), value: 'mosque' },
  { title: t('building'), value: 'building' },
  { title: t('facility'), value: 'facility' },
]

// API functions
const api = useApi()

const fetchDemolitionRequests = async () => {
  try {
    loading.value = true
    const response = await api('/DemolitionRequests')
    demolitionRequests.value = response.data || []
  } catch (error) {
    console.error('Error fetching demolition requests:', error)
  } finally {
    loading.value = false
  }
}

const addDemolitionRequest = async () => {
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

  if (!validateRequired(newRequest.value.buildingType, 'buildingType', t('buildingTypeRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.demolitionReason, 'demolitionReason', t('demolitionReasonRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.estimatedCost, 'estimatedCost', t('estimatedCostRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.safetyMeasures, 'safetyMeasures', t('safetyMeasuresRequired'))) {
    isValid = false
  }

  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('buildingType')
  setFieldTouched('demolitionReason')
  setFieldTouched('estimatedCost')
  setFieldTouched('safetyMeasures')

  if (!isValid) return

  try {
    loading.value = true

    if (isEdit.value) {
      await api(`/DemolitionRequests/${selectedRequest.value.id}`, {
        method: 'PUT',
        body: newRequest.value,
      })
    } else {
      await api('/DemolitionRequests', {
        method: 'POST',
        body: newRequest.value,
      })
    }

    await fetchDemolitionRequests()
    dialog.value = false
    resetForm()
  } catch (error) {
    console.error('Error saving demolition request:', error)
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
      await api(`/DemolitionRequests/${id}`, {
        method: 'DELETE',
      })
      await fetchDemolitionRequests()
    } catch (error) {
      console.error('Error deleting demolition request:', error)
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
    buildingType: '',
    demolitionReason: '',
    estimatedCost: '',
    safetyMeasures: '',
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
  fetchDemolitionRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('demolitionRequests') }}</span>
      <VBtn
        color="primary"
        prepend-icon="tabler-plus"
        @click="openDialog"
      >
        {{ t('addDemolitionRequest') }}
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
          { title: t('buildingType'), key: 'buildingType' },
          { title: t('demolitionReason'), key: 'demolitionReason' },
          { title: t('estimatedCost'), key: 'estimatedCost' },
          { title: t('safetyMeasures'), key: 'safetyMeasures' },
          { title: t('actions'), key: 'actions', sortable: false },
        ]"
        :items="demolitionRequests"
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
          {{ isEdit ? t('editDemolitionRequest') : t('addDemolitionRequest') }}
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="addDemolitionRequest">
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
                  v-model="newRequest.buildingType"
                  :items="buildingTypeOptions"
                  :label="t('buildingType')"
                  :error-messages="errors.buildingType"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.demolitionReason"
                  :label="t('demolitionReason')"
                  :error-messages="errors.demolitionReason"
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

              <VCol cols="12">
                <VTextarea
                  v-model="newRequest.safetyMeasures"
                  :label="t('safetyMeasures')"
                  :error-messages="errors.safetyMeasures"
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
            @click="addDemolitionRequest"
          >
            {{ isEdit ? t('update') : t('save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 