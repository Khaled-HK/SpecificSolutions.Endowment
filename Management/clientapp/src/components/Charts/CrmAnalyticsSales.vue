<template>
    <apexchart height="380"
               :options="visitorBarChartConfig"
               :series="[{  name: 'Sales',data: CrmAnalyticsSalesSeries },{name: 'Value',data: CrmAnalyticsSalesSeries1 }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'CrmAnalyticsSales',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            CrmAnalyticsSalesSeries: {
                type: Array,
                required: true,
            },
            CrmAnalyticsSalesSeries1: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {





                    chart: {
                        type: 'radar',
                        toolbar: { show: false },
                    },
                    plotOptions: {
                        radar: {
                            polygons: {
                                strokeColors: '#e6e6e8', 
                                connectorColors: '#e6e6e8',
                            },
                        },
                    },
                    stroke: {
                        show: false,
                        width: 0,
                    },
                    legend: {
                        show: true,
                        fontSize: '13px',
                        position: 'bottom',
                        labels: {
                            colors: '#7367AA',
                            useSeriesColors: false,
                        },
                        markers: {
                            height: 12,
                            width: 12,
                            offsetX: -8,
                        },
                        itemMargin: { horizontal: 10 },
                        onItemHover: { highlightDataSeries: false },
                    },
                    colors: [
                        '#7367AA',
                        '#00BAD1',
                    ],
                    fill: {
                        opacity: [
                            1,
                            0.85,
                        ],
                    },
                    markers: { size: 0 },
                    grid: {
                        show: false,
                        padding: {
                            top: 0,
                            bottom: -5,
                        },
                    },
                    xaxis: {
                        categories: [
                            'Jan',
                            'Feb',
                            'Mar',
                            'Apr',
                            'May',
                            'Jun',
                        ],
                        labels: {
                            show: true,
                            style: {
                                colors: [
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                    '#7367AA',
                                ],
                                fontSize: '13px',
                                fontFamily: 'Public Sans',
                            },
                        },
                    },
                    yaxis: {
                        show: false,
                        min: 0,
                        max: 40,
                        tickAmount: 4,
                    },
                    responsive: [{
                        breakpoint: 769,
                        options: { chart: { height: 372 } },
                    }],





                },
            };
        },
    };
</script>
