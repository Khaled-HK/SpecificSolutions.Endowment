////import Swal from "sweetalert2";
import moment from "moment";
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../../../Shared/HelperMixin.vue'

export default {
    name: "Add",
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatus();
        if (this.loginDetails.userType != 50)
            this.logout();
    },
    components: {
        flatPickr,
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format("MMMM Do YYYY");
        },
    },
    data() {
        return {
            Offices: [],

            AddButton:false,
            ruleForm: {
                Id: 0,
                FileSecretKey: '',
                SerialNumber: '',
                fileBase64: '',

            },
            rules: {
                FileSecretKey: this.$helper.RequiredInput(' رقم الملف السري'),
                SerialNumber: this.$helper.RequiredInput('  الرقم التسلسلي للباقة  '),
            },

        };
    },

    methods: {


        SelectCoverAttachment(file) {
            let str = file.raw.type;
            str = str.substring(0, 5);
            if (str != "text/csv" && str != "text/") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
            }else {
                var $this = this;
                var reader = new FileReader();
                reader.readAsDataURL(file.raw);
                reader.onload = function () {
                    $this.ruleForm.fileBase64 = reader.result;
                };

                this.AddButton = true;

                //this.ImageruleForm.Id = this.selectedItem.id;
                //this.$http.ActivePackgeDistributors(this.ruleForm)
                //    .then(response => {
                //        this.$blockUI.Stop();
                //        this.$helper.ShowMessage('success', 'عملية ناجحة', 'تمت عملية التشفير بنجاح');
                //        const url = window.URL.createObjectURL(new Blob([response.data]));
                //        const fileName = response.headers['content-disposition'].split(';')[1].trim().split('=')[1];
                //        const link = document.createElement('a');
                //        link.href = url;
                //        link.setAttribute('download', fileName);
                //        document.body.appendChild(link);
                //        link.click();
                //        document.body.removeChild(link);
                //    })
                //    .catch((err) => {
                //        this.$blockUI.Stop();
                //        this.$helper.ShowMessage('error', 'خطأ بالعملية', err.response.data);
                //    });

            }

            

        },



        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    if (!this.ruleForm.fileBase64) {
                        this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من إختيار  الملف');
                    } else {

                        this.$blockUI.Start();
                        this.$http.ActivePackgeDistributors(this.ruleForm)
                            .then(response => {
                                //this.resetForm(formName);
                                //this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                                //this.$blockUI.Stop();
                                this.$helper.ShowMessage('success', 'عملية ناجحة', 'تمت عملية فك التشفير بنجاح');
                                const url = window.URL.createObjectURL(new Blob([response.data]));
                                const fileName = response.headers['content-disposition'].split(';')[1].trim().split('=')[1];
                                const link = document.createElement('a');
                                link.href = url;
                                link.setAttribute('download', fileName);
                                document.body.appendChild(link);
                                link.click();
                                document.body.removeChild(link);
                                this.$blockUI.Stop();
                            })
                            .catch((err) => {
                                this.$blockUI.Stop();
                                this.$helper.ShowMessage('error', 'خطأ بعملية فك التشفير', err.response.data);
                            });
                    }

                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        resetForm(formName) {
            this.$refs[formName].resetFields();
        },
    },
};
