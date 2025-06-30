<template>
    <apexchart 
                :height="62"
               :options="visitorBarChartConfig"
               :series="[{ name: '2020',data: AnalyticsAverageDailySalesSeries }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'AnalyticsAverageDailySales',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            AnalyticsAverageDailySalesSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {

                    chart: {
                        height: 90,
                        parentHeightOffset: 0,
                        type: 'bar',
                        toolbar: { show: false },
                    },
                    tooltip: { enabled: false },
                    plotOptions: {
                        bar: {
                            barHeight: '100%',
                            columnWidth: '30%',
                            startingShape: 'rounded',
                            endingShape: 'rounded',
                            borderRadius: 4,
                            colors: {
                                backgroundBarColors: [
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                ],
                                backgroundBarRadius: 4,
                            },
                        },
                    },
                    colors: ['#7367AA'],
                    grid: {
                        show: false,
                        padding: {
                            top: -30,
                            left: -16,
                            bottom: 0,
                            right: -6,
                        },
                    },
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
                        labels: { show: false },
                    },
                    yaxis: { labels: { show: false } },
                    responsive: [
                        {
                            breakpoint: 1441,
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
                            breakpoint: 1368,
                            options: { plotOptions: { bar: { columnWidth: '48%' } } },
                        },
                        {
                            breakpoint: 1264,
                            options: {
                                plotOptions: {
                                    bar: {
                                        borderRadius: 6,
                                        columnWidth: '30%',
                                        colors: { backgroundBarRadius: 6 },
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 960,
                            options: {
                                plotOptions: {
                                    bar: {
                                        columnWidth: '15%',
                                        borderRadius: 4,
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 883,
                            options: { plotOptions: { bar: { columnWidth: '20%' } } },
                        },
                        {
                            breakpoint: 768,
                            options: { plotOptions: { bar: { columnWidth: '25%' } } },
                        },
                        {
                            breakpoint: 600,
                            options: {
                                plotOptions: {
                                    bar: {
                                        columnWidth: '15%',
                                        borderRadius: 4,
                                    },
                                    colors: { backgroundBarRadius: 9 },
                                },
                            },
                        },
                        {
                            breakpoint: 479,
                            options: {
                                plotOptions: {
                                    bar: { borderRadius: 4 },
                                    colors: { backgroundBarRadius: 9 },
                                },
                                grid: {
                                    padding: {
                                        right: -15,
                                        left: -15,
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 400,
                            options: { plotOptions: { bar: { borderRadius: 4 } } },
                        },
                    ],
                },
            };
        },
    };
</script>
