<script setup lang="ts">
type RegionCount = { name: string; count: number }

const props = defineProps<{
  value?: number | string
  profit?: number | string
  regions?: RegionCount[]
}>()

const moreList = [
  { title: 'Share', value: 'Share' },
  { title: 'Refresh', value: 'Refresh' },
  { title: 'Update', value: 'Update' },
]
</script>

<template>
  <VCard>
    <VCardItem>
      <VCardTitle>Requests & Maintenance</VCardTitle>

      <template #append>
        <div class="me-n3">
          <MoreBtn :menu-list="moreList" />
        </div>
      </template>
    </VCardItem>

    <VCardText>
      <div class="d-flex align-center">
        <h3 class="text-h3">
          {{ props.value ?? 0 }}
        </h3>

        <VIcon
          size="24"
          icon="ri-arrow-up-s-line"
          color="success"
        />
        <div class="text-success">
          {{ props.profit ?? 0 }}
        </div>
      </div>
      <div class="text-body-1 mb-12">
        New requests in last 30 days / Upcoming maintenance next 30 days
      </div>

      <VList class="card-list">
        <VListItem
          v-for="r in (props.regions ?? [])"
          :key="r.name"
        >
          <VListItemTitle class="font-weight-medium">
            {{ r.name }}
          </VListItemTitle>
          <template #append>
            <div>
              <h6 class="text-h6 mb-2">
                {{ r.count }}
              </h6>
            </div>
          </template>
        </VListItem>
      </VList>
    </VCardText>
  </VCard>
</template>

<style lang="scss" scoped>
.card-list {
  --v-card-list-gap: 1.5rem;
}
</style>
