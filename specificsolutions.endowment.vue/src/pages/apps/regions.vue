<script setup lang="ts">
import { ref, onMounted } from 'vue'

// Define interfaces for better type safety
interface Region {
  id: number
  key: string
  value: string
  name: string
  description: string
}

interface NewRegion {
  name: string
  description: string
}

// Define header interface for proper typing
interface DataTableHeader {
  title: string
  key: string
  sortable: boolean
  sticky?: boolean
  width?: number
  align?: 'start' | 'center' | 'end'
}

definePage({
  meta: {
    action: 'View',
    subject: 'Region',
  },
})

const regions = ref<Region[]>([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedRegion = ref<Region | null>(null)
const selectedRows = ref<Region[]>([])
const search = ref('')

const newRegion = ref<NewRegion>({
  name: '',
  description: '',
})

const editRegion = ref<Region>({
  id: 0,
  key: '',
  value: '',
  name: '',
  description: '',
})

// Headers with sticky columns configuration - properly typed
const headers: DataTableHeader[] = [
  { 
    title: 'ID', 
    key: 'key', 
    sortable: true,
    sticky: true,
    width: 100
  },
  { 
    title: 'Name', 
    key: 'value', 
    sortable: true,
    sticky: true,
    width: 250
  },
  { 
    title: 'Description', 
    key: 'description', 
    sortable: true,
    width: 400
  },
  { 
    title: 'Actions', 
    key: 'actions', 
    sortable: false,
    sticky: true,
    width: 120,
    align: 'center' as const
  },
]

// Table options for enhanced functionality (Fixed TypeScript error)
const tableOptions = ref({
  page: 1,
  itemsPerPage: 10,
  sortBy: [{ key: 'key', order: 'asc' as const }],
})

const loadRegions = async () => {
  loading.value = true
  try {
    const response = await $api('/Region/GetRegions')
    regions.value = response.data
  } catch (error) {
    console.error('Error loading regions:', error)
  } finally {
    loading.value = false
  }
}

const addRegion = async () => {
  try {
    await $api('/Regions', {
      method: 'POST',
      body: newRegion.value,
    })
    dialog.value = false
    resetNewRegion()
    loadRegions()
  } catch (error) {
    console.error('Error adding region:', error)
  }
}

const updateRegion = async () => {
  try {
    await $api(`/Regions/${editRegion.value.id}`, {
      method: 'PUT',
      body: editRegion.value,
    })
    editDialog.value = false
    loadRegions()
  } catch (error) {
    console.error('Error updating region:', error)
  }
}

const deleteRegion = async () => {
  if (!selectedRegion.value) return
  
  try {
    await $api(`/Regions/${selectedRegion.value.id}`, {
      method: 'DELETE',
    })
    deleteDialog.value = false
    loadRegions()
  } catch (error) {
    console.error('Error deleting region:', error)
  }
}

const deleteSelectedRows = async () => {
  if (selectedRows.value.length === 0) return
  
  try {
    const deletePromises = selectedRows.value.map(region => 
      $api(`/Regions/${region.id}`, { method: 'DELETE' })
    )
    await Promise.all(deletePromises)
    selectedRows.value = []
    loadRegions()
  } catch (error) {
    console.error('Error deleting selected regions:', error)
  }
}

const openEditDialog = (region: Region) => {
  editRegion.value = { ...region }
  editDialog.value = true
}

const openDeleteDialog = (region: Region) => {
  selectedRegion.value = region
  deleteDialog.value = true
}

const resetNewRegion = () => {
  newRegion.value = {
    name: '',
    description: '',
  }
}

onMounted(() => {
  loadRegions()
})
</script>

<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center pa-6">
        <span class="text-h5">إدارة المناطق - جدول بأعمدة ثابتة</span>
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
            إضافة منطقة
          </VBtn>
        </div>
      </VCardTitle>

      <VDivider />

      <VCardText>
        <!-- Search Bar -->
        <div class="d-flex justify-space-between align-center mb-4">
          <VTextField
            v-model="search"
            prepend-inner-icon="mdi-magnify"
            label="البحث في المناطق..."
            variant="outlined"
            density="compact"
            style="max-width: 300px;"
            hide-details
          />
          
          <VChip
            color="success"
            variant="tonal"
            prepend-icon="mdi-table-column"
          >
            الأعمدة الثابتة نشطة ✓
          </VChip>
          
          <div class="text-caption text-medium-emphasis">
            المجموع: {{ regions.length }} منطقة
          </div>
        </div>

        <!-- Enhanced Data Table with Fixed Columns -->
        <VDataTable
          v-model="selectedRows"
          v-model:page="tableOptions.page"
          v-model:items-per-page="tableOptions.itemsPerPage"
          v-model:sort-by="tableOptions.sortBy"
          :headers="headers"
          :items="regions"
          :loading="loading"
          :search="search"
          class="elevation-1 text-no-wrap fixed-columns-table"
          item-value="id"
          show-select
          fixed-header
          height="600"
          hover
          density="compact"
        >
          <!-- Custom ID column with badge -->
          <template #item.key="{ item }">
            <VChip
              color="primary"
              variant="tonal"
              size="small"
              class="font-weight-bold"
            >
              #{{ item.key }}
            </VChip>
          </template>

          <!-- Enhanced Name column -->
          <template #item.value="{ item }">
            <div class="d-flex align-center">
              <VAvatar
                color="primary"
                variant="tonal"
                size="32"
                class="me-3"
              >
                {{ item.value.charAt(0).toUpperCase() }}
              </VAvatar>
              <div>
                <div class="text-body-1 font-weight-medium">
                  {{ item.value }}
                </div>
                <div class="text-caption text-medium-emphasis">
                  الرقم: {{ item.key }}
                </div>
              </div>
            </div>
          </template>

          <!-- Description with truncation -->
          <template #item.description="{ item }">
            <div class="text-body-2" style="max-width: 350px;">
              <VTooltip
                v-if="item.description && item.description.length > 50"
                location="top"
                max-width="400"
              >
                <template #activator="{ props }">
                  <span
                    v-bind="props"
                    class="text-truncate d-block"
                  >
                    {{ item.description }}
                  </span>
                </template>
                <span>{{ item.description }}</span>
              </VTooltip>
              <span 
                v-else 
                class="text-medium-emphasis"
              >
                {{ item.description || 'لا يوجد وصف' }}
              </span>
            </div>
          </template>

          <!-- Enhanced Actions column -->
          <template #item.actions="{ item }">
            <div class="d-flex gap-1 justify-center">
              <VBtn
                icon="mdi-pencil"
                size="small"
                color="primary"
                variant="text"
                @click="openEditDialog(item)"
              >
                <VIcon>mdi-pencil</VIcon>
                <VTooltip
                  activator="parent"
                  location="top"
                >
                  تعديل
                </VTooltip>
              </VBtn>
              <VBtn
                icon="mdi-delete"
                size="small"
                color="error"
                variant="text"
                @click="openDeleteDialog(item)"
              >
                <VIcon>mdi-delete</VIcon>
                <VTooltip
                  activator="parent"
                  location="top"
                >
                  حذف
                </VTooltip>
              </VBtn>
            </div>
          </template>

          <!-- Custom loading state -->
          <template #loading>
            <VSkeletonLoader
              class="mx-auto"
              max-width="100%"
              type="table-row@10"
            />
          </template>

          <!-- Empty state -->
          <template #no-data>
            <div class="text-center py-8">
              <VIcon
                size="64"
                color="grey-lighten-2"
                class="mb-4"
              >
                mdi-table-search
              </VIcon>
              <div class="text-h6 text-medium-emphasis">
                لا توجد مناطق
              </div>
              <div class="text-body-2 text-medium-emphasis">
                جرب تعديل البحث أو إضافة منطقة جديدة
              </div>
            </div>
          </template>
        </VDataTable>

        <!-- Info Card for Fixed Columns -->
        <VCard
          class="mt-4"
          color="info"
          variant="tonal"
        >
          <VCardText class="d-flex align-center gap-3">
            <VIcon>mdi-information</VIcon>
            <div>
              <div class="text-body-2 font-weight-medium">
                الأعمدة الثابتة (Fixed Columns)
              </div>
              <div class="text-caption">
                أعمدة الرقم والاسم والإجراءات ثابتة ولا تتحرك عند التمرير الأفقي
              </div>
            </div>
          </VCardText>
        </VCard>
      </VCardText>
    </VCard>

    <!-- Add Region Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">إضافة منطقة جديدة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addRegion">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="newRegion.name"
                  label="اسم المنطقة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المنطقة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="newRegion.description"
                  label="الوصف"
                  variant="outlined"
                  rows="3"
                  auto-grow
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
            @click="addRegion"
          >
            حفظ
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Region Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
      persistent
    >
      <VCard>
        <VCardTitle class="text-h6">تعديل المنطقة</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateRegion">
            <VRow>
              <VCol cols="12">
                <VTextField
                  v-model="editRegion.name"
                  label="اسم المنطقة"
                  variant="outlined"
                  required
                  :rules="[v => !!v || 'اسم المنطقة مطلوب']"
                />
              </VCol>
              <VCol cols="12">
                <VTextarea
                  v-model="editRegion.description"
                  label="الوصف"
                  variant="outlined"
                  rows="3"
                  auto-grow
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
            @click="updateRegion"
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
          هل أنت متأكد من حذف المنطقة "<strong>{{ selectedRegion?.value }}</strong>"؟
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
            @click="deleteRegion"
          >
            حذف
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

<style scoped>
.fixed-columns-table {
  /* Enhanced styling for fixed columns */
  overflow-x: auto;
}

.fixed-columns-table :deep(.v-data-table__th--sticky) {
  background-color: rgb(var(--v-theme-surface));
  z-index: 2;
  box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
}

.fixed-columns-table :deep(.v-data-table__td--sticky) {
  background-color: rgb(var(--v-theme-surface));
  z-index: 1;
  box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
}

/* RTL support for Arabic text */
.v-application--is-rtl .fixed-columns-table :deep(.v-data-table__th--sticky),
.v-application--is-rtl .fixed-columns-table :deep(.v-data-table__td--sticky) {
  box-shadow: -2px 0 4px rgba(0, 0, 0, 0.1);
}
</style>

