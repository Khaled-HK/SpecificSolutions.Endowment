import Vue from 'vue';
import VueI18n from 'vue-i18n'
import VueRouter from 'vue-router';
import ElementUI from 'element-ui';
import Vuetify from 'vuetify'
import 'element-ui/lib/theme-chalk/index.css';
import { BootstrapVue } from 'bootstrap-vue'; 

//import UploaderPlugin  from '@syncfusion/ej2-vue-inputs';
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';

import App from './App.vue';
import Layout from './components/Layout/Layout.vue';
import Login from './components/Login/Login.vue';
import Home from './components/Home/Home.vue';
import DataService from './Shared/DataService';
import Helper from './Shared/Helper';

import VueEllipseProgress from 'vue-ellipse-progress';

//Schools
import Schools from './components/Schools/Schools.vue';
import SchoolsSuspend from './components/Schools/Suspend/Suspend.vue';


//Classes
import Classes from './components/Classes/Classes.vue';

//Course 
import Courses from './components/Courses/Courses.vue';
import ClosedCourses from './components/Courses/ClosedCourses/ClosedCourses.vue';
import CoursesDashboard from './components/Courses/CoursesDashboard/CoursesDashboard.vue';

//Students
import Students from './components/Students/Students.vue';
import StudentsSuspend from './components/Students/Suspend/Suspend.vue';
import StudentsChangeRequest from './components/Students/ChangeRequest/ChangeRequest.vue';
import StudentsRegersterRequest from './components/Students/RegersterRequest/RegersterRequest.vue';
import StudentsDashboard from './components/Students/Dashboard/Dashboard.vue';

//Instructors
import Instructors from './components/Instructors/Instructors.vue';
import InstructorsSuspend from './components/Instructors/Suspend/Suspend.vue';

//Products
import Products from './components/Products/Products.vue';

//Financial
//import Instructors from './components/Instructors/Instructors.vue';
import FinancialSubscriptions from './components/Financial/Subscriptions/Subscriptions.vue';
import FinancialRecharge from './components/Financial/Recharge/Recharge.vue';
import FinancialTracker from './components/Financial/Tracker/Tracker.vue';

//VoucherCards
import VoucherCards from './components/VoucherCards/VoucherCards.vue';
import VoucherCardsDashboard from './components/VoucherCards/Dashboard/Dashboard.vue';
import VoucherCardsDistributors from './components/VoucherCards/Distributors/Distributors.vue';
import VoucherCardsCards from './components/VoucherCards/Cards/Cards.vue';
import VoucherCardsTryAttemp from './components/VoucherCards/TryAttemp/TryAttemp.vue';

//Dictionaries
import SubscriptionsType from './components/Dictionaries/SubscriptionsType/SubscriptionsType.vue';
import AcademicSpecializations from './components/Dictionaries/AcademicSpecializations/AcademicSpecializations.vue';
import PaymentMethods from './components/Dictionaries/PaymentMethods/PaymentMethods.vue';
import Subjects from './components/Dictionaries/Subjects/Subjects.vue';
import Faq from './components/Dictionaries/Faq/Faq.vue';


//Users
import Users from './components/Users/Users.vue';
import UsersProfile from './components/Users/Profile/Profile.vue';

//Chat
import Chat from './components/Chat/Chat.vue';


Vue.use(VueEllipseProgress);

Vue.use(Vuetify)
Vue.use(VueI18n);
Vue.use(VueRouter);
//Vue.use(UploaderPlugin);
Vue.use(ElementUI, { locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;
Vue.prototype.$helper = Helper;
Vue.use(BootstrapVue);

export const eventBus = new Vue();

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        {
            path: '/',
            component: Login,
        },
         {
            path: '/',
            component: App,
            children: [
                {
                    path: '',
                    component: Layout,
                    children: [
                        { path: '/Home', component: Home },


                        //Schools
                        { path: 'Schools', component: Schools },
                        { path: 'SchoolsSuspend', component: SchoolsSuspend },

                        //Classes
                        { path: 'Classes', component: Classes },

                        //Courses Component 
                        { path: 'Courses', component: Courses },
                        { path: 'ClosedCourses', component: ClosedCourses },
                        { path: 'CoursesDashboard', component: CoursesDashboard },

                        //Students
                        { path: 'Students', component: Students },
                        { path: 'StudentsSuspend', component: StudentsSuspend },
                        { path: 'StudentsChangeRequest', component: StudentsChangeRequest },
                        { path: 'StudentsRegersterRequest', component: StudentsRegersterRequest },
                        { path: 'StudentsDashboard', component: StudentsDashboard },

                        //Instructors
                        { path: 'Instructors', component: Instructors },
                        { path: 'InstructorsSuspend', component: InstructorsSuspend },

                        //Products
                        { path: 'Products', component: Products },

                        //Financial
                        //{ path: 'Instructors', component: Instructors },
                        { path: 'FinancialSubscriptions', component: FinancialSubscriptions },
                        { path: 'FinancialRecharge', component: FinancialRecharge },
                        { path: 'FinancialTracker', component: FinancialTracker },

                        //VoucherCards
                        { path: 'VoucherCards', component: VoucherCards },
                        { path: 'VoucherCardsDashboard', component: VoucherCardsDashboard },
                        { path: 'VoucherCardsDistributors', component: VoucherCardsDistributors },
                        { path: 'VoucherCardsCards', component: VoucherCardsCards },
                        { path: 'VoucherCardsTryAttemp', component: VoucherCardsTryAttemp },

                        //Dictionaries
                        { path: 'SubscriptionsType', component: SubscriptionsType },
                        { path: 'AcademicSpecializations', component: AcademicSpecializations },
                        { path: 'PaymentMethods', component: PaymentMethods },
                        { path: 'Subjects', component: Subjects },
                        { path: 'Faq', component: Faq },

                        //Users
                        { path: 'Users', component: Users },
                        { path: 'UsersProfile', component: UsersProfile },


                        //Chat
                        { path: 'Chat', component: Chat },
                       
                       
                    ]
                },
            ],
        }
    ]
});


Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    router,
    render: h => {
        return h(App);
    }
}).$mount('#cpanel-management');
