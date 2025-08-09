<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'NameChangeRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { t } = useI18n()

// Data
const nameChangeRequests = ref([])
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
  currentName: '',
  newName: '',
  changeReason: '',
  buildingType: '',
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

const fetchNameChangeRequests = async () => {
  try {
    loading.value = true
    const response = await api('/NameChangeRequests')
    nameChangeRequests.value = response.data || []
  } catch (error) {
    console.error('Error fetching name change requests:', error)
  } finally {
    loading.value = false
  }
}

const addNameChangeRequest = async () => {
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

  if (!validateRequired(newRequest.value.currentName, 'currentName', t('currentNameRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.newName, 'newName', t('newNameRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.changeReason, 'changeReason', t('changeReasonRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.buildingType, 'buildingType', t('buildingTypeRequired'))) {
    isValid = false
  }

  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('currentName')
  setFieldTouched('newName')
  setFieldTouched('changeReason')
  setFieldTouched('buildingType')

  if (!isValid) return

  try {
    loading.value = true

    if (isEdit.value) {
      await api(`/NameChangeRequests/${selectedRequest.value.id}`, {
        method: 'PUT',
        body: newRequest.value,
      })
    } else {
      await api('/NameChangeRequests', {
        method: 'POST',
        body: newRequest.value,
      })
    }

    await fetchNameChangeRequests()
    dialog.value = false
    resetForm()
  } catch (error) {
    console.error('Error saving name change request:', error)
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
      await api(`/NameChangeRequests/${id}`, {
        method: 'DELETE',
      })
      await fetchNameChangeRequests()
    } catch (error) {
      console.error('Error deleting name change request:', error)
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
    currentName: '',
    newName: '',
    changeReason: '',
    buildingType: '',
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
  fetchNameChangeRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('nameChangeRequests') }}</span>
      <VBtn
        color="primary"
        prepend-icon="tabler-plus"
        @click="openDialog"
      >
        {{ t('addNameChangeRequest') }}
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
          { title: t('currentName'), key: 'currentName' },
          { title: t('newName'), key: 'newName' },
          { title: t('changeReason'), key: 'changeReason' },
          { title: t('buildingType'), key: 'buildingType' },
          { title: t('actions'), key: 'actions', sortable: false },
        ]"
        :items="nameChangeRequests"
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
          {{ isEdit ? t('editNameChangeRequest') : t('addNameChangeRequest') }}
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="addNameChangeRequest">
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
                <VTextField
                  v-model="newRequest.currentName"
                  :label="t('currentName')"
                  :error-messages="errors.currentName"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.newName"
                  :label="t('newName')"
                  :error-messages="errors.newName"
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

              <VCol cols="12">
                <VTextarea
                  v-model="newRequest.changeReason"
                  :label="t('changeReason')"
                  :error-messages="errors.changeReason"
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
            @click="addNameChangeRequest"
          >
            {{ isEdit ? t('update') : t('save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 