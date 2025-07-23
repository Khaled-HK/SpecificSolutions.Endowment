<script setup lang="ts">
import { ref, onMounted, watch, reactive, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'

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

const { t } = useI18n()

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

// استخدام نظام التحقق الجديد
const {
  validationState,
  addError,
  removeError,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  hasFieldError,
  getFieldErrors,
  getFirstFieldError,
  setFieldTouched,
  setFieldDirty,
  shouldShowFieldError,
  validateRequired,
  validateEmail,
  validateLength,
} = useFormValidation()

const newProduct = reactive<NewProduct>({
  name: '',
  description: '',
  price: 0,
  quantity: 0,
})

const editProduct = reactive<Product>({
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

// Headers using dynamic i18n translations
const headers = computed(() => [
  {
    title: t('tableHeaders.products.name'),
    key: 'name',
  },
  {
    title: t('tableHeaders.products.description'),
    key: 'description',
  },
  {
    title: t('tableHeaders.products.price'),
    key: 'price',
  },
  {
    title: t('tableHeaders.products.quantity'),
    key: 'quantity',
  },
  {
    title: t('tableHeaders.products.actions'),
    key: 'actions',
    sortable: false,
  },
])

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
  // Clear previous errors
  clearErrors()
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(newProduct.name, 'name', 'اسم المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(newProduct.name, 'name', 2, 100, 'اسم المادة يجب أن يكون بين 2 و 100 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(newProduct.description, 'description', 'وصف المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(newProduct.description, 'description', 10, 500, 'وصف المادة يجب أن يكون بين 10 و 500 حرف')) {
    isValid = false
  }
  
  if (newProduct.price < 0) {
    addError('price', 'السعر لا يمكن أن يكون سالب')
    isValid = false
  }
  
  if (newProduct.quantity < 0) {
    addError('quantity', 'الكمية لا يمكن أن تكون سالبة')
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await $api('/Product', {
      method: 'POST',
      body: {
        name: newProduct.name,
        description: newProduct.description,
        price: newProduct.price,
        quantity: newProduct.quantity,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else {
        const errorMsg = response.message || 'حدث خطأ أثناء إضافة المادة'
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
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
  // Clear previous errors
  clearErrors()
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(editProduct.name, 'editName', 'اسم المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(editProduct.name, 'editName', 2, 100, 'اسم المادة يجب أن يكون بين 2 و 100 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(editProduct.description, 'editDescription', 'وصف المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(editProduct.description, 'editDescription', 10, 500, 'وصف المادة يجب أن يكون بين 10 و 500 حرف')) {
    isValid = false
  }
  
  if ((editProduct.price || 0) < 0) {
    addError('editPrice', 'السعر لا يمكن أن يكون سالب')
    isValid = false
  }
  
  if ((editProduct.quantity || 0) < 0) {
    addError('editQuantity', 'الكمية لا يمكن أن تكون سالبة')
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await $api(`/Product/${editProduct.id}`, {
      method: 'PUT',
      body: {
        id: editProduct.id,
        name: editProduct.name,
        description: editProduct.description,
        price: editProduct.price || 0,
        quantity: editProduct.quantity || 0,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else {
        const errorMsg = response.message || 'حدث خطأ أثناء تحديث المادة'
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
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
  clearErrors() // Clear previous validation errors
  editProduct.id = product.id
  editProduct.key = product.key
  editProduct.value = product.value
  editProduct.name = product.name
  editProduct.description = product.description
  editProduct.price = product.price || 0
  editProduct.quantity = product.quantity || 0
  editDialog.value = true
}

const openDeleteDialog = (product: Product) => {
  clearErrors() // Clear any validation errors
  selectedProduct.value = product
  deleteDialog.value = true
}

const resetNewProduct = () => {
  newProduct.name = ''
  newProduct.description = ''
  newProduct.price = 0
  newProduct.quantity = 0
  clearErrors() // Clear validation errors
}

const resetEditProduct = () => {
  editProduct.id = 0
  editProduct.key = ''
  editProduct.value = ''
  editProduct.name = ''
  editProduct.description = ''
  editProduct.price = 0
  editProduct.quantity = 0
  clearErrors() // Clear validation errors
}

const validateNewProduct = () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(newProduct.name, 'name', 'اسم المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(newProduct.name, 'name', 2, 100, 'اسم المادة يجب أن يكون بين 2 و 100 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(newProduct.description, 'description', 'وصف المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(newProduct.description, 'description', 10, 500, 'وصف المادة يجب أن يكون بين 10 و 500 حرف')) {
    isValid = false
  }
  
  if (newProduct.price < 0) {
    addError('price', 'السعر لا يمكن أن يكون سالب')
    isValid = false
  }
  
  if (newProduct.quantity < 0) {
    addError('quantity', 'الكمية لا يمكن أن تكون سالبة')
    isValid = false
  }
  
  setFieldTouched('name')
  setFieldTouched('description')
  setFieldTouched('price')
  setFieldTouched('quantity')
  
  return isValid
}

const validateEditProduct = () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(editProduct.name, 'editName', 'اسم المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(editProduct.name, 'editName', 2, 100, 'اسم المادة يجب أن يكون بين 2 و 100 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(editProduct.description, 'editDescription', 'وصف المادة مطلوب')) {
    isValid = false
  } else if (!validateLength(editProduct.description, 'editDescription', 10, 500, 'وصف المادة يجب أن يكون بين 10 و 500 حرف')) {
    isValid = false
  }
  
  if (editProduct.price < 0) {
    addError('editPrice', 'السعر لا يمكن أن يكون سالب')
    isValid = false
  }
  
  if (editProduct.quantity < 0) {
    addError('editQuantity', 'الكمية لا يمكن أن تكون سالبة')
    isValid = false
  }
  
  setFieldTouched('editName')
  setFieldTouched('editDescription')
  setFieldTouched('editPrice')
  setFieldTouched('editQuantity')
  
  return isValid
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
        @click="() => { clearErrors(); dialog = true; }"
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
                  :error="shouldShowFieldError('name')"
                  :error-messages="getFieldErrors('name')"
                  @blur="setFieldTouched('name')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newProduct.description"
                  label="الوصف"
                  rows="3"
                  :error="shouldShowFieldError('description')"
                  :error-messages="getFieldErrors('description')"
                  @blur="setFieldTouched('description')"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="newProduct.price"
                  label="السعر"
                  type="number"
                  min="0"
                  step="0.01"
                  :error="shouldShowFieldError('price')"
                  :error-messages="getFieldErrors('price')"
                  @blur="setFieldTouched('price')"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="newProduct.quantity"
                  label="الكمية"
                  type="number"
                  min="0"
                  :error="shouldShowFieldError('quantity')"
                  :error-messages="getFieldErrors('quantity')"
                  @blur="setFieldTouched('quantity')"
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
            :disabled="hasErrors"
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
                  :error="shouldShowFieldError('editName')"
                  :error-messages="getFieldErrors('editName')"
                  @blur="setFieldTouched('editName')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editProduct.description"
                  label="الوصف"
                  rows="3"
                  :error="shouldShowFieldError('editDescription')"
                  :error-messages="getFieldErrors('editDescription')"
                  @blur="setFieldTouched('editDescription')"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="editProduct.price"
                  label="السعر"
                  type="number"
                  min="0"
                  step="0.01"
                  :error="shouldShowFieldError('editPrice')"
                  :error-messages="getFieldErrors('editPrice')"
                  @blur="setFieldTouched('editPrice')"
                />
              </VCol>
              <VCol cols="6">
                <VTextField
                  v-model.number="editProduct.quantity"
                  label="الكمية"
                  type="number"
                  min="0"
                  :error="shouldShowFieldError('editQuantity')"
                  :error-messages="getFieldErrors('editQuantity')"
                  @blur="setFieldTouched('editQuantity')"
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
            :disabled="hasErrors"
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