import ReportsDoughnutChart from "../Charts/ReportsDoughnutChart.vue";
import ThinBarChart from "../Charts/ThinBarChart.vue";
import SupportTracker from '../Charts/SupportTracker.vue';
import HelperMixin from '../../Shared/HelperMixin.vue'
export default {
    name: 'home',
    mixins: [HelperMixin],
    components: {
        ReportsDoughnutChart,
        ThinBarChart,
        SupportTracker,
    },

    async created() {
        window.scrollTo(0, 0);
        await this.CheckLoginStatus();
        this.GetInfo();
    },
    data() {
        return {

            Info: [],
        };
    },
    methods: {

        GetInfo() {
            this.$blockUI.Start();
            this.$http.GetDashboardInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

    }    
}
