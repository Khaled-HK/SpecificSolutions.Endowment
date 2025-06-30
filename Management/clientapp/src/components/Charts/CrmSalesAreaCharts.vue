<template>
    <apexchart 
                :height="62"
               :options="visitorBarChartConfig"
               :series="[{ name: 'Subscribers',data: CrmSalesAreaChartsSeries }]"></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    export default {
        name: 'CrmSalesAreaCharts',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            CrmSalesAreaChartsSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                visitorBarChartConfig: {






                    chart: {
                        type: 'area',
                        parentHeightOffset: 0,
                        toolbar: { show: false },
                        sparkline: { enabled: true },
                    },
                    markers: {
                        colors: 'transparent',
                        strokeColors: 'transparent',
                    },
                    grid: { show: false },
                    colors: ['#28C76F'],
                    fill: {
                        type: 'gradient',
                        gradient: {
                            shadeIntensity: 0.9,
                            opacityFrom: 0.5,
                            opacityTo: 0.07,
                            stops: [
                                0,
                                80,
                                100,
                            ],
                        },
                    },
                    dataLabels: { enabled: false },
                    stroke: {
                        width: 2,
                        curve: 'smooth',
                    },
                    xaxis: {
                        show: true,
                        lines: { show: false },
                        labels: { show: false },
                        stroke: { width: 0 },
                        axisBorder: { show: false },
                    },
                    yaxis: {
                        stroke: { width: 0 },
                        show: false,
                    },
                    tooltip: { enabled: false },






                },
            };
        },
    };
</script>
