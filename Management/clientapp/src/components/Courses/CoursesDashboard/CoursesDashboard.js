import moment from 'moment';
//import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
import PaginationHelper from '../../../Shared/PaginationHelper.vue';
import QuillEditor from '../../../Shared/QuillEditor.vue';
import ReviewsChart from '../../Charts/ReviewsChart.vue';
import BarChart from '../../Charts/BarChart.vue';

export default {
    name: 'Courses',
    mixins: [HelperMixin],
    components: {
        PaginationHelper,
        QuillEditor,
        ReviewsChart,
        BarChart,
    },

    async created() {
        await this.CheckLoginStatus();
        await this.GetAllInstructors();
        await this.GetAllSubjects();
        await this.GetAllAcademicLevels();

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

            ScreenTitle: '  الدورات التدريبية   ',
            ScreenTitleSingle: '  دورة تدريبية   ',
            ScreenTitleSingleAl: ' الدورة التدريبية  ',

            Info: [],
            Statistics: [],

        };
    },



    methods: {

        //Get Dictionaries
        async GetAcademicSpecializations() {
            this.GetAllAcademicSpecializations(this.ruleForm.AcademicLevelId)
        },



        GetInfo() {
            this.$blockUI.Start();
            this.$http.GetAllCoursesChartInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                })
                .catch(() => {
                this.$blockUI.Stop();
                });
        },

        //Support Info

        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },

        ChangeFilter(id) {
            this.FilterBy = id;
            this.IsFree = '';
            this.IsDiscount = '';
            this.SalesStatus = '';
            this.ViewStatus = '';
        },
       
        Refresh() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.FilterBy = 1;
            this.Search = '';
            this.IsFree = '';
            this.IsDiscount = '';
            this.SalesStatus = '';
            this.ViewStatus = '';
            this.GetInfo(this.pageNo);
        }

    }
}
