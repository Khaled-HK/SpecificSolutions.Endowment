<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface Product {
  id: number
  key: string
  value: string
  name: string
  description: string
  price?: number
  quantity?: number
}

interface NewProduct {
  name: string
  description: string
  price: number
  quantity: number
}

definePage({
  meta: {
    action: 'View',
    subject: 'Product',
  },
})

const products = ref<Product[]>([])
const loading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedProduct = ref<Product | null>(null)
const selectedRows = ref<Product[]>([])
const search = ref('')

const newProduct = ref<NewProduct>({
  name: '',
  description: '',
  price: 0,
  quantity: 0,
})

const editProduct = ref<Product>({
  id: 0,
  key: '',
  value: '',
  name: '',
  description: '',
  price: 0,
  quantity: 0,
})

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using the ready-made template structure
const headers = [
  {
    title: 'اسم المادة',
    key: 'name',
  },
  {
    title: 'الوصف',
    key: 'description',
  },
  {
    title: 'السعر',
    key: 'price',
  },
  {
    title: 'الكمية',
    key: 'quantity',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadProducts = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Product/filter?${params}`)
    products.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadProducts() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading products:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل المواد'
    alertType.value = 'error'
    showAlert.value = true
    products.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const addProduct = async () => {
  try {
    const response = await $api('/Product', {
      method: 'POST',
      body: {
        name: newProduct.value.name,
        description: newProduct.value.description,
        price: newProduct.value.price,
        quantity: newProduct.value.quantity,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة المادة'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    dialog.value = false
    resetNewProduct()
    loadProducts()
    alertMessage.value = 'تم إضافة المادة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding product:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة المادة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateProduct = async () => {
  try {
    const response = await $api(`/Product/${editProduct.value.id}`, {
      method: 'PUT',
      body: {
        id: editProduct.value.id,
        name: editProduct.value.name,
        description: editProduct.value.description,
        price: editProduct.value.price,
        quantity: editProduct.value.quantity,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تحديث المادة'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    editDialog.value = false
    loadProducts()
    alertMessage.value = 'تم تحديث المادة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating product:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث المادة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteProduct = async () => {
  if (!selectedProduct.value) return
  
  try {
    const response = await $api(`/Product/${selectedProduct.value.id}`, {
      method: 'DELETE',
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف المادة'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    deleteDialog.value = false
    selectedProduct.value = null
    loadProducts()
    alertMessage.value = 'تم حذف المادة بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting product:', error)
    alertMessage.value = 'حدث خطأ أثناء حذف المادة'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (product: Product) => {
  editProduct.value = { ...product }
  editDialog.value = true
}

const openDeleteDialog = (product: Product) => {
  selectedProduct.value = product
  deleteDialog.value = true
}

const resetNewProduct = () => {
  newProduct.value = {
    name: '',
    description: '',
    price: 0,
    quantity: 0,
  }
}

const resetEditProduct = () => {
  editProduct.value = {
    id: 0,
    key: '',
    value: '',
    name: '',
    description: '',
    price: 0,
    quantity: 0,
  }
}

// Watch for changes in options to reload data
watch(options, () => {
  loadProducts()
}, { deep: true })

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadProducts()
})

onMounted(() => {
  loadProducts()
})
</script>

<template>
  <VCard>
    <!-- Header -->
    <VCardText class="d-flex align-center flex-wrap gap-4">
      <div class="me-3">
        <h4 class="text-h4">
          إدارة المواد
        </h4>
        <p class="mb-0">
          عرض وإدارة المواد في النظام
        </p>
      </div>
      <VSpacer />
      <VBtn
        prepend-icon="tabler-plus"
        @click="dialog = true"
      >
        إضافة مادة جديدة
      </VBtn>
    </VCardText>

    <!-- Search and Filters -->
    <VDivider />

    <VCardText>
      <VRow>
        <VCol
          cols="12"
          sm="4"
        >
          <VTextField
            v-model="search"
            prepend-inner-icon="tabler-search"
            label="البحث في المواد"
            single-line
            hide-details
            density="compact"
          />
        </VCol>
      </VRow>
    </VCardText>

    <!-- Data Table -->
    <VDataTable
      v-model="selectedRows"
      :headers="headers"
      :items="products"
      :loading="loading"
      :items-per-page="options.itemsPerPage"
      :page="options.page"
      :total="totalItems"
      class="text-no-wrap"
      @update:options="options = $event"
    >
      <!-- Price Column -->
      <template #item.price="{ item }">
        <VChip
          :color="item.price > 0 ? 'success' : 'warning'"
          size="small"
        >
          {{ item.price || 0 }} ريال
        </VChip>
      </template>

      <!-- Quantity Column -->
      <template #item.quantity="{ item }">
        <VChip
          :color="item.quantity > 0 ? 'primary' : 'error'"
          size="small"
        >
          {{ item.quantity || 0 }}
        </VChip>
      </template>

      <!-- Actions Column -->
      <template #item.actions="{ item }">
        <VBtn
          icon="tabler-edit"
          variant="text"
          size="small"
          color="primary"
          @click="openEditDialog(item)"
        />
        <VBtn
          icon="tabler-trash"
          variant="text"
          size="small"
          color="error"
          @click="openDeleteDialog(item)"
        />
      </template>
    </VDataTable>

    <!-- Add Product Dialog -->
    <VDialog
      v-model="dialog"
      max-width="500px"
    >
      <VCard>
        <VCardTitle>
          <span class="text-h5">إضافة مادة جديدة</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newProduct.name"
                  label="اسم المادة"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newProduct.description"
                  label="الوصف"
                  rows="3"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="newProduct.price"
                  label="السعر"
                  type="number"
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="newProduct.quantity"
                  label="الكمية"
                  type="number"
                  min="0"
                />
              </VCol>
            </VRow>
          </VContainer>
        </VCardText>

        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue-darken-1"
            variant="text"
            @click="dialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="blue-darken-1"
            @click="addProduct"
          >
            إضافة
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Product Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="500px"
    >
      <VCard>
        <VCardTitle>
          <span class="text-h5">تعديل المادة</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editProduct.name"
                  label="اسم المادة"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editProduct.description"
                  label="الوصف"
                  rows="3"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="editProduct.price"
                  label="السعر"
                  type="number"
                  min="0"
                  step="0.01"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="editProduct.quantity"
                  label="الكمية"
                  type="number"
                  min="0"
                />
              </VCol>
            </VRow>
          </VContainer>
        </VCardText>

        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue-darken-1"
            variant="text"
            @click="editDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="blue-darken-1"
            @click="updateProduct"
          >
            تحديث
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
        <VCardTitle class="text-h5">
          تأكيد الحذف
        </VCardTitle>
        <VCardText>
          هل أنت متأكد من حذف المادة "{{ selectedProduct?.name }}"؟
          <br>
          <strong>لا يمكن التراجع عن هذا الإجراء.</strong>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="error"
            @click="deleteProduct"
          >
            حذف
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Alert -->
    <VSnackbar
      v-model="showAlert"
      :color="alertType"
      :timeout="3000"
    >
      {{ alertMessage }}
    </VSnackbar>
  </VCard>
</template> 