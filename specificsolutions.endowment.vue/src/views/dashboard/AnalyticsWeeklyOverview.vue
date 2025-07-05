<script setup lang="ts">
import { useTheme } from 'vuetify'
import { hexToRgb } from '@layouts/utils'

const vuetifyTheme = useTheme()

const options = computed(() => {
  const currentTheme = ref(vuetifyTheme.current.value.colors)
  const variableTheme = ref(vuetifyTheme.current.value.variables)

  // Add fallback values for colors
  const surfaceColor = currentTheme.value.surface || '#FFFFFF'
  const trackBgColor = currentTheme.value['track-bg'] || '#f5f5f5'
  const onSurfaceColor = currentTheme.value['on-surface'] || '#000000'
  const disabledOpacity = variableTheme.value['disabled-opacity'] || 0.38
  const borderColorValue = String(variableTheme.value['border-color'] || '#e0e0e0')
  const borderOpacity = variableTheme.value['border-opacity'] || 0.12

  const disabledColor = `rgba(${hexToRgb(onSurfaceColor)},${disabledOpacity})`
  const borderColor = `rgba(${hexToRgb(borderColorValue)},${borderOpacity})`

  return {
    chart: {
      offsetY: -10,
      offsetX: -15,
      parentHeightOffset: 0,
      toolbar: { show: false },
    },
    plotOptions: {
      bar: {
        borderRadius: 6,
        distributed: true,
        columnWidth: '30%',
      },
    },
    stroke: {
      width: 2,
      colors: [surfaceColor],
    },
    legend: { show: false },
    grid: {
      borderColor,
      strokeDashArray: 7,
      xaxis: { lines: { show: false } },
    },
    dataLabels: { enabled: false },
    colors: [
      trackBgColor,
      trackBgColor,
      trackBgColor,
      'rgba(var(--v-theme-primary),1)',
      trackBgColor,
      trackBgColor,
    ],
    states: {
      hover: { filter: { type: 'none' } },
      active: { filter: { type: 'none' } },
    },
    xaxis: {
      categories: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      tickPlacement: 'on',
      labels: { show: false },
      crosshairs: { opacity: 0 },
      axisTicks: { show: false },
      axisBorder: { show: false },
    },
    yaxis: {
      show: true,
      tickAmount: 4,
      labels: {
        style: {
          colors: disabledColor,
          fontSize: '13px',
        },

        formatter: (value: number) => `${value > 999 ? `${(value / 1000).toFixed(0)}` : value}k`,
      },
    },
    responsive: [
      {
        breakpoint: 1560,
        options: {
          plotOptions: {
            bar: {
              columnWidth: '35%',
            },
          },
        },
      },
      {
        breakpoint: 1380,
        options: {
          plotOptions: {
            bar: {
              columnWidth: '45%',
            },
          },
        },
      },
    ],
  }
})

const series = [{ data: [37, 57, 45, 75, 57, 40, 65] }]

const moreList = [
  { title: 'Share', value: 'Share' },
  { title: 'Refresh', value: 'Refresh' },
  { title: 'Update', value: 'Update' },
]
</script>

<template>
  <VCard>
    <VCardItem>
      <VCardTitle>Weekly Overview</VCardTitle>

      <template #append>
        <div class="me-n3">
          <MoreBtn :menu-list="moreList" />
        </div>
      </template>
    </VCardItem>

    <VCardText>
      <VueApexCharts
        type="bar"
        :options="options"
        :series="series"
        :height="200"
      />

      <div class="d-flex align-center mb-5 gap-x-4">
        <h4 class="text-h4">
          45%
        </h4>
        <p class="mb-0">
          Your sales performance is 45% <span class="text-high-emphasis">😎</span> better compared to last month
        </p>
      </div>

      <VBtn block>
        Details
      </VBtn>
    </VCardText>
  </VCard>
</template>
