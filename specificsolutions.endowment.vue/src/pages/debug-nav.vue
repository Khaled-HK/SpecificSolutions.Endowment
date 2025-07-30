<script setup lang="ts">
import NavItemsDebug from '@/layouts/components/NavItemsDebug.vue'

definePage({
  meta: {
    layout: 'default',
  },
})
</script>

<template>
  <VCard>
    <VCardTitle>Debug Navigation</VCardTitle>
    <VCardText>
      <NavItemsDebug />
    </VCardText>
  </VCard>
</template> 