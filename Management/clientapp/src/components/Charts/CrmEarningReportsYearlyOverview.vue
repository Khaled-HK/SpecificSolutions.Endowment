<template>
    <apexchart height="250"
               ref="refVueApexChart"
                class="mt-3"
               :options="visitorBarChartConfig"
               :series="[{ data: CrmEarningReportsYearlyOverviewSeries }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'CrmEarningReportsYearlyOverview',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            CrmEarningReportsYearlyOverviewSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {


                        chart: {
                            parentHeightOffset: 0,
                            type: 'bar',
                            toolbar: { show: false },
                        },
                        plotOptions: {
                            bar: {
                                columnWidth: '32%',
                                borderRadiusApplication: 'end',
                                borderRadius: 4,
                                distributed: true,
                                dataLabels: { position: 'top' },
                            },
                        },
                        grid: {
                            show: false,
                            padding: {
                                top: 0,
                                bottom: 0,
                                left: -10,
                                right: -10,
                            },
                        },
                        colors: [
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
                        dataLabels: {
                            enabled: true,
                            formatter(val) {
                                return `${val}k`
                            },
                            offsetY: -25,
                            style: {
                                fontSize: '15px',
                                colors: ['#7367AA'],
                                fontWeight: '600',
                                fontFamily: 'Public Sans',
                            },
                        },
                        legend: { show: false },
                        tooltip: { enabled: false },
                        xaxis: {
                            categories: [
                                'Jan',
                                'Feb',
                                'Mar',
                                'Apr',
                                'May',
                                'Jun',
                                'Jul',
                                'Aug',
                                'Sep',
                            ],
                            axisBorder: {
                                show: true,
                                color: '#7367AA',
                            },
                            axisTicks: { show: false },
                            labels: {
                                style: {
                                    colors: '#7367AA',
                                    fontSize: '13px',
                                    fontFamily: 'Public Sans',
                                },
                            },
                        },
                        yaxis: {
                            labels: {
                                offsetX: -15,
                                formatter(val) {
                                    return `${val / 1}k`
                                },
                                style: {
                                    fontSize: '13px',
                                    colors: '#7367AA',
                                    fontFamily: 'Public Sans',
                                },
                                min: 0,
                                max: 60000,
                                tickAmount: 6,
                            },
                        },
                        responsive: [
                            {
                                breakpoint: 1441,
                                options: { plotOptions: { bar: { columnWidth: '41%' } } },
                            },
                            {
                                breakpoint: 590,
                                options: {
                                    plotOptions: { bar: { columnWidth: '61%' } },
                                    yaxis: { labels: { show: false } },
                                    grid: {
                                        padding: {
                                            right: 0,
                                            left: -20,
                                        },
                                    },
                                    dataLabels: {
                                        style: {
                                            fontSize: '12px',
                                            fontWeight: '400',
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
