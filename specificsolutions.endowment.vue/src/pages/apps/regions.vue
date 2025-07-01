<script setup lang="ts">
import { ref, onMounted } from 'vue'

definePage({
  meta: {
    action: 'read',
    subject: 'Auth',
  },
})

const regions = ref([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedRegion = ref(null)

const newRegion = ref({
  name: '',
  description: '',
})

const editRegion = ref({
  id: null,
  name: '',
  description: '',
})

const headers = [
  { title: 'ID', key: 'id' },
  { title: 'Name', key: 'name' },
  { title: 'Description', key: 'description' },
  { title: 'Actions', key: 'actions', sortable: false },
]

const loadRegions = async () => {
  loading.value = true
  try {
    const response = await $api('/Regions')
    regions.value = response
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

const openEditDialog = (region) => {
  editRegion.value = { ...region }
  editDialog.value = true
}

const openDeleteDialog = (region) => {
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
      <VCardTitle class="d-flex justify-space-between align-center">
        <span>Regions Management</span>
        <VBtn
          color="primary"
          @click="dialog = true"
        >
          Add Region
        </VBtn>
      </VCardTitle>

      <VCardText>
        <VDataTable
          :headers="headers"
          :items="regions"
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

    <!-- Add Region Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Add New Region</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addRegion">
            <VTextField
              v-model="newRegion.name"
              label="Region Name"
              required
            />
            <VTextarea
              v-model="newRegion.description"
              label="Description"
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
            Cancel
          </VBtn>
          <VBtn
            color="blue darken-1"
            text
            @click="addRegion"
          >
            Save
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Region Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Edit Region</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateRegion">
            <VTextField
              v-model="editRegion.name"
              label="Region Name"
              required
            />
            <VTextarea
              v-model="editRegion.description"
              label="Description"
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
            Cancel
          </VBtn>
          <VBtn
            color="blue darken-1"
            text
            @click="updateRegion"
          >
            Update
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
        <VCardTitle>Confirm Delete</VCardTitle>
        <VCardText>
          Are you sure you want to delete this region?
        </VCardText>
        <VCardActions>
          <VSpacer />
          <VBtn
            color="blue darken-1"
            text
            @click="deleteDialog = false"
          >
            Cancel
          </VBtn>
          <VBtn
            color="red darken-1"
            text
            @click="deleteRegion"
          >
            Delete
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

