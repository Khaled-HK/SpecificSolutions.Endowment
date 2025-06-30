import HelperMixin from '../../Shared/HelperMixin.vue'

export default {
    name: 'Login',
    mixins: [HelperMixin],
    components: {
    },
    created() {
        this.$blockUI.$loading = this.$loading;
        this.logout();
    },
    data() {
        return {
            isAuthenticated: false,
            isActive: false,
            form: {
                Password: null,
                Email: null
            }
        };
    },

    mounted() {
        const loadScript = (src) => {
            document.body.appendChild(
                Object.assign(document.createElement('script'), { src: src, async: true })
            );
        };

        loadScript('./assets/vendor/libs/jquery/jquery.js');
        loadScript('./assets/vendor/libs/popper/popper.js');
        loadScript('./assets/vendor/js/bootstrap.js');
        loadScript('./assets/vendor/libs/node-waves/node-waves.js');
        loadScript('./assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js');
        loadScript('./assets/vendor/libs/hammer/hammer.js');
        loadScript('./assets/vendor/libs/i18n/i18n.js');
        loadScript('./assets/vendor/libs/typeahead-js/typeahead.js');
        loadScript('./assets/vendor/js/menu.js');
        loadScript('./assets/vendor/libs/@form-validation/popular.js');
        loadScript('./assets/vendor/libs/datatables-bs5/datatables-bootstrap5.js');
        loadScript('./assets/js/main.js');
        loadScript('./assets/vendor/libs/@form-validation/auto-focus.js');
        loadScript('./assets/vendor/libs/sweetalert2/sweetalert2.js');
        loadScript('./assets/js/pages-auth.js');
        //loadScript('./assets/vendor/libs/block-ui/block-ui.js');
    },

    methods: {

        GoToPlatform() {
            window.location.href = 'https://platform.test.traneem.ly/';
            //window.href('https://platform.test.traneem.ly/');
        },

        logout() {

            localStorage.removeItem('currentUser-client');
            document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
            this.$blockUI.Start();
            this.$http.Logout()
                .then(() => {
                    this.$blockUI.Stop();
                    //  window.location.href = "/Login";
                })
                .catch((err) => {
                    this.$blockUI.Stop(err);
                    //console.error(err);
                });
        },

        login() {

            if (!this.form.Email) {
                this.$helper.ShowMessage('error', 'عملية ناجحة', '<strong>' + 'الرجاء إدخال البريد الإلكتروني' + '</strong>');
               
            }
            if (!this.form.Password) {
                this.$helper.ShowMessage('error', 'عملية ناجحة', '<strong>' + 'الرجاء إدخال الرقم السري' + '</strong>');
            }

            //this.Loading = true;
            this.$blockUI.Start();
            this.$http.login(this.form)
                .then(response => {
                    //debugger;
                    this.$blockUI.Stop();
                    console.log(response.data);
                    localStorage.setItem('currentUser-client', this.encrypt(JSON.stringify(response.data), this.PlatFormPass));
                    window.location.href = '/Home';
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('error', 'خطأ بعملية الاضافة', err.response.data);
                  
                });
        }
    }
}
