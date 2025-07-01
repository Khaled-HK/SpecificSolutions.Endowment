<script setup lang="ts">
import { ref, onMounted } from 'vue'

definePage({
  meta: {
    action: 'read',
    subject: 'Auth',
  },
})

const cities = ref([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedCity = ref(null)

const newCity = ref({
  name: '',
  regionId: null,
  description: '',
})

const editCity = ref({
  id: null,
  name: '',
  regionId: null,
  description: '',
})

const regions = ref([])

const headers = [
  { title: 'ID', key: 'id' },
  { title: 'Name', key: 'name' },
  { title: 'Region', key: 'regionName' },
  { title: 'Description', key: 'description' },
  { title: 'Actions', key: 'actions', sortable: false },
]

const loadCities = async () => {
  loading.value = true
  try {
    const response = await $api('/Cities')
    cities.value = response
  } catch (error) {
    console.error('Error loading cities:', error)
  } finally {
    loading.value = false
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

const addCity = async () => {
  try {
    await $api('/Cities', {
      method: 'POST',
      body: newCity.value,
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
    await $api(`/Cities/${editCity.value.id}`, {
      method: 'PUT',
      body: editCity.value,
    })
    editDialog.value = false
    loadCities()
  } catch (error) {
    console.error('Error updating city:', error)
  }
}

const deleteCity = async () => {
  try {
    await $api(`/Cities/${selectedCity.value.id}`, {
      method: 'DELETE',
    })
    deleteDialog.value = false
    loadCities()
  } catch (error) {
    console.error('Error deleting city:', error)
  }
}

const openEditDialog = (city) => {
  editCity.value = { ...city }
  editDialog.value = true
}

const openDeleteDialog = (city) => {
  selectedCity.value = city
  deleteDialog.value = true
}

const resetNewCity = () => {
  newCity.value = {
    name: '',
    regionId: null,
    description: '',
  }
}

onMounted(() => {
  loadCities()
  loadRegions()
})
</script>

<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center">
        <span>Cities Management</span>
        <VBtn
          color="primary"
          @click="dialog = true"
        >
          Add City
        </VBtn>
      </VCardTitle>

      <VCardText>
        <VDataTable
          :headers="headers"
          :items="cities"
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

    <!-- Add City Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Add New City</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addCity">
            <VTextField
              v-model="newCity.name"
              label="City Name"
              required
            />
            <VSelect
              v-model="newCity.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextarea
              v-model="newCity.description"
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
            @click="addCity"
          >
            Save
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit City Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Edit City</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateCity">
            <VTextField
              v-model="editCity.name"
              label="City Name"
              required
            />
            <VSelect
              v-model="editCity.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextarea
              v-model="editCity.description"
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
            @click="updateCity"
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
          Are you sure you want to delete this city?
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
            @click="deleteCity"
          >
            Delete
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

