<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'ExpenditureChangeRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'

const { t } = useI18n()

// Data
const expenditureChangeRequests = ref([])
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
  currentExpenditure: '',
  newExpenditure: '',
  changeReason: '',
  estimatedCost: '',
  approvalRequired: false,
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

// API functions
const api = useApi()

const fetchExpenditureChangeRequests = async () => {
  try {
    loading.value = true
    const response = await api('/ExpenditureChangeRequests')
    expenditureChangeRequests.value = response.data || []
  } catch (error) {
    console.error('Error fetching expenditure change requests:', error)
  } finally {
    loading.value = false
  }
}

const addExpenditureChangeRequest = async () => {
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

  if (!validateRequired(newRequest.value.currentExpenditure, 'currentExpenditure', t('currentExpenditureRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.newExpenditure, 'newExpenditure', t('newExpenditureRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.changeReason, 'changeReason', t('changeReasonRequired'))) {
    isValid = false
  }

  if (!validateRequired(newRequest.value.estimatedCost, 'estimatedCost', t('estimatedCostRequired'))) {
    isValid = false
  }

  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('currentExpenditure')
  setFieldTouched('newExpenditure')
  setFieldTouched('changeReason')
  setFieldTouched('estimatedCost')

  if (!isValid) return

  try {
    loading.value = true

    if (isEdit.value) {
      await api(`/ExpenditureChangeRequests/${selectedRequest.value.id}`, {
        method: 'PUT',
        body: newRequest.value,
      })
    } else {
      await api('/ExpenditureChangeRequests', {
        method: 'POST',
        body: newRequest.value,
      })
    }

    await fetchExpenditureChangeRequests()
    dialog.value = false
    resetForm()
  } catch (error) {
    console.error('Error saving expenditure change request:', error)
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
      await api(`/ExpenditureChangeRequests/${id}`, {
        method: 'DELETE',
      })
      await fetchExpenditureChangeRequests()
    } catch (error) {
      console.error('Error deleting expenditure change request:', error)
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
    currentExpenditure: '',
    newExpenditure: '',
    changeReason: '',
    estimatedCost: '',
    approvalRequired: false,
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
  fetchExpenditureChangeRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('expenditureChangeRequests') }}</span>
      <VBtn
        color="primary"
        prepend-icon="tabler-plus"
        @click="openDialog"
      >
        {{ t('addExpenditureChangeRequest') }}
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
          { title: t('currentExpenditure'), key: 'currentExpenditure' },
          { title: t('newExpenditure'), key: 'newExpenditure' },
          { title: t('changeReason'), key: 'changeReason' },
          { title: t('estimatedCost'), key: 'estimatedCost' },
          { title: t('approvalRequired'), key: 'approvalRequired' },
          { title: t('actions'), key: 'actions', sortable: false },
        ]"
        :items="expenditureChangeRequests"
        :loading="loading"
      >
        <template #item.approvalRequired="{ item }">
          <VChip
            :color="item.raw.approvalRequired ? 'warning' : 'success'"
            size="small"
          >
            {{ item.raw.approvalRequired ? t('yes') : t('no') }}
          </VChip>
        </template>
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
          {{ isEdit ? t('editExpenditureChangeRequest') : t('addExpenditureChangeRequest') }}
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="addExpenditureChangeRequest">
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
                  v-model="newRequest.currentExpenditure"
                  :label="t('currentExpenditure')"
                  :error-messages="errors.currentExpenditure"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.newExpenditure"
                  :label="t('newExpenditure')"
                  :error-messages="errors.newExpenditure"
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
                <VCheckbox
                  v-model="newRequest.approvalRequired"
                  :label="t('approvalRequired')"
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
            @click="addExpenditureChangeRequest"
          >
            {{ isEdit ? t('update') : t('save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 