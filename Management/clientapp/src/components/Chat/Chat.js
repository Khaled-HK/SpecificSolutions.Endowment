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

        this.GetInfo();
    },

    computed: {
        totalPages() {
            return Math.ceil(this.pages / this.pageSize);
        }
    },

    filters: {
       
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,
            Search: '',

            ScreenTitle: '  المواد الدراسية  ',
            ScreenTitleSingle: '  مادة دراسية  ',
            ScreenTitleSingleAl: ' المادة الدراسية ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem:'',

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

    mounted() {
        this.scrollToBottom(); // Scroll to bottom when component is mounted
    },
    updated() {
        this.scrollToBottom(); // Scroll to bottom when messages update
    },


    methods: {

        scrollToBottom() {
            const chatHistory = this.$refs.chatHistory;
            chatHistory.scrollTop = chatHistory.scrollHeight; // Scroll to the bottom
        },

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetSubjects(this.pageNo, this.pageSize, this.Search)
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
                    this.$http.AddSubjects(this.ruleForm)
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
            
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.$blockUI.Start();
                    this.$http.EditSubjects(this.ruleForm)
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
                    this.$http.ChangeStatusSubjects(Id)
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
                    this.$http.DeleteSubjects(Id)
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
        }


    }
}
