<template>
    <apexchart type="bar"
               style="width:110%"
               :options="visitorBarChartConfig"
               :series="[{ data: visitorBarChartSeries }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'VisitorBarChart',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            visitorBarChartSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {
                    chart: {
                        height: 350,
                        type: 'bar',
                      
                    },
                    plotOptions: {
                        bar: {
                            horizontal: true,
                            barHeight: '75%',
                            distributed: true,
                            borderRadius: 6,
                            borderRadiusApplication: 'end',
                        },
                    },
                    grid: {
                        //borderColor,
                        strokeDashArray: 10,
                        xaxis: { lines: { show: true } },
                        yaxis: { lines: { show: false } },
                        padding: {
                            top: -35,
                            bottom: -12,
                        },
                    },
                    colors: [
                        '#7367AA', // Replace with config.colors.primary
                        '#00BAD1', // Replace with config.colors.info
                        '#28C76F', // Replace with config.colors.success
                        '#808390', // Replace with config.colors.secondary
                        '#FF4C51', // Replace with config.colors.danger
                        '#FF9F43', // Replace with config.colors.warning
                    ],
                    dataLabels: {
                        enabled: true,
                        style: {
                            colors: ['#fff'],
                            fontWeight: 200,
                            fontSize: '13px',
                        },
                        offsetX: 0,
                        dropShadow: { enabled: false },
                        //formatter(val, opt) {
                        //    return this.labels[opt.dataPointIndex]
                        //},
                    },

                    labels: [
                        'UI Design',
                        'UX Design',
                        'Music',
                        'Animation',
                        'Vue',
                        'SEO',
                    ],
                    xaxis: {
                        categories: [
                            '1',
                            '2',
                            '3',
                            '4',
                            '5',
                            '6',
                        ],
                        axisBorder: { show: false },
                        axisTicks: { show: false },
                        labels: {
                            style: {
                                colors: 'rgba(var(--v-theme-on-background), var(--v-disabled-opacity))',
                                fontSize: '13px',
                            },
                            formatter(val) {
                                return `${val}%`
                            },
                        },
                    },
                    yaxis: {
                        max: 35,
                        labels: {
                            style: {
                                colors: 'rgba(var(--v-theme-on-background), var(--v-disabled-opacity))',
                                fontSize: '13px',
                            },
                        },
                    },

                    tooltip: {
                        enabled: true,
                        style: { fontSize: '12px' },
                        onDatasetHover: { highlightDataSeries: false },
                    },
                    legend: { show: false },
                },
            };
        },
    };
</script>
