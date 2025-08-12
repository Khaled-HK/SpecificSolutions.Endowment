<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'ExpenditureChangeRequest',
    requiresAuth: true,
  },
})

import { ref, onMounted, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { t, locale } = useI18n()

// Data
const expenditureChangeRequests = ref([])
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
  currentExpenditure: '',
  newExpenditure: '',
  changeReason: '',
  estimatedCost: '',
  approvalRequired: false,
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

// API functions
const api = useApi()

const fetchExpenditureChangeRequests = async () => {
  try {
    loading.value = true
    const params = new URLSearchParams({
      PageNumber: String(options.value.page),
      PageSize: String(options.value.itemsPerPage)
    }).toString()
    const response = await api(`/ExpenditureChangeRequest/filter?${params}`, {
      headers: { 'Accept-Language': locale.value }
    })
    expenditureChangeRequests.value = response.data?.items || []
    totalItems.value = response.data?.totalCount || 0
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
      const idToUpdate = (selectedRequest.value as any)?.id
      if (!idToUpdate) throw new Error('Missing id')
      await api(`/ExpenditureChangeRequest/${idToUpdate}`, {
        method: 'PUT',
        body: newRequest.value,
        headers: { 'Accept-Language': locale.value }
      })
    } else {
      await api('/ExpenditureChangeRequest', {
        method: 'POST',
        body: newRequest.value,
        headers: { 'Accept-Language': locale.value }
      })
    }

    await fetchExpenditureChangeRequests()
    dialog.value = false
    resetForm()
  } catch (e: any) {
    console.error('Error saving expenditure change request:', e)
    const resp: any = e?.response?.data || e?.data
    if (resp?.errors) {
      Object.keys(resp.errors).forEach((field: string) => {
        ;(errors as any)[field] = resp.errors[field]
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

const deleteRequest = async (id: string | number) => {
  if (confirm(t('pages.requests.confirmDelete'))) {
    try {
      await api(`/ExpenditureChangeRequest/${id}`, {
        method: 'DELETE',
        headers: { 'Accept-Language': locale.value }
      })
      await fetchExpenditureChangeRequests()
    } catch (e: any) {
      console.error('Error deleting expenditure change request:', e)
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

watch([() => options.value.page, () => options.value.itemsPerPage], () => {
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
          { title: t('tableHeaders.expenditureChangeRequests.title'), key: 'title' },
          { title: t('tableHeaders.expenditureChangeRequests.description'), key: 'description' },
          { title: t('tableHeaders.expenditureChangeRequests.priority'), key: 'priority' },
          { title: t('tableHeaders.expenditureChangeRequests.location'), key: 'location' },
          { title: t('tableHeaders.expenditureChangeRequests.referenceNumber'), key: 'referenceNumber' },
          { title: t('tableHeaders.expenditureChangeRequests.requestStatus'), key: 'requestStatus' },
          { title: t('tableHeaders.expenditureChangeRequests.currentExpenditure'), key: 'currentExpenditure' },
          { title: t('tableHeaders.expenditureChangeRequests.newExpenditure'), key: 'newExpenditure' },
          { title: t('tableHeaders.expenditureChangeRequests.changeReason'), key: 'changeReason' },
          { title: t('tableHeaders.expenditureChangeRequests.estimatedCost'), key: 'estimatedCost' },
          { title: t('tableHeaders.expenditureChangeRequests.approvalRequired'), key: 'approvalRequired' },
          { title: t('tableHeaders.expenditureChangeRequests.actions'), key: 'actions', sortable: false },
        ]"
        :items="expenditureChangeRequests"
        :loading="loading"
        :items-per-page="options.itemsPerPage"
        :page="options.page"
      >
        <template #item.approvalRequired="{ item }: { item: any }">
          <VChip
            :color="item.raw.approvalRequired ? 'warning' : 'success'"
            size="small"
          >
            {{ item.raw.approvalRequired ? t('yes') : t('no') }}
          </VChip>
        </template>
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
          {{ isEdit ? t('editExpenditureChangeRequest') : t('addExpenditureChangeRequest') }}
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="addExpenditureChangeRequest">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.title"
                  :label="t('tableHeaders.expenditureChangeRequests.title')"
                  :error-messages="errors.title"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.description"
                  :label="t('tableHeaders.expenditureChangeRequests.description')"
                  :error-messages="errors.description"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.priority"
                  :items="priorityOptions"
                  :label="t('tableHeaders.expenditureChangeRequests.priority')"
                  :error-messages="errors.priority"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.location"
                  :label="t('tableHeaders.expenditureChangeRequests.location')"
                  :error-messages="errors.location"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.referenceNumber"
                  :label="t('tableHeaders.expenditureChangeRequests.referenceNumber')"
                  :error-messages="errors.referenceNumber"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newRequest.requestStatus"
                  :items="statusOptions"
                  :label="t('tableHeaders.expenditureChangeRequests.requestStatus')"
                  :error-messages="errors.requestStatus"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.currentExpenditure"
                  :label="t('tableHeaders.expenditureChangeRequests.currentExpenditure')"
                  :error-messages="errors.currentExpenditure"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.newExpenditure"
                  :label="t('tableHeaders.expenditureChangeRequests.newExpenditure')"
                  :error-messages="errors.newExpenditure"
                  required
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newRequest.estimatedCost"
                  :label="t('tableHeaders.expenditureChangeRequests.estimatedCost')"
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
                  :label="t('tableHeaders.expenditureChangeRequests.changeReason')"
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
            @click="addExpenditureChangeRequest"
          >
            {{ isEdit ? t('common.update') : t('common.save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template> 