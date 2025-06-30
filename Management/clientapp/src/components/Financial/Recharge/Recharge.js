import moment from 'moment';
//import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
import PaginationHelper from '../../../Shared/PaginationHelper.vue';
export default {
    name: 'Courses',
    mixins: [HelperMixin],
    components: {
        PaginationHelper
    },

    async created() {
        await this.CheckLoginStatus();

        await this.GetAllPaymentMethods();
        await this.GetAllUsers();
        await this.GetAllDistributors();

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
            Search: '',

            ScreenTitle: '  عمليات الدفع  ',
            ScreenTitleSingle: '  عملية   ',
            ScreenTitleSingleAl: ' العملية  ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',

            From: '',
            To:'',
            FilterBy:1,
            UserId:'',
            DistributorsId:'',
            PaymentMethodId:'',

            ruleForm: {
                Id: '',
                Name: '',
                Descriptions: '',
                Image: '',
                ImageName: '',
            },
            rules: {
                Name: this.$helper.Required(),
            },
        };
    },

    methods: {

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (this.From)
                this.From = this.ChangeDate(this.From);

            if (this.To)
                    this.To = this.ChangeDate(this.To);

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetRecharge(this.pageNo, this.pageSize, this.Search, this.From, this.To,
                this.UserId, this.DistributorsId, this.PaymentMethodId)
                .then(response => {
                    if (!this.Search)
                        this.$blockUI.Stop();

                    this.Info = response.data.info;
                    this.pages = response.data.count;
                    this.Statistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.Search)
                        this.$blockUI.Stop();
                    this.pages = 0;
                });
        },


        ChangeFilter(id) {
            this.FilterBy = id;
            this.UserId = '';
            this.DistributorsId = '';
            this.PaymentMethodId = '';
        },


        Refresh() {
            this.UserId = '';
            this.DistributorsId = '';
            this.PaymentMethodId = '';
            this.From = '';
            this.To = '';
            this.GetInfo();
        }

    }
}
