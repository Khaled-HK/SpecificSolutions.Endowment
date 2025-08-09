<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center">
        <span>{{ t('pages.accounts.title') }}</span>
        <VBtn
          color="primary"
          @click="openAddDialog"
        >
          {{ t('pages.accounts.addAccount') }}
        </VBtn>
      </VCardTitle>

      <VCardText>
        <VDataTable
          :headers="headers"
          :items="accounts"
          :loading="loading"
          :items-per-page="10"
          class="elevation-1"
        >
          <template #item.actions="{ item }">
            <VBtn
              icon
              size="small"
              color="primary"
              @click="openEditDialog(item)"
            >
              <VIcon>mdi-pencil</VIcon>
            </VBtn>
            <VBtn
              icon
              size="small"
              color="error"
              @click="confirmDelete(item)"
            >
              <VIcon>mdi-delete</VIcon>
            </VBtn>
          </template>

          <template #item.gender="{ item }">
            <VChip
              :color="item.gender === 1 ? 'blue' : 'pink'"
              size="small"
            >
              {{ item.gender === 1 ? t('pages.accounts.genderMale') : t('pages.accounts.genderFemale') }}
            </VChip>
          </template>

          <template #item.status="{ item }">
            <VChip
              :color="item.status === 1 ? 'green' : 'red'"
              size="small"
            >
              {{ item.status === 1 ? t('pages.accounts.statusActive') : t('pages.accounts.statusInactive') }}
            </VChip>
          </template>

          <template #item.socialStatus="{ item }">
            <VChip
              color="info"
              size="small"
            >
              {{ getSocialStatusText(item.socialStatus) }}
            </VChip>
          </template>

          <template #item.type="{ item }">
            <VChip
              color="warning"
              size="small"
            >
              {{ getAccountTypeText(item.type) }}
            </VChip>
          </template>

          <template #item.isActive="{ item }">
            <VChip
              :color="item.isActive ? 'green' : 'red'"
              size="small"
            >
              {{ item.isActive ? t('pages.accounts.isActive') : t('pages.accounts.isInactive') }}
            </VChip>
          </template>
        </VDataTable>
      </VCardText>
    </VCard>

    <!-- Add/Edit Dialog -->
    <VDialog
      v-model="dialog"
      max-width="800px"
    >
      <VCard>
        <VCardTitle>
          <span v-if="!editing">{{ t('pages.accounts.addNewAccount') }}</span>
          <span v-else>{{ t('pages.accounts.editAccount') }}</span>
        </VCardTitle>

        <VCardText>
          <VForm @submit.prevent="editing ? updateAccount() : addAccount()">
            <VRow>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.name"
                  :label="t('pages.accounts.name')"
                  :error="hasError('name')"
                  :error-messages="getError('name')"
                  @blur="setFieldTouched('name')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.motherName"
                  :label="t('pages.accounts.motherName')"
                  :error="hasError('motherName')"
                  :error-messages="getError('motherName')"
                  @blur="setFieldTouched('motherName')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.birthDate"
                  :label="t('pages.accounts.birthDate')"
                  type="date"
                  :error="hasError('birthDate')"
                  :error-messages="getError('birthDate')"
                  @blur="setFieldTouched('birthDate')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newAccount.gender"
                  :label="t('pages.accounts.gender')"
                  :items="genderOptions"
                  :error="hasError('gender')"
                  :error-messages="getError('gender')"
                  @blur="setFieldTouched('gender')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.barcode"
                  :label="t('pages.accounts.barcode')"
                  :error="hasError('barcode')"
                  :error-messages="getError('barcode')"
                  @blur="setFieldTouched('barcode')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newAccount.status"
                  :label="t('pages.accounts.status')"
                  :items="statusOptions"
                  :error="hasError('status')"
                  :error-messages="getError('status')"
                  @blur="setFieldTouched('status')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.lockerFileNumber"
                  :label="t('pages.accounts.lockerFileNumber')"
                  type="number"
                  :error="hasError('lockerFileNumber')"
                  :error-messages="getError('lockerFileNumber')"
                  @blur="setFieldTouched('lockerFileNumber')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newAccount.socialStatus"
                  :label="t('pages.accounts.socialStatus')"
                  :items="socialStatusOptions"
                  :error="hasError('socialStatus')"
                  :error-messages="getError('socialStatus')"
                  @blur="setFieldTouched('socialStatus')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.bookNumber"
                  :label="t('pages.accounts.bookNumber')"
                  type="number"
                  :error="hasError('bookNumber')"
                  :error-messages="getError('bookNumber')"
                  @blur="setFieldTouched('bookNumber')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.paperNumber"
                  :label="t('pages.accounts.paperNumber')"
                  type="number"
                  :error="hasError('paperNumber')"
                  :error-messages="getError('paperNumber')"
                  @blur="setFieldTouched('paperNumber')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.registrationNumber"
                  :label="t('pages.accounts.registrationNumber')"
                  type="number"
                  :error="hasError('registrationNumber')"
                  :error-messages="getError('registrationNumber')"
                  @blur="setFieldTouched('registrationNumber')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.accountNumber"
                  :label="t('pages.accounts.accountNumber')"
                  :error="hasError('accountNumber')"
                  :error-messages="getError('accountNumber')"
                  @blur="setFieldTouched('accountNumber')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSelect
                  v-model="newAccount.type"
                  :label="t('pages.accounts.type')"
                  :items="accountTypeOptions"
                  :error="hasError('type')"
                  :error-messages="getError('type')"
                  @blur="setFieldTouched('type')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSwitch
                  v-model="newAccount.lookOver"
                  :label="t('pages.accounts.lookOver')"
                  :error="hasError('lookOver')"
                  @blur="setFieldTouched('lookOver')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.nid"
                  :label="t('pages.accounts.nid')"
                  type="number"
                  :error="hasError('nid')"
                  :error-messages="getError('nid')"
                  @blur="setFieldTouched('nid')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VSwitch
                  v-model="newAccount.isActive"
                  :label="t('pages.accounts.isActive')"
                  :error="hasError('isActive')"
                  @blur="setFieldTouched('isActive')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.balance"
                  :label="t('pages.accounts.balance')"
                  type="number"
                  step="0.01"
                  :error="hasError('balance')"
                  :error-messages="getError('balance')"
                  @blur="setFieldTouched('balance')"
                  required
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.address"
                  :label="t('pages.accounts.address')"
                  :error="hasError('address')"
                  :error-messages="getError('address')"
                  @blur="setFieldTouched('address')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.city"
                  :label="t('pages.accounts.city')"
                  :error="hasError('city')"
                  :error-messages="getError('city')"
                  @blur="setFieldTouched('city')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.country"
                  :label="t('pages.accounts.country')"
                  :error="hasError('country')"
                  :error-messages="getError('country')"
                  @blur="setFieldTouched('country')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model="newAccount.contactNumber"
                  :label="t('pages.accounts.contactNumber')"
                  :error="hasError('contactNumber')"
                  :error-messages="getError('contactNumber')"
                  @blur="setFieldTouched('contactNumber')"
                />
              </VCol>
              <VCol cols="12" md="6">
                <VTextField
                  v-model.number="newAccount.floors"
                  :label="t('pages.accounts.floors')"
                  type="number"
                  :error="hasError('floors')"
                  :error-messages="getError('floors')"
                  @blur="setFieldTouched('floors')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newAccount.note"
                  :label="t('pages.accounts.note')"
                  :error="hasError('note')"
                  :error-messages="getError('note')"
                  @blur="setFieldTouched('note')"
                  rows="3"
                />
              </VCol>
            </VRow>
          </VForm>
        </VCardText>

        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey"
            @click="dialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            v-if="!editing"
            color="primary"
            :disabled="hasErrors"
            @click="addAccount"
          >
            {{ t('common.save') }}
          </VBtn>
          <VBtn
            v-else
            color="primary"
            :disabled="hasErrors"
            @click="updateAccount"
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
        <VCardTitle>{{ t('pages.accounts.confirmDelete') }}</VCardTitle>
        <VCardText>
          {{ t('pages.accounts.deleteConfirmation') }}
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey"
            @click="deleteDialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="error"
            @click="deleteAccount"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'
