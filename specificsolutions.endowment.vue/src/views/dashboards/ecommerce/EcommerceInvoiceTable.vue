<script setup>
const searchQuery = ref('')
const selectedStatus = ref(null)
const selectedRows = ref([])

// Data table options
const itemsPerPage = ref(6)
const page = ref(1)
const sortBy = ref()
const orderBy = ref()

const updateOptions = options => {
  page.value = options.page
  sortBy.value = options.sortBy[0]?.key
  orderBy.value = options.sortBy[0]?.order
}

// 👉 headers
const headers = [
  {
    title: '#',
    key: 'id',
  },
  {
    title: 'Status',
    key: 'status',
    sortable: false,
  },
  {
    title: 'Total',
    key: 'total',
  },
  {
    title: 'Issued Date',
    key: 'date',
  },
  {
    title: 'Balance',
    key: 'balance',
  },
  {
    title: 'Actions',
    key: 'actions',
    sortable: false,
  },
]

const {
  data: invoiceData,
  execute: fetchInvoices,
} = await useApi(createUrl('/apps/invoice', {
  query: {
    q: searchQuery,
    status: selectedStatus,
    itemsPerPage,
    page,
    sortBy,
    orderBy,
  },
}))

// Fallback data in case API fails
const fallbackInvoices = [
  {
    id: 1,
    name: 'Invoice #3492',
    email: 'contact@anas-mazza.com',
    img: '/images/avatars/1.png',
    amount: '$459.30',
    invoiceStatus: 'Downloaded',
    balance: '$459.30',
    dueDate: '12 Apr 2022',
    total: 459.30,
    issuedDate: '12 Apr 2022',
  },
  {
    id: 2,
    name: 'Invoice #4593',
    email: 'contact@anas-mazza.com',
    img: '/images/avatars/2.png',
    amount: '$299.00',
    invoiceStatus: 'Paid',
    balance: '$0.00',
    dueDate: '15 Apr 2022',
    total: 299.00,
    issuedDate: '15 Apr 2022',
  },
  {
    id: 3,
    name: 'Invoice #4594',
    email: 'contact@anas-mazza.com',
    img: '/images/avatars/3.png',
    amount: '$399.00',
    invoiceStatus: 'Partial Payment',
    balance: '$199.00',
    dueDate: '20 Apr 2022',
    total: 399.00,
    issuedDate: '20 Apr 2022',
  },
]

const invoices = computed(() => invoiceData.value?.invoices || fallbackInvoices)
const totalInvoices = computed(() => invoiceData.value?.totalInvoices || fallbackInvoices.length)

// 👉 Invoice balance variant resolver
const resolveInvoiceBalanceVariant = (balance, total) => {
  if (balance === total)
    return {
      status: 'Unpaid',
      chip: { color: 'error' },
    }
  if (balance === 0)
    return {
      status: 'Paid',
      chip: { color: 'success' },
    }
  
  return {
    status: balance,
    chip: { variant: 'text' },
  }
}

const resolveInvoiceStatusVariantAndIcon = status => {
  if (status === 'Partial Payment')
    return {
      variant: 'warning',
      icon: 'tabler-chart-pie-2',
    }
  if (status === 'Paid')
    return {
      variant: 'success',
      icon: 'tabler-check',
    }
  if (status === 'Downloaded')
    return {
      variant: 'info',
      icon: 'tabler-arrow-down',
    }
  if (status === 'Draft')
    return {
      variant: 'primary',
      icon: 'tabler-folder',
    }
  if (status === 'Sent')
    return {
      variant: 'secondary',
      icon: 'tabler-mail',
    }
  if (status === 'Past Due')
    return {
      variant: 'error',
      icon: 'tabler-help',
    }
  
  return {
    variant: 'secondary',
    icon: 'tabler-x',
  }
}

const computedMoreList = computed(() => {
  return paramId => [
    {
      title: 'Download',
      value: 'download',
      prependIcon: 'tabler-download',
    },
    {
      title: 'Edit',
      value: 'edit',
      prependIcon: 'tabler-pencil',
      to: {
        name: 'apps-invoice-edit-id',
        params: { id: paramId },
      },
    },
    {
      title: 'Duplicate',
      value: 'duplicate',
      prependIcon: 'tabler-layers-intersect',
    },
  ]
})

const deleteInvoice = async id => {
  try {
    await $api(`/apps/invoice/${ id }`, { method: 'DELETE' })
    fetchInvoices()
  } catch (error) {
    console.error('Error deleting invoice:', error)
    // Handle error gracefully
  }
}
</script>

