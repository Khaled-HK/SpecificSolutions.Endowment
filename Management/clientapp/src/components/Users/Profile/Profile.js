//import Swal from "sweetalert2";
import HelperMixin from '../../../Shared/HelperMixin.vue';
export default {
    name: 'Courses',
    mixins: [HelperMixin],

    async created() {
        await this.CheckLoginStatus();

        this.ruleForm.Name = this.loginDetails.name;
        this.ruleForm.LoginName = this.loginDetails.loginName;
        this.ruleForm.Email = this.loginDetails.email;
        this.ruleForm.Phone = this.loginDetails.phone;
    },

    data() {
        var validatePass = (rules, value, callback) => {
            if (value === '') {
                callback(new Error('الرجاء إدخال كلمة المرور'));
            } else {
                if (this.ruleForm.ConfimPassword !== '') {
                    this.$refs.ruleForm.validateField('ConfimPassword');
                }
                callback();
            }
        };
        var validatePass2 = (rrulesule, value, callback) => {
            if (value === '') {
                callback(new Error('الرجاء كتابه اعاده كلمه المرور'));
            } else if (value !== this.ruleForm.Password) {
                callback(new Error('الرجاء التأكد من تطابق كلمة المرور'));
            } else {
                callback();
            }
        };
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            pageNoT: 1,
            pageSizeT: 10,
            pagesT: 0,
            state: 0,
            Search: '',

            ScreenTitle: '  المستخدمين ',
            ScreenTitleSingle: '  مستخدم   ',
            ScreenTitleSingleAl: ' المستخدم  ',

            Info: [],
            InfoT: [],
            Statistics: [],

            AddDialog: false,

            SelectedItem: '',

            ruleForm: {
                Id: '',
                Name: '',
                LoginName: '',
                Email: '',
                Phone: '',
                Image: '',
                ImageName: '',
                Password: '',
                ConfirmPassword: '',
                NewPassword: '',
            },
            rules: {
                Name: this.$helper.Required(),
                Phone: this.$helper.Phone(),
                Email: this.$helper.EmailOnly(),
                LoginName: [
                    { required: true, message: 'الرجاء إدخال اسم الدخول', trigger: 'blur' },
                    { required: true, pattern: /^[A-Za-z]{0,40}$/, message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه', trigger: 'blur' }
                ],
            },


            PasswordruleForm: {
                Id: '',
                Password: '',
                ConfirmPassword: '',
                NewPassword: '',
            },
            Passwordrules: {
                Password: [
                    { validator: validatePass, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: '  تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
                ConfirmPassword: [
                    { validator: validatePass2, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: ' تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
                NewPassword: [
                    { validator: validatePass2, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: ' تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
            },

            PoicruleForm: {
                Id: '',
                Image: '',
                ImageName: '',
            },
            Picrules: {
                Image: this.$helper.Required(),
                ImageName: this.$helper.Required(),
                
            },
        };
    },



    methods: {

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(0);
                    this.$blockUI.Start();
                    this.$http.ChangeProfileInfo(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.logout();
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

        submitChangePasswordForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);

                    if (this.ruleForm.NewPassword != this.ruleForm.ConfirmPassword)
                        this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', 'الرجاء التأكد من تطابق كلمة المرور');

                    this.$blockUI.Start();
                    this.$http.ChangeProfilePassword(this.PasswordruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
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
                $this.PoicruleForm.ImageName = file.name;
                $this.PoicruleForm.Image = reader.result;


                $this.$blockUI.Start();
                $this.$http.ChangeProfilePicture($this.PoicruleForm)
                    .then((response) => {
                        $this.$blockUI.Stop();
                        $this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                        $this.logout();
                    })
                    .catch((err) => {
                        $this.$blockUI.Stop();
                        if (err.response && err.response.data) {
                            $this.$helper.ShowMessage('error', 'خطأ بعملية تغير الصورة', err.response.data);
                        } else {
                            $this.$helper.ShowMessage('error', 'خطأ بعملية تغير الصورة ', 'حدت خطاء غير متوقع');
                        }
                    });
                return;


            };
        },


    }
}
