<script setup lang="ts">
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
    requiresAuth: true, // Require authentication
  },
})

import AnalyticsAward from '@/views/dashboard/AnalyticsAward.vue'
import AnalyticsBarCharts from '@/views/dashboard/AnalyticsBarCharts.vue'
import AnalyticsDepositWithdraw from '@/views/dashboard/AnalyticsDepositWithdraw.vue'
import AnalyticsSalesByCountries from '@/views/dashboard/AnalyticsSalesByCountries.vue'
import AnalyticsTotalEarning from '@/views/dashboard/AnalyticsTotalEarning.vue'
import AnalyticsTotalProfitLineCharts from '@/views/dashboard/AnalyticsTotalProfitLineCharts.vue'
import AnalyticsTransactions from '@/views/dashboard/AnalyticsTransactions.vue'
import AnalyticsUserTable from '@/views/dashboard/AnalyticsUserTable.vue'
import AnalyticsWeeklyOverview from '@/views/dashboard/AnalyticsWeeklyOverview.vue'
import CardStatisticsVertical from '@core/components/cards/CardStatisticsVertical.vue'
import { ref, onMounted } from 'vue'
import { useApi } from '@/composables/useApi'

const api = useApi()

const loading = ref(true)
const error = ref('')
const summary = ref<any | null>(null)

onMounted(async () => {
  try {
    const resp = await api('/dashboard/summary')
    summary.value = (resp && typeof resp === 'object' && 'data' in resp) ? (resp as any).data : resp
  } catch (e: any) {
    error.value = e?.message || 'Failed to load dashboard'
  } finally {
    loading.value = false
  }
})

const totalProfit = {
  title: 'Total Profit',
  color: 'secondary',
  icon: 'ri-pie-chart-2-line',
  stats: '$25.6k',
  change: 42,
  subtitle: 'Weekly Project',
}

const newProject = {
  title: 'New Project',
  color: 'primary',
  icon: 'ri-file-word-2-line',
  stats: '862',
  change: -18,
  subtitle: 'Yearly Project',
}
</script>

<template>
  <VRow class="match-height">
    <VCol
      cols="12"
      md="4"
    >
      <AnalyticsAward />
    </VCol>

    <VCol
      cols="12"
      md="8"
    >
      <AnalyticsTransactions :statistics="[
        { title: 'Mosques', stats: String(summary?.mosqueCount ?? 0), icon: 'ri-building-4-line', color: 'primary' },
        { title: 'Buildings', stats: String(summary?.buildingCount ?? 0), icon: 'ri-community-line', color: 'success' },
        { title: 'Offices', stats: String(summary?.officesCount ?? 0), icon: 'ri-building-line', color: 'warning' },
        { title: 'Regions', stats: String(summary?.regionsCount ?? 0), icon: 'ri-map-pin-2-line', color: 'info' },
      ]" />
    </VCol>

    <VCol
      cols="12"
      md="4"
    >
      <AnalyticsWeeklyOverview :series="[{ name: 'Requests', data: (summary?.weeklyOverview||[]).map((p:any)=>p.value) }]" :labels="(summary?.weeklyOverview||[]).map((p:any)=>p.label)" />
    </VCol>

    <VCol
      cols="12"
      md="4"
    >
      <AnalyticsTotalEarning :value="summary?.newRequestsLast30Days" :profit="summary?.upcomingMaintenanceNext30Days" :regions="summary?.topRegionsByMosques" />
    </VCol>

    <VCol
      cols="12"
      md="4"
    >
      <VRow class="match-height">
        <VCol
          cols="12"
          sm="6"
        >
          <AnalyticsTotalProfitLineCharts />
        </VCol>

        <VCol
          cols="12"
          sm="6"
        >
          <CardStatisticsVertical v-bind="{...totalProfit, stats: summary ? `$${(summary.totalProfit??0).toLocaleString()}` : totalProfit.stats }" />
        </VCol>

        <VCol
          cols="12"
          sm="6"
        >
          <CardStatisticsVertical v-bind="{...newProject, stats: summary ? String(summary.productCount??0) : newProject.stats }" />
        </VCol>

        <VCol
          cols="12"
          sm="6"
        >
          <AnalyticsBarCharts />
        </VCol>
      </VRow>
    </VCol>

    <VCol
      cols="12"
      md="4"
    >
      <AnalyticsSalesByCountries />
    </VCol>

    <VCol
      cols="12"
      md="8"
    >
      <AnalyticsDepositWithdraw />
    </VCol>

    <VCol cols="12">
      <AnalyticsUserTable />
    </VCol>
  </VRow>
</template>
