
import Swal from "sweetalert2";
import HelperMixin from '../../Shared/HelperMixin.vue';
import PaginationHelper from '../../Shared/PaginationHelper.vue';
export default {
    name: 'Subscriptions',
    mixins: [HelperMixin],
    components: {
        PaginationHelper
    },

    async created() {
        await this.CheckLoginStatus();
        await this.GetAllSubscriptionsType();
        await this.GetOffices();
        await this.GetRegions();

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

            activePanels: ['1'], // Default open panels
             
            ScreenTitle: ' المساجد   ',
            ScreenTitleSingle: ' مرفق  ',
            ScreenTitleSingleAl: 'المدرسة  ',

            Info: [],
            Statistics: [],
            ProfileYearsrules: [],
            ProfileYearsScreenTitleSingleAl: [],
            WalletScreenTitleSingle: [],
            WalletInfoTracker: [],
            Walletrules: [],
            ProfileYearsScreenTitleSingle: [],
            WalletInfoPurchases: [],
            WalletScreenTitle: [],
            WalletInfo: [],

            AddDialog: false,
            SelectedItem: '',
            UserId: '',

            SearchTerm: '',

            WalletruleForm: {
            },
             
            buildingDetailRuleForm: {
                Name: '',
                WithinMosqueArea: false,
                Floors: 0,  // Ensure this is number, not string
                BuildingCategory: 'Facility',  // Must match enum name exactly
                MosqueID: ''
            },

            buildingDetailRules: {
            },

            ruleForm: {
                Id: '',
                MosqueName: '',
                FileNumber: '',
                Definition: '',
                Classification: '',
                OfficeId: '',
                Unit: '',
                RegionId: '',
                NearestLandmark: '',
                MapLocation: '',
                Sanitation: '',
                UserId: '',
                ElectricityMeter: '',
                AlternativeEnergySource: '',
                WaterSource: '',
                BriefDescription: '',
                PicturePath: '',
                LandDonorName: '',
                PrayerCapacity: '',
                MosqueDefinition: '',
            },

            queryParams: {
                PageNumber: this.pageNo,
                PageSize: this.pageSize,  
                SearchTerm: this.SearchTerm
            },

            MosqueData: [],
            BuildingDetailData: [],

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
            mosqueDefinitions: {
                Friday: 'جمعة',
                Times: 'أوقات',
                CentralMosque: 'مسجد مركزي',
                PublicPrayerHall: 'قاعة صلاة عامة',
            },
            mosqueClassifications: {
                General: 'عام',
                National: 'وطني',
            },

            BuildingCategories: {
                'Facility': 'منشأة',  // Key must match enum name
                'Endowment': 'وقف'
            }
        };
    },

    methods: {

        async GetAcademicSpecializations() {
             this.GetAllAcademicSpecializations(this.ruleForm.AcademicLevelId)
        },

        GetInfo(pageNo) {
            this.MosqueData = []; // Reset the data

            this.pageNo = pageNo || 1; // Default to page 1 if pageNo is undefined
            this.queryParams.PageNumber = this.pageNo;
            this.queryParams.PageSize = this.pageSize;
            this.queryParams.SearchTerm = this.SearchTerm;

            this.$blockUI.Start(); // Start loading indicator

            this.$http.FilterMosque(this.queryParams)
                .then(response => {
                    if (!this.Search) {
                        this.$blockUI.Stop(); // Stop loading indicator
                    }

                    // Log the entire response for debugging
                    console.log('API Response: ', response);

                    // Access the 'items' property from the 'data' object
                    if (response.data && response.data.data && response.data.data.items && response.data.data.items.length > 0) {
                        this.MosqueData = response.data.data.items;
                        console.log('MosqueData: ', this.MosqueData);
                    } else {
                        console.warn('No mosque data found.');
                        this.MosqueData = []; // Reset data if no items are returned
                    }
                })
                .catch(error => {
                    if (!this.Search) {
                        this.$blockUI.Stop(); // Stop loading indicator
                    }
                    console.error('Error fetching mosque data: ', error);
                    this.MosqueData = []; // Reset data on error
                    this.pages = 0;
                });
        },

        //InfoDialog
        OpentInfoDialog(item) {
            console.log('item: ', item);
            console.log('SelectedItem: ', item.mosqueName);
            console.log('MosqueID: ', item.mosqueID);
            this.SelectedItem = item;
            //this.ruleForm.Id = item.id;
            //this.ruleForm.MosqueName = item.mosqueName;
            //this.ruleForm.FileNumber = item.fileNumber;
            //this.GetSubscriptionsInfo();
            this.state = 1;
        },

        GetBuildingDetails(pageNo) {
            this.BuildingDetailData = []; // Reset the data

            this.pageNo = pageNo || 1; // Default to page 1 if pageNo is undefined
            this.queryParams.PageNumber = this.pageNo;
            this.queryParams.PageSize = this.pageSize;
            this.queryParams.SearchTerm = this.SearchTerm;
            this.queryParams.BuildingId = this.SelectedItem.mosqueID;

            console.log('this.SelectedItem.Building: ', this.SelectedItem.mosqueID);

            this.$blockUI.Start(); // Start loading indicator

            this.$http.FilterBuildingDetail(this.queryParams)
                .then(response => {
                    if (!this.Search) {
                        this.$blockUI.Stop(); // Stop loading indicator
                    }

                    // Log the entire response for debugging
                    console.log('API Response BuildingDetailData: ', response);

                    // Access the 'items' property from the 'data' object
                    if (response.data && response.data.data && response.data.data.items && response.data.data.items.length > 0) {
                        this.BuildingDetailData = response.data.data.items;
                        console.log('BuildingDetailData: ', this.BuildingDetailData);
                    } else {
                        console.log('No BuildingDetail data found.');
                        this.BuildingDetailData = []; // Reset data if no items are returned
                    }
                })
                .catch(error => {
                    if (!this.Search) {
                        this.$blockUI.Stop(); // Stop loading indicator
                    }
                    console.error('Error fetching BuildingDetail data: ', error);
                    this.BuildingDetailData = []; // Reset data on error
                    this.pages = 0;
                });
        },

        ChangeTab(tabname) {
            if (tabname == 'Subscriptions') {
                this.GetSubscriptionsInfo();
            } else if (tabname == 'Wallet') {
                this.GetWalletInfo();
            } else if (tabname == 'ProfileYears') {
                this.GetProfileYearsInfo();
            } else if (tabname == 'buildingDetailRuleForm') {
                this.GetBuildingDetails(1);
            }
        },

        buildingDetailSubmitForm(formName) {
            console.log('this.SelectedItem.mosqueID: ', this.SelectedItem.mosqueID);
            this.buildingDetailRuleForm.MosqueID = this.SelectedItem.mosqueID;
            console.log('BuildingDetailSubmitForm: ', this.buildingDetailRuleForm.WithinMosqueArea);
            //console.log('BuildingDetailSubmitForm: ', this.buildingDetailRuleForm.BuildingCategory);
            console.log('BuildingDetailSubmitForm: ', this.buildingDetailRuleForm.MosqueID);
            this.buildingDetailRuleForm.MosqueID = this.SelectedItem?.mosqueID || '';

  
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.CreateBuildingDetail(this.buildingDetailRuleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            //this.GetProfileYearsInfo();
                            this.ClearForm(this.buildingDetailRuleForm);
                            //this.$helper.DestroyElemntById('SubscriptionsValueClose');

                            //console.log('response.message: ', response.message);
                            //console.log('response.data  : ', response.data);
                            //console.log('response.data.message : ', response.data.message);

                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.message);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response && err.response.data) {
                                this.$helper.ShowMessage('error', 'خطأ بعملية الإظافة', err);
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

        formatDate(date) {
                if (!date) return '';
                return new Date(date).toLocaleDateString(); // Adjust the format as needed
        },

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(0);
                    this.$blockUI.Start();
                    this.$http.CreateMosque(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                             this.GetInfo();

                            this.ClearForm(this.ruleForm);
                            this.$helper.DestroyElemntById('Close');
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.message);
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
  
