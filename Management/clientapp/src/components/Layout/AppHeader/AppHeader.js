import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'AppHeader',
    mixins: [HelperMixin],
    async created() {

        await this.CheckLoginStatus();
        if (this.loginDetails) {
            //if (this.loginDetails.userType != 1
            //    && this.loginDetails.userType != 3)
            //    this.logout();

        }
    },
    data() {
        return {  
            Info: [],
            TempInfo: [],
            Count: 0,
        };
    },
  
    methods: {

        //href(url) {
        //    this.$router.push(url);
        //},
        href(url, id) {

            if (this.$route.path === url) 
                return; // Do nothing if the current URL matches the target URL

            this.$router.push(url);
            for (var i = 0; i < 20; i++) {
                if (i == id) {
                    this.$set(this.menuFlag, id, '');
                } else {
                    this.$set(this.menuFlag, i, '');
                }
            }
        },

    }    
}
