<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'

definePage({
  meta: {
    action: 'View',
    subject: 'ConstructionRequest',
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

const constructionRequests = ref([])
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
  buildingType: '',
  proposedLocation: '',
  proposedArea: 0,
  estimatedCost: 0,
  contractorName: '',
})

const editRequest = ref({
  id: null,
  title: '',
  description: '',
  priority: '',
  location: '',
  referenceNumber: '',
  requestStatus: '',
  buildingType: '',
  proposedLocation: '',
  proposedArea: 0,
  estimatedCost: 0,
  contractorName: '',
})

const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.constructionRequests.title'), key: 'title' },
  { title: t('tableHeaders.constructionRequests.description'), key: 'description' },
  { title: t('tableHeaders.constructionRequests.priority'), key: 'priority' },
  { title: t('tableHeaders.constructionRequests.location'), key: 'location' },
  { title: t('tableHeaders.constructionRequests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.constructionRequests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.constructionRequests.buildingType'), key: 'buildingType' },
  { title: t('tableHeaders.constructionRequests.proposedLocation'), key: 'proposedLocation' },
  { title: t('tableHeaders.constructionRequests.proposedArea'), key: 'proposedArea' },
  { title: t('tableHeaders.constructionRequests.estimatedCost'), key: 'estimatedCost' },
  { title: t('tableHeaders.constructionRequests.contractorName'), key: 'contractorName' },
  { title: t('tableHeaders.constructionRequests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.constructionRequests.actions'), key: 'actions', sortable: false },
])

