<script setup lang="ts">
import { ref, onMounted } from 'vue'

definePage({
  meta: {
    action: 'read',
    subject: 'Auth',
  },
})

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

const headers = [
  { title: 'ID', key: 'id' },
  { title: 'Name', key: 'name' },
  { title: 'Address', key: 'address' },
  { title: 'City', key: 'cityName' },
  { title: 'Region', key: 'regionName' },
  { title: 'Actions', key: 'actions', sortable: false },
]

const loadBuildings = async () => {
  loading.value = true
  try {
    const response = await $api('/Buildings')
    buildings.value = response
  } catch (error) {
    console.error('Error loading buildings:', error)
  } finally {
    loading.value = false
  }
}

const loadCities = async () => {
  try {
    const response = await $api('/Cities')
    cities.value = response
  } catch (error) {
    console.error('Error loading cities:', error)
  }
}

const loadRegions = async () => {
  try {
    const response = await $api('/Regions')
    regions.value = response
  } catch (error) {
    console.error('Error loading regions:', error)
  }
}

const addBuilding = async () => {
  try {
    await $api('/Buildings', {
      method: 'POST',
      body: newBuilding.value,
    })
    dialog.value = false
    resetNewBuilding()
    loadBuildings()
  } catch (error) {
    console.error('Error adding building:', error)
  }
}

const updateBuilding = async () => {
  try {
    await $api(`/Buildings/${editBuilding.value.id}`, {
      method: 'PUT',
      body: editBuilding.value,
    })
    editDialog.value = false
    loadBuildings()
  } catch (error) {
    console.error('Error updating building:', error)
  }
}

const deleteBuilding = async () => {
  try {
    await $api(`/Buildings/${selectedBuilding.value.id}`, {
      method: 'DELETE',
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
        <span>Buildings Management</span>
        <VBtn
          color="primary"
          @click="dialog = true"
        >
          Add Building
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
        <VCardTitle>Add New Building</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addBuilding">
            <VTextField
              v-model="newBuilding.name"
              label="Building Name"
              required
            />
            <VTextField
              v-model="newBuilding.address"
              label="Address"
              required
            />
            <VSelect
              v-model="newBuilding.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              label="City"
              required
            />
            <VSelect
              v-model="newBuilding.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextarea
              v-model="newBuilding.description"
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
            @click="addBuilding"
          >
            Save
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
        <VCardTitle>Edit Building</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateBuilding">
            <VTextField
              v-model="editBuilding.name"
              label="Building Name"
              required
            />
            <VTextField
              v-model="editBuilding.address"
              label="Address"
              required
            />
            <VSelect
              v-model="editBuilding.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              label="City"
              required
            />
            <VSelect
              v-model="editBuilding.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextarea
              v-model="editBuilding.description"
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
            @click="updateBuilding"
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
          Are you sure you want to delete this building?
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
            @click="deleteBuilding"
          >
            Delete
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

