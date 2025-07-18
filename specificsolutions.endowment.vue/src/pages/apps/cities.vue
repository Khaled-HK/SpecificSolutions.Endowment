<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'

// Define interfaces for better type safety
interface City {
  id: number
  name: string
  description: string
  regionId?: string
  regionName?: string
  country?: string
}

interface NewCity {
  name: string
  regionId: string
  description: string
}

definePage({
  meta: {
    action: 'View',
    subject: 'City',
  },
})

const cities = ref<City[]>([])
const regions = ref<any[]>([])
const loading = ref(false)
const regionsLoading = ref(false)
const totalItems = ref(0)

const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedCity = ref<City | null>(null)
const selectedRows = ref<City[]>([])
const search = ref('')

const newCity = ref<NewCity>({
  name: '',
  regionId: '',
  description: '',
})

const editCity = ref<City>({
  id: 0,
  name: '',
  description: '',
  regionId: '',
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
    title: 'المدينة',
    key: 'name',
  },
  {
    title: 'المنطقة',
    key: 'regionName',
  },
  {
    title: 'الدولة',
    key: 'country',
  },
  {
    title: 'الإجراءات',
    key: 'actions',
    sortable: false,
  },
]

const loadCities = async () => {
  loading.value = true
  try {
    const params = new URLSearchParams({
      PageNumber: options.value.page.toString(),
      PageSize: options.value.itemsPerPage.toString(),
      SearchTerm: search.value || ''
    })
    
    const response = await $api(`/City/filter?${params}`)
    cities.value = response.data.items || []
    
    // Update total count for pagination
    if (response.data) {
      totalItems.value = response.data.totalCount || 0
      
      // Ensure page number is valid
      const totalPages = Math.ceil(totalItems.value / options.value.itemsPerPage)
      if (options.value.page > totalPages && totalPages > 0) {
        options.value.page = totalPages
        await loadCities() // Reload with correct page
      }
    }
  } catch (error) {
    console.error('Error loading cities:', error)
    cities.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}

const loadRegions = async () => {
  regionsLoading.value = true
  try {
    const response = await $api('/Region/filter?PageSize=100')
    regions.value = response.data.items || []
  } catch (error) {
    console.error('Error loading regions:', error)
  } finally {
    regionsLoading.value = false
  }
}

const addCity = async () => {
  try {
    await $api('/City', {
      method: 'POST',
      body: {
        name: newCity.value.name,
        regionId: newCity.value.regionId,
        description: newCity.value.description,
      },
    })
    dialog.value = false
    resetNewCity()
    loadCities()
  } catch (error) {
    console.error('Error adding city:', error)
  }
}

const updateCity = async () => {
  try {
    await $api(`/City/${editCity.value.id}`, {
      method: 'PUT',
      body: {
        id: editCity.value.id,
        name: editCity.value.name,
        regionId: editCity.value.regionId,
        description: editCity.value.description,
      },
    })
    editDialog.value = false
    loadCities()
  } catch (error) {
    console.error('Error updating city:', error)
  }
}

const deleteCity = async () => {
  if (!selectedCity.value) return
  
  try {
    await $api(`/City/${selectedCity.value.id}`, {
      method: 'DELETE',
    })
    deleteDialog.value = false
    loadCities()
  } catch (error) {
    console.error('Error deleting city:', error)
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(city => 
      $api(`/City/${city.id}`, { method: 'DELETE' })
    )
    await Promise.all(deletePromises)
    selectedRows.value = []
    loadCities()
  } catch (error) {
    console.error('Error deleting selected cities:', error)
  }
}

const openEditDialog = (city: City) => {
  editCity.value = { ...city }
  editDialog.value = true
}

const openDeleteDialog = (city: City) => {
  selectedCity.value = city
  deleteDialog.value = true
}

const resetNewCity = () => {
  newCity.value = {
    name: '',
    regionId: '',
    description: '',
  }
}

// Watch for search changes
watch(search, () => {
  options.value.page = 1 // Reset to first page when searching
  loadCities()
}, { immediate: false })

// Watch for pagination changes
watch([() => options.value.page, () => options.value.itemsPerPage], () => {
  loadCities()
}, { immediate: false })

onMounted(() => {
  loadCities()
  loadRegions()
})
</script>

<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center pa-6">
        <span class="text-h5">إدارة المدن</span>
        <div class="d-flex gap-2">
          <VBtn
            v-if="selectedRows.length > 0"
            color="error"
            variant="outlined"
            @click="deleteSelectedRows"
          >
            حذف المحدد ({{ selectedRows.length }})
          </VBtn>
          <VBtn
            color="primary"
            @click="dialog = true"
          >
            إضافة مدينة
          </VBtn>
        </div>
      </VCardTitle>

      <VDivider />

      <VCardText>
        <!-- Search Bar using ready-made template -->
        <VRow>
          <VCol
            cols="12"
            offset-md="8"
            md="4"
          >
            <VTextField
              v-model="search"
              placeholder="البحث في المدن..."
              prepend-inner-icon="mdi-magnify"
              single-line
              hide-details
              dense
              outlined
            />
          </VCol>
        </VRow>

        <!-- Data Table using ready-made template -->
        <VDataTable
          :headers="headers"
          :items="cities"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          :options="options"
          class="text-no-wrap"
          show-select
          v-model="selectedRows"
        >
          <!-- City name using ready-made template -->
          <template #item.name="{ item }">
            <div class="d-flex align-center">
              <VAvatar
                size="32"
                color="primary"
                variant="tonal"
              >
                {{ item.name.charAt(0).toUpperCase() }}
              </VAvatar>
              <div class="d-flex flex-column ms-3">
                <span class="d-block font-weight-medium text-truncate text-high-emphasis">{{ item.name }}</span>
                <small class="text-medium-emphasis">{{ item.description || 'لا يوجد وصف' }}</small>
              </div>
            </div>
          </template>

          <!-- Region using ready-made template -->
          <template #item.regionName="{ item }">
            <VChip
              v-if="item.regionName"
              color="secondary"
              variant="tonal"
              size="small"
            >
              {{ item.regionName }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد منطقة
            </span>
          </template>

          <!-- Country using ready-made template -->
          <template #item.country="{ item }">
            <VChip
              v-if="item.country"
              color="info"
              variant="tonal"
              size="small"
            >
              {{ item.country }}
            </VChip>
            <span 
              v-else 
              class="text-medium-emphasis"
            >
              لا توجد دولة
            </span>
          </template>

          <!-- Actions using template icons -->
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

          <!-- External pagination using ready-made template -->
          <template #bottom>
            <VCardText class="pt-2">
              <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
                <VSelect
                  v-model="options.itemsPerPage"
                  :items="[5, 10, 25, 50, 100]"
                  label="عناصر في الصفحة:"
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

    <!-- Add City Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة مدينة جديدة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addCity">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newCity.name"
                  label="اسم المدينة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المدينة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="newCity.regionId"
                  label="المنطقة"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  no-data-text="لا توجد مناطق متاحة"
                  required
                  :rules="[v => !!v || 'المنطقة مطلوبة']"
                  prepend-inner-icon="mdi-map-marker"
                  placeholder="اختر المنطقة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newCity.description"
                  label="الوصف"
                  variant="outlined"
                  rows="3"
                  placeholder="أدخل وصف المدينة..."
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
            @click="dialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="addCity"
          >
            حفظ
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit City Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المدينة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateCity">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editCity.name"
                  label="اسم المدينة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المدينة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VAutocomplete
                  v-model="editCity.regionId"
                  label="المنطقة"
                  variant="outlined"
                  :items="regions"
                  item-title="name"
                  item-value="id"
                  :loading="regionsLoading"
                  clearable
                  no-data-text="لا توجد مناطق متاحة"
                  required
                  :rules="[v => !!v || 'المنطقة مطلوبة']"
                  prepend-inner-icon="mdi-map-marker"
                  placeholder="اختر المنطقة..."
                  hide-no-data
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editCity.description"
                  label="الوصف"
                  variant="outlined"
                  rows="3"
                  placeholder="أدخل وصف المدينة..."
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
            @click="editDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="primary"
            variant="flat"
            @click="updateCity"
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
        <VCardTitle class="text-h6">تأكيد الحذف</VCardTitle>
        <VCardText>
          هل أنت متأكد من حذف المدينة "<strong>{{ selectedCity?.name }}</strong>"؟
          <br>
          <span class="text-error">لا يمكن التراجع عن هذا الإجراء.</span>
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="grey-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            إلغاء
          </VBtn>
          <VBtn
            color="error"
            variant="flat"
            @click="deleteCity"
          >
            حذف
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
/* Using template styling */
</style>

