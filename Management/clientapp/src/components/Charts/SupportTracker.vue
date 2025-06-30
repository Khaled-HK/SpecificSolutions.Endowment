<template>
  <apexchart
    type="radialBar"
    style="width:110%"
    :options="chartOptions"
    :series="[series]"
  ></apexchart>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed

    export default {
        name: 'RadialBarChart',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            series: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                chartOptions: {
                    labels: ['Completed'],
                    chart: {
                        height: 350,
                        type: 'radialBar'
                    },
                    plotOptions: {
                        radialBar: {
                            offsetY: 10,
                            startAngle: -140,
                            endAngle: 130,
                            hollow: { size: '65%' },
                            track: {
                                background: '#f5f5f5', // Replace with your theme color
                                strokeWidth: '100%',
                            },
                            dataLabels: {
                                name: {
                                    offsetY: -20,
                                    color: '#333', // Replace with your theme color
                                    fontSize: '13px',
                                    fontWeight: '400',
                                    fontFamily: 'Public Sans',
                                },
                                value: {
                                    offsetY: 10,
                                    color: '#333', // Replace with your theme color
                                    fontSize: '38px',
                                    fontWeight: '500',
                                    fontFamily: 'Public Sans',
                                },
                            },
                        },
                    },
                    colors: ['#7367AA'], // Replace with your theme color
                    fill: {
                        type: 'gradient',
                        gradient: {
                            shade: 'dark',
                            shadeIntensity: 0.5,
                            gradientToColors: ['#7367AA'], // Replace with your theme color
                            inverseColors: true,
                            opacityFrom: 1,
                            opacityTo: 0.6,
                            stops: [30, 70, 100],
                        },
                    },
                    stroke: { dashArray: 10 },
                    grid: {
                        padding: {
                            top: -20,
                            bottom: 5,
                        },
                    },
                    states: {
                        hover: { filter: { type: 'none' } },
                        active: { filter: { type: 'none' } },
                    },
                    responsive: [{
                        breakpoint: 960,
                        options: { chart: { height: 280 } },
                    }]
                },
            };
        },
    };
</script>

