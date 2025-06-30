<template>
    <apexchart :height="162"
               :options="visitorBarChartConfig"
               :series="[{ data: CrmRevenueGrowthSeries }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'CrmRevenueGrowth',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            CrmRevenueGrowthSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {



                    chart: {
                      height: 162,
                      type: 'bar',
                      parentHeightOffset: 0,
                      toolbar: { show: false },
                    },
                    plotOptions: {
                      bar: {
                        barHeight: '80%',
                        columnWidth: '30%',
                        startingShape: 'rounded',
                        endingShape: 'rounded',
                        borderRadius: 6,
                        distributed: true,
                      },
                    },
                    tooltip: { enabled: false },
                    grid: {
                      show: false,
                      padding: {
                        top: -20,
                        bottom: -12,
                        left: -10,
                        right: 0,
                      },
                    },
                    colors: [
                      '#28C76F',
                      '#28C76F',
                      '#28C76F',
                      '#28C76F',
                      '#28C76F',
                      '#28C76F',
                      '#28C76F',
                    ],
                    dataLabels: { enabled: false },
                    legend: { show: false },
                    xaxis: {
                      categories: [
                        'M',
                        'T',
                        'W',
                        'T',
                        'F',
                        'S',
                        'S',
                      ],
                      axisBorder: { show: false },
                      axisTicks: { show: false },
                      labels: {
                        style: {
                          colors: '#28C76F',
                          fontSize: '13px',
                          fontFamily: 'Public sans',
                        },
                      },
                    },
                    yaxis: { labels: { show: false } },
                    states: { hover: { filter: { type: 'none' } } },
                    responsive: [
                      {
                        breakpoint: 1640,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '40%',
                              borderRadius: 6,
                            },
                          },
                        },
                      },
                      {
                        breakpoint: 1471,
                        options: { plotOptions: { bar: { columnWidth: '50%' } } },
                      },
                      {
                        breakpoint: 1350,
                        options: { plotOptions: { bar: { columnWidth: '57%' } } },
                      },
                      {
                        breakpoint: 1032,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '50%',
                              borderRadius: 4,
                            },
                          },
                        },
                      },
                      {
                        breakpoint: 992,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '40%',
                              borderRadius: 4,
                            },
                          },
                        },
                      },
                      {
                        breakpoint: 855,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '50%',
                              borderRadius: 6,
                            },
                          },
                        },
                      },
                      {
                        breakpoint: 440,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '30%',
                              borderRadius: 4,
                            },
                          },
                        },
                      },
                      {
                        breakpoint: 381,
                        options: {
                          plotOptions: {
                            bar: {
                              columnWidth: '45%',
                              borderRadius: 4,
                            },
                          },
                        },
                      },
                    ],







                },
            };
        },
    };
</script>
