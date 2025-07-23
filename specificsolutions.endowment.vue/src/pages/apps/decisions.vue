<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'

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

// استخدام نظام التحقق من النماذج
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
} = useFormValidation()

// استخدام i18n للترجمة
const { t, locale } = useI18n()

// Using the ready-made template structure
const options = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [''],
  sortDesc: [false],
})

// Headers using i18n translations
const headers = computed(() => [
  {
    title: t('tableHeaders.decisions.title'),
    key: 'title',
  },
  {
    title: t('tableHeaders.decisions.referenceNumber'),
    key: 'referenceNumber',
  },
  {
    title: t('tableHeaders.decisions.description'),
    key: 'description',
  },
  {
    title: t('tableHeaders.decisions.createdDate'),
    key: 'createdDate',
  },
  {
    title: t('tableHeaders.decisions.actions'),
    key: 'actions',
    sortable: false,
  },
])

const loadDecisions = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/Decision/filter?${params}`, {
      headers: {
        'Accept-Language': locale.value
      }
    })
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
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء تحميل القرارات' : 'Error loading decisions'
    alertType.value = 'error'
    showAlert.value = true
    decisions.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const addDecision = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من عنوان القرار
  if (!validateRequired(newDecision.value.title, 'title', locale.value === 'ar' ? 'عنوان القرار مطلوب' : 'Decision title is required')) {
    isValid = false
  } else if (!validateLength(newDecision.value.title, 'title', 1, 100, locale.value === 'ar' ? 'عنوان القرار يجب أن يكون بين 1 و 100 حرف' : 'Decision title must be between 1 and 100 characters')) {
    isValid = false
  }
  
  // التحقق من وصف القرار
  if (!validateRequired(newDecision.value.description, 'description', locale.value === 'ar' ? 'وصف القرار مطلوب' : 'Decision description is required')) {
    isValid = false
  } else if (!validateLength(newDecision.value.description, 'description', 1, 500, locale.value === 'ar' ? 'وصف القرار يجب أن يكون بين 1 و 500 حرف' : 'Decision description must be between 1 and 500 characters')) {
    isValid = false
  }
  
  // التحقق من رقم المرجع
  if (!validateRequired(newDecision.value.referenceNumber, 'referenceNumber', locale.value === 'ar' ? 'رقم المرجع مطلوب' : 'Reference number is required')) {
    isValid = false
  } else if (!validateLength(newDecision.value.referenceNumber, 'referenceNumber', 1, 50, locale.value === 'ar' ? 'رقم المرجع يجب أن يكون بين 1 و 50 حرف' : 'Reference number must be between 1 and 50 characters')) {
    isValid = false
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('referenceNumber')
  
  if (!isValid) {
    return
  }

  try {
    const requestBody = {
      title: newDecision.value.title.trim(),
      description: newDecision.value.description.trim(),
      referenceNumber: newDecision.value.referenceNumber.trim(),
      createdDate: new Date().toISOString(), // إضافة تاريخ الإنشاء (اختياري)
    }
    
    console.log('Sending decision data:', requestBody)
    
    const response = await $api('/Decision', {
      method: 'POST',
      body: requestBody,
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    console.log('Response received:', response)
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
        // تعيين جميع الحقول كملموسة لعرض الأخطاء
        setFieldTouched('title')
        setFieldTouched('description')
        setFieldTouched('referenceNumber')
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء إضافة القرار' : 'Error adding decision')
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    dialog.value = false
    resetNewDecision()
    loadDecisions()
    alertMessage.value = locale.value === 'ar' ? 'تم إضافة القرار بنجاح' : 'Decision added successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error adding decision:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء إضافة القرار' : 'Error adding decision'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const updateDecision = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من عنوان القرار
  if (!validateRequired(editDecision.value.title, 'title', locale.value === 'ar' ? 'عنوان القرار مطلوب' : 'Decision title is required')) {
    isValid = false
  } else if (!validateLength(editDecision.value.title, 'title', 1, 100, locale.value === 'ar' ? 'عنوان القرار يجب أن يكون بين 1 و 100 حرف' : 'Decision title must be between 1 and 100 characters')) {
    isValid = false
  }
  
  // التحقق من وصف القرار
  if (!validateRequired(editDecision.value.description, 'description', locale.value === 'ar' ? 'وصف القرار مطلوب' : 'Decision description is required')) {
    isValid = false
  } else if (!validateLength(editDecision.value.description, 'description', 1, 500, locale.value === 'ar' ? 'وصف القرار يجب أن يكون بين 1 و 500 حرف' : 'Decision description must be between 1 and 500 characters')) {
    isValid = false
  }
  
  // التحقق من رقم المرجع
  if (!validateRequired(editDecision.value.referenceNumber, 'referenceNumber', locale.value === 'ar' ? 'رقم المرجع مطلوب' : 'Reference number is required')) {
    isValid = false
  } else if (!validateLength(editDecision.value.referenceNumber, 'referenceNumber', 1, 50, locale.value === 'ar' ? 'رقم المرجع يجب أن يكون بين 1 و 50 حرف' : 'Reference number must be between 1 and 50 characters')) {
    isValid = false
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('referenceNumber')
  
  if (!isValid) {
    return
  }

  try {
    const response = await $api(`/Decision/${editDecision.value.id}`, {
      method: 'PUT',
      body: {
        id: editDecision.value.id,
        title: editDecision.value.title.trim(),
        description: editDecision.value.description.trim(),
        referenceNumber: editDecision.value.referenceNumber.trim(),
      },
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      // Handle backend validation errors
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
        // تعيين جميع الحقول كملموسة لعرض الأخطاء
        setFieldTouched('title')
        setFieldTouched('description')
        setFieldTouched('referenceNumber')
      } else {
        const errorMsg = response.message || (locale.value === 'ar' ? 'حدث خطأ أثناء تحديث القرار' : 'Error updating decision')
        alertMessage.value = errorMsg
        alertType.value = 'error'
        showAlert.value = true
      }
      return
    }
    
    editDialog.value = false
    loadDecisions()
    alertMessage.value = locale.value === 'ar' ? 'تم تحديث القرار بنجاح' : 'Decision updated successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error updating decision:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء تحديث القرار' : 'Error updating decision'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const deleteDecision = async () => {
  if (!selectedDecision.value) return
  
  try {
    const response = await $api(`/Decision/${selectedDecision.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || (locale.value === 'ar' ? 'حدث خطأ أثناء حذف القرار' : 'Error deleting decision')
      alertMessage.value = errorMsg
      alertType.value = 'error'
      showAlert.value = true
      return
    }
    
    deleteDialog.value = false
    selectedDecision.value = null
    loadDecisions()
    alertMessage.value = locale.value === 'ar' ? 'تم حذف القرار بنجاح' : 'Decision deleted successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting decision:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء حذف القرار' : 'Error deleting decision'
    alertType.value = 'error'
    showAlert.value = true
  }
}

