<template>
    <VCard>
        <VCardItem class="pb-sm-0">
            <br />
            <br />

            <template #append>
                <div class="mt-n4 me-n2">
                    <MoreBtn size="small"
                             :menu-list="moreList" />
                </div>
            </template>
        </VCardItem>

        <VCardText>
            <VRow v-if="Total && ThisWeek && InrolledSeries!=0">
                <VCol cols="12"
                      sm="5"
                      lg="6"
                      class="d-flex flex-column align-self-center">
                    <div class="d-flex align-center gap-2 mb-3 flex-wrap">
                        <h2 class="text-h2"> <i class="ti ti-users ti-sm"></i> {{Total}}</h2>
                        <VChip label size="small" style="color:green">+{{ThisWeek}}</VChip>
                    </div>

                    <br />
                </VCol>

                <VCol cols="12" sm="7" lg="6">
                    <apexchart :options="chartOptions"
                                  :series="[{ data: InrolledSeries }]"
                                   height="161" />
                </VCol>
            </VRow>

        </VCardText>
    </VCard>
</template>

<script>
    import VueApexCharts from 'vue-apexcharts';  // Ensure this package is installed

    export default {
        name: 'EarningsReport',
        components: {
            apexchart: VueApexCharts,
        },
        props: {
            Total: {
                type: Number,
                required: true,
            },
            ThisWeek: {
                type: Number,
                required: true,
            },
            InrolledSeries: {
                type: Array,
                required: true,
            },
        },
        data() {
            return {
                //series: [{
                //    data: [40, 65, 50, 45, 90, 55, 70],
                //}],
                chartOptions: {
                    chart: {
                        parentHeightOffset: 0,
                        type: 'bar',
                        toolbar: { show: false },
                    },
                    plotOptions: {
                        bar: {
                            barHeight: '60%',
                            columnWidth: '38%',
                            startingShape: 'rounded',
                            endingShape: 'rounded',
                            borderRadius: 4,
                            distributed: true,
                        },
                    },
                    grid: {
                        show: false,
                        padding: {
                            top: -30,
                            bottom: 0,
                            left: -10,
                            right: -10,
                        },
                    },
                    colors: [
                        'rgba(115, 103, 170, 0.7)', // Example color
                        'rgba(115, 103, 170, 0.7)',
                        'rgba(115, 103, 170, 0.7)',
                        'rgba(115, 103, 170, 0.7)',
                        'rgba(115, 103, 170, 1)',
                        'rgba(115, 103, 170, 0.7)',
                        'rgba(115, 103, 170, 0.7)',
                    ],
                    dataLabels: { enabled: false },
                    legend: { show: false },
                    xaxis: {
                        categories: ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'],
                        axisBorder: { show: false },
                        axisTicks: { show: false },
                        labels: {
                            style: {
                                colors: 'rgba(0, 0, 0, 0.54)',
                                fontSize: '13px',
                                fontFamily: 'Public Sans',
                            },
                        },
                    },
                    yaxis: { labels: { show: false } },
                    tooltip: { enabled: false },
                    responsive: [{
                        breakpoint: 1025,
                        options: { chart: { height: 199 } },
                    }],
                },
                earningsReports: [
                    {
                        color: 'primary',
                        icon: 'tabler-currency-dollar',
                        title: 'Earnings',
                        amount: '$545.69',
                        progress: '55',
                    },
                    {
                        color: 'info',
                        icon: 'tabler-chart-pie-2',
                        title: 'Profit',
                        amount: '$256.34',
                        progress: '25',
                    },
                    {
                        color: 'error',
                        icon: 'tabler-brand-paypal',
                        title: 'Expense',
                        amount: '$74.19',
                        progress: '65',
                    },
                ],
                moreList: [
                    { title: 'View More', value: 'View More' },
                    { title: 'Delete', value: 'Delete' },
                ],
            };
        },
    };
</script>

