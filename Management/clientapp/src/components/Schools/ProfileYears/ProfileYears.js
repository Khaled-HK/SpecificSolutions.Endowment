﻿
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

            ScreenTitle: ' طلبات تغير الأجهزة  ',
            ScreenTitleSingle: ' طلب ',
            ScreenTitleSingleAl: 'الطلب ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',


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
            this.$http.GetChangeRequest(this.pageNo, this.pageSize, this.Search)
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




        Accept(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية الموافقة على الطلب  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.AcceptChangeRequest(Id)
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

        Reject(Id) {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية رفض الطلب  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.RejectChangeRequest(Id)
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
                text: ' هـل انت متأكد من عملية حذف الطلب  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteChangeRequest(Id)
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

        AcceptAll() {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عمليةالموافقة على جميع الطلبات  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.AcceptAllChangeRequest()
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


        DeleteAll() {
            Swal.fire({
                title: 'تأكيد العملية ',
                text: ' هـل انت متأكد من عملية حذف جميع الطلبات  ؟',
                icon: 'question',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
                confirmButtonText: `تأكيد العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteAllChangeRequest()
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










        //Support Info
        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },

    }
}
