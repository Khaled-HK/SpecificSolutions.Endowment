
import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
import PaginationHelper from '../../../Shared/PaginationHelper.vue';
export default {
    name: 'Subscriptions',
    mixins: [HelperMixin],
    components: {
        PaginationHelper
    },

    async created() {
        await this.CheckLoginStatus();
        await this.GetAllSubscriptionsType();
        await this.GetAllPaymentsMethods();

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

            ScreenTitle: ' المدارس   ',
            ScreenTitleSingle: ' مدرسة  ',
            ScreenTitleSingleAl: 'المدرسة  ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',

            ruleForm: {
                Id: '',
                Name: '',
                Descriptions: '',
                UserName: '',
                LoginName: '',
                Email: '',
                Phone: '',
                Image: '',
                ImageName: '',
            },
            rules: {
                Name: this.$helper.Required(),
                UserName: this.$helper.Required(),
                LoginName: [
                    { required: true, message: 'الرجاء إدخال اسم الدخول', trigger: 'blur' },
                    { required: true, pattern: /^[A-Za-z]{0,40}$/, message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه', trigger: 'blur' }
                ],
                Phone: this.$helper.Phone(),
            },



            //Suspends
            SuspendsruleForm: {
                Id: '',
                DropResone: '',
            },
            Suspendsrules: {
                DropResone: this.$helper.Required(),
            },






            //Subscriptions
            SubscriptionsSearch: '',
            SubscriptionsInfo: [],
            SubscriptionsDropInfo: [],
            SubscriptionsStatistics: [],
            SubscriptionsSelectedItem: '',
            SubscriptionsScreenTitle: ' الإشتراكات  ',
            SubscriptionsScreenTitleSingle: ' إشتراك ',
            SubscriptionsScreenTitleSingleAl: 'الإشتراك  ',
            SubscriptionsruleForm: {
                Id: '',
                SchoolsId: '',
                SubscriptionsTypeId: '',
                Value: '',
            },
            Subscriptionsrules: {
                SubscriptionsTypeId: this.$helper.Required(),
                Value: this.$helper.Required(),
            },

            SubscriptionsValueruleForm: {
                Id: '',
                Value: '',
            },
            SubscriptionsValuerules: {
                Value: this.$helper.Required(),
            },

            SubscriptionsDropruleForm: {
                Id: '',
                DropResone: '',
            },
            SubscriptionsDroprules: {
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
                SchoolsId: '',
                PaymentMethodId: '',
                Value: '',
            },
            Walletrules: {
                PaymentMethodId: this.$helper.Required(),
                Value: this.$helper.Required(),
            },


            //ProfileYears
            ProfileYearsInfo: [],
            ProfileYearsSelectedItem: '',
            ProfileYearsScreenTitle: ' السنوات الدراسية  ',
            ProfileYearsScreenTitleSingle: ' سنة دراسية  ',
            ProfileYearsScreenTitleSingleAl: 'السنة الدراسية  ',
            ProfileYearsruleForm: {
                SchoolsId: '',
                Name: '',
                Descriptions: '',
            },
            ProfileYearsrules: {
                Name: this.$helper.Required(),
                Descriptions: this.$helper.Required(),
            },




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
            this.$http.GetSuspendsSchools(this.pageNo, this.pageSize, this.Search)
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
                    this.$http.CanselSuspendsSchools(Id)
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




        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(0);
                    this.$blockUI.Start();
                    this.$http.AddSchools(this.ruleForm)
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
            this.ruleForm.Name = item.name;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.UserName = item.userName;
            this.ruleForm.Phone = item.phone;
            this.ruleForm.LoginName = item.loginName;
            this.ruleForm.Email = item.email;
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
                    this.$http.EditSchools(this.ruleForm)
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
                    this.$http.ChangeStatusSchools(Id)
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
                    this.$http.ResetDeviceSchools(Id)
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
                    this.$http.ResetPasswordSchools(Id)
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
                    this.$http.DeleteSchools(Id)
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
                    this.$http.SuspendsSchools(this.SuspendsruleForm)
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
            this.ruleForm.Name = item.name;
            this.ruleForm.Descriptions = item.descriptions;
            this.ruleForm.UserName = item.userName;
            this.ruleForm.Phone = item.phone;
            this.ruleForm.LoginName = item.loginName;
            this.ruleForm.Email = item.email;
            this.GetSubscriptionsInfo();
            this.state = 1;

        },




        ChangeTab(tabname) {
            if (tabname == 'Subscriptions') {
                this.GetSubscriptionsInfo();
            } else if (tabname == 'Wallet') {
                this.GetWalletInfo();
            } else if (tabname == 'ProfileYears') {
                this.GetProfileYearsInfo();
            }
        },


        //Subscriptions
        GetSubscriptionsInfo() {
            this.SubscriptionsInfo = [];
            this.SubscriptionsDropInfo = [];
            this.SubscriptionsStatistics = [];

            if (!this.SubscriptionsSearch)
                this.$blockUI.Start();

            this.$http.GetSchoolsSubscriptions(this.SelectedItem.id, this.SubscriptionsSearch)
                .then(response => {
                    if (!this.SubscriptionsSearch)
                        this.$blockUI.Stop();

                    this.SubscriptionsInfo = response.data.info;
                    this.SubscriptionsDropInfo = response.data.dropInfo;
                    this.SubscriptionsStatistics = response.data.statistics;
                })
                .catch(() => {
                    if (!this.SubscriptionsSearch)
                        this.$blockUI.Stop();
                });
        },

        SubscriptionssubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this. SubscriptionsruleForm.Id = Number(0);
                    this. SubscriptionsruleForm.Value = Number(this.SubscriptionsruleForm.Value);
                    this.SubscriptionsruleForm.SchoolsId = this.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddSchoolsSubscriptions(this.SubscriptionsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetSubscriptionsInfo();
                            this.ClearForm(this.SubscriptionsruleForm);
                            this.$helper.DestroyElemntById('SubscriptionsClose');
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

        OpenSubscriptionsValue(item) {
            this.SubscriptionsSelectedItem = item;
        },

        SubscriptionsValuesubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.SubscriptionsValueruleForm.Id = this.SubscriptionsSelectedItem.id;
                    this.SubscriptionsValueruleForm.Value = Number(this.SubscriptionsValueruleForm.Value);
                    this.$blockUI.Start();
                    this.$http.AddSchoolsSubscriptionsValue(this.SubscriptionsValueruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetSubscriptionsInfo();
                            this.ClearForm(this.SubscriptionsValueruleForm);
                            this.$helper.DestroyElemntById('SubscriptionsValueClose');
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

        OpenSubscriptionsDrop(item) {
            this.SubscriptionsSelectedItem = item;
        },

        SubscriptionsDropsubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.SubscriptionsDropruleForm.Id = this.SubscriptionsSelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.DropSchoolsSubscriptions(this.SubscriptionsDropruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetSubscriptionsInfo();
                            this.ClearForm(this.SubscriptionsDropruleForm);
                            this.$helper.DestroyElemntById('SubscriptionsDropClose');
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
            this.$http.GetSchoolsWallet(this.SelectedItem.id)
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
                    this.WalletruleForm.SchoolsId = this.SelectedItem.id;
                    this.WalletruleForm.Value = Number(this.WalletruleForm.Value);
                    this.$blockUI.Start();
                    this.$http.AddSchoolsWalletValue(this.WalletruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetWalletInfo();
                            this.ClearForm(this.SubscriptionsValueruleForm);
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





        //ProfileYears
        GetProfileYearsInfo() {
            this.ProfileYearsInfo = [];
            this.$blockUI.Start();
            this.$http.GetSchoolsProfileYears(this.SelectedItem.id)
                .then(response => {
                    this.$blockUI.Stop();
                    this.ProfileYearsInfo = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        ProfileYearsruleFormsubmitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ProfileYearsruleForm.SchoolsId = this.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.AddProfileYears(this.ProfileYearsruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetProfileYearsInfo();
                            this.ClearForm(this.ProfileYearsruleForm);
                            this.$helper.DestroyElemntById('SubscriptionsValueClose');
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

        ActiveProfileYear(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هل أنت متأكد من تفعيل   الفصل الدراسي (لا يمكن تفعيل أكتر من عام دراسي في نفس الوقت )  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ActiveProfileYears(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetProfileYearsInfo();
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

        CloseProfileYear(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هل أنت متأكد من    إقفال الفصل الدراسي   ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.CloseProfileYears(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetProfileYearsInfo();
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








        //Support Info
        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },


    }
}
