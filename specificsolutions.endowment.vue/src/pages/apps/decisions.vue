<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'
import { useApi } from '@/composables/useApi'

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

// Alerts: use the app-wide alerts only

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

// استخدام نظام التنبيهات الموحد
import { useAppAlerts } from '@/composables/useAppAlerts'

const { success: showSuccess, error: showError, warning: showWarning, info: showInfo } = useAppAlerts()

// استخدام i18n للترجمة
const { t, te, locale } = useI18n()

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

// Get API instance
const api = useApi()

const loadDecisions = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await api(`/Decision/filter?${params}`, {
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
    showError(t('pages.decisions.errorLoading'), { timeout: 0, clickToDismiss: true })
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
  if (!validateRequired(newDecision.value.title, 'title', t('pages.decisions.decisionTitle') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(newDecision.value.title, 'title', 1, 100, t('pages.decisions.decisionTitle') + ' (1-100)')) {
    isValid = false
  }
  
  // التحقق من وصف القرار
  if (!validateRequired(newDecision.value.description, 'description', t('pages.decisions.description') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(newDecision.value.description, 'description', 1, 500, t('pages.decisions.description') + ' (1-500)')) {
    isValid = false
  }
  
  // التحقق من رقم المرجع
  if (!validateRequired(newDecision.value.referenceNumber, 'referenceNumber', t('pages.decisions.referenceNumber') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(newDecision.value.referenceNumber, 'referenceNumber', 1, 50, t('pages.decisions.referenceNumber') + ' (1-50)')) {
    isValid = false
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('referenceNumber')
  
  if (!isValid) {
    showWarning(t('pages.decisions.validationWarning'), { timeout: 5000, clickToDismiss: true })
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
    
    const response = await api('/Decision', {
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
        
        showWarning(t('pages.decisions.validationWarning'), { timeout: 5000, clickToDismiss: true })
      } else {
        const errorMsg = response.message || t('pages.decisions.errorAdd')
        showError(errorMsg, { timeout: 0, clickToDismiss: true })
      }
      return
    }
    
    dialog.value = false
    resetNewDecision()
    loadDecisions()
    showSuccess(t('pages.decisions.successAdd'), { timeout: 4000, clickToDismiss: true })
  } catch (error) {
    console.error('Error adding decision:', error)
    showError(t('pages.decisions.errorAdd'), { timeout: 0, clickToDismiss: true })
  }
}

const updateDecision = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // التحقق من صحة البيانات قبل الإرسال
  let isValid = true
  
  // التحقق من عنوان القرار
  if (!validateRequired(editDecision.value.title, 'title', t('pages.decisions.decisionTitle') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(editDecision.value.title, 'title', 1, 100, t('pages.decisions.decisionTitle') + ' (1-100)')) {
    isValid = false
  }
  
  // التحقق من وصف القرار
  if (!validateRequired(editDecision.value.description, 'description', t('pages.decisions.description') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(editDecision.value.description, 'description', 1, 500, t('pages.decisions.description') + ' (1-500)')) {
    isValid = false
  }
  
  // التحقق من رقم المرجع
  if (!validateRequired(editDecision.value.referenceNumber, 'referenceNumber', t('pages.decisions.referenceNumber') + ' ' + t('validation.required', { field: '' }).replace(' {field}', ''))) {
    isValid = false
  } else if (!validateLength(editDecision.value.referenceNumber, 'referenceNumber', 1, 50, t('pages.decisions.referenceNumber') + ' (1-50)')) {
    isValid = false
  }
  
  // تعيين جميع الحقول كملموسة لعرض الأخطاء
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('referenceNumber')
  
  if (!isValid) {
    showWarning(te('validation.fixHighlighted') ? t('validation.fixHighlighted') : '⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', { timeout: 5000, clickToDismiss: true })
    return
  }

  try {
    const response = await api(`/Decision/${editDecision.value.id}`, {
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
        
        showWarning(t('pages.decisions.validationWarning'), { timeout: 5000, clickToDismiss: true })
      } else {
        const errorMsg = response.message || t('pages.decisions.errorUpdate')
        showError(errorMsg, { timeout: 0, clickToDismiss: true })
      }
      return
    }
    
    editDialog.value = false
    loadDecisions()
    showSuccess(t('pages.decisions.successUpdate'), { timeout: 4000, clickToDismiss: true })
  } catch (error) {
    console.error('Error updating decision:', error)
    showError(t('pages.decisions.errorUpdate'), { timeout: 0, clickToDismiss: true })
  }
}

const deleteDecision = async () => {
  if (!selectedDecision.value) return
  
  try {
    const response = await api(`/Decision/${selectedDecision.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // Check if the response indicates success - response comes directly
    if (response && response.isSuccess === false) {
      const errorMsg = response.message || response.errors?.[0]?.errorMessage || t('pages.decisions.errorDelete')
      showError(errorMsg, { timeout: 0, clickToDismiss: true })
      return
    }
    
    deleteDialog.value = false
    selectedDecision.value = null
    loadDecisions()
    showSuccess(t('pages.decisions.successDelete'), { timeout: 4000, clickToDismiss: true })
  } catch (error) {
    console.error('Error deleting decision:', error)
    showError(t('pages.decisions.errorDelete'), { timeout: 0, clickToDismiss: true })
  }
}

const openEditDialog = (decision: Decision) => {
  clearErrors() // Clear previous validation errors
  editDecision.value = { ...decision }
  editDialog.value = true
}

const openDeleteDialog = (decision: Decision) => {
  clearErrors() // Clear any validation errors
  selectedDecision.value = decision
  deleteDialog.value = true
}

const resetNewDecision = () => {
  newDecision.value = {
    title: '',
    description: '',
    referenceNumber: '',
  }
  clearErrors() // Clear validation errors
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
      api(`/Decision/${decision.id}`, {
        method: 'DELETE',
        headers: {
          'Accept-Language': locale.value
        }
      })
    )
    
    await Promise.all(deletePromises)
    
    selectedRows.value = []
    await loadDecisions()
    showSuccess(t('pages.decisions.selectedDeleted'), { timeout: 4000, clickToDismiss: true })
  } catch (error) {
    console.error('Error deleting selected decisions:', error)
    showError(t('pages.decisions.selectedDeleteError'), { timeout: 0, clickToDismiss: true })
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
    <VCardTitle class="d-flex justify-space-between align-center pa-6">
      <span class="text-h5">{{ t('pages.decisions.title') }}</span>
      <div class="d-flex gap-2">
        <VBtn
          v-if="selectedRows.length > 0"
          color="error"
          variant="outlined"
          @click="() => { clearErrors(); deleteSelectedRows(); }"
        >
          {{ t('pages.decisions.deleteSelected', { count: selectedRows.length }) }}
        </VBtn>
        <VBtn
          color="primary"
          @click="() => { clearErrors(); resetNewDecision(); dialog = true; }"
        >
          {{ t('pages.decisions.addDecision') }}
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
            :placeholder="t('pages.decisions.searchPlaceholder')"
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
              <small class="text-medium-emphasis">{{ item.description || t('pages.decisions.noDescription') }}</small>
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
            {{ t('pages.decisions.noReferenceNumber') }}
         </span>
       </template>

       <!-- Description Column -->
       <template #item.description="{ item }">
         <span class="text-medium-emphasis">
            {{ item.description || t('pages.decisions.noDescription') }}
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
                :label="t('pages.decisions.itemsPerPage')"
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
          <span class="text-h5">{{ t('pages.decisions.addNewDecision') }}</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.title"
                  :label="t('pages.decisions.decisionTitle')"
                  required
                  :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title"
                  :error-messages="validationState.errors.title || []"
                  @blur="setFieldTouched('title')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="newDecision.referenceNumber"
                  :label="t('pages.decisions.referenceNumber')"
                  required
                  :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber"
                  :error-messages="validationState.errors.referenceNumber || []"
                  @blur="setFieldTouched('referenceNumber')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newDecision.description"
                  :label="t('pages.decisions.description')"
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
            @click="() => { clearErrors(); dialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="blue-darken-1"
            :disabled="hasErrors"
            @click="() => { clearErrors(); addDecision(); }"
          >
            {{ t('common.add') }}
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
          <span class="text-h5">{{ t('pages.decisions.editDecision') }}</span>
        </VCardTitle>

        <VCardText>
          <VContainer>
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.title"
                  :label="t('pages.decisions.decisionTitle')"
                  required
                  :error="validationState.errors.title && validationState.errors.title.length > 0 && validationState.touched.title"
                  :error-messages="validationState.errors.title || []"
                  @blur="setFieldTouched('title')"
                />
              </VCol>
              <VCol cols="12">
                <VTextField
                  v-model="editDecision.referenceNumber"
                  :label="t('pages.decisions.referenceNumber')"
                  required
                  :error="validationState.errors.referenceNumber && validationState.errors.referenceNumber.length > 0 && validationState.touched.referenceNumber"
                  :error-messages="validationState.errors.referenceNumber || []"
                  @blur="setFieldTouched('referenceNumber')"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editDecision.description"
                  :label="t('pages.decisions.description')"
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
            @click="() => { clearErrors(); editDialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="blue-darken-1"
            :disabled="hasErrors"
            @click="() => { clearErrors(); updateDecision(); }"
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
        <VCardTitle class="text-h5">
          {{ t('pages.decisions.confirmDelete') }}
        </VCardTitle>
        <VCardText>
          {{ t('pages.decisions.deleteConfirmation', { title: selectedDecision?.title }) }}
          <br>
          <strong>{{ t('pages.decisions.deleteWarning') }}</strong>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue-darken-1"
            variant="text"
            @click="() => { clearErrors(); deleteDialog = false; }"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="error"
            @click="() => { clearErrors(); deleteDecision(); }"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
</template> 