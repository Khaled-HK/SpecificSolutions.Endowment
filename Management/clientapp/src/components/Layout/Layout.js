import AppHeader from './AppHeader/AppHeader.vue';
import AppFooter from './AppFooter/AppFooter.vue';
import Dashboard from './Dashboard/Dashboard.vue';

export default {
    name: 'layout',   
    components: {
        'app-header': AppHeader,
        'app-footer': AppFooter,
        'app-dasboard': Dashboard,
       
    },
    created() {
        this.$blockUI.$loading = this.$loading;
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


        loadScript('./assets/vendor/libs/apex-charts/apexcharts.js');
        loadScript('./assets/vendor/libs/swiper/swiper.js');
        loadScript('./assets/vendor/libs/block-ui/block-ui.js');
        loadScript('./assets/vendor/libs/cleavejs/cleave.js');
        loadScript('./assets/vendor/libs/cleavejs/cleave-phone.js');
        loadScript('./assets/vendor/libs/select2/select2.js');
        loadScript('./assets/vendor/libs/bs-stepper/bs-stepper.js');
        loadScript('./assets/js/modal-create-app.js');
        loadScript('./assets/js/modal-add-new-cc.js');
        loadScript('./assets/js/app-chat.js');
        loadScript('./assets/vendor/libs/bootstrap-maxlength/bootstrap-maxlength.js');


        //Add Product 
        
        //loadScript('./assets/vendor/libs/quill/katex.js');
        //loadScript('./assets/vendor/libs/quill/quill.js');
        //loadScript('./assets/vendor/libs/dropzone/dropzone.js');
        loadScript('./assets/vendor/libs/jquery-repeater/jquery-repeater.js');
        loadScript('./assets/vendor/libs/flatpickr/flatpickr.js');
        loadScript('./assets/vendor/libs/tagify/tagify.js');
        loadScript('./assets/js/app-ecommerce-product-add.js');
        //loadScript('./assets/js/app-ecommerce-order-details.js');
        



        //loadScript('./assets/vendor/libs/moment/moment.js');
        //loadScript('./assets/js/app-invoice-list.js');
        //loadScript('./assets/js/dashboards-analytics.js');
    },
    
    data() {
        return {
        
        };
    },
    methods: {
       
    }
}
