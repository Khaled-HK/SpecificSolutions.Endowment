<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'
import { useI18n } from 'vue-i18n'
import { useApi } from '@/composables/useApi'

definePage({
  meta: {
    action: 'View',
    subject: 'Building',
  },
})

// استخدام نظام التحقق
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
  addError,
} = useFormValidation()

// استخدام i18n للترجمة
const { t, locale } = useI18n()

const buildings = ref([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedBuilding = ref(null)

const newBuilding = ref({
  name: '',
  address: '',
  cityId: null,
  regionId: null,
  description: '',
})

const editBuilding = ref({
  id: null,
  name: '',
  address: '',
  cityId: null,
  regionId: null,
  description: '',
})

const cities = ref([])
const regions = ref([])

// Headers ديناميكية مع دعم الترجمة
const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.buildings.name'), key: 'name' },
  { title: t('tableHeaders.buildings.address'), key: 'address' },
  { title: t('tableHeaders.buildings.city'), key: 'cityName' },
  { title: t('tableHeaders.buildings.region'), key: 'regionName' },
  { title: t('tableHeaders.buildings.actions'), key: 'actions', sortable: false },
])

// Get API instance
const api = useApi()

const loadBuildings = async () => {
  loading.value = true
  try {
    const response = await api('/Buildings/filter?PageNumber=1&PageSize=10', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    buildings.value = response.data?.items || response
  } catch (error) {
    console.error('Error loading buildings:', error)
  } finally {
    loading.value = false
  }
}

const loadCities = async () => {
  try {
    const response = await api('/City/GetCities', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    cities.value = response
  } catch (error) {
    console.error('Error loading cities:', error)
  }
}

const loadRegions = async () => {
  try {
    const response = await api('/Region/GetRegions', {
      headers: {
        'Accept-Language': locale.value
      }
    })
    regions.value = response
  } catch (error) {
    console.error('Error loading regions:', error)
  }
}

const addBuilding = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // تعيين الحقول كملموسة لعرض الأخطاء
  setFieldTouched('name')
  setFieldTouched('address')
  setFieldTouched('cityId')
  setFieldTouched('regionId')
  
  // التحقق من صحة البيانات
  let isValid = true
  
  if (!validateRequired(newBuilding.value.name, 'name', locale.value === 'ar' ? 'اسم المبنى مطلوب' : 'Building name is required')) {
    isValid = false
  } else if (!validateLength(newBuilding.value.name, 'name', 2, 200, locale.value === 'ar' ? 'اسم المبنى يجب أن يكون بين 2 و 200 حرف' : 'Building name must be between 2 and 200 characters')) {
    isValid = false
  }
  
  if (!validateRequired(newBuilding.value.address, 'address', locale.value === 'ar' ? 'عنوان المبنى مطلوب' : 'Building address is required')) {
    isValid = false
  } else if (!validateLength(newBuilding.value.address, 'address', 5, 500, locale.value === 'ar' ? 'عنوان المبنى يجب أن يكون بين 5 و 500 حرف' : 'Building address must be between 5 and 500 characters')) {
    isValid = false
  }
  
  if (!newBuilding.value.cityId) {
    addError('cityId', locale.value === 'ar' ? 'المدينة مطلوبة' : 'City is required')
    isValid = false
  }
  
  if (!newBuilding.value.regionId) {
    addError('regionId', locale.value === 'ar' ? 'المنطقة مطلوبة' : 'Region is required')
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await api('/Buildings', {
      method: 'POST',
      body: newBuilding.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // معالجة أخطاء الباك إند
    if (response && response.isSuccess === false) {
      if (response.errors && response.errors.length > 0) {
        setErrorsFromResponse(response)
        setFieldTouched('name')
        setFieldTouched('address')
        setFieldTouched('cityId')
        setFieldTouched('regionId')
        return
      }
    }
    
    dialog.value = false
    resetNewBuilding()
    loadBuildings()
  } catch (error) {
    console.error('Error adding building:', error)
  }
}

const updateBuilding = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // تعيين الحقول كملموسة لعرض الأخطاء
  setFieldTouched('editName')
  setFieldTouched('editAddress')
  setFieldTouched('editCityId')
  setFieldTouched('editRegionId')
  
  // التحقق من صحة البيانات
  let isValid = true
  
  if (!validateRequired(editBuilding.value.name, 'editName', locale.value === 'ar' ? 'اسم المبنى مطلوب' : 'Building name is required')) {
    isValid = false
  } else if (!validateLength(editBuilding.value.name, 'editName', 2, 200, locale.value === 'ar' ? 'اسم المبنى يجب أن يكون بين 2 و 200 حرف' : 'Building name must be between 2 and 200 characters')) {
    isValid = false
  }
  
  if (!validateRequired(editBuilding.value.address, 'editAddress', locale.value === 'ar' ? 'عنوان المبنى مطلوب' : 'Building address is required')) {
    isValid = false
  } else if (!validateLength(editBuilding.value.address, 'editAddress', 5, 500, locale.value === 'ar' ? 'عنوان المبنى يجب أن يكون بين 5 و 500 حرف' : 'Building address must be between 5 and 500 characters')) {
    isValid = false
  }
  
  if (!editBuilding.value.cityId) {
    addError('editCityId', locale.value === 'ar' ? 'المدينة مطلوبة' : 'City is required')
    isValid = false
  }
  
  if (!editBuilding.value.regionId) {
    addError('editRegionId', locale.value === 'ar' ? 'المنطقة مطلوبة' : 'Region is required')
    isValid = false
  }
  
  if (!isValid) {
    return
  }
  
  try {
    const response = await api(`/Buildings/${editBuilding.value.id}`, {
      method: 'PUT',
      body: editBuilding.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    
    // معالجة أخطاء الباك إند
    if (response && response.isSuccess === false) {
      if (response.errors && response.errors.length > 0) {
        setErrorsFromResponse(response)
        setFieldTouched('editName')
        setFieldTouched('editAddress')
        setFieldTouched('editCityId')
        setFieldTouched('editRegionId')
        return
      }
    }
    
    editDialog.value = false
    loadBuildings()
  } catch (error) {
    console.error('Error updating building:', error)
  }
}

const deleteBuilding = async () => {
  try {
    await api(`/Buildings/${selectedBuilding.value.id}`, {
      method: 'DELETE',
      headers: {
        'Accept-Language': locale.value
      }
    })
    deleteDialog.value = false
    loadBuildings()
  } catch (error) {
    console.error('Error deleting building:', error)
  }
}

const openEditDialog = (building) => {
  editBuilding.value = { ...building }
  editDialog.value = true
  clearErrors() // مسح أخطاء التحقق
}

const openDeleteDialog = (building) => {
  selectedBuilding.value = building
  deleteDialog.value = true
}

const resetNewBuilding = () => {
  newBuilding.value = {
    name: '',
    address: '',
    cityId: null,
    regionId: null,
    description: '',
  }
  clearErrors() // مسح أخطاء التحقق
}

onMounted(() => {
  loadBuildings()
  loadCities()
  loadRegions()
})
</script>

<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center">
        <span>{{ t('pages.buildings.title') }}</span>
        <VBtn
          color="primary"
          @click="dialog = true"
        >
          {{ t('pages.buildings.addBuilding') }}
        </VBtn>
      </VCardTitle>

      <VCardText>
        <VDataTable
          :headers="headers"
          :items="buildings"
          :loading="loading"
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
              @click="openDeleteDialog(item)"
            >
              <VIcon>mdi-delete</VIcon>
            </VBtn>
          </template>
        </VDataTable>
      </VCardText>
    </VCard>

    <!-- Add Building Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>{{ t('pages.buildings.addNewBuilding') }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addBuilding">
            <VTextField
              v-model="newBuilding.name"
              :label="t('pages.buildings.buildingName')"
              required
              :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
              :error-messages="validationState.errors.name || []"
              @blur="setFieldTouched('name')"
            />
            <VTextField
              v-model="newBuilding.address"
              :label="t('pages.buildings.address')"
              required
              :error="validationState.errors.address && validationState.errors.address.length > 0 && validationState.touched.address"
              :error-messages="validationState.errors.address || []"
              @blur="setFieldTouched('address')"
            />
            <VSelect
              v-model="newBuilding.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              :label="t('pages.buildings.city')"
              required
              :error="validationState.errors.cityId && validationState.errors.cityId.length > 0 && validationState.touched.cityId"
              :error-messages="validationState.errors.cityId || []"
              @blur="setFieldTouched('cityId')"
            />
            <VSelect
              v-model="newBuilding.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              :label="t('pages.buildings.region')"
              required
              :error="validationState.errors.regionId && validationState.errors.regionId.length > 0 && validationState.touched.regionId"
              :error-messages="validationState.errors.regionId || []"
              @blur="setFieldTouched('regionId')"
            />
            <VTextarea
              v-model="newBuilding.description"
              :label="t('pages.buildings.description')"
            />
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue darken-1"
            text
            @click="dialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="blue darken-1"
            text
            @click="addBuilding"
            :disabled="hasErrors"
          >
            {{ t('common.save') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Building Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>{{ t('pages.buildings.editBuilding') }}</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateBuilding">
            <VTextField
              v-model="editBuilding.name"
              :label="t('pages.buildings.buildingName')"
              required
              :error="validationState.errors.editName && validationState.errors.editName.length > 0 && validationState.touched.editName"
              :error-messages="validationState.errors.editName || []"
              @blur="setFieldTouched('editName')"
            />
            <VTextField
              v-model="editBuilding.address"
              :label="t('pages.buildings.address')"
              required
              :error="validationState.errors.editAddress && validationState.errors.editAddress.length > 0 && validationState.touched.editAddress"
              :error-messages="validationState.errors.editAddress || []"
              @blur="setFieldTouched('editAddress')"
            />
            <VSelect
              v-model="editBuilding.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              :label="t('pages.buildings.city')"
              required
              :error="validationState.errors.editCityId && validationState.errors.editCityId.length > 0 && validationState.touched.editCityId"
              :error-messages="validationState.errors.editCityId || []"
              @blur="setFieldTouched('editCityId')"
            />
            <VSelect
              v-model="editBuilding.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              :label="t('pages.buildings.region')"
              required
              :error="validationState.errors.editRegionId && validationState.errors.editRegionId.length > 0 && validationState.touched.editRegionId"
              :error-messages="validationState.errors.editRegionId || []"
              @blur="setFieldTouched('editRegionId')"
            />
            <VTextarea
              v-model="editBuilding.description"
              :label="t('pages.buildings.description')"
            />
          </VForm>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue darken-1"
            text
            @click="editDialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="blue darken-1"
            text
            @click="updateBuilding"
            :disabled="hasErrors"
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
        <VCardTitle>{{ t('pages.buildings.confirmDelete') }}</VCardTitle>
        <VCardText>
          {{ t('pages.buildings.deleteConfirmation') }}
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue darken-1"
            text
            @click="deleteDialog = false"
          >
            {{ t('common.cancel') }}
          </VBtn>
          <VBtn
            color="red darken-1"
            text
            @click="deleteBuilding"
          >
            {{ t('common.delete') }}
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* تخصيص مظهر الحقل عند وجود خطأ */
:deep(.v-field--error) {
  border-color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-field__outline) {
  color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-label) {
  color: rgb(var(--v-theme-error)) !important;
}

/* تخصيص مظهر رسائل الخطأ */
:deep(.v-messages__message) {
  color: rgb(var(--v-theme-error)) !important;
  font-size: 0.75rem;
  margin-top: 4px;
}
</style>

