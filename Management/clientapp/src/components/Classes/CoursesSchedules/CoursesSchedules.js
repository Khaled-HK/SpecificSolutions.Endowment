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

            ScreenTitle: '  الجدول الدراسي  ',
            ScreenTitleSingle: '  جدول دراسي ',
            ScreenTitleSingleAl: '  الجدول الدراسي  ',

            Info: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem:'',

            ruleForm: {
                Id: '',
                ClasseId: this.$parent.SelectedItem.id,
                CoursesId: '',
                Day: '',
                Number: '',
            },
            rules: {
                CoursesId: this.$helper.Required(),
                Day: this.$helper.Required(),
                Number: this.$helper.Required(),
            },


            schedule: Array(6).fill(null).map((_, index) => ({
                Day: ['الاحد', 'الاتنين', 'التلاتاء', 'الاربعاء', 'الخميس', 'السبت'][index],
                Courses: Array(7).fill(null)    
            })),

        };
    },

    mounted() {
        this.GetInfo();
    },


    methods: {

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (!this.Search)
                this.$blockUI.Start();
            this.$http.GetCoursesSchedules(this.pageNo, this.pageSize, this.Search, this.$parent.SelectedItem.id)
                .then(response => {
                    if (!this.Search)
                        this.$blockUI.Stop();

                    this.Info = response.data.info;
                    this.pages = response.data.count;
                    this.Statistics = response.data.statistics;
                    this.organizeByDay(response.data.info);
                })
                .catch(() => {
                    if (!this.Search)
                        this.$blockUI.Stop();
                    this.pages = 0;
                });
        },


       

        organizeByDay(data) {
            // Reset schedule
            this.schedule.forEach(day => day.Courses.fill(null));

            data.forEach(item => {
                // Adjust the dayIndex since your days start from 1 (Sunday)
                const dayIndex = item.day - 1; // Convert to 0-based index (0 = Sunday)
                const lectureIndex = item.number - 1; // Convert to 0-based index (1-7 to 0-6)

                if (dayIndex >= 0 && dayIndex < 7 && lectureIndex >= 0 && lectureIndex < 7) {
                    this.schedule[dayIndex].Courses[lectureIndex] = {
                        Name: item.name,
                        Id: item.coursesId,
                        Status: item.status,
                        CreatedBy: item.createdBy,
                        CreatedOn: item.createdOn
                    };
                }
            });
        },

        getDayName(dayIndex) {
            const dayNames = ['الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'];
            return dayNames[dayIndex] || 'غير معروف';
        },

        getCourse(day, lectureNumber) {
            const course = day.Courses[lectureNumber - 1];
            return course ? course.Name : ''; // Return course name or empty string
        },








        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(0);
                    this.ruleForm.Day = Number(this.ruleForm.Day);
                    this.ruleForm.Number = Number(this.ruleForm.Number);
                    this.$blockUI.Start();
                    this.$http.AddCoursesSchedules(this.ruleForm)
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
            this.ruleForm.CoursesId = item.coursesId;
            this.ruleForm.Day = item.day;
            this.ruleForm.Number = item.number;
            this.state = 2;
            
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.ruleForm.Day = Number(this.ruleForm.Day);
                    this.ruleForm.Number = Number(this.ruleForm.Number);
                    this.ruleForm.ClasseId = this.$parent.SelectedItem.id;
                    this.$blockUI.Start();
                    this.$http.EditCoursesSchedules(this.ruleForm)
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
                    this.$http.ChangeStatusCoursesSchedules(Id)
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
                    this.$http.DeleteCoursesSchedules(Id)
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



        Back() {
            this.state = 0;
            this.ClearForm(this.ruleForm);
            this.SelectedItem = '';
            this.GetInfo(this.pageNo);
        },

    }
}
