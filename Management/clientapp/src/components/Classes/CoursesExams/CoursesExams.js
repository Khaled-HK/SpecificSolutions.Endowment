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
        await this.GetAllCourses(this.$parent.SelectedItem.id);


        //this.$helper.ShowMessage('error', 'خطأ بعملية الإظافة', 'حدت خطاء غير متوقع');
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
            SearchExamsGrades: '',
            CoursesId: '',

            ScreenTitle: '  الإختبارات ',
            ScreenTitleSingle: '  إختبار ',
            ScreenTitleSingleAl: ' الإختبار  ',

            Info: [],
            Statistics: [],
            InfoExamsGrades: [],
            StatisticsExamsGrades: [],

            AddDialog: false,

            SelectedItem:'',

            ruleForm: {
                Id: '',
                ClasseId: this.$parent.SelectedItem.id,
                CoursesId: '',
                Type: '',
                ExamDate: '',
                Degree: '',
                Descriptions: '',
                Image: '',
                ImageName: '',
            },
            rules: {
                CoursesId: this.$helper.Required(),
                Type: this.$helper.Required(),
                ExamDate: this.$helper.Required(),
                Degree: this.$helper.Required(),
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
            this.$http.GetCoursesExams(this.pageNo, this.pageSize, this.Search, this.$parent.SelectedItem.id, this.CoursesId)
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
                    this.ruleForm.Degree = Number(this.ruleForm.Degree);
                    this.ruleForm.ClasseId = this.$parent.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddCoursesExams(this.ruleForm)
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

        
        OpenEditDialog(item) {
            this.SelectedItem = item;
            this.ruleForm.Id = item.id;
            this.ruleForm.CoursesId = item.coursesId;
            this.ruleForm.Type = item.type;
            this.ruleForm.ExamDate = item.examDate;
            this.ruleForm.Degree = item.degree;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.Image = item.image;
            this.state = 2;
            
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.ruleForm.Degree = Number(this.ruleForm.Degree);
                    this.ruleForm.ClasseId = this.$parent.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.EditCoursesExams(this.ruleForm)
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
                    this.$http.ChangeStatusCoursesExams(Id)
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
                    this.$http.DeleteCoursesExams(Id)
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

        //Upload File
        SelectImage(file) {

            // Check if file is defined
            if (!file) {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'يرجى اختيار ملف');
                return;
            }

            let str = file.type; // Access type property safely
            str = str.substring(0, 5);

            // Check if the file type is an image
            if (str !== "image") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
                return;
            }

            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                $this.ruleForm.ImageName = file.name;
                $this.ruleForm.Image = reader.result;
            };
        },







        //ExamsGrades
        OpenExamsGrades(item) { 
            this.state = 3;
            this.SelectedItem = item;
            this.$blockUI.Start();
            this.$http.GetExamsGrades(this.SelectedItem.id, this.SearchExamsGrades)
                .then(response => {
                    this.$blockUI.Stop();
                    this.InfoExamsGrades = response.data.info;
                    this.StatisticsExamsGrades = response.data.statistics;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        FilterExamsGrades() {
            if (!this.SearchExamsGrades)
                this.$blockUI.Start();
            this.$http.GetCoursesExams(this.pageNo, this.pageSize, this.SearchExamsGrades, this.$parent.SelectedItem.id, this.CoursesId)
                .then(response => {
                    if (!this.SearchExamsGrades)
                        this.$blockUI.Stop();

                    this.InfoExamsGrades = response.data.info;
                    this.StatisticsExamsGrades = response.data.statistics;
                })
                .catch(() => {
                    if (!this.SearchExamsGrades)
                        this.$blockUI.Stop();
                });
        },

        ChangeStatusExamsGrades(Id) {
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
                    this.$http.ChangeStatusExamsGrades(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.FilterExamsGrades();
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

        DeleteExamsGrades(Id) {
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
                    this.$http.DeleteExamsGrades(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.FilterExamsGrades();
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


        //AddExamsGrades
        OpenAddExamsGrades(item) {
            this.InfoExamsGrades = [];
            this.SelectedItem = item;
            this.state = 4;
            this.$blockUI.Start();
            this.$http.GetAllStudentsByClassForDegree(this.$parent.SelectedItem.id, this.ruleForm.EnrollmentDate)
                .then(response => {
                    this.$blockUI.Stop();
                    this.InfoExamsGrades = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        submitListForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    this.$blockUI.Start();
                    this.$http.AddExamsGrades(this.InfoExamsGrades)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.Back();
                            this.InfoExamsGrades = [];
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

        validateInteger(event) {
            const value = event.target.value;
            if (!Number.isInteger(Number(value)) && value !== '') {
                // If the value is not an integer, you can handle it here
                event.target.value = Math.floor(Number(value)); // Optionally round down
            }
        },





        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },

    }
}
