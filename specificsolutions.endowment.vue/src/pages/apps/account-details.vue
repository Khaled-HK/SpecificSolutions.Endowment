<template>
  <VCard>
    <VCardTitle class="d-flex align-center justify-space-between">
      <span>{{ t('pages.accountDetails.title') }}</span>
      <VBtn
        color="primary"
        @click="openAddDialog"
      >
        {{ t('pages.accountDetails.addAccountDetail') }}
      </VBtn>
    </VCardTitle>

    <VCardText>
      <VDataTable
        :headers="headers"
        :items="accountDetails"
        :loading="loading"
        :items-per-page="10"
        class="text-no-wrap"
      >
        <template #item.debtor="{ item }">
          {{ formatCurrency(item.debtor) }}
        </template>

        <template #item.creditor="{ item }">
          {{ formatCurrency(item.creditor) }}
        </template>

        <template #item.balance="{ item }">
          {{ formatCurrency(item.balance) }}
        </template>

        <template #item.operationType="{ item }">
          <VChip
            :color="item.operationType === 'Credit' ? 'success' : 'error'"
            size="small"
          >
            {{ t(`pages.accountDetails.operationType${item.operationType}`) }}
          </VChip>
        </template>

        <template #item.date="{ item }">
          {{ formatDate(item.date) }}
        </template>

        <template #item.actions="{ item }">
          <VBtn
            icon="mdi-pencil"
            size="small"
            color="primary"
            variant="text"
            @click="openEditDialog(item)"
          />
          <VBtn
            icon="mdi-delete"
            size="small"
            color="error"
            variant="text"
            @click="confirmDelete(item)"
          />
        </template>
      </VDataTable>
    </VCardText>

    <!-- Add/Edit Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>
          <span v-if="isEdit">{{ t('pages.accountDetails.editAccountDetail') }}</span>
          <span v-else>{{ t('pages.accountDetails.addNewAccountDetail') }}</span>
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="isEdit ? updateAccountDetail() : addAccountDetail()">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccountDetail.debtor"
                  :label="t('pages.accountDetails.debtor')"
                  type="number"
                  step="0.01"
                  :error="!!validationState.errors.debtor"
                  :error-messages="validationState.errors.debtor"
                  @blur="setFieldTouched('debtor')"
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccountDetail.creditor"
                  :label="t('pages.accountDetails.creditor')"
                  type="number"
                  step="0.01"
                  :error="!!validationState.errors.creditor"
                  :error-messages="validationState.errors.creditor"
                  @blur="setFieldTouched('creditor')"
                />
              </VCol>

              <VCol cols="12" md="6">
                <VSelect
                  v-model="newAccountDetail.operationType"
                  :label="t('pages.accountDetails.operationType')"
                  :items="operationTypeOptions"
                  item-title="text"
                  item-value="value"
                  :error="!!validationState.errors.operationType"
                  :error-messages="validationState.errors.operationType"
                  @blur="setFieldTouched('operationType')"
                />
              </VCol>

              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccountDetail.operationNumber"
                  :label="t('pages.accountDetails.operationNumber')"
                  type="number"
                  :error="!!validationState.errors.operationNumber"
                  :error-messages="validationState.errors.operationNumber"
                  @blur="setFieldTouched('operationNumber')"
                />
              </VCol>

              <VCol cols="12">
                <VTextarea
                  v-model="newAccountDetail.note"
                  :label="t('pages.accountDetails.note')"
                  rows="3"
                  :error="!!validationState.errors.note"
                  :error-messages="validationState.errors.note"
                  @blur="setFieldTouched('note')"
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
            @click="closeDialog"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            v-if="!isEdit"
            color="primary"
            :disabled="hasErrors"
            @click="addAccountDetail"
          >
            {{ t('common.save') }}
          </VBtn>
          <VBtn
            v-else
            color="primary"
            :disabled="hasErrors"
            @click="updateAccountDetail"
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
        <VCardTitle>{{ t('pages.accountDetails.confirmDelete') }}</VCardTitle>
        <VCardText>
          {{ t('pages.accountDetails.deleteConfirmation') }}
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
            @click="deleteAccountDetail"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </VCard>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'

// Composables
const { t, locale } = useI18n()
const {
  validationState,
  clearErrors,
  setFieldTouched,
  validateRequired,
  validateNumeric,
  addError,
  hasErrors,
  setErrorsFromResponse
} = useFormValidation()

