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
        await this.GetAllStudentsByClass(this.$parent.SelectedItem.id);

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
            ByDate: '',

            ScreenTitle: '  الحضور والانصراف ',
            ScreenTitleSingle: '  حضور  ',
            ScreenTitleSingleAl: ' الحضور',

            Info: [],
            InfoEnrollments: [],
            Statistics: [],
            

            AddDialog: false,

            SelectedItem:'',

            ruleForm: {
                Id: '',
                ClasseId: this.$parent.SelectedItem.id,
                StudentId: '',
                EnrollmentDate: '',
                EnrollmentState: '',
                Descriptions: '',
            },
            rules: {
                StudentId: this.$helper.Required(),
                EnrollmentDate: this.$helper.Required(),
                EnrollmentState: this.$helper.Required(),
            },
        };
    },



    methods: {

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (this.ByDate)
                this.ByDate = this.ChangeDate(this.ByDate);

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetClassesEnrollments(this.pageNo, this.pageSize, this.Search, this.$parent.SelectedItem.id,this.ByDate)
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
                    this.ruleForm.ClasseId = this.$parent.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddClassesEnrollments(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.Back();
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
            this.ruleForm.StudentId = item.studentId;
            this.ruleForm.EnrollmentDate = item.enrollmentDate;
            this.ruleForm.EnrollmentState = item.enrollmentState;
            this.ruleForm.Descriptions = item.descriptions;
            this.state = 2;
            
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.ruleForm.ClasseId = this.$parent.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.EditClassesEnrollments(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.Back();
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
                    this.$http.ChangeStatusClassesEnrollments(Id)
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
                    this.$http.DeleteClassesEnrollments(Id)
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




        //Enroll Day
        GetAllStudentsByClassForEnrollments() {
            this.InfoEnrollments = [];

            if (this.ruleForm.EnrollmentDate)
                this.ruleForm.EnrollmentDate = this.ChangeDate(this.ruleForm.EnrollmentDate);

           
            this.$blockUI.Start();
            this.$http.GetAllStudentsByClassForEnrollments(this.$parent.SelectedItem.id, this.ruleForm.EnrollmentDate)
                .then(response => {
                    this.$blockUI.Stop();
                    this.InfoEnrollments = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        submitListForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    
                    this.$blockUI.Start();
                    this.$http.AddClassesEnrollmentsList(this.InfoEnrollments)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.Back();
                            this.InfoEnrollments = [];
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









        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },

    }
}
