<script setup>
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})
</script>

<template>
  <VCard>
    <VCardText class="py-16">
      <!-- 👉 App Pricing components -->
      <VRow>
        <VCol
          cols="12"
          sm="8"
          md="12"
          lg="10"
          class="mx-auto"
        >
          <AppPricing md="4" />
        </VCol>
      </VRow>
    </VCardText>
  </VCard>
</template>
