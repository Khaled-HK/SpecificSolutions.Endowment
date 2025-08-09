
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'appHeader',
    mixins: [HelperMixin],
    async created() {

        const loginDetails = await this.CheckLoginStatus();
        if (loginDetails) {
            //if (this.loginDetails.userType != 50
            //    && this.loginDetails.userType != 40
            //    && this.loginDetails.userType != 1
            //    && this.loginDetails.userType != 2
            //    && this.loginDetails.userType != 3)
            //    this.logout();

            if (this.loginDetails.userType == 1 || this.loginDetails.userType == 3) {
                setInterval(() => {
                    this.GetChangeOfPathRequestCount();
                }, 5000);
            }
        }
       

        window.scrollTo(0, 0);

        
    },
    data() {
        return {      
            ChangeOfPathRequestCount: '',
            active: 1,
            menuFlag: [20],
            
        };
    }, 
  
    methods: {

        href(url) {
            if (url !== this.$router.currentRoute.path) {
                this.$router.push(url);
            }
        },

        GetChangeOfPathRequestCount() {
            const fn = this.$http.GetChangeOfPathRequestCount || this.$http.GetChangeRequestCount;
            if (!fn) return;
            fn()
                .then(response => {
                    const d = response && response.data ? response.data : {};
                    this.ChangeOfPathRequestCount = d.changeOfPathRequestCount ?? d.changeRequestCount ?? '';
                })
        },

        OpenMenuByToggle(code) {

            if (document.getElementById(code).parentElement.classList.contains('open')) {
                document.getElementById(code).parentElement.classList.remove('menu-item-animating');
                document.getElementById(code).parentElement.classList.remove('open');

            } else {
                
                document.getElementById(code).parentElement.classList.add('menu-item-animating');
                document.getElementById(code).parentElement.classList.add('open');
            }
        },

       

       


    }    
}
