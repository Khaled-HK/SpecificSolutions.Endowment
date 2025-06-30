
import Swal from "sweetalert2";
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
        await this.GetAllAcademicLevels();
        await this.GetAllSubjects();
        await this.GetAllPaymentMethods();

        this.GetInfo();
    },

    computed: {
        totalPages() {
            return Math.ceil(this.pages / this.pageSize);
        }
    },

    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,
            Search: '',

            ScreenTitle: ' المدربين الموقوفين  ',
            ScreenTitleSingle: ' إيقاف ',
            ScreenTitleSingleAl: 'الإيقاف ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',

            ruleForm: {
                Id: '',
                AcademicLevelId: '',
                AcademicSpecializationId: '',
                FirstName: '',
                FatherName: '',
                SirName: '',
                Phone: '',
                ExtraPhone: '',
                LoginName: '',
                Email: '',
                Image: '',
                ImageName: '',
                Descriptions: '',
            },
            rules: {
                AcademicLevelId: this.$helper.Required(),
                AcademicSpecializationId: this.$helper.Required(),
                FirstName: this.$helper.Required(),
                FatherName: this.$helper.Required(),
                SirName: this.$helper.Required(),
                Phone: this.$helper.Phone(),
                LoginName: this.$helper.Required(),
                Email: this.$helper.EmailOnly(),
            },







            //Courses
            CoursesSearch: '',
            CoursesInfo: [],
            CoursesDropInfo: [],
            CoursesStatistics: [],
            CoursesSelectedItem: '',
            CoursesScreenTitle: ' الدورات التدريبية  ',
            CoursesScreenTitleSingle: ' دورة تدريبية ',
            CoursesScreenTitleSingleAl: 'الدورة التدريبية ',
            CoursesruleForm: {
                Id: '',
                SubjectId: '',
                AcademicLevelId: '',
                AcademicSpecializationId: '',
                CourseId: '',
                StudentId: '',
                Value: '',
            },
            Coursesrules: {
                AcademicLevelId: this.$helper.Required(),
                SubjectId: this.$helper.Required(),
                AcademicSpecializationId: this.$helper.Required(),
                CourseId: this.$helper.Required(),
                StudentId: this.$helper.Required(),
                Value: this.$helper.Required(),
            },

            CoursesValueruleForm: {
                Id: '',
                Value: '',
            },
            CoursesValuerules: {
                Value: this.$helper.Required(),
            },

            CoursesDropruleForm: {
                Id: '',
                DropResone: '',
            },
            CoursesDroprules: {
                DropResone: this.$helper.Required(),
            },








            //Wallet
            WalletInfo: [],
            WalletStatistics: [],
            WalletInfoPurchases: [],
            WalletInfoTracker: [],
            WalletSelectedItem: '',
            WalletScreenTitle: ' المحفظة الإلكترونية  ',
            WalletScreenTitleSingle: ' قيمة ',
            WalletScreenTitleSingleAl: 'القيمة ',
            WalletruleForm: {
                StudentId: '',
                PaymentMethodId: '',
                Value: '',
            },
            Walletrules: {
                PaymentMethodId: this.$helper.Required(),
                Value: this.$helper.Required(),
            },


            //Devices
            DevicesInfo: [],


        };
    },



    methods: {

        async GetAcademicSpecializations() {
            this.GetAllAcademicSpecializations(this.ruleForm.AcademicLevelId)
        },

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetSuspendsInstructors(this.pageNo, this.pageSize, this.Search)
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


        CanselSuspendsInfo(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية تفعيل الحساب  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.CanselInstructors(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetInfo();
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response && err.response.data) {
                                this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', err.response.data);
                            } else {
                                this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', 'حدت خطاء غير متوقع');
                            }
                        });
                    return;
                }
            });
        },






    }
}
