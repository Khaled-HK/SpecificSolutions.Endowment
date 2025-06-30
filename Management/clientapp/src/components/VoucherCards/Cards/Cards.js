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
            SerialNumber: '',
            VoucherNumber: '',

            ScreenTitle: '  الكروت   ',
            ScreenTitleSingle: '  كرت   ',
            ScreenTitleSingleAl: ' الكرت  ',

            Info: [],
            FillInfo: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',

        };
    },

    methods: {

        GetInfo() {

            this.$blockUI.Start();
            this.$http.GetCards()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Statistics = response.data.statistics;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },



        GetBySerialNumber() {
            this.Info = '';
            this.$blockUI.Start();
            this.$http.GetCardInfoBySerialNumber(this.SerialNumber)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.FillInfo = response.data.fillInfo;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        GetByVoucherNumber() {
            this.Info = '';
            this.$blockUI.Start();
            this.$http.GetCardInfoByVoucherNumber(this.VoucherNumber)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.FillInfo = response.data.fillInfo;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },


        Refresh() {
            this.Info = [];
            this.FillInfo = [];
            this.VoucherNumber = '';
            this.SerialNumber = '';
            this.GetInfo();
        }
    }
}
