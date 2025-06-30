
import Swal from "sweetalert2";
import HelperMixin from '../../Shared/HelperMixin.vue';
import PaginationHelper from '../../Shared/PaginationHelper.vue';
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

            ScreenTitle: ' الطلبة  ',
            ScreenTitleSingle: ' طالب ',
            ScreenTitleSingleAl: 'الطالب ',

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
                LoginName: [
                    { required: true, message: 'الرجاء إدخال اسم الدخول', trigger: 'blur' },
                    { required: true, pattern: /^[A-Za-z]{0,40}$/, message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه', trigger: 'blur' }
                ],
                Email: this.$helper.EmailOnly(),
            },

            //Suspends
            SuspendsruleForm: {
                Id: '',
                DropResone: '',
            },
            Suspendsrules: {
                DropResone: this.$helper.Required(),
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
            this.$http.GetStudents(this.pageNo, this.pageSize, this.Search)
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
                    this.$blockUI.Start();
                    this.$http.AddStudents(this.ruleForm)
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
            this.ruleForm.FirstName = item.firstName;
            this.ruleForm.FatherName = item.fatherName;
            this.ruleForm.SirName = item.sirName;
            this.ruleForm.Phone = item.phone;
            this.ruleForm.ExtraPhone = item.extraPhone;
            this.ruleForm.LoginName = item.loginName;
            this.ruleForm.Email = item.email;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.AcademicLevelId = item.academicLevelId;
            this.GetAllAcademicSpecializations(item.academicLevelId);
            this.ruleForm.AcademicSpecializationId = item.academicSpecializationId;

        },

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

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.$blockUI.Start();
                    this.$http.EditStudents(this.ruleForm)
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
                text: ' هـل انت متأكد من عملية تغير حالة الحساب  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeStatusStudents(Id)
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

        ResetDevice(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية تهيئة الجهاز   ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ResetDeviceStudents(Id)
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

        ResetPassword(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية تهيئة كلمة المرور   ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ResetPasswordStudents(Id)
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
                    this.$http.DeleteStudents(Id)
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


        //Suspends
        OpentSuspendsDialog(item) {
            this.SelectedItem = item;
        },

        SuspendssubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.SuspendsruleForm.Id = this.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.SuspendsStudents(this.SuspendsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.ClearForm(this.SuspendsruleForm);
                            this.$helper.DestroyElemntById('SuspendsClose');
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






        //InfoDialog
        OpentInfoDialog(item) {
            this.SelectedItem = item;
            this.ruleForm.Id = item.id;
            this.ruleForm.FirstName = item.firstName;
            this.ruleForm.FatherName = item.fatherName;
            this.ruleForm.SirName = item.sirName;
            this.ruleForm.Phone = item.phone;
            this.ruleForm.ExtraPhone = item.extraPhone;
            this.ruleForm.LoginName = item.loginName;
            this.ruleForm.Email = item.email;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.AcademicLevelId = item.academicLevelId;
            this.ruleForm.AcademicSpecializationId = item.academicSpecializationId;
            this.GetCoursesInfo();
            this.state = 1;

        },




        ChangeTab(tabname) {
            if (tabname == 'Courses') {
                this.GetCoursesInfo();
            } else if (tabname == 'Wallet') {
                this.GetWalletInfo();
            } else if (tabname == 'Devices') {
                this.GetDevicesInfo();
            }
        },


        //Courses
        GetCoursesInfo() {
            this.CoursesInfo = [];
            this.CoursesDropInfo = [];
            this.CoursesStatistics = [];

            if (!this.CoursesSearch)
                this.$blockUI.Start();

            this.$http.GetStudentsCourses(this.SelectedItem.id, this.CoursesSearch)
                .then(response => {
                    if (!this.CoursesSearch)
                        this.$blockUI.Stop();

                    this.CoursesInfo = response.data.info;
                    this.CoursesDropInfo = response.data.dropInfo;
                    this.CoursesStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.CoursesSearch)
                        this.$blockUI.Stop();
                });
        },

        Add_GetAcademicSpecializations() {
            this.GetAllAcademicSpecializations(this.CoursesruleForm.AcademicSpecializationId);
        },

        Add_GetCourses() {
            if (this.CoursesruleForm.AcademicSpecializationId && this.CoursesruleForm.SubjectId)
                this.GetAllCourses(this.CoursesruleForm.SubjectId, this.CoursesruleForm.AcademicSpecializationId);
        },

        CoursessubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this. CoursesruleForm.Id = Number(0);
                    this. CoursesruleForm.Value = Number(this.CoursesruleForm.Value);
                    this. CoursesruleForm.StudentId = this.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddStudentsCourses(this.CoursesruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetCoursesInfo();
                            this.ClearForm(this.CoursesruleForm);
                            this.$helper.DestroyElemntById('CoursesClose');
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

        OpenCoursesValue(item) {
            this.CoursesSelectedItem = item;
        },

        CoursesValuesubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.CoursesValueruleForm.Id = this.CoursesSelectedItem.id;
                    this.CoursesValueruleForm.Value = Number(this.CoursesValueruleForm.Value);
                    this.$blockUI.Start();
                    this.$http.AddStudentsCoursesValue(this.CoursesValueruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetCoursesInfo();
                            this.ClearForm(this.CoursesValueruleForm);
                            this.$helper.DestroyElemntById('CoursesValueClose');
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

        OpenCoursesDrop(item) {
            this.CoursesSelectedItem = item;
        },

        CoursesDropsubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.CoursesDropruleForm.Id = this.CoursesSelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.DropStudentsCourses(this.CoursesDropruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetCoursesInfo();
                            this.ClearForm(this.CoursesDropruleForm);
                            this.$helper.DestroyElemntById('CoursesDropClose');
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






        //Wallet
        GetWalletInfo() {
            this.WalletInfo = [];
            this.WalletInfoPurchases = [];
            this.WalletInfoTracker = [];
            this.$blockUI.Start();
            this.$http.GetStudentsWallet(this.SelectedItem.id)
                .then(response => {
                    this.$blockUI.Stop();
                    this.WalletInfo = response.data.info;
                    this.WalletInfoPurchases = response.data.infoPurchases;
                    this.WalletInfoTracker = response.data.infoTracker;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },


        WalletsubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.WalletruleForm.StudentId = this.SelectedItem.id;
                    this.WalletruleForm.Value = Number(this.WalletruleForm.Value);
                    this.$blockUI.Start();
                    this.$http.AddStudentsWalletValue(this.WalletruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetWalletInfo();
                            this.ClearForm(this.CoursesValueruleForm);
                            this.$helper.DestroyElemntById('WalletClose');
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





        //Devices
        GetDevicesInfo() {
            this.DevicesInfo = [];
            this.$blockUI.Start();
            this.$http.GetStudentsDevices(this.SelectedItem.id)
                .then(response => {
                    this.$blockUI.Stop();
                    this.DevicesInfo = response.data.info;
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


    }
}
