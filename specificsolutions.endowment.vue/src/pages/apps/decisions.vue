<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface Decision {
  id: number
  key: string
  value: string
  title: string
  description: string
  referenceNumber: string
  createdDate: string
}

interface NewDecision {
  title: string
  description: string
  referenceNumber: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'Decision',
  },
})

const decisions = ref<Decision[]>([])
const loading = ref(false)
const totalItems = ref(0)

// Simple alert state
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref('success')

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedDecision = ref<Decision | null>(null)
const selectedRows = ref<Decision[]>([])
const search = ref('')

const newDecision = ref<NewDecision>({
  title: '',
  description: '',
  referenceNumber: '',
})

const editDecision = ref<Decision>({
  id: 0,
  key: '',
  value: '',
  title: '',
  description: '',
  referenceNumber: '',
  createdDate: '',
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
    title: 'عنوان القرار',
    key: 'title',
  },
  {
    title: 'رقم المرجع',
    key: 'referenceNumber',
  },
  {
    title: 'الوصف',
    key: 'description',
  },
  {
    title: 'تاريخ الإنشاء',
    key: 'createdDate',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadDecisions = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Decision/filter?${params}`)
    decisions.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadDecisions() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading decisions:', error)
    alertMessage.value = 'حدث خطأ أثناء تحميل القرارات'
    alertType.value = 'error'
    showAlert.value = true
    decisions.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const addDecision = async () => {
  try {
    const response = await $api('/Decision', {
      method: 'POST',
      body: {
        title: newDecision.value.title,
        description: newDecision.value.description,
        referenceNumber: newDecision.value.referenceNumber,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء إضافة القرار'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    dialog.value = false
    resetNewDecision()
    loadDecisions()
    alertMessage.value = 'تم إضافة القرار بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding decision:', error)
    alertMessage.value = 'حدث خطأ أثناء إضافة القرار'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateDecision = async () => {
  try {
    const response = await $api(`/Decision/${editDecision.value.id}`, {
      method: 'PUT',
      body: {
        id: editDecision.value.id,
        title: editDecision.value.title,
        description: editDecision.value.description,
        referenceNumber: editDecision.value.referenceNumber,
      },
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء تحديث القرار'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    editDialog.value = false
    loadDecisions()
    alertMessage.value = 'تم تحديث القرار بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating decision:', error)
    alertMessage.value = 'حدث خطأ أثناء تحديث القرار'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteDecision = async () => {
  if (!selectedDecision.value) return
  
  try {
    const response = await $api(`/Decision/${selectedDecision.value.id}`, {
      method: 'DELETE',
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || 'حدث خطأ أثناء حذف القرار'
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    deleteDialog.value = false
    selectedDecision.value = null
    loadDecisions()
    alertMessage.value = 'تم حذف القرار بنجاح'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting decision:', error)
    alertMessage.value = 'حدث خطأ أثناء حذف القرار'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (decision: Decision) => {
  editDecision.value = { ...decision }
  editDialog.value = true
}

const openDeleteDialog = (decision: Decision) => {
  selectedDecision.value = decision
  deleteDialog.value = true
}

const resetNewDecision = () => {
  newDecision.value = {
    title: '',
    description: '',
    referenceNumber: '',
  }
}

const resetEditDecision = () => {
  editDecision.value = {
    id: 0,
    key: '',
    value: '',
    title: '',
    description: '',
    referenceNumber: '',
    createdDate: '',
  }
}

// Watch for changes in options to reload data
watch(options, () => {
  loadDecisions()
}, { deep: true })

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadDecisions()
})

onMounted(() => {
  loadDecisions()
})
</script>

<template>
  <VCard>
    <!-- Header -->
    <VCardText class="d-flex align-center flex-wrap gap-4">
      <div class="me-3">
        <h4 class="text-h4">
          إدارة القرارات
        </h4>
        <p class="mb-0">
          عرض وإدارة القرارات في النظام
        </p>
      </div>
      <VSpacer />
      <VBtn
        prepend-icon="tabler-plus"
        @click="dialog = true"
      >
        إضافة قرار جديد
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
            label="البحث في القرارات"
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
      :items="decisions"
      :loading="loading"
      :items-per-page="options.itemsPerPage"
      :page="options.page"
      :total="totalItems"
      class="text-no-wrap"
      @update:options="options = $event"
    >
      <!-- Title Column -->
      <template #item.title="{ item }">
        <div class="d-flex align-center">
          <VAvatar
            size="32"
            color="primary"
            variant="tonal"
          >
            {{ item.title.charAt(0).toUpperCase() }}
          </VAvatar>
          <div class="d-flex flex-column ms-3">
            <span class="d-block font-weight-medium text-truncate text-high-emphasis">{{ item.title }}</span>
          </div>
        </div>
      </template>

      <!-- Reference Number Column -->
      <template #item.referenceNumber="{ item }">
        <VChip
          :color="item.referenceNumber ? 'success' : 'warning'"
          size="small"
        >
          {{ item.referenceNumber || 'لا يوجد رقم مرجعي' }}
        </VChip>
      </template>

      <!-- Description Column -->
      <template #item.description="{ item }">
        <span class="text-medium-emphasis">
          {{ item.description || 'لا يوجد وصف' }}
        </span>
      </template>

      <!-- Created Date Column -->
      <template #item.createdDate="{ item }">
        <VChip
          color="info"
          size="small"
        >
          {{ new Date(item.createdDate).toLocaleDateString('ar-SA') }}
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

    <!-- Add Decision Dialog -->
    <VDialog
      v-model="dialog"
      max-width="500px"
    >
      <VCard>
        <VCardTitle>
          <span class="text-h5">إضافة قرار جديد</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.title"
                  label="عنوان القرار"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.referenceNumber"
                  label="رقم المرجع"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newDecision.description"
                  label="الوصف"
                  rows="3"
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
            @click="addDecision"
          >
            إضافة
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Decision Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="500px"
    >
      <VCard>
        <VCardTitle>
          <span class="text-h5">تعديل القرار</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.title"
                  label="عنوان القرار"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.referenceNumber"
                  label="رقم المرجع"
                  required
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editDecision.description"
                  label="الوصف"
                  rows="3"
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
            @click="updateDecision"
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
          هل أنت متأكد من حذف القرار "{{ selectedDecision?.title }}"؟
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
            @click="deleteDecision"
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