const loadConstructionRequests = async () => {
  loading.value = true
  try {
    const response = await $api('/ConstructionRequests/GetConstructionRequests', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    constructionRequests.value = response
  } catch (error) {
    console.error('Error loading construction requests:', error)
  } finally {
    loading.value = false
  }
}

const addConstructionRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('buildingType')
  setFieldTouched('proposedLocation')
  setFieldTouched('proposedArea')
  setFieldTouched('estimatedCost')
  setFieldTouched('contractorName')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.constructionRequests.title') }))
  }
  if (!validateRequired(newRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.constructionRequests.description') }))
  }
  if (!validateRequired(newRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.constructionRequests.priority') }))
  }
  if (!validateRequired(newRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.constructionRequests.location') }))
  }
  if (!validateRequired(newRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.constructionRequests.referenceNumber') }))
  }
  if (!validateRequired(newRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.constructionRequests.requestStatus') }))
  }
  if (!validateRequired(newRequest.value.buildingType, 'buildingType')) {
    addError('buildingType', t('validation.required', { field: t('pages.constructionRequests.buildingType') }))
  }
  if (!validateRequired(newRequest.value.proposedLocation, 'proposedLocation')) {
    addError('proposedLocation', t('validation.required', { field: t('pages.constructionRequests.proposedLocation') }))
  }
  if (!validateRequired(newRequest.value.contractorName, 'contractorName')) {
    addError('contractorName', t('validation.required', { field: t('pages.constructionRequests.contractorName') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await $api('/ConstructionRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    dialog.value = false
    resetNewRequest()
    await loadConstructionRequests()
  } catch (error) {
    console.error('Error adding construction request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        BuildingType: 'buildingType',
        ProposedLocation: 'proposedLocation',
        ProposedArea: 'proposedArea',
        EstimatedCost: 'estimatedCost',
        ContractorName: 'contractorName',
      })
    }
  }
}

const updateConstructionRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('buildingType')
  setFieldTouched('proposedLocation')
  setFieldTouched('proposedArea')
  setFieldTouched('estimatedCost')
  setFieldTouched('contractorName')

  if (!validateRequired(editRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.constructionRequests.title') }))
  }
  if (!validateRequired(editRequest.value.description, 'description')) {
    addError('description', t('validation.required', { field: t('pages.constructionRequests.description') }))
  }
  if (!validateRequired(editRequest.value.priority, 'priority')) {
    addError('priority', t('validation.required', { field: t('pages.constructionRequests.priority') }))
  }
  if (!validateRequired(editRequest.value.location, 'location')) {
    addError('location', t('validation.required', { field: t('pages.constructionRequests.location') }))
  }
  if (!validateRequired(editRequest.value.referenceNumber, 'referenceNumber')) {
    addError('referenceNumber', t('validation.required', { field: t('pages.constructionRequests.referenceNumber') }))
  }
  if (!validateRequired(editRequest.value.requestStatus, 'requestStatus')) {
    addError('requestStatus', t('validation.required', { field: t('pages.constructionRequests.requestStatus') }))
  }
  if (!validateRequired(editRequest.value.buildingType, 'buildingType')) {
    addError('buildingType', t('validation.required', { field: t('pages.constructionRequests.buildingType') }))
  }
  if (!validateRequired(editRequest.value.proposedLocation, 'proposedLocation')) {
    addError('proposedLocation', t('validation.required', { field: t('pages.constructionRequests.proposedLocation') }))
  }
  if (!validateRequired(editRequest.value.contractorName, 'contractorName')) {
    addError('contractorName', t('validation.required', { field: t('pages.constructionRequests.contractorName') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await $api(`/ConstructionRequests/${editRequest.value.id}`, {
      method: 'PUT',
      body: editRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    editDialog.value = false
    await loadConstructionRequests()
  } catch (error) {
    console.error('Error updating construction request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        BuildingType: 'buildingType',
        ProposedLocation: 'proposedLocation',
        ProposedArea: 'proposedArea',
        EstimatedCost: 'estimatedCost',
        ContractorName: 'contractorName',
      })
    }
  }
}

const deleteConstructionRequest = async () => {
  try {
    await $api(`/ConstructionRequests/${selectedRequest.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    deleteDialog.value = false
    selectedRequest.value = null
    await loadConstructionRequests()
  } catch (error) {
    console.error('Error deleting construction request:', error)
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
    buildingType: request.buildingType,
    proposedLocation: request.proposedLocation,
    proposedArea: request.proposedArea,
    estimatedCost: request.estimatedCost,
    contractorName: request.contractorName,
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
    buildingType: '',
    proposedLocation: '',
    proposedArea: 0,
    estimatedCost: 0,
    contractorName: '',
  }
}

onMounted(() => {
  loadConstructionRequests()
})
</script>

<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('pages.constructionRequests.title') }}</span>
      <VBtn
        color="primary"
        @click="dialog = true"
      >
        {{ t('pages.constructionRequests.addRequest') }}
      </VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable
        :headers="headers"
        :items="constructionRequests"
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

  <!-- Add Construction Request Dialog -->
  <VDialog
    v-model="dialog"
    max-width="800px"
  >
    <VCard>
      <VCardTitle>
        {{ t('pages.constructionRequests.addNewRequest') }}
      </VCardTitle>

      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField
              v-model="newRequest.title"
              :label="t('pages.constructionRequests.title')"
              :error="validationState.errors.title"
              :error-messages="validationState.errors.title"
              @blur="setFieldTouched('title')"
            />
          </VCol>

          <VCol cols="12">
            <VTextarea
              v-model="newRequest.description"
              :label="t('pages.constructionRequests.description')"
              :error="validationState.errors.description"
              :error-messages="validationState.errors.description"
              @blur="setFieldTouched('description')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.priority"
              :label="t('pages.constructionRequests.priority')"
              :error="validationState.errors.priority"
              :error-messages="validationState.errors.priority"
              @blur="setFieldTouched('priority')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.location"
              :label="t('pages.constructionRequests.location')"
              :error="validationState.errors.location"
              :error-messages="validationState.errors.location"
              @blur="setFieldTouched('location')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.referenceNumber"
              :label="t('pages.constructionRequests.referenceNumber')"
              :error="validationState.errors.referenceNumber"
              :error-messages="validationState.errors.referenceNumber"
              @blur="setFieldTouched('referenceNumber')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.requestStatus"
              :label="t('pages.constructionRequests.requestStatus')"
              :error="validationState.errors.requestStatus"
              :error-messages="validationState.errors.requestStatus"
              @blur="setFieldTouched('requestStatus')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.buildingType"
              :label="t('pages.constructionRequests.buildingType')"
              :error="validationState.errors.buildingType"
              :error-messages="validationState.errors.buildingType"
              @blur="setFieldTouched('buildingType')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="newRequest.proposedLocation"
              :label="t('pages.constructionRequests.proposedLocation')"
              :error="validationState.errors.proposedLocation"
              :error-messages="validationState.errors.proposedLocation"
              @blur="setFieldTouched('proposedLocation')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model.number="newRequest.proposedArea"
              :label="t('pages.constructionRequests.proposedArea')"
              type="number"
              :error="validationState.errors.proposedArea"
              :error-messages="validationState.errors.proposedArea"
              @blur="setFieldTouched('proposedArea')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model.number="newRequest.estimatedCost"
              :label="t('pages.constructionRequests.estimatedCost')"
              type="number"
              :error="validationState.errors.estimatedCost"
              :error-messages="validationState.errors.estimatedCost"
              @blur="setFieldTouched('estimatedCost')"
            />
          </VCol>

          <VCol cols="12">
            <VTextField
              v-model="newRequest.contractorName"
              :label="t('pages.constructionRequests.contractorName')"
              :error="validationState.errors.contractorName"
              :error-messages="validationState.errors.contractorName"
              @blur="setFieldTouched('contractorName')"
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
          @click="addConstructionRequest"
        >
          {{ t('common.save') }}
        </VBtn>
      </VCardActions>
    </VCard>
  </VDialog>

  <!-- Edit Construction Request Dialog -->
  <VDialog
    v-model="editDialog"
    max-width="800px"
  >
    <VCard>
      <VCardTitle>
        {{ t('pages.constructionRequests.editRequest') }}
      </VCardTitle>

      <VCardText>
        <VRow>
          <VCol cols="12">
            <VTextField
              v-model="editRequest.title"
              :label="t('pages.constructionRequests.title')"
              :error="validationState.errors.title"
              :error-messages="validationState.errors.title"
              @blur="setFieldTouched('title')"
            />
          </VCol>

          <VCol cols="12">
            <VTextarea
              v-model="editRequest.description"
              :label="t('pages.constructionRequests.description')"
              :error="validationState.errors.description"
              :error-messages="validationState.errors.description"
              @blur="setFieldTouched('description')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.priority"
              :label="t('pages.constructionRequests.priority')"
              :error="validationState.errors.priority"
              :error-messages="validationState.errors.priority"
              @blur="setFieldTouched('priority')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.location"
              :label="t('pages.constructionRequests.location')"
              :error="validationState.errors.location"
              :error-messages="validationState.errors.location"
              @blur="setFieldTouched('location')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.referenceNumber"
              :label="t('pages.constructionRequests.referenceNumber')"
              :error="validationState.errors.referenceNumber"
              :error-messages="validationState.errors.referenceNumber"
              @blur="setFieldTouched('referenceNumber')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.requestStatus"
              :label="t('pages.constructionRequests.requestStatus')"
              :error="validationState.errors.requestStatus"
              :error-messages="validationState.errors.requestStatus"
              @blur="setFieldTouched('requestStatus')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.buildingType"
              :label="t('pages.constructionRequests.buildingType')"
              :error="validationState.errors.buildingType"
              :error-messages="validationState.errors.buildingType"
              @blur="setFieldTouched('buildingType')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model="editRequest.proposedLocation"
              :label="t('pages.constructionRequests.proposedLocation')"
              :error="validationState.errors.proposedLocation"
              :error-messages="validationState.errors.proposedLocation"
              @blur="setFieldTouched('proposedLocation')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model.number="editRequest.proposedArea"
              :label="t('pages.constructionRequests.proposedArea')"
              type="number"
              :error="validationState.errors.proposedArea"
              :error-messages="validationState.errors.proposedArea"
              @blur="setFieldTouched('proposedArea')"
            />
          </VCol>

          <VCol cols="6">
            <VTextField
              v-model.number="editRequest.estimatedCost"
              :label="t('pages.constructionRequests.estimatedCost')"
              type="number"
              :error="validationState.errors.estimatedCost"
              :error-messages="validationState.errors.estimatedCost"
              @blur="setFieldTouched('estimatedCost')"
            />
          </VCol>

          <VCol cols="12">
            <VTextField
              v-model="editRequest.contractorName"
              :label="t('pages.constructionRequests.contractorName')"
              :error="validationState.errors.contractorName"
              :error-messages="validationState.errors.contractorName"
              @blur="setFieldTouched('contractorName')"
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
          @click="updateConstructionRequest"
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
        {{ t('pages.constructionRequests.confirmDelete') }}
      </VCardTitle>

      <VCardText>
        {{ t('pages.constructionRequests.deleteConfirmation') }}
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
          @click="deleteConstructionRequest"
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