import { useApi } from '@/composables/useApi'

const { t, locale } = useI18n()
// Get API instance
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
  getError,
  hasError,
  setErrorsFromResponse
} = useFormValidation()

// Data
const accounts = ref([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const editing = ref(false)
const selectedAccount = ref(null)

// Form data
const newAccount = ref({
  name: '',
  motherName: '',
  birthDate: '',
  gender: null,
  barcode: '',
  status: null,
  lockerFileNumber: null,
  socialStatus: null,
  bookNumber: null,
  paperNumber: null,
  registrationNumber: null,
  accountNumber: '',
  type: null,
  lookOver: false,
  note: '',
  nid: null,
  isActive: true,
  balance: 0,
  address: '',
  city: '',
  country: '',
  contactNumber: '',
  floors: 0
})

// Options for selects
const genderOptions = computed(() => [
  { title: t('pages.accounts.genderMale'), value: 1 },
  { title: t('pages.accounts.genderFemale'), value: 2 }
])

const statusOptions = computed(() => [
  { title: t('pages.accounts.statusActive'), value: 1 },
  { title: t('pages.accounts.statusInactive'), value: 2 }
])

const socialStatusOptions = computed(() => [
  { title: t('pages.accounts.socialStatusMarried'), value: 1 },
  { title: t('pages.accounts.socialStatusWidower'), value: 2 },
  { title: t('pages.accounts.socialStatusDivorced'), value: 3 },
  { title: t('pages.accounts.socialStatusSingle'), value: 4 },
  { title: t('pages.accounts.socialStatusChild'), value: 5 }
])

const accountTypeOptions = computed(() => [
  { title: t('pages.accounts.accountTypeMartyr'), value: 1 },
  { title: t('pages.accounts.accountTypeDisability'), value: 2 },
  { title: t('pages.accounts.accountTypeBeneficiary'), value: 3 },
  { title: t('pages.accounts.accountTypeTreasury'), value: 4 },
  { title: t('pages.accounts.accountTypePaymentMethod'), value: 5 },
  { title: t('pages.accounts.accountTypeMarket'), value: 6 },
  { title: t('pages.accounts.accountTypeMarketer'), value: 7 },
  { title: t('pages.accounts.accountTypeCustomer'), value: 8 },
  { title: t('pages.accounts.accountTypeSupplierCompany'), value: 9 },
  { title: t('pages.accounts.accountTypeAccounting'), value: 10 },
  { title: t('pages.accounts.accountTypeRevenues'), value: 11 },
  { title: t('pages.accounts.accountTypeProducts'), value: 12 },
  { title: t('pages.accounts.accountTypeLiabilities'), value: 13 },
  { title: t('pages.accounts.accountTypeExpenses'), value: 14 }
])

// Table headers
const headers = computed(() => [
  { title: t('tableHeaders.accounts.name'), key: 'name' },
  { title: t('tableHeaders.accounts.motherName'), key: 'motherName' },
  { title: t('tableHeaders.accounts.birthDate'), key: 'birthDate' },
  { title: t('tableHeaders.accounts.gender'), key: 'gender' },
  { title: t('tableHeaders.accounts.barcode'), key: 'barcode' },
  { title: t('tableHeaders.accounts.status'), key: 'status' },
  { title: t('tableHeaders.accounts.lockerFileNumber'), key: 'lockerFileNumber' },
  { title: t('tableHeaders.accounts.socialStatus'), key: 'socialStatus' },
  { title: t('tableHeaders.accounts.bookNumber'), key: 'bookNumber' },
  { title: t('tableHeaders.accounts.paperNumber'), key: 'paperNumber' },
  { title: t('tableHeaders.accounts.registrationNumber'), key: 'registrationNumber' },
  { title: t('tableHeaders.accounts.accountNumber'), key: 'accountNumber' },
  { title: t('tableHeaders.accounts.type'), key: 'type' },
  { title: t('tableHeaders.accounts.lookOver'), key: 'lookOver' },
  { title: t('tableHeaders.accounts.note'), key: 'note' },
  { title: t('tableHeaders.accounts.nid'), key: 'nid' },
  { title: t('tableHeaders.accounts.isActive'), key: 'isActive' },
  { title: t('tableHeaders.accounts.balance'), key: 'balance' },
  { title: t('tableHeaders.accounts.actions'), key: 'actions', sortable: false }
])

// Helper functions
const getSocialStatusText = (status: number) => {
  const statusMap = {
    1: t('pages.accounts.socialStatusMarried'),
    2: t('pages.accounts.socialStatusWidower'),
    3: t('pages.accounts.socialStatusDivorced'),
    4: t('pages.accounts.socialStatusSingle'),
    5: t('pages.accounts.socialStatusChild')
  }
  return statusMap[status] || status
}

const getAccountTypeText = (type: number) => {
  const typeMap = {
    1: t('pages.accounts.accountTypeMartyr'),
    2: t('pages.accounts.accountTypeDisability'),
    3: t('pages.accounts.accountTypeBeneficiary'),
    4: t('pages.accounts.accountTypeTreasury'),
    5: t('pages.accounts.accountTypePaymentMethod'),
    6: t('pages.accounts.accountTypeMarket'),
    7: t('pages.accounts.accountTypeMarketer'),
    8: t('pages.accounts.accountTypeCustomer'),
    9: t('pages.accounts.accountTypeSupplierCompany'),
    10: t('pages.accounts.accountTypeAccounting'),
    11: t('pages.accounts.accountTypeRevenues'),
    12: t('pages.accounts.accountTypeProducts'),
    13: t('pages.accounts.accountTypeLiabilities'),
    14: t('pages.accounts.accountTypeExpenses')
  }
  return typeMap[type] || type
}

// Load accounts
const loadAccounts = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: currentPage.value.toString(),
      PageSize: itemsPerPage.value.toString(),
      SearchTerm: searchQuery.value || '',
    }).toString()
    
    const response = await api('/Accounts/filter', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    accounts.value = response.data.items || []
    totalItems.value = response.totalCount || 0
  } catch (error) {
    console.error('Error loading accounts:', error)
  } finally {
    loading.value = false
  }
}

