import Swal from "sweetalert2";
import HelperMixin from '../../Shared/HelperMixin.vue';
import PaginationHelper from '../../Shared/PaginationHelper.vue';
import QuillEditor from '../../Shared/QuillEditor.vue';
import Courses from './Courses/Courses.vue';
import CoursesSchedules from './CoursesSchedules/CoursesSchedules.vue';
import Students from './Students/Students.vue';
import Certificates from './Certificates/Certificates.vue';
import Enrollments from './Enrollments/Enrollments.vue';
import CoursesExams from './CoursesExams/CoursesExams.vue';


export default {
    name: 'Classes',
    mixins: [HelperMixin],
    components: {
        PaginationHelper,
        QuillEditor,
        Courses,
        CoursesSchedules,
        Students,
        Certificates,
        Enrollments,
        CoursesExams,
    },

    async created() {
        await this.CheckLoginStatus();

        await this.GetAllSchools();
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
            FilterBy: 1,
            Search: '',
            
            SchoolsId: '',
            ProfileYearsId: '',

            ScreenTitle: '  الفصول    ',
            ScreenTitleSingle: '  فصل  ',
            ScreenTitleSingleAl: ' الفصل  ',

            Info: [],
            Statistics: [],
            

            AddDialog: false,

            SelectedItem: '',

            ruleForm: {
                Id: '',
                SchoolsId: '',
                Name: '',
                Capacity: '',
                Descriptions: '',
                Price: '',
            },
            rules: {
                SchoolsId: this.$helper.Required(),
                Name: this.$helper.Required(),
                Capacity: this.$helper.Required(),
                Price: this.$helper.Required(),
            },



        };
    },



    methods: {

       



        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetClasses(this.pageNo, this.pageSize, this.Search, this.SchoolsId, this.ProfileYearsId)
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

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(0);
                    this.ruleForm.Price = Number(this.ruleForm.Price);
                    this.ruleForm.Capacity = Number(this.ruleForm.Capacity);

                    this.$blockUI.Start();
                    this.$http.AddClasses(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.ClearForm(this.ruleForm);
                            this.$helper.DestroyElemntById('Close');
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response && err.response.data) {
                                this.$helper.ShowMessage('error', 'خطأ بعملية الإظافة', err.response.data);
                            } else {
                                this.$helper.ShowMessage('error', 'خطأ بعملية الإظافة', 'حدت خطاء غير متوقع');
                            }
                        });


                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        
        OpentEditDialog(item) {
            this.SelectedItem = item;
            this.ruleForm.Id = item.id;
            this.ruleForm.SchoolsId = item.schoolsId;
            this.ruleForm.Capacity = item.capacity;
            this.ruleForm.Price = item.price;
            this.ruleForm.Name = item.name;
            this.ruleForm.Descriptions = item.descriptions;
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.ruleForm.Price = Number(this.ruleForm.Price);
                    this.ruleForm.Capacity = Number(this.ruleForm.Capacity);
                    this.$blockUI.Start();
                    this.$http.EditClasses(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.ClearForm(this.ruleForm);
                            this.$helper.DestroyElemntById('Close');
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response && err.response.data) {
                                this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', err.response.data);
                            } else {
                                this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', 'حدت خطاء غير متوقع');
                            }
                        });


                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        ChangeStatus(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية تغير حالة العرض  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeStatusClasses(Id)
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

        ChangeSalesStatus(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية تغير حالة البيع  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeSalesStatusClasses(Id)
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

        Delete(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية الحذف  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteClasses(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetInfo();
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response && err.response.data) {
                                this.$helper.ShowMessage('error', 'خطأ بعملية الحذف', err.response.data);
                            } else {
                                this.$helper.ShowMessage('error', 'خطأ بعملية الحذف', 'حدت خطاء غير متوقع');
                            }
                        });
                    return;
                }
            });
        },






        //Info
        OpentInfoDialog(item) {
            this.SelectedItem = item;
            this.state = 2;
        },

        ChangeTab(tabname) {
            //if (tabname == 'Courses') {
            //    this.GetCoursesInfo();
            //} else if (tabname == 'Wallet') {
            //    this.GetWalletInfo();
            //} else if (tabname == 'Devices') {
            //    this.GetDevicesInfo();
            //}
            return tabname;
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
