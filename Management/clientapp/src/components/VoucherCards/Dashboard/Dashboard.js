import moment from 'moment';
//import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
import PaginationHelper from '../../../Shared/PaginationHelper.vue';
import QuillEditor from '../../../Shared/QuillEditor.vue';
import ReviewsChart from '../../Charts/ReviewsChart.vue';
import BarChart from '../../Charts/BarChart.vue';
import SupportTracker from '../../Charts/SupportTracker.vue';
import EarningReports from '../../Charts/EarningReports.vue';
import AnalyticsAverageDailySales from '../../Charts/AnalyticsAverageDailySales.vue';
import CrmSalesAreaCharts from '../../Charts/CrmSalesAreaCharts.vue';
import CrmRevenueGrowth from '../../Charts/CrmRevenueGrowth.vue';
import CrmEarningReportsYearlyOverview from '../../Charts/CrmEarningReportsYearlyOverview.vue';
import CrmAnalyticsSales from '../../Charts/CrmAnalyticsSales.vue';

export default {
    name: 'Courses',
    mixins: [HelperMixin],
    components: {
        PaginationHelper,
        QuillEditor,
        ReviewsChart,
        BarChart,
        SupportTracker,
        EarningReports,
        AnalyticsAverageDailySales,
        CrmSalesAreaCharts,
        CrmRevenueGrowth,
        CrmEarningReportsYearlyOverview,
        CrmAnalyticsSales,
    },

    async created() {
        await this.CheckLoginStatus();
        this.GetInfo();
    },

    computed: {
        totalPages() {
            return Math.ceil(this.pages / this.pageSize);
        }
    },

    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('DD/MM/YYYY');
        },

        momentTime: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('DD/MM/YYYY || HH:MM');
        }
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,
            FilterBy: 1,
            Search: '',

            ScreenTitle: '  الطلبة  ',
            ScreenTitleSingle: '  طالب  ',
            ScreenTitleSingleAl: ' الطالب  ',

            Info: [],
            Statistics: [],

            DeviceRequest: 1,

            TotalInfo:0,
            ThisWeekInfo:0,
            InrolledSeriesInfo: [0, 0, 0, 0, 0, 0, 0],



            //Dashboard
            AnalyticsAverageDailySalesSeriesInfo: [0, 0, 0, 0, 0, 0, 0],

            //
            CrmSalesAreaChartsSeriesInfo: [0, 0, 0, 0],

            CrmRevenueGrowthSeriesInfo: [0, 0, 0, 0, 0, 0, 0],

            CrmEarningReportsYearlyOverviewSeriesInfo: [0, 0, 0, 0, 0, 0, 0, 0, 0],
            CrmEarningReportsYearlyOverviewSeriesInfo1: [0, 0, 0, 0, 0, 0, 0, 0, 0],

            CrmAnalyticsSalesSeriesInfo: [0,0,0, 0,0,0],
            CrmAnalyticsSalesSeriesInfo1: [0, 0,0, 0, 0, 0]

        };
    },



    methods: {



        GetInfo() {
            this.$blockUI.Start();
            this.$http.GetVoucherCardsChart()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;

                    this.AnalyticsAverageDailySalesSeriesInfo = this.Info.dailyChargeCounts;
                    this.CrmSalesAreaChartsSeriesInfo = this.Info.dailySalesCounts;
                    this.CrmRevenueGrowthSeriesInfo = this.Info.dailyChargeCounts;
                    this.CrmEarningReportsYearlyOverviewSeriesInfo = this.Info.monthlySalesCounts;
                    this.CrmAnalyticsSalesSeriesInfo = this.Info.monthlySalesCountsSix;
                    this.CrmAnalyticsSalesSeriesInfo1 = this.Info.monthlySalesCountsSix1;

////Dashboard
//AnalyticsAverageDailySalesSeriesInfo: [0, 0, 0, 0, 0, 0, 0],

//    //
//CrmSalesAreaChartsSeriesInfo: [0, 0, 0, 0],

//        CrmRevenueGrowthSeriesInfo: [0, 0, 0, 0, 0, 0, 0],

//            CrmEarningReportsYearlyOverviewSeriesInfo: [0, 0, 0, 0, 0, 0, 0, 0, 0],

//                CrmAnalyticsSalesSeriesInfo: [0, 0, 0, 0, 0, 0],
//                    CrmAnalyticsSalesSeriesInfo1: [0, 0, 0, 0, 0, 0]

                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },


        

       

    }
}