// Reset form
const resetNewAccount = () => {
  newAccount.value = {
    name: '',
    motherName: '',
    birthDate: '',
    gender: null,
    barcode: '',
    status: null,
    lockerFileNumber: null,
    socialStatus: null,
    bookNumber: null,
    paperNumber: null,
    registrationNumber: null,
    accountNumber: '',
    type: null,
    lookOver: false,
    note: '',
    nid: null,
    isActive: true,
    balance: 0,
    address: '',
    city: '',
    country: '',
    contactNumber: '',
    floors: 0
  }
  clearErrors()
}

// Open add dialog
const openAddDialog = () => {
  editing.value = false
  resetNewAccount()
  dialog.value = true
}

// Open edit dialog
const openEditDialog = (account: any) => {
  editing.value = true
  selectedAccount.value = account
  newAccount.value = { ...account }
  clearErrors()
  dialog.value = true
}

// Add account
const addAccount = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('name')
  setFieldTouched('motherName')
  setFieldTouched('birthDate')
  setFieldTouched('gender')
  setFieldTouched('barcode')
  setFieldTouched('status')
  setFieldTouched('lockerFileNumber')
  setFieldTouched('socialStatus')
  setFieldTouched('bookNumber')
  setFieldTouched('paperNumber')
  setFieldTouched('registrationNumber')
  setFieldTouched('accountNumber')
  setFieldTouched('type')
  setFieldTouched('nid')
  setFieldTouched('balance')

  if (!validateRequired(newAccount.value.name, 'name')) {
    addError('name', t('validation.required', { field: t('pages.accounts.name') }))
  }
  if (!validateRequired(newAccount.value.motherName, 'motherName')) {
    addError('motherName', t('validation.required', { field: t('pages.accounts.motherName') }))
  }
  if (!validateRequired(newAccount.value.birthDate, 'birthDate')) {
    addError('birthDate', t('validation.required', { field: t('pages.accounts.birthDate') }))
  }
  if (!newAccount.value.gender) {
    addError('gender', t('validation.required', { field: t('pages.accounts.gender') }))
  }
  if (!validateRequired(newAccount.value.barcode, 'barcode')) {
    addError('barcode', t('validation.required', { field: t('pages.accounts.barcode') }))
  }
  if (!newAccount.value.status) {
    addError('status', t('validation.required', { field: t('pages.accounts.status') }))
  }
  if (!newAccount.value.lockerFileNumber) {
    addError('lockerFileNumber', t('validation.required', { field: t('pages.accounts.lockerFileNumber') }))
  }
  if (!newAccount.value.socialStatus) {
    addError('socialStatus', t('validation.required', { field: t('pages.accounts.socialStatus') }))
  }
  if (!newAccount.value.bookNumber) {
    addError('bookNumber', t('validation.required', { field: t('pages.accounts.bookNumber') }))
  }
  if (!newAccount.value.paperNumber) {
    addError('paperNumber', t('validation.required', { field: t('pages.accounts.paperNumber') }))
  }
  if (!newAccount.value.registrationNumber) {
    addError('registrationNumber', t('validation.required', { field: t('pages.accounts.registrationNumber') }))
  }
  if (!validateRequired(newAccount.value.accountNumber, 'accountNumber')) {
    addError('accountNumber', t('validation.required', { field: t('pages.accounts.accountNumber') }))
  }
  if (!newAccount.value.type) {
    addError('type', t('validation.required', { field: t('pages.accounts.type') }))
  }
  if (!newAccount.value.nid) {
    addError('nid', t('validation.required', { field: t('pages.accounts.nid') }))
  }
  if (!newAccount.value.balance && newAccount.value.balance !== 0) {
    addError('balance', t('validation.required', { field: t('pages.accounts.balance') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api('/Accounts', {
      method: 'POST',
      body: newAccount.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewAccount()
    await loadAccounts()
  } catch (error) {
    console.error('Error adding account:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Name: 'name',
        MotherName: 'motherName',
        BirthDate: 'birthDate',
        Gender: 'gender',
        Barcode: 'barcode',
        Status: 'status',
        LockerFileNumber: 'lockerFileNumber',
        SocialStatus: 'socialStatus',
        BookNumber: 'bookNumber',
        PaperNumber: 'paperNumber',
        RegistrationNumber: 'registrationNumber',
        AccountNumber: 'accountNumber',
        Type: 'type',
        LookOver: 'lookOver',
        Note: 'note',
        NID: 'nid',
        IsActive: 'isActive',
        Balance: 'balance',
        Address: 'address',
        City: 'city',
        Country: 'country',
        ContactNumber: 'contactNumber',
        Floors: 'floors'
      })
    }
  }
}

// Update account
const updateAccount = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('name')
  setFieldTouched('motherName')
  setFieldTouched('birthDate')
  setFieldTouched('gender')
  setFieldTouched('barcode')
  setFieldTouched('status')
  setFieldTouched('lockerFileNumber')
  setFieldTouched('socialStatus')
  setFieldTouched('bookNumber')
  setFieldTouched('paperNumber')
  setFieldTouched('registrationNumber')
  setFieldTouched('accountNumber')
  setFieldTouched('type')
  setFieldTouched('nid')
  setFieldTouched('balance')

  if (!validateRequired(newAccount.value.name, 'name')) {
    addError('name', t('validation.required', { field: t('pages.accounts.name') }))
  }
  if (!validateRequired(newAccount.value.motherName, 'motherName')) {
    addError('motherName', t('validation.required', { field: t('pages.accounts.motherName') }))
  }
  if (!validateRequired(newAccount.value.birthDate, 'birthDate')) {
    addError('birthDate', t('validation.required', { field: t('pages.accounts.birthDate') }))
  }
  if (!newAccount.value.gender) {
    addError('gender', t('validation.required', { field: t('pages.accounts.gender') }))
  }
  if (!validateRequired(newAccount.value.barcode, 'barcode')) {
    addError('barcode', t('validation.required', { field: t('pages.accounts.barcode') }))
  }
  if (!newAccount.value.status) {
    addError('status', t('validation.required', { field: t('pages.accounts.status') }))
  }
  if (!newAccount.value.lockerFileNumber) {
    addError('lockerFileNumber', t('validation.required', { field: t('pages.accounts.lockerFileNumber') }))
  }
  if (!newAccount.value.socialStatus) {
    addError('socialStatus', t('validation.required', { field: t('pages.accounts.socialStatus') }))
  }
  if (!newAccount.value.bookNumber) {
    addError('bookNumber', t('validation.required', { field: t('pages.accounts.bookNumber') }))
  }
  if (!newAccount.value.paperNumber) {
    addError('paperNumber', t('validation.required', { field: t('pages.accounts.paperNumber') }))
  }
  if (!newAccount.value.registrationNumber) {
    addError('registrationNumber', t('validation.required', { field: t('pages.accounts.registrationNumber') }))
  }
  if (!validateRequired(newAccount.value.accountNumber, 'accountNumber')) {
    addError('accountNumber', t('validation.required', { field: t('pages.accounts.accountNumber') }))
  }
  if (!newAccount.value.type) {
    addError('type', t('validation.required', { field: t('pages.accounts.type') }))
  }
  if (!newAccount.value.nid) {
    addError('nid', t('validation.required', { field: t('pages.accounts.nid') }))
  }
  if (!newAccount.value.balance && newAccount.value.balance !== 0) {
    addError('balance', t('validation.required', { field: t('pages.accounts.balance') }))
  }

  if (hasErrors.value) {
    return
  }

  try {
    await api(`/Accounts/${selectedAccount.value.id}`, {
      method: 'PUT',
      body: newAccount.value,
      headers: {
        'Accept-Language': locale.value
      }
    })

    dialog.value = false
    resetNewAccount()
    await loadAccounts()
  } catch (error) {
    console.error('Error updating account:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Name: 'name',
        MotherName: 'motherName',
        BirthDate: 'birthDate',
        Gender: 'gender',
        Barcode: 'barcode',
        Status: 'status',
        LockerFileNumber: 'lockerFileNumber',
        SocialStatus: 'socialStatus',
        BookNumber: 'bookNumber',
        PaperNumber: 'paperNumber',
        RegistrationNumber: 'registrationNumber',
        AccountNumber: 'accountNumber',
        Type: 'type',
        LookOver: 'lookOver',
        Note: 'note',
        NID: 'nid',
        IsActive: 'isActive',
        Balance: 'balance',
        Address: 'address',
        City: 'city',
        Country: 'country',
        ContactNumber: 'contactNumber',
        Floors: 'floors'
      })
    }
  }
}

// Delete account
const confirmDelete = (account: any) => {
  selectedAccount.value = account
  deleteDialog.value = true
}

const deleteAccount = async () => {
  try {
    await api(`/Accounts/${selectedAccount.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })

    deleteDialog.value = false
    await loadAccounts()
  } catch (error) {
    console.error('Error deleting account:', error)
  }
}

// Load data on mount
onMounted(() => {
  loadAccounts()
})
</script>

<style scoped>
.v-data-table :deep(.v-data-table__td) {
  padding: 8px 16px;
}

.v-data-table :deep(.v-data-table__th) {
  padding: 8px 16px;
  font-weight: 600;
}

.v-data-table :deep(.v-data-table__row:hover) {
  background-color: rgba(0, 0, 0, 0.04);
}

.v-dialog :deep(.v-card__title) {
  font-size: 1.25rem;
  font-weight: 600;
}

.v-dialog :deep(.v-card__text) {
  padding: 24px;
}

.v-dialog :deep(.v-card__actions) {
  padding: 16px 24px;
}
</style> 