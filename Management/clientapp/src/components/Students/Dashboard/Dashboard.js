import moment from 'moment';
//import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
import PaginationHelper from '../../../Shared/PaginationHelper.vue';
import QuillEditor from '../../../Shared/QuillEditor.vue';
import ReviewsChart from '../../Charts/ReviewsChart.vue';
import BarChart from '../../Charts/BarChart.vue';
import SupportTracker from '../../Charts/SupportTracker.vue';
import EarningReports from '../../Charts/EarningReports.vue';

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
            InrolledSeriesInfo: [0,0,0,0,0,0,0],

        };
    },



    methods: {



        GetInfo() {
            this.$blockUI.Start();
            this.$http.GetAllStudentsChartInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.TotalInfo = this.Info.studentCount;
                    this.ThisWeekInfo = this.Info.studentCountThisWeek;
                    this.InrolledSeriesInfo = this.Info.dailyEnrolledCounts;

                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },


        

       

    }
}
