<template>
    <div id="reviewsChart">
        <apexchart type="bar"
                   :options="visitorBarChartConfig"
                   :series="[{ data: visitorBarChartSeries }]"></apexchart>
    </div>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts'; // Ensure this package is installed
    import ApexCharts from 'apexcharts';
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
            //visitorBarChartSeries: [
            // {
            // data: [20, 40, 20, 80, 100, 80, 60],
            // },
            //],
        },
        data() {
            return {
                visitorBarChartConfig: {
                    chart: {
                        height: 160,
                        width: 190,
                        type: 'bar',
                        toolbar: {
                            show: false,
                        },
                    },
                    plotOptions: {
                        bar: {
                            barHeight: '75%',
                            columnWidth: '40%',
                            startingShape: 'rounded',
                            endingShape: 'rounded',
                            borderRadius: 5,
                            distributed: true,
                        },
                    },
                    grid: {
                        show: false,
                        padding: {
                            top: -25,
                            bottom: -12,
                        },
                    },
                    colors: [],
                    dataLabels: {
                        enabled: false,
                    },
                    legend: {
                        show: false,
                    },
                    xaxis: {
                        categories: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
                        axisBorder: {
                            show: false,
                        },
                        axisTicks: {
                            show: false,
                        },
                        labels: {
                            style: {
                                colors: this.labelColor, // Ensure 'labelColor' is defined in your data
                                fontSize: '13px',
                            },
                        },
                    },
                    yaxis: {
                        labels: {
                            show: false,
                        },
                    },
                    responsive: [
                        {
                            breakpoint: 0,
                            options: {
                                chart: {
                                    width: '100%',
                                },
                                plotOptions: {
                                    bar: {
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 1440,
                            options: {
                                chart: {
                                    height: 150,
                                    width: 190,
                                    toolbar: {
                                        show: false,
                                    },
                                },
                                plotOptions: {
                                    bar: {
                                        borderRadius: 6,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 1400,
                            options: {
                                plotOptions: {
                                    bar: {
                                        borderRadius: 6,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 1200,
                            options: {
                                chart: {
                                    height: 130,
                                    width: 190,
                                    toolbar: {
                                        show: false,
                                    },
                                },
                                plotOptions: {
                                    bar: {
                                        borderRadius: 6,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 992,
                            options: {
                                chart: {
                                    height: 150,
                                    width: 190,
                                    toolbar: {
                                        show: false,
                                    },
                                },
                                plotOptions: {
                                    bar: {
                                        borderRadius: 5,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 883,
                            options: {
                                plotOptions: {
                                    bar: {
                                        borderRadius: 5,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 768,
                            options: {
                                chart: {
                                    height: 150,
                                    width: 190,
                                    toolbar: {
                                        show: false,
                                    },
                                },
                                plotOptions: {
                                    bar: {
                                        borderRadius: 4,
                                        columnWidth: '40%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 576,
                            options: {
                                chart: {
                                    width: '100%',
                                    height: 200,
                                    type: 'bar',
                                },
                                plotOptions: {
                                    bar: {
                                        borderRadius: 6,
                                        columnWidth: '30%',
                                    },
                                },
                            },
                        },
                        {
                            breakpoint: 420,
                            options: {
                                plotOptions: {
                                    chart: {
                                        width: '100%',
                                        height: 200,
                                        type: 'bar',
                                    },
                                    bar: {
                                        borderRadius: 3,
                                        columnWidth: '30%',
                                    },
                                },
                            },
                        },
                    ],
                },
            };
        },
        mounted() {
            this.setChartColors();
            const expensesRadialChartEl = this.$refs.expensesRadialChart;
            if (expensesRadialChartEl) {
                this.expensesRadialChart = new ApexCharts(expensesRadialChartEl, this.expensesRadialChartConfig);
                this.expensesRadialChart.render();
            }
        },
        beforeDestroy() {
            if (this.expensesRadialChart) {
                this.expensesRadialChart.destroy(); // Clean up the chart instance
            }
        },
        methods: {
            setChartColors() {
                this.visitorBarChartConfig.colors = [
                    '#7367AA', // Replace with config.colors.primary
                    '#00BAD1', // Replace with config.colors.info
                    '#28C76F', // Replace with config.colors.success
                    '#808390', // Replace with config.colors.secondary
                    '#FF4C51', // Replace with config.colors.danger
                    '#FF9F43', // Replace with config.colors.warning
                    '#7367AA', // Replace with config.colors.primary
                ];
            },
        },
    }
</script>