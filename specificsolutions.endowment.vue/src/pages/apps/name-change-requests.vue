<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'NameChangeRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { t, locale } = useI18n()

// Data
const nameChangeRequests = ref([])
const loading = ref(false)
const totalItems = ref(0)
const options = ref({ page: 1, itemsPerPage: 10 })
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
const { validationState, clearErrors, setFieldTouched, validateRequired, validateEmail } = useFormValidation()
const errors = validationState.errors

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
    const params = new URLSearchParams({
      PageNumber: String(options.value.page),
      PageSize: String(options.value.itemsPerPage)
    }).toString()
    const response = await api(`/NameChangeRequest/filter?${params}`, {
      headers: { 'Accept-Language': locale.value }
    })
    nameChangeRequests.value = response.data?.items || []
    totalItems.value = response.data?.totalCount || 0
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

    if (isEdit.value && selectedRequest.value) {
      const idToUpdate = (selectedRequest.value as any).id
      await api(`/NameChangeRequest/${idToUpdate}`, {
        method: 'PUT',
        body: newRequest.value,
        headers: { 'Accept-Language': locale.value }
      })
    } else {
      await api('/NameChangeRequest', {
        method: 'POST',
        body: newRequest.value,
        headers: { 'Accept-Language': locale.value }
      })
    }

    await fetchNameChangeRequests()
    dialog.value = false
    resetForm()
  } catch (error: any) {
    console.error('Error saving name change request:', error)
    if (error.data?.errors) {
      Object.keys(error.data.errors).forEach((field: string) => {
        ;(errors as any)[field] = error.data.errors[field]
      })
    }
  } finally {
    loading.value = false
  }
}

const editRequest = (request: any) => {
  isEdit.value = true
  selectedRequest.value = request
  newRequest.value = { ...request }
  dialog.value = true
}

const deleteRequest = async (id: number) => {
  if (confirm(t('pages.requests.confirmDelete'))) {
    try {
      await api(`/NameChangeRequest/${id}`, {
        method: 'DELETE',
        headers: { 'Accept-Language': locale.value }
      })
      await fetchNameChangeRequests()
    } catch (error: any) {
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

watch([() => options.value.page, () => options.value.itemsPerPage], () => {
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
          { title: t('tableHeaders.nameChangeRequests.title'), key: 'title' },
          { title: t('tableHeaders.nameChangeRequests.description'), key: 'description' },
          { title: t('tableHeaders.nameChangeRequests.priority'), key: 'priority' },
          { title: t('tableHeaders.nameChangeRequests.location'), key: 'location' },
          { title: t('tableHeaders.nameChangeRequests.referenceNumber'), key: 'referenceNumber' },
          { title: t('tableHeaders.nameChangeRequests.requestStatus'), key: 'requestStatus' },
          { title: t('tableHeaders.nameChangeRequests.currentName'), key: 'currentName' },
          { title: t('tableHeaders.nameChangeRequests.newName'), key: 'newName' },
          { title: t('tableHeaders.nameChangeRequests.changeReason'), key: 'changeReason' },
          { title: t('tableHeaders.nameChangeRequests.buildingType'), key: 'buildingType' },
          { title: t('tableHeaders.nameChangeRequests.actions'), key: 'actions', sortable: false },
        ]"
        :items="nameChangeRequests"
        :loading="loading"
        :items-per-page="options.itemsPerPage"
        :page="options.page"
      >
        <template #item.actions="{ item }: { item: any }">
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
        <template #bottom>
          <VCardText class="pt-2">
            <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
              <VSelect
                v-model="options.itemsPerPage"
                :items="[5,10,25,50,100]"
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
                  :label="t('tableHeaders.nameChangeRequests.title')"
                  :error-messages="errors.title"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.description"
                  :label="t('tableHeaders.nameChangeRequests.description')"
                  :error-messages="errors.description"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.priority"
                  :items="priorityOptions"
                  :label="t('tableHeaders.nameChangeRequests.priority')"
                  :error-messages="errors.priority"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.location"
                  :label="t('tableHeaders.nameChangeRequests.location')"
                  :error-messages="errors.location"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.referenceNumber"
                  :label="t('tableHeaders.nameChangeRequests.referenceNumber')"
                  :error-messages="errors.referenceNumber"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.requestStatus"
                  :items="statusOptions"
                  :label="t('tableHeaders.nameChangeRequests.requestStatus')"
                  :error-messages="errors.requestStatus"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.currentName"
                  :label="t('tableHeaders.nameChangeRequests.currentName')"
                  :error-messages="errors.currentName"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.newName"
                  :label="t('tableHeaders.nameChangeRequests.newName')"
                  :error-messages="errors.newName"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.buildingType"
                  :items="buildingTypeOptions"
                  :label="t('tableHeaders.nameChangeRequests.buildingType')"
                  :error-messages="errors.buildingType"
                  required
                />
              </VCol>

              <VCol cols="12">
                <VTextarea
                  v-model="newRequest.changeReason"
                  :label="t('tableHeaders.nameChangeRequests.changeReason')"
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
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="primary"
            :loading="loading"
            :disabled="Object.keys(errors).length > 0"
            @click="addNameChangeRequest"
          >
            {{ isEdit ? t('common.update') : t('common.save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 