// Data
const accountDetails = ref([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const isEdit = ref(false)
const selectedItem = ref(null)

const newAccountDetail = ref({
  debtor: 0,
  creditor: 0,
  note: '',
  operationType: '',
  operationNumber: 0,
  balance: 0
})

// Computed
const headers = computed(() => [
  {
    title: t('tableHeaders.accountDetails.debtor'),
    key: 'debtor',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.creditor'),
    key: 'creditor',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.date'),
    key: 'date',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.operationType'),
    key: 'operationType',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.operationNumber'),
    key: 'operationNumber',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.balance'),
    key: 'balance',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.note'),
    key: 'note',
    sortable: true
  },
  {
    title: t('tableHeaders.accountDetails.actions'),
    key: 'actions',
    sortable: false
  }
])

const operationTypeOptions = computed(() => [
  { text: t('pages.accountDetails.operationTypeCredit'), value: 'Credit' },
  { text: t('pages.accountDetails.operationTypeDebit'), value: 'Debit' }
])

// Methods
const loadAccountDetails = async () => {
  loading.value = true
  try {
    const response = await $api('/AccountDetails/filter', {
      method: 'GET',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    if (response && response.data) {
      accountDetails.value = response.data.items || response.data
    }
  } catch (error) {
    console.error('Error loading account details:', error)
  } finally {
    loading.value = false
  }
}

const openAddDialog = () => {
  isEdit.value = false
  dialog.value = true
  clearErrors()
  resetNewAccountDetail()
}

const openEditDialog = (item: any) => {
  isEdit.value = true
  selectedItem.value = item
  dialog.value = true
  clearErrors()
  
  newAccountDetail.value = {
    debtor: item.debtor,
    creditor: item.creditor,
    note: item.note || '',
    operationType: item.operationType,
    operationNumber: item.operationNumber,
    balance: item.balance
  }
}

const closeDialog = () => {
  dialog.value = false
  resetNewAccountDetail()
  clearErrors()
}

const resetNewAccountDetail = () => {
  newAccountDetail.value = {
    debtor: 0,
    creditor: 0,
    note: '',
    operationType: '',
    operationNumber: 0,
    balance: 0
  }
}

const addAccountDetail = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('debtor')
  setFieldTouched('creditor')
  setFieldTouched('operationType')
  setFieldTouched('operationNumber')

  if (!validateNumeric(newAccountDetail.value.debtor.toString(), 'debtor')) {
    addError('debtor', t('validation.required', { field: t('pages.accountDetails.debtor') }))
  }
  if (!validateNumeric(newAccountDetail.value.creditor.toString(), 'creditor')) {
    addError('creditor', t('validation.required', { field: t('pages.accountDetails.creditor') }))
  }
  if (!newAccountDetail.value.operationType) {
    addError('operationType', t('validation.required', { field: t('pages.accountDetails.operationType') }))
  }
  if (!validateNumeric(newAccountDetail.value.operationNumber.toString(), 'operationNumber')) {
    addError('operationNumber', t('validation.required', { field: t('pages.accountDetails.operationNumber') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await $api('/AccountDetails', {
      method: 'POST',
      body: newAccountDetail.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewAccountDetail()
    await loadAccountDetails()
  } catch (error) {
    console.error('Error adding account detail:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Debtor: 'debtor',
        Creditor: 'creditor',
        Note: 'note',
        OperationType: 'operationType',
        OperationNumber: 'operationNumber',
        Balance: 'balance'
      })
    }
  }
}

const updateAccountDetail = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('debtor')
  setFieldTouched('creditor')
  setFieldTouched('operationType')
  setFieldTouched('operationNumber')

  if (!validateNumeric(newAccountDetail.value.debtor.toString(), 'debtor')) {
    addError('debtor', t('validation.required', { field: t('pages.accountDetails.debtor') }))
  }
  if (!validateNumeric(newAccountDetail.value.creditor.toString(), 'creditor')) {
    addError('creditor', t('validation.required', { field: t('pages.accountDetails.creditor') }))
  }
  if (!newAccountDetail.value.operationType) {
    addError('operationType', t('validation.required', { field: t('pages.accountDetails.operationType') }))
  }
  if (!validateNumeric(newAccountDetail.value.operationNumber.toString(), 'operationNumber')) {
    addError('operationNumber', t('validation.required', { field: t('pages.accountDetails.operationNumber') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await $api(`/AccountDetails/${selectedItem.value.id}`, {
      method: 'PUT',
      body: newAccountDetail.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewAccountDetail()
    await loadAccountDetails()
  } catch (error) {
    console.error('Error updating account detail:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Debtor: 'debtor',
        Creditor: 'creditor',
        Note: 'note',
        OperationType: 'operationType',
        OperationNumber: 'operationNumber',
        Balance: 'balance'
      })
    }
  }
}

const confirmDelete = (item: any) => {
  selectedItem.value = item
  deleteDialog.value = true
}

const deleteAccountDetail = async () => {
  try {
    await $api(`/AccountDetails/${selectedItem.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })

    deleteDialog.value = false
    await loadAccountDetails()
  } catch (error) {
    console.error('Error deleting account detail:', error)
  }
}

const formatCurrency = (value: number) => {
  return new Intl.NumberFormat(locale.value === 'ar' ? 'ar-LY' : 'en-US', {
    style: 'currency',
    currency: 'LYD'
  }).format(value)
}

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString(locale.value === 'ar' ? 'ar-LY' : 'en-US')
}

// Lifecycle
onMounted(() => {
  loadAccountDetails()
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