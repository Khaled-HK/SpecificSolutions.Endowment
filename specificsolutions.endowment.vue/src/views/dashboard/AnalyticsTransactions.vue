<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{ statistics?: { title: string; stats: string; icon: string; color: string }[] }>()
const fallback = [
  { title: 'Mosques', stats: '0', icon: 'ri-building-4-line', color: 'primary' },
  { title: 'Buildings', stats: '0', icon: 'ri-community-line', color: 'success' },
  { title: 'Offices', stats: '0', icon: 'ri-building-line', color: 'warning' },
  { title: 'Regions', stats: '0', icon: 'ri-map-pin-2-line', color: 'info' },
]

const statistics = computed(() => props.statistics ?? fallback)

const moreList = [
  { title: 'Share', value: 'Share' },
  { title: 'Refresh', value: 'Refresh' },
  { title: 'Update', value: 'Update' },
]
</script>

<template>
  <VCard title="Transactions">
    <template #subtitle>
      <p class="text-body-1 mb-0">
        <span class="d-inline-block font-weight-medium text-high-emphasis">Total 48.5% Growth</span> <span class="text-high-emphasis">😎</span> this month
      </p>
    </template>

    <template #append>
      <MoreBtn :menu-list="moreList" />
    </template>

    <VCardText class="pt-10">
      <VRow>
        <VCol
          v-for="item in statistics"
          :key="item.title"
          cols="12"
          sm="6"
          md="3"
        >
          <div class="d-flex align-center gap-x-3">
            <VAvatar
              :color="item.color"
              rounded
              size="40"
              class="elevation-2"
            >
              <VIcon
                size="24"
                :icon="item.icon"
              />
            </VAvatar>

            <div class="d-flex flex-column">
              <div class="text-body-1">
                {{ item.title }}
              </div>
              <h5 class="text-h5">
                {{ item.stats }}
              </h5>
            </div>
          </div>
        </VCol>
      </VRow>
    </VCardText>
  </VCard>
</template>