<template>
  <VCard
    v-if="invoices"
    id="invoice-list"
  >
    <VCardText>
      <div class="d-flex justify-space-between flex-wrap gap-4">
        <div class="d-flex gap-4 align-center">
          <div class="d-flex align-center gap-x-2">
            <div>
              Show
            </div>
            <AppSelect
              :model-value="itemsPerPage"
              :items="[
                { value: 6, title: '6' },
                { value: 10, title: '10' },
                { value: 25, title: '25' },
                { value: 50, title: '50' },
                { value: 100, title: '100' },
                { value: -1, title: 'All' },
              ]"
              @update:model-value="itemsPerPage = parseInt($event, 10)"
            />
          </div>
          <!-- 👉 Create invoice -->
          <VBtn
            prepend-icon="tabler-plus"
            :to="{ name: 'apps-invoice-add' }"
          >
            Create invoice
          </VBtn>
        </div>
        <div class="d-flex align-center flex-wrap gap-4">
          <!-- 👉 Search  -->
          <div class="invoice-list-filter">
            <AppTextField
              v-model="searchQuery"
              placeholder="Search Invoice"
            />
          </div>
          <!-- 👉 Select status -->
          <div class="invoice-list-filter">
            <AppSelect
              v-model="selectedStatus"
              placeholder="Select Status"
              clearable
              clear-icon="tabler-x"
              single-line
              :items="['Downloaded', 'Draft', 'Sent', 'Paid', 'Partial Payment', 'Past Due']"
            />
          </div>
        </div>
      </div>
    </VCardText>

    <VDivider />

    <!-- SECTION Datatable -->
    <VDataTableServer
      v-model="selectedRows"
      v-model:items-per-page="itemsPerPage"
      v-model:page="page"
      show-select
      :items-length="totalInvoices"
      :headers="headers"
      :items="invoices"
      item-value="id"
      class="text-no-wrap"
      @update:options="updateOptions"
    >
      <!-- id -->
      <template #item.id="{ item }">
        <RouterLink :to="{ name: 'apps-invoice-preview-id', params: { id: item.id } }">
          #{{ item.id }}
        </RouterLink>
      </template>

      <!-- status -->
      <template #item.status="{ item }">
        <VTooltip>
          <template #activator="{ props }">
            <VAvatar
              :size="28"
              v-bind="props"
              :color="resolveInvoiceStatusVariantAndIcon(item.invoiceStatus).variant"
              variant="tonal"
            >
              <VIcon
                :size="16"
                :icon="resolveInvoiceStatusVariantAndIcon(item.invoiceStatus).icon"
              />
            </VAvatar>
          </template>
          <p class="mb-0">
            {{ item.invoiceStatus }}
          </p>
          <p class="mb-0">
            Balance: {{ item.balance }}
          </p>
          <p class="mb-0">
            Due date: {{ item.dueDate }}
          </p>
        </VTooltip>
      </template>

      <!-- Total -->
      <template #item.total="{ item }">
        ${{ item.total }}
      </template>

      <!-- Date -->
      <template #item.date="{ item }">
        {{ item.issuedDate }}
      </template>

      <!-- Balance -->
      <template #item.balance="{ item }">
        <VChip
          v-if="typeof ((resolveInvoiceBalanceVariant(item.balance, item.total)).status) === 'string'"
          :color="resolveInvoiceBalanceVariant(item.balance, item.total).chip.color"
          label
          size="x-small"
        >
          {{ (resolveInvoiceBalanceVariant(item.balance, item.total)).status }}
        </VChip>

        <template v-else>
          <span class="text-base text-high-emphasis">
            {{ Number((resolveInvoiceBalanceVariant(item.balance, item.total)).status) > 0 ? `$${(resolveInvoiceBalanceVariant(item.balance, item.total)).status}` : `-$${Math.abs(Number((resolveInvoiceBalanceVariant(item.balance, item.total)).status))}` }}
          </span>
        </template>
      </template>

      <!-- Actions -->
      <template #item.actions="{ item }">
        <IconBtn @click="deleteInvoice(item.id)">
          <VIcon icon="tabler-trash" />
        </IconBtn>

        <IconBtn :to="{ name: 'apps-invoice-preview-id', params: { id: item.id } }">
          <VIcon icon="tabler-eye" />
        </IconBtn>

        <MoreBtn
          :menu-list="computedMoreList(item.id)"
          item-props
          color="undefined"
          size="small"
        />
      </template>

      <!-- pagination -->
      <template #bottom>
        <TablePagination
          v-model:page="page"
          :items-per-page="itemsPerPage"
          :total-items="totalInvoices"
        />
      </template>
    </VDataTableServer>
    <!-- !SECTION -->
  </VCard>
</template>

<style lang="scss">
#invoice-list {
  .invoice-list-actions {
    inline-size: 8rem;
  }

  .invoice-list-filter {
    inline-size: 12rem;
  }
}
</style>
