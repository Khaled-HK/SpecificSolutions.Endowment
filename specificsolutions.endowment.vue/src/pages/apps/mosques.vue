<script setup lang="ts">
import { ref, onMounted } from 'vue'

definePage({
  meta: {
    action: 'View',
    subject: 'Mosque',
  },
})

const mosques = ref([])
const loading = ref(false)
const dialog = ref(false)
const editDialog = ref(false)
const deleteDialog = ref(false)
const selectedMosque = ref(null)

const newMosque = ref({
  name: '',
  address: '',
  cityId: null,
  regionId: null,
  imamName: '',
  capacity: null,
  description: '',
})

const editMosque = ref({
  id: null,
  name: '',
  address: '',
  cityId: null,
  regionId: null,
  imamName: '',
  capacity: null,
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
  { title: 'Imam', key: 'imamName' },
  { title: 'Capacity', key: 'capacity' },
  { title: 'Actions', key: 'actions', sortable: false },
]

const loadMosques = async () => {
  loading.value = true
  try {
    const response = await $api('/Mosques')
    mosques.value = response
  } catch (error) {
    console.error('Error loading mosques:', error)
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

const addMosque = async () => {
  try {
    await $api('/Mosques', {
      method: 'POST',
      body: newMosque.value,
    })
    dialog.value = false
    resetNewMosque()
    loadMosques()
  } catch (error) {
    console.error('Error adding mosque:', error)
  }
}

const updateMosque = async () => {
  try {
    await $api(`/Mosques/${editMosque.value.id}`, {
      method: 'PUT',
      body: editMosque.value,
    })
    editDialog.value = false
    loadMosques()
  } catch (error) {
    console.error('Error updating mosque:', error)
  }
}

const deleteMosque = async () => {
  try {
    await $api(`/Mosques/${selectedMosque.value.id}`, {
      method: 'DELETE',
    })
    deleteDialog.value = false
    loadMosques()
  } catch (error) {
    console.error('Error deleting mosque:', error)
  }
}

const openEditDialog = (mosque) => {
  editMosque.value = { ...mosque }
  editDialog.value = true
}

const openDeleteDialog = (mosque) => {
  selectedMosque.value = mosque
  deleteDialog.value = true
}

const resetNewMosque = () => {
  newMosque.value = {
    name: '',
    address: '',
    cityId: null,
    regionId: null,
    imamName: '',
    capacity: null,
    description: '',
  }
}

onMounted(() => {
  loadMosques()
  loadCities()
  loadRegions()
})
</script>

<template>
  <div>
    <VCard>
      <VCardTitle class="d-flex justify-space-between align-center">
        <span>Mosques Management</span>
        <VBtn
          color="primary"
          @click="dialog = true"
        >
          Add Mosque
        </VBtn>
      </VCardTitle>

      <VCardText>
        <VDataTable
          :headers="headers"
          :items="mosques"
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

    <!-- Add Mosque Dialog -->
    <VDialog
      v-model="dialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Add New Mosque</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="addMosque">
            <VTextField
              v-model="newMosque.name"
              label="Mosque Name"
              required
            />
            <VTextField
              v-model="newMosque.address"
              label="Address"
              required
            />
            <VSelect
              v-model="newMosque.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              label="City"
              required
            />
            <VSelect
              v-model="newMosque.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextField
              v-model="newMosque.imamName"
              label="Imam Name"
            />
            <VTextField
              v-model="newMosque.capacity"
              label="Capacity"
              type="number"
            />
            <VTextarea
              v-model="newMosque.description"
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
            @click="addMosque"
          >
            Save
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>

    <!-- Edit Mosque Dialog -->
    <VDialog
      v-model="editDialog"
      max-width="600px"
    >
      <VCard>
        <VCardTitle>Edit Mosque</VCardTitle>
        <VCardText>
          <VForm @submit.prevent="updateMosque">
            <VTextField
              v-model="editMosque.name"
              label="Mosque Name"
              required
            />
            <VTextField
              v-model="editMosque.address"
              label="Address"
              required
            />
            <VSelect
              v-model="editMosque.cityId"
              :items="cities"
              item-title="name"
              item-value="id"
              label="City"
              required
            />
            <VSelect
              v-model="editMosque.regionId"
              :items="regions"
              item-title="name"
              item-value="id"
              label="Region"
              required
            />
            <VTextField
              v-model="editMosque.imamName"
              label="Imam Name"
            />
            <VTextField
              v-model="editMosque.capacity"
              label="Capacity"
              type="number"
            />
            <VTextarea
              v-model="editMosque.description"
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
            @click="updateMosque"
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
          Are you sure you want to delete this mosque?
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
            @click="deleteMosque"
          >
            Delete
          </VBtn>
        </VCardActions>
      </VCard>
    </VDialog>
  </div>
</template>

