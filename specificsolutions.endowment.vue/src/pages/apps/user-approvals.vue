<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'User',
    requiresAuth: true,
  },
})

import { ref, onMounted, watch, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'
import { useAppAlerts } from '@/composables/useAppAlerts'

const { t, locale } = useI18n()
const { success: showSuccess, error: showError, warning: showWarning } = useAppAlerts()

// Data
const pendingUsers = ref<any[]>([])
const loading = ref(false)
const totalItems = ref(0)
const options = ref({ page: 1, itemsPerPage: 10 })
const selectedUsers = ref<string[]>([])

// API functions
const api = useApi()

// Fetch pending users
const fetchPendingUsers = async () => {
  loading.value = true
  try {
    const response = await api('/user-approvals/pending', {
      method: 'GET',
      params: {
        page: options.value.page,
        itemsPerPage: options.value.itemsPerPage
      }
    })
    
    if (response.isSuccess) {
      pendingUsers.value = response.data || []
      totalItems.value = response.data?.length || 0
      console.log('✅ نمط خالد: البيانات المستلمة:', response.data)
      console.log('✅ نمط خالد: عدد المستخدمين:', pendingUsers.value.length)
      console.log('✅ نمط خالد: totalItems:', totalItems.value)
    } else {
      showError(response.message || 'حدث خطأ أثناء جلب المستخدمين المعلقين')
    }
  } catch (error) {
    console.error('خطأ في جلب المستخدمين المعلقين:', error)
    showError('حدث خطأ أثناء جلب المستخدمين المعلقين')
  } finally {
    loading.value = false
  }
}

// Approve user
const approveUser = async (userId: string, roleName: string = 'Employee') => {
  try {
    const response = await api(`/user-approvals/approve/${userId}`, {
      method: 'POST',
      body: { roleName }
    })
    
    if (response.isSuccess) {
      showSuccess('تم قبول المستخدم بنجاح')
      await fetchPendingUsers()
    } else {
      showError(response.message || 'حدث خطأ أثناء الموافقة على المستخدم')
    }
  } catch (error) {
    console.error('خطأ في الموافقة على المستخدم:', error)
    showError('حدث خطأ أثناء الموافقة على المستخدم')
  }
}

// Reject user
const rejectUser = async (userId: string) => {
  try {
    const response = await api(`/user-approvals/reject/${userId}`, {
      method: 'DELETE'
    })
    
    if (response.isSuccess) {
      showWarning('تم رفض وحذف المستخدم')
      await fetchPendingUsers()
    } else {
      showError(response.message || 'حدث خطأ أثناء رفض المستخدم')
    }
  } catch (error) {
    console.error('خطأ في رفض المستخدم:', error)
    showError('حدث خطأ أثناء رفض المستخدم')
  }
}

// Approve multiple users
const approveSelectedUsers = async () => {
  if (selectedUsers.value.length === 0) {
    showWarning('يرجى اختيار مستخدمين للموافقة عليهم')
    return
  }

  try {
    for (const userId of selectedUsers.value) {
      await approveUser(userId)
    }
    selectedUsers.value = []
    showSuccess(`تم قبول ${selectedUsers.value.length} مستخدم بنجاح`)
  } catch (error) {
    showError('حدث خطأ أثناء الموافقة على المستخدمين المحددين')
  }
}

// Reject multiple users
const rejectSelectedUsers = async () => {
  if (selectedUsers.value.length === 0) {
    showWarning('يرجى اختيار مستخدمين لرفضهم')
    return
  }

  try {
    for (const userId of selectedUsers.value) {
      await rejectUser(userId)
    }
    selectedUsers.value = []
    showWarning(`تم رفض وحذف ${selectedUsers.value.length} مستخدم`)
  } catch (error) {
    showError('حدث خطأ أثناء رفض المستخدمين المحددين')
  }
}

// Table headers
const headers = computed(() => [
  { title: t('tableHeaders.userApprovals.user'), key: 'user', sortable: false },
  { title: t('tableHeaders.userApprovals.email'), key: 'email' },
  { title: t('tableHeaders.userApprovals.phone'), key: 'phoneNumber' },
  { title: t('tableHeaders.userApprovals.registrationDate'), key: 'createdDate' },
  { title: t('tableHeaders.userApprovals.actions'), key: 'actions', sortable: false },
])

// Widget data for statistics
const widgetData = computed(() => [
  {
    title: t('userApprovals.widgets.totalPending'),
    value: totalItems.value.toString(),
    change: 0,
    desc: t('userApprovals.widgets.awaitingApproval'),
    icon: 'tabler-user-search',
    iconColor: 'warning',
  },
  {
    title: t('userApprovals.widgets.selectedUsers'),
    value: selectedUsers.value.length.toString(),
    change: 0,
    desc: t('userApprovals.widgets.selectedForAction'),
    icon: 'tabler-user-check',
    iconColor: 'info',
  },
])

// Role options for approval
const roleOptions = [
  { title: t('roles.admin'), value: 'Admin' },
  { title: t('roles.employee'), value: 'Employee' },
  { title: t('roles.customer'), value: 'Customer' },
]

// Format date
const formatDate = (date: string) => {
  if (!date) return '-'
  return new Date(date).toLocaleDateString('ar-SA')
}

// Get user full name
const getUserFullName = (user: any) => {
  return user.fullName || `${user.firstName || ''} ${user.lastName || ''}`.trim() || user.userName
}

// Watch for options changes
watch(options, () => {
  fetchPendingUsers()
}, { deep: true })

// Load data on mount
onMounted(() => {
  fetchPendingUsers()
})
</script>

<template>
  <section>
    <!-- Statistics Widgets -->
    <div class="d-flex mb-6">
      <VRow>
        <template
          v-for="(data, id) in widgetData"
          :key="id"
        >
          <VCol
            cols="12"
            md="6"
            sm="6"
          >
            <VCard>
              <VCardText>
                <div class="d-flex justify-space-between">
                  <div class="d-flex flex-column gap-y-1">
                    <div class="text-body-1 text-high-emphasis">
                      {{ data.title }}
                    </div>
                    <div class="d-flex gap-x-2 align-center">
                      <h4 class="text-h4">
                        {{ data.value }}
                      </h4>
                    </div>
                    <div class="text-sm">
                      {{ data.desc }}
                    </div>
                  </div>
                  <VAvatar
                    :color="data.iconColor"
                    variant="tonal"
                    rounded
                    size="42"
                  >
                    <VIcon
                      :icon="data.icon"
                      size="26"
                    />
                  </VAvatar>
                </div>
              </VCardText>
            </VCard>
          </VCol>
        </template>
      </VRow>
    </div>

    <!-- Main Card -->
    <VCard>
      <VCardTitle class="d-flex align-center justify-space-between pa-6">
        <div class="d-flex align-center gap-2">
          <VIcon icon="tabler-user-search" size="24" />
          {{ t('userApprovals.title') }}
        </div>
        
        <!-- Bulk Actions -->
        <div class="d-flex gap-2" v-if="selectedUsers.length > 0">
          <VBtn
            color="success"
            variant="tonal"
            prepend-icon="tabler-check"
            @click="approveSelectedUsers"
          >
            {{ t('userApprovals.approveSelected') }} ({{ selectedUsers.length }})
          </VBtn>
          <VBtn
            color="error"
            variant="tonal"
            prepend-icon="tabler-x"
            @click="rejectSelectedUsers"
          >
            {{ t('userApprovals.rejectSelected') }} ({{ selectedUsers.length }})
          </VBtn>
        </div>
      </VCardTitle>

      <VDivider />

      <VCardText>
        <!-- Data Table -->
        <VDataTable
          v-model="selectedUsers"
          :headers="headers"
          :items="pendingUsers"
          :loading="loading"
          :items-per-page="options.itemsPerPage"
          :page="options.page"
          item-value="id"
          show-select
          class="text-no-wrap"
        >
          <!-- User Column -->
          <template #item.user="{ item }">
            <div class="d-flex align-center gap-x-3">
              <VAvatar
                size="34"
                variant="tonal"
                color="primary"
              >
                <span>{{ getUserFullName(item).charAt(0).toUpperCase() }}</span>
              </VAvatar>
              <div class="d-flex flex-column">
                <h6 class="text-base font-weight-medium">
                  {{ getUserFullName(item) }}
                </h6>
                <div class="text-sm text-medium-emphasis">
                  {{ item.userName }}
                </div>
              </div>
            </div>
          </template>

          <!-- Email Column -->
          <template #item.email="{ item }">
            <div class="text-body-2">
              {{ item.email }}
            </div>
          </template>

          <!-- Phone Column -->
          <template #item.phoneNumber="{ item }">
            <div class="text-body-2">
              {{ item.phoneNumber || '-' }}
            </div>
          </template>

          <!-- Registration Date Column -->
          <template #item.createdDate="{ item }">
            <div class="text-body-2">
              {{ formatDate(item.createdDate) }}
            </div>
          </template>

          <!-- Actions Column -->
          <template #item.actions="{ item }">
            <div class="d-flex gap-2">
              <!-- Approve Button -->
              <VBtn
                icon
                variant="text"
                size="small"
                color="success"
                @click="approveUser(item.id)"
              >
                <VIcon icon="tabler-check" />
                <VTooltip activator="parent">
                  {{ t('userApprovals.approve') }}
                </VTooltip>
              </VBtn>

              <!-- Reject Button -->
              <VBtn
                icon
                variant="text"
                size="small"
                color="error"
                @click="rejectUser(item.id)"
              >
                <VIcon icon="tabler-x" />
                <VTooltip activator="parent">
                  {{ t('userApprovals.reject') }}
                </VTooltip>
              </VBtn>

              <!-- More Actions Menu -->
              <VBtn
                icon
                variant="text"
                size="small"
                color="medium-emphasis"
              >
                <VIcon icon="tabler-dots-vertical" />
                <VMenu activator="parent">
                  <VList>
                    <VListItem @click="approveUser(item.id, 'Admin')">
                      <template #prepend>
                        <VIcon icon="tabler-crown" />
                      </template>
                      <VListItemTitle>{{ t('userApprovals.approveAsAdmin') }}</VListItemTitle>
                    </VListItem>

                    <VListItem @click="approveUser(item.id, 'Employee')">
                      <template #prepend>
                        <VIcon icon="tabler-user" />
                      </template>
                      <VListItemTitle>{{ t('userApprovals.approveAsEmployee') }}</VListItemTitle>
                    </VListItem>

                    <VListItem @click="approveUser(item.id, 'Customer')">
                      <template #prepend>
                        <VIcon icon="tabler-user-check" />
                      </template>
                      <VListItemTitle>{{ t('userApprovals.approveAsCustomer') }}</VListItemTitle>
                    </VListItem>

                    <VDivider />

                    <VListItem @click="rejectUser(item.id)">
                      <template #prepend>
                        <VIcon icon="tabler-trash" />
                      </template>
                      <VListItemTitle>{{ t('userApprovals.rejectAndDelete') }}</VListItemTitle>
                    </VListItem>
                  </VList>
                </VMenu>
              </VBtn>
            </div>
          </template>

          <!-- Empty State -->
          <template #no-data>
            <div class="text-center py-8">
              <VIcon
                icon="tabler-user-check"
                size="64"
                color="success"
                class="mb-4"
              />
              <h6 class="text-h6 mb-2">
                {{ t('userApprovals.noDataTitle') }}
              </h6>
              <p class="text-body-2 text-medium-emphasis">
                {{ t('userApprovals.noDataDescription') }}
              </p>
            </div>
          </template>

          <!-- Pagination -->
          <template #bottom>
            <VCardText class="pt-2">
              <div class="d-flex flex-wrap justify-center justify-sm-space-between gap-y-2 mt-2">
                <VSelect
                  v-model="options.itemsPerPage"
                  :items="[5,10,25,50,100]"
                  :label="t('common.itemsPerPage')"
                  variant="underlined"
                  style="max-inline-size: 8rem;min-inline-size: 5rem;"
                />

                <VPagination
                  v-model="options.page"
                  :total-visible="$vuetify.display.smAndDown ? 3 : 5"
                  :length="Math.ceil(totalItems / options.itemsPerPage) || 1"
                />
              </div>
            </VCardText>
          </template>
        </VDataTable>
      </VCardText>
    </VCard>
  </section>
</template>

<style scoped>
.text-no-wrap {
  white-space: nowrap;
}
</style>
