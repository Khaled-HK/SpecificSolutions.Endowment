<script setup>
import InvoiceEditable from '@/views/apps/invoice/InvoiceEditable.vue'
import InvoiceSendInvoiceDrawer from '@/views/apps/invoice/InvoiceSendInvoiceDrawer.vue'

// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})

// 👉 Default Blank Data
const invoiceData = ref({
  invoice: {
    id: 5037,
    issuedDate: '',
    service: '',
    total: 0,
    avatar: '',
    invoiceStatus: '',
    dueDate: '',
    balance: 0,
    client: {
      address: '112, Lorem Ipsum, Florida',
      company: 'Greeva Inc',
      companyEmail: 'johndoe@greeva.com',
      contact: '+1 123 3452 12',
      country: 'USA',
      name: 'John Doe',
    },
  },
  paymentDetails: {
    totalDue: '$12,110.55',
    bankName: 'American Bank',
    country: 'United States',
    iban: 'ETD95476213',
    swiftCode: 'BR91905',
  },
  purchasedProducts: [{
    title: '',
    cost: 0,
    hours: 0,
    description: '',
  }],
  note: '',
  paymentMethod: '',
  salesperson: '',
  thanksNote: '',
})

const paymentTerms = ref(true)
const clientNotes = ref(false)
const paymentStub = ref(false)
const selectedPaymentMethod = ref('Bank Account')

const paymentMethods = [
  'Bank Account',
  'PayPal',
  'UPI Transfer',
]

const isSendPaymentSidebarVisible = ref(false)

const addProduct = value => {
  invoiceData.value?.purchasedProducts.push(value)
}

const removeProduct = id => {
  invoiceData.value?.purchasedProducts.splice(id, 1)
}
</script>

<template>
  <VRow>
    <!-- 👉 InvoiceEditable -->
    <VCol
      cols="12"
      md="9"
    >
      <InvoiceEditable
        :data="invoiceData"
        @push="addProduct"
        @remove="removeProduct"
      />
    </VCol>

    <!-- 👉 Right Column: Invoice Action -->
    <VCol
      cols="12"
      md="3"
    >
      <VCard class="mb-8">
        <VCardText>
          <!-- 👉 Send Invoice -->
          <VBtn
            block
            prepend-icon="tabler-send"
            class="mb-4"
            @click="isSendPaymentSidebarVisible = true"
          >
            Send Invoice
          </VBtn>

          <!-- 👉 Preview -->
          <VBtn
            block
            color="secondary"
            variant="tonal"
            class="mb-4"
            :to="{ name: 'apps-invoice-preview-id', params: { id: '5036' } }"
          >
            Preview
          </VBtn>

          <!-- 👉 Save -->
          <VBtn
            block
            color="secondary"
            variant="tonal"
          >
            Save
          </VBtn>
        </VCardText>
      </VCard>

      <!-- 👉 Select payment method -->
      <AppSelect
        id="payment-method"
        v-model="selectedPaymentMethod"
        :items="paymentMethods"
        label="Accept Payment Via"
        class="mb-6"
      />

      <!-- 👉 Payment Terms -->
      <div class="d-flex align-center justify-space-between">
        <VLabel for="payment-terms">
          Payment Terms
        </VLabel>
        <div>
          <VSwitch
            id="payment-terms"
            v-model="paymentTerms"
          />
        </div>
      </div>

      <!-- 👉  Client Notes -->
      <div class="d-flex align-center justify-space-between">
        <VLabel for="client-notes">
          Client Notes
        </VLabel>
        <div>
          <VSwitch
            id="client-notes"
            v-model="clientNotes"
          />
        </div>
      </div>

      <!-- 👉  Payment Stub -->
      <div class="d-flex align-center justify-space-between">
        <VLabel for="payment-stub">
          Payment Stub
        </VLabel>
        <div>
          <VSwitch
            id="payment-stub"
            v-model="paymentStub"
          />
        </div>
      </div>
    </VCol>
  </VRow>

  <!-- 👉 Send Invoice Sidebar -->
  <InvoiceSendInvoiceDrawer v-model:is-drawer-open="isSendPaymentSidebarVisible" />
</template>