const openEditDialog = (decision: Decision) => {
  editDecision.value = { ...decision }
  editDialog.value = true
  // مسح أخطاء التحقق عند فتح النافذة
  clearErrors()
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
  // مسح أخطاء التحقق
  clearErrors()
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

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(decision => 
      $api(`/Decision/${decision.id}`, {
        method: 'DELETE',
        headers: {
          'Accept-Language': locale.value
        }
      })
    )
    
    await Promise.all(deletePromises)
    
    selectedRows.value = []
    await loadDecisions()
    alertMessage.value = locale.value === 'ar' ? 'تم حذف القرارات المحددة بنجاح' : 'Selected decisions deleted successfully'
    alertType.value = 'success'
    showAlert.value = true
  } catch (error) {
    console.error('Error deleting selected decisions:', error)
    alertMessage.value = locale.value === 'ar' ? 'حدث خطأ أثناء حذف القرارات المحددة' : 'Error deleting selected decisions'
    alertType.value = 'error'
    showAlert.value = true
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
  <!-- Alert for showing messages -->
  <VAlert
    v-if="showAlert"
    :type="alertType as 'success' | 'info' | 'warning' | 'error'"
    :title="alertType === 'error' ? 'خطأ' : alertType === 'success' ? 'نجح' : 'معلومات'"
    closable
    class="mb-4"
  >
    {{ alertMessage }}
  </VAlert>

  <VCard>
    <VCardTitle class="d-flex justify-space-between align-center pa-6">
      <span class="text-h5">{{ locale === 'ar' ? 'إدارة القرارات' : 'Decisions Management' }}</span>
      <div class="d-flex gap-2">
        <VBtn
          v-if="selectedRows.length > 0"
          color="error"
          variant="outlined"
          @click="deleteSelectedRows"
        >
          {{ locale === 'ar' ? `حذف المحدد (${selectedRows.length})` : `Delete Selected (${selectedRows.length})` }}
        </VBtn>
        <VBtn
          color="primary"
          @click="() => { clearErrors(); dialog = true; }"
        >
          {{ locale === 'ar' ? 'إضافة قرار' : 'Add Decision' }}
        </VBtn>
      </div>
    </VCardTitle>

    <VDivider />

    <VCardText>
      <!-- Search Bar -->
      <VRow>
        <VCol
          cols="12"
          offset-md="8"
          md="4"
        >
          <VTextField
            v-model="search"
            :placeholder="locale === 'ar' ? 'البحث في القرارات...' : 'Search decisions...'"
            prepend-inner-icon="mdi-magnify"
            single-line
            hide-details
            dense
            outlined
          />
        </VCol>
      </VRow>

      <!-- Data Table -->
      <VDataTable
        :headers="headers"
        :items="decisions"
        :loading="loading"
        :items-per-page="options.itemsPerPage"
        :page="options.page"
        :options="options"
        class="text-no-wrap"
        show-select
        v-model="selectedRows"
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
             <small class="text-medium-emphasis">{{ item.description || (locale === 'ar' ? 'لا يوجد وصف' : 'No description') }}</small>
           </div>
         </div>
       </template>

       <!-- Reference Number Column -->
       <template #item.referenceNumber="{ item }">
         <VChip
           v-if="item.referenceNumber"
           color="success"
           variant="tonal"
           size="small"
         >
           {{ item.referenceNumber }}
         </VChip>
         <span 
           v-else 
           class="text-medium-emphasis"
         >
           {{ locale === 'ar' ? 'لا يوجد رقم مرجعي' : 'No reference number' }}
         </span>
       </template>

       <!-- Description Column -->
       <template #item.description="{ item }">
         <span class="text-medium-emphasis">
           {{ item.description || (locale === 'ar' ? 'لا يوجد وصف' : 'No description') }}
         </span>
       </template>

       <!-- Created Date Column -->
       <template #item.createdDate="{ item }">
         <VChip
           color="info"
           variant="tonal"
           size="small"
         >
           {{ new Date(item.createdDate).toLocaleDateString(locale === 'ar' ? 'ar-SA' : 'en-US') }}
         </VChip>
       </template>

       <!-- Actions Column -->
       <template #item.actions="{ item }">
         <div class="d-flex gap-1">
           <IconBtn @click="openEditDialog(item)">
             <VIcon icon="tabler-edit" />
           </IconBtn>
           <IconBtn @click="openDeleteDialog(item)">
             <VIcon icon="tabler-trash" />
           </IconBtn>
         </div>
       </template>

       <!-- External pagination -->
       <template #bottom>
         <VCardText class="pt-2">
           <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
             <VSelect
               v-model="options.itemsPerPage"
               :items="[5, 10, 25, 50, 100]"
               :label="locale === 'ar' ? 'عناصر في الصفحة:' : 'Items per page:'"
               variant="underlined"
               style="max-inline-size: 8rem;min-inline-size: 5rem;"
             />

             <VPagination
               v-model="options.page"
               :total-visible="$vuetify.display.smAndDown ? 3 : 5"
               :length="Math.ceil(totalItems / options.itemsPerPage)"
             />
           </div>
         </VCardText>
       </template>
     </VDataTable>
   </VCardText>
 </VCard>

    <!-- Add Decision Dialog -->
    <VDialog
      v-model="dialog"
      max-width="500px"
    >
      <VCard>
        <VCardTitle>
          <span class="text-h5">{{ locale === 'ar' ? 'إضافة قرار جديد' : 'Add New Decision' }}</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.title"
                  :label="locale === 'ar' ? 'عنوان القرار' : 'Decision Title'"
                  required
                  :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title"
                  :error-messages="validationState.errors.title || []"
                  @blur="setFieldTouched('title')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.referenceNumber"
                  :label="locale === 'ar' ? 'رقم المرجع' : 'Reference Number'"
                  required
                  :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber"
                  :error-messages="validationState.errors.referenceNumber || []"
                  @blur="setFieldTouched('referenceNumber')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newDecision.description"
                  :label="locale === 'ar' ? 'الوصف' : 'Description'"
                  rows="3"
                  :error="validationState.errors.description && validationState.errors.description.length > 0 && validationState.touched.description"
                  :error-messages="validationState.errors.description || []"
                  @blur="setFieldTouched('description')"
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
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="blue-darken-1"
            :disabled="hasErrors"
            @click="addDecision"
          >
            {{ locale === 'ar' ? 'إضافة' : 'Add' }}
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
          <span class="text-h5">{{ locale === 'ar' ? 'تعديل القرار' : 'Edit Decision' }}</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.title"
                  :label="locale === 'ar' ? 'عنوان القرار' : 'Decision Title'"
                  required
                  :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title"
                  :error-messages="validationState.errors.title || []"
                  @blur="setFieldTouched('title')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.referenceNumber"
                  :label="locale === 'ar' ? 'رقم المرجع' : 'Reference Number'"
                  required
                  :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber"
                  :error-messages="validationState.errors.referenceNumber || []"
                  @blur="setFieldTouched('referenceNumber')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editDecision.description"
                  :label="locale === 'ar' ? 'الوصف' : 'Description'"
                  rows="3"
                  :error="validationState.errors.description && validationState.errors.description.length > 0 && validationState.touched.description"
                  :error-messages="validationState.errors.description || []"
                  @blur="setFieldTouched('description')"
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
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="blue-darken-1"
            :disabled="hasErrors"
            @click="updateDecision"
          >
            {{ locale === 'ar' ? 'تحديث' : 'Update' }}
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
          {{ locale === 'ar' ? 'تأكيد الحذف' : 'Confirm Delete' }}
        </VCardTitle>
        <VCardText>
          {{ locale === 'ar' ? `هل أنت متأكد من حذف القرار "${selectedDecision?.title}"؟` : `Are you sure you want to delete the decision "${selectedDecision?.title}"?` }}
          <br>
          <strong>{{ locale === 'ar' ? 'لا يمكن التراجع عن هذا الإجراء.' : 'This action cannot be undone.' }}</strong>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            {{ locale === 'ar' ? 'إلغاء' : 'Cancel' }}
          </VBtn>
          <VBtn
            color="error"
            @click="deleteDecision"
          >
            {{ locale === 'ar' ? 'حذف' : 'Delete' }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
</template> 