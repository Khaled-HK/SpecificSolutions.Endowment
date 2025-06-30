import moment from 'moment';
import Swal from "sweetalert2";
import HelperMixin from '../../Shared/HelperMixin.vue';
import PaginationHelper from '../../Shared/PaginationHelper.vue';
import QuillEditor from '../../Shared/QuillEditor.vue';
import ReviewsChart from '../Charts/ReviewsChart.vue';
import BarChart from '../Charts/BarChart.vue';

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
            
            IsFree: '',
            IsDiscount: '',
            SalesStatus: '',
            ViewStatus: '',

            ScreenTitle: '  الدورات التدريبية   ',
            ScreenTitleSingle: '  دورة تدريبية   ',
            ScreenTitleSingleAl: ' الدورة التدريبية  ',

            Info: [],
            Statistics: [],
            

            AddDialog: false,

            SelectedItem: '',

            ruleForm: {
                Id: '',
                Name: '',
                AcademicLevelId: '',
                AcademicSpecializationId: '',
                SubjectId: '',
                InstructorId: '',
                Descriptions: '',
                IsFree: '',
                IsDiscount: '',
                Price: '',
                PriceDiscount: '',
                IntroUrl: '',
                Telgram: '',
                Image: '',
                ImageName: '',
            },
            rules: {
                Name: this.$helper.Required(),
                AcademicLevelId: this.$helper.Required(),
                AcademicSpecializationId: this.$helper.Required(),
                SubjectId: this.$helper.Required(),
                InstructorId: this.$helper.Required(),
                IntroUrl: this.$helper.Required(),
                Descriptions: this.$helper.Required(),
                Price: this.$helper.Required(),
            },





            //View Info
            ChartInfo: [],



            //Shapter
            ShapterSearch: '',
            ShapterInfo: [],
            ShapterStatistics: [],
            ShapterSelectedItem: '',
            ShapterScreenTitle: ' الشباتر ',
            ShapterScreenTitleSingle: ' شبتر',
            ShapterScreenTitleSingleAl: 'الشبتر',
            ShapterruleForm: {
                Id: '',
                CourseId: '',
                Name: '',
                Number: '',
                Descriptions: '',
                Image: '',
                ImageName: '',
            },
            Shapterrules: {
                Name: this.$helper.Required(),
                Number: this.$helper.Required(),
            },




            //Lectures
            LecturesSearch: '',
            LecturesShapterId: '',
            LecturesInfo: [],
            LecturesStatistics: [],
            LecturesSelectedItem: '',
            LecturesScreenTitle: ' المحاضرات ',
            LecturesScreenTitleSingle: ' محاضرة',
            LecturesScreenTitleSingleAl: 'المحاضرة',
            LecturesruleForm: {
                Id: '',
                ShapterId: '',
                Name: '',
                Number: '',
                Descriptions: '',
            },
            Lecturesrules: {
                Name: this.$helper.Required(),
                Number: this.$helper.Required(),
                ShapterId: this.$helper.Required(),
            },



            //LecturesAttashments
            LecturesAttashmentsDialog: false,
            LecturesAttashmentsSearch: '',
            LectureId: '',
            LecturesAttashmentsInfo: [],
            LecturesAttashmentsStatistics: [],
            LecturesAttashmentsSelectedItem: '',
            LecturesAttashmentsScreenTitle: ' المرفقات الخاصة بالمحاضرة ',
            LecturesAttashmentsScreenTitleSingle: ' مرفق',
            LecturesAttashmentsScreenTitleSingleAl: 'المرفق',
            LecturesAttashmentsruleForm: {
                Id: '',
                LectureId: '',
                Name: '',
                Number: '',
                Descriptions: '',
                Type: '',
                Image: '',
                ImageName: '',
            },
            LecturesAttashmentsrules: {
                Name: this.$helper.Required(),
                Number: this.$helper.Required(),
                LectureId: this.$helper.Required(),
            },










            //Lectures
            ExamsSearch: '',
            ExamsShapterId: '',
            ExamsInfo: [],
            ExamsStatistics: [],
            ExamsSelectedItem: '',
            ExamsScreenTitle: ' الإختبارات  ',
            ExamsScreenTitleSingle: ' إختبار',
            ExamsScreenTitleSingleAl: 'الإختبار ',
            ExamsruleForm: {
                Id: '',
                ShapterId: '',
                Name: '',
                Number: '',
                Descriptions: '',
                HasLimght: false,
                Limght: '',
                Marck: '',
                SucessMarck: '',
            },
            Examsrules: {
                Name: this.$helper.Required(),
                Number: this.$helper.Required(),
                ShapterId: this.$helper.Required(),
                Marck: this.$helper.Required(),
                SucessMarck: this.$helper.Required(),
            },



            //ExamsQuestions
            ExamsQuestionsDialog: false,
            ExamsQuestionsSearch: '',
            ExamsQuestionsInfo: [],
            ExamsQuestionsStatistics: [],
            ExamsQuestionsSelectedItem: '',
            ExamsQuestionsScreenTitle: ' الأسئلة  ',
            ExamsQuestionsScreenTitleSingle: ' سؤال',
            ExamsQuestionsScreenTitleSingleAl: 'السؤال',
            ExamsQuestionsruleForm: {
                Id: '',
                ExamId: '',
                Question: '',
                Number: '',
                Marck: '',
                Type: '',
                Answer: '',
                CompleteAnswer: '',
                A1: '',
                A2: '',
                A3: '',
                A4: '',
                Image: '',
                ImageName: '',
            },
            ExamsQuestionsrules: {
                ExamId: this.$helper.Required(),
                Question: this.$helper.Required(),
                Number: this.$helper.Required(),
                Marck: this.$helper.Required(),
                Type: this.$helper.Required(),
                Answer: this.$helper.Required(),
                CompleteAnswer: this.$helper.Required(),
                A1: this.$helper.Required(),
                A2: this.$helper.Required(),
                A3: this.$helper.Required(),
                A4: this.$helper.Required(),
                
            },






            //Students
            StudentsSearch: '',
            StudentsInfo: [],
            StudentsStatistics: [],
            StudentsSelectedItem: '',
            StudentsScreenTitle: ' الشباتر ',
            StudentsScreenTitleSingle: ' شبتر',
            StudentsScreenTitleSingleAl: 'الشبتر',



            //Review
            ReviewSearch: '',
            ReviewInfo: [],
            ReviewStatistics: [],
            ReviewSelectedItem: '',
            ReviewScreenTitle: ' التقيمات  ',
            ReviewScreenTitleSingle: ' تقيم',
            ReviewScreenTitleSingleAl: 'التقيم',




        };
    },



    methods: {

        //Get Dictionaries
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
            this.$http.GetCourses(this.pageNo, this.pageSize, this.Search,this.IsFree,this.IsDiscount,this.SalesStatus,this.ViewStatus)
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

                    if (this.ruleForm.IsDiscount) {
                        this.ruleForm.IsDiscount = Boolean(this.ruleForm.IsDiscount);
                    } else {
                        this.ruleForm.IsDiscount = Boolean(0);
                    }
                        

                    if (this.ruleForm.IsFree) {
                        this.ruleForm.IsFree = Boolean(this.ruleForm.IsFree);
                    } else {
                        this.ruleForm.IsFree = Boolean(0);
                    }
                        

                    if (this.ruleForm.Price) {
                        this.ruleForm.Price = Number(this.ruleForm.Price);
                    } else {
                        this.ruleForm.Price = Number(0);
                    }
                        

                    if (this.ruleForm.PriceDiscount) {
                        this.ruleForm.PriceDiscount = Number(this.ruleForm.PriceDiscount);
                    } else {
                        this.ruleForm.PriceDiscount = Number(0);
                    }
                        



                    this.$blockUI.Start();
                    this.$http.AddCourses(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.Back();
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
            this.ruleForm.Name = item.name;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.AcademicLevelId = item.academicLevelId;
            this.GetAllAcademicSpecializations(item.academicLevelId);
            this.ruleForm.AcademicSpecializationId = item.academicSpecializationId;
            this.ruleForm.SubjectId = item.subjectId;
            this.ruleForm.InstructorId = item.instructorId;
            this.ruleForm.IsFree = item.isFree;
            this.ruleForm.IsDiscount = item.isDiscount;
            this.ruleForm.Price = item.price;
            this.ruleForm.PriceDiscount = item.priceDiscount;
            this.ruleForm.IntroUrl = item.introUrl;
            this.ruleForm.Telgram = item.telgram;
            this.state = 2;

        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.$blockUI.Start();
                    this.$http.EditCourses(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.Back();
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
                    this.$http.ChangeStatusCourses(Id)
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
                    this.$http.ChangeSalesStatusCourses(Id)
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

        CloseCourses(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من إغلاق الدورة التدريبية  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.CloseCourses(Id)
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
                    this.$http.DeleteCourses(Id)
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









        //Info
        ChangeTab(tabname) {
            if (tabname == 'Chart') {
                this.GetChartInfo();
            } else if (tabname == 'Shapter') {
                this.GetShapterInfo();
            } else if (tabname == 'Students') {
                this.GetStudentsInfo();
            } else if (tabname == 'Review') {
                this.GetReviewInfo();
            } else if (tabname == 'Lectures') {
                this.GetAllShapters(this.SelectedItem.id);
                this.GetLecturesInfo();
                this.LecturesAttashmentsDialog = false;
            } else if (tabname == 'Exams') {
                this.GetAllShapters(this.SelectedItem.id);
                this.GetExamsInfo();
                this.ExamsQuestionsDialog = false;
            }
        },

        OpentInfoDialog(item) {
            this.SelectedItem = item;
            this.ruleForm.Id = item.id;
            this.ruleForm.Name = item.name;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.AcademicLevelId = item.academicLevelId;
            this.GetAllAcademicSpecializations(item.academicLevelId);
            this.ruleForm.AcademicSpecializationId = item.academicSpecializationId;
            this.ruleForm.SubjectId = item.subjectId;
            this.ruleForm.InstructorId = item.instructorId;
            this.ruleForm.IsFree = item.isFree;
            this.ruleForm.IsDiscount = item.isDiscount;
            this.ruleForm.Price = item.price;
            this.ruleForm.PriceDiscount = item.priceDiscount;
            this.ruleForm.IntroUrl = item.introUrl;
            this.ruleForm.Telgram = item.telgram;
            this.state = 3;
            this.GetChartInfo();

        },



        //Charts
        GetChartInfo() {
            this.ChartInfo = [];
            this.$http.GetCoursesChartInfo(this.SelectedItem.id)
                .then(response => {
                    this.ChartInfo = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },






        //Shapter
        GetShapterInfo() {
            this.ShapterInfo = [];
            this.ShapterStatistics = [];

            if (!this.ShapterSearch)
                this.$blockUI.Start();

            this.$http.GetCourseShapters(this.SelectedItem.id, this.ShapterSearch)
                .then(response => {
                    if (!this.ShapterSearch)
                        this.$blockUI.Stop();

                    this.ShapterInfo = response.data.info;
                    this.ShapterStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.ShapterSearch)
                        this.$blockUI.Stop();
                });
        },
        
        ShapterSelectImage(file) {

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
                $this.ShapterruleForm.ImageName = file.name;
                $this.ShapterruleForm.Image = reader.result;
            };
        },

        ShaptersubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ShapterruleForm.Id = Number(0);
                    this.ShapterruleForm.Number = Number(this.ShapterruleForm.Number);
                    this.ShapterruleForm.CourseId = this.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddShapters(this.ShapterruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetShapterInfo();
                            this.ClearForm(this.ShapterruleForm);
                            this.$helper.DestroyElemntById('ShapterClose');
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

        ShapterOpentEditDialog(item) {
            this.ShapterSelectedItem = item;
            this.ShapterruleForm.Id = item.id;
            this.ShapterruleForm.Name = item.name;
            this.ShapterruleForm.Number = item.number;
            this.ShapterruleForm.CourseId = item.courseId;
            this.ShapterruleForm.Descriptions = item.descriptions;

        },

        ShaptersubmitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ShapterruleForm.Id = Number(this.ShapterruleForm.Id);
                    this.ShapterruleForm.Number = Number(this.ShapterruleForm.Number);
                    this.$blockUI.Start();
                    this.$http.EditShapters(this.ShapterruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetShapterInfo();
                            this.ClearForm(this.ShapterruleForm);
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

        ShapterChangeStatus(Id) {
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
                    this.$http.ChangeStatusShapters(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetShapterInfo();
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

        ShapterDelete(Id) {
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
                    this.$http.DeleteShapters(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetShapterInfo();
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









        //Lectures
        GetLecturesInfo() {
            this.LecturesInfo = [];
            this.LecturesStatistics = [];

            if (!this.LecturesSearch)
                this.$blockUI.Start();

            this.$http.GetCourseLectures(this.SelectedItem.id, this.LecturesShapterId, this.LecturesSearch)
                .then(response => {
                    if (!this.LecturesSearch)
                        this.$blockUI.Stop();

                    this.LecturesInfo = response.data.info;
                    this.LecturesStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.LecturesSearch)
                        this.$blockUI.Stop();
                });
        },

        LecturessubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.LecturesruleForm.Id = Number(0);
                    this.LecturesruleForm.Number = Number(this.LecturesruleForm.Number);
                    this.$blockUI.Start();
                    this.$http.AddLectures(this.LecturesruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetLecturesInfo();
                            this.ClearForm(this.LecturesruleForm);
                            this.$helper.DestroyElemntById('LecturesClose');
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

        LecturesOpentEditDialog(item) {
            this.LecturesSelectedItem = item;
            this.LecturesruleForm.Id = item.id;
            this.LecturesruleForm.Name = item.name;
            this.LecturesruleForm.Number = item.number;
            this.LecturesruleForm.ShapterId = item.shapterId;
            this.LecturesruleForm.Descriptions = item.descriptions;

        },

        LecturessubmitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.LecturesruleForm.Id = Number(this.LecturesruleForm.Id);
                    this.LecturesruleForm.Number = Number(this.LecturesruleForm.Number);
                    this.$blockUI.Start();
                    this.$http.EditLectures(this.LecturesruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetLecturesInfo();
                            this.ClearForm(this.LecturesruleForm);
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

        LecturesChangeStatus(Id) {
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
                    this.$http.ChangeStatusLectures(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetLecturesInfo();
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

        LecturesDelete(Id) {
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
                    this.$http.DeleteLectures(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetLecturesInfo();
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








        //LecturesAttashments
        OpentLecturesAttashmentsDialog(item) {
            this.LecturesSelectedItem = item;
            this.LecturesAttashmentsDialog = true;
            this.GetLecturesAttashmentsInfo();

        },

        BackToLectures() {
            this.LecturesAttashmentsDialog = false;
        },

        GetLecturesAttashmentsInfo() {
            this.LecturesAttashmentsInfo = [];
            this.LecturesAttashmentsStatistics = [];
            if (!this.LecturesAttashmentsSearch)
                this.$blockUI.Start();

            this.$http.GetCourseLecturesAttashments(this.LecturesSelectedItem.id, this.LecturesAttashmentsSearch)
                .then(response => {
                    if (!this.LecturesAttashmentsSearch)
                        this.$blockUI.Stop();

                    this.LecturesAttashmentsInfo = response.data.info;
                    this.LecturesAttashmentsStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.LecturesAttashmentsSearch)
                        this.$blockUI.Stop();
                });
        },

        LecturesAttashmentsSelectImage(file) {

            // Check if file is defined
            if (!file) {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'يرجى اختيار ملف');
                return;
            }

            let str = file.type; // Access type property safely
            //str = str.substring(0, 5);


            // Check if the file type is an image
            if (str !== "image" &&
                str !== "application/pdf" &&
                str !== "application/vnd.openxmlformats-officedocument.presentationml.presentation" &&
                str !== "video") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
                return;
            }

            if (str == "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                this.LecturesAttashmentsruleForm.Type = 3;
            }

            if (str == "application/pdf") {
                this.LecturesAttashmentsruleForm.Type = 3;
            }

            str = str.substring(0, 5);

            if (str == "image") {
                this.LecturesAttashmentsruleForm.Type = 2;
            }

            if (str == "video") {
                this.LecturesAttashmentsruleForm.Type = 1;
            }

            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                $this.LecturesAttashmentsruleForm.ImageName = file.name;
                $this.LecturesAttashmentsruleForm.Image = reader.result;
            };
        },

        LecturesAttashmentssubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.LecturesAttashmentsruleForm.Id = Number(0);
                    this.LecturesAttashmentsruleForm.LectureId = this.LecturesSelectedItem.id;
                    this.LecturesAttashmentsruleForm.Number = Number(this.LecturesAttashmentsruleForm.Number);
                    this.$blockUI.Start();
                    this.$http.AddLecturesAttashments(this.LecturesAttashmentsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetLecturesAttashmentsInfo();
                            this.ClearForm(this.LecturesAttashmentsruleForm);
                            this.$helper.DestroyElemntById('LecturesAttashmentsClose');
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

        LecturesAttashmentsOpentEditDialog(item) {
            this.LecturesAttashmentsSelectedItem = item;
            this.LecturesAttashmentsruleForm.Id = item.id;
            this.LecturesAttashmentsruleForm.Name = item.name;
            this.LecturesAttashmentsruleForm.Number = item.number;
            this.LecturesAttashmentsruleForm.LectureId = item.lectureId;
            this.LecturesAttashmentsruleForm.Descriptions = item.descriptions;

        },

        LecturesAttashmentssubmitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.LecturesAttashmentsruleForm.Id = Number(this.LecturesAttashmentsruleForm.Id);
                    this.LecturesAttashmentsruleForm.Number = Number(this.LecturesAttashmentsruleForm.Number);
                    if (!this.LecturesAttashmentsruleForm.Type)
                        this.LecturesAttashmentsruleForm.Type = Number(0);
                    this.$blockUI.Start();
                    this.$http.EditLecturesAttashments(this.LecturesAttashmentsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetLecturesAttashmentsInfo();
                            this.ClearForm(this.LecturesAttashmentsruleForm);
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

        LecturesAttashmentsChangeStatus(Id) {
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
                    this.$http.ChangeStatusLecturesAttashments(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetLecturesAttashmentsInfo();
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

        LecturesAttashmentsDelete(Id) {
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
                    this.$http.DeleteLecturesAttashments(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetLecturesAttashmentsInfo();
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












        //Exams
        GetExamsInfo() {
            this.ExamsInfo = [];
            this.ExamsStatistics = [];
            if (!this.ExamsSearch)
                this.$blockUI.Start();

            this.$http.GetCourseExams(this.SelectedItem.id,this.ExamsShapterId, this.ExamsSearch)
                .then(response => {
                    if (!this.ExamsSearch)
                        this.$blockUI.Stop();

                    this.ExamsInfo = response.data.info;
                    this.ExamsStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.ExamsSearch)
                        this.$blockUI.Stop();
                });
        },

        ExamssubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ExamsruleForm.Id = Number(0);
                    this.ExamsruleForm.Number = Number(this.ExamsruleForm.Number);
                    this.ExamsruleForm.Marck = Number(this.ExamsruleForm.Marck);
                    this.ExamsruleForm.SucessMarck = Number(this.ExamsruleForm.SucessMarck);
                    if (this.ExamsruleForm.HasLimght == true) {
                        this.ExamsruleForm.Limght = Number(this.ExamsruleForm.Limght);
                    } else {
                        this.ExamsruleForm.Limght = Number(0);
                    }
                        
                    this.$blockUI.Start();
                    this.$http.AddExams(this.ExamsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetExamsInfo();
                            this.ClearForm(this.ExamsruleForm);
                            this.$helper.DestroyElemntById('ExamsClose');
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
       
        ExamsOpentEditDialog(item) {
            this.ExamsSelectedItem = item;
            this.ExamsruleForm.Id = item.id;
            this.ExamsruleForm.Name = item.name;
            this.ExamsruleForm.Number = item.number;
            this.ExamsruleForm.ShapterId = item.shapterId;
            this.ExamsruleForm.Descriptions = item.descriptions;
            this.ExamsruleForm.HasLimght = item.hasLimght;
            this.ExamsruleForm.Limght = item.limght;
            this.ExamsruleForm.Marck = item.marck;
            this.ExamsruleForm.SucessMarck = item.sucessMarck;

        },

        ExamssubmitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ExamsruleForm.Id = Number(this.ExamsruleForm.Id);
                    this.ExamsruleForm.Number = Number(this.ExamsruleForm.Number);
                    this.ExamsruleForm.Marck = Number(this.ExamsruleForm.Marck);
                    this.ExamsruleForm.SucessMarck = Number(this.ExamsruleForm.SucessMarck);
                    if (this.ExamsruleForm.HasLimght == true) {
                        this.ExamsruleForm.Limght = Number(this.ExamsruleForm.Limght);
                    } else {
                        this.ExamsruleForm.Limght = Number(0);
                    }

                    this.$blockUI.Start();
                    this.$http.EditExams(this.ExamsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetExamsInfo();
                            this.ClearForm(this.ExamsruleForm);
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

        ExamsChangeStatus(Id) {
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
                    this.$http.ChangeStatusExams(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetExamsInfo();
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

        ExamsDelete(Id) {
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
                    this.$http.DeleteExams(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetExamsInfo();
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








        //ExamsQuestions
        OpentExamsQuestionsDialog(item) {
            this.ExamsSelectedItem = item;
            this.ExamsQuestionsDialog = true;
            this.GetExamsQuestionsInfo();

        },

        BackToExams() {
            this.ExamsQuestionsDialog = false;
        },

        GetExamsQuestionsInfo() {
            this.ExamsQuestionsInfo = [];
            this.ExamsQuestionsStatistics = [];

            if (!this.ExamsQuestionsSearch)
                this.$blockUI.Start();

            this.$http.GetCourseExamsQuestions(this.ExamsSelectedItem.id, this.ExamsQuestionsSearch)
                .then(response => {
                    if (!this.ExamsQuestionsSearch)
                        this.$blockUI.Stop();

                    this.ExamsQuestionsInfo = response.data.info;
                    this.ExamsQuestionsStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.ExamsQuestionsSearch)
                        this.$blockUI.Stop();
                });
        },

        ExamsQuestionsSelectImage(file) {

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
                $this.ExamsQuestionsruleForm.ImageName = file.name;
                $this.ExamsQuestionsruleForm.Image = reader.result;
            };
        },

        ExamsQuestionssubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ExamsQuestionsruleForm.Id = Number(0);
                    this.ExamsQuestionsruleForm.ExamId = this.ExamsSelectedItem.id;
                    this.ExamsQuestionsruleForm.Number = Number(this.ExamsQuestionsruleForm.Number);
                    this.ExamsQuestionsruleForm.Marck = Number(this.ExamsQuestionsruleForm.Marck);
                    this.ExamsQuestionsruleForm.Type = Number(this.ExamsQuestionsruleForm.Type);
                    this.$blockUI.Start();
                    this.$http.AddExamsQuestions(this.ExamsQuestionsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetExamsQuestionsInfo();
                            this.ClearForm(this.ExamsQuestionsruleForm);
                            this.$helper.DestroyElemntById('ExamsQuestionsClose');
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

        ExamsQuestionsOpentEditDialog(item) {
            this.ExamsQuestionsSelectedItem = item;
            this.ExamsQuestionsruleForm.Id = item.id;
            this.ExamsQuestionsruleForm.ExamId = item.examId;
            this.ExamsQuestionsruleForm.Question = item.question;
            this.ExamsQuestionsruleForm.Number = item.number;
            this.ExamsQuestionsruleForm.Marck = item.marck;
            this.ExamsQuestionsruleForm.Type = item.type;
            this.ExamsQuestionsruleForm.Answer = item.answer;
            this.ExamsQuestionsruleForm.CompleteAnswer = item.completeAnswer;
            this.ExamsQuestionsruleForm.A1 = item.a1;
            this.ExamsQuestionsruleForm.A2 = item.a2;
            this.ExamsQuestionsruleForm.A3 = item.a3;
            this.ExamsQuestionsruleForm.A4 = item.a4;

        },

        ExamsQuestionssubmitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ExamsQuestionsruleForm.Id = Number(this.ExamsQuestionsruleForm.Id);
                    this.ExamsQuestionsruleForm.Number = Number(this.ExamsQuestionsruleForm.Number);
                    this.ExamsQuestionsruleForm.Type = Number(this.ExamsQuestionsruleForm.Type);
                    this.ExamsQuestionsruleForm.Marck = Number(this.ExamsQuestionsruleForm.Marck);

                    this.$blockUI.Start();
                    this.$http.EditExamsQuestions(this.ExamsQuestionsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetExamsQuestionsInfo();
                            this.ClearForm(this.ExamsQuestionsruleForm);
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

        ExamsQuestionsChangeStatus(Id) {
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
                    this.$http.ChangeStatusExamsQuestions(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetExamsQuestionsInfo();
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

        ExamsQuestionsDelete(Id) {
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
                    this.$http.DeleteExamsQuestions(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetExamsQuestionsInfo();
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









        //Students
        GetStudentsInfo() {
            this.StudentsInfo = [];
            this.StudentsStatistics = [];

            if (!this.StudentsSearch)
                this.$blockUI.Start();

            this.$http.GetCourseStudents(this.SelectedItem.id, this.StudentsSearch)
                .then(response => {
                    if (!this.StudentsSearch)
                        this.$blockUI.Stop();

                    this.StudentsInfo = response.data.info;
                    this.StudentsStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.StudentsSearch)
                        this.$blockUI.Stop();
                });
        },


        //Review
        GetReviewInfo() {
            this.ReviewInfo = [];
            this.ReviewStatistics = [];

            if (!this.ReviewSearch)
                this.$blockUI.Start();

            this.$http.GetCourseReview(this.SelectedItem.id, this.ReviewSearch)
                .then(response => {
                    if (!this.ReviewSearch)
                        this.$blockUI.Stop();

                    this.ReviewInfo = response.data.info;
                    this.ReviewStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.ReviewSearch)
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
