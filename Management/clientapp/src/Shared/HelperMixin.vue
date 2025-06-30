<script>
    import blockUI from './BlockUIService.js';
    import moment from 'moment';
    import DataService from './DataService.js';
    import CryptoJS from 'crypto-js';
    export default {

        data() {
            return {
                //SearchTerm: '',

                queryParams: {
                    PageNumber: this.pageNo,
                    PageSize: this.pageSize,
                    SearchTerm: this.SearchTerm
                },
                Offices: [],
                Regions: [],

                loginDetails: '',
 
                SubscriptionsType: [],
                Schools: [],
                SchoolsProfileYears: [],
                AcademicSpecializations: [],
                Subjects: [],
                Teachers: [],
                PaymentsMethods: [],

                Shapters: [],
                Courses: [],
                Students: [],
                StudentsByClass: [],
                Users: [],
                Distributors: [],


                //Cities: [],
                //Municipalities: [],
                //Locations: [],
                
                //PaymentsMethods: [],
                //Courses: [],
                //FromCourses: [],
                //ToCourses: [],


                //Distributors: [],

                /*Publeish*/
                //ServerUrl: 'https://dashboard.platform.traneem.ly',

                /*Local*/
                ServerUrl: 'http://localhost:5000',
                Facebock: 'https://www.facebook.com/p/Traneem-100064940462078/?locale=ar_AR',
                Instagram: 'https://www.instagram.com/traneem5__/',
                TraneemPhone: '+218 94 457 81 48',
                TraneemEmail: 'info@traneem.ly',
                PlatFormPass: 'Traneem!@Platformv1',


                WebSiteName: 'الهيئة العامة للأوقاف والشؤون الإسلامية',
                BuildFor: 'Evo Space',
                logoPath: '/assets/img/1/2.png',
                CoverPath: '/assets/img/illustrations/auth-register-illustration-dark.png',
                PdfPath: '/assets/img/icons/misc/pdf1.png',

            }
        },
        methods: {

            encrypt: function encrypt(data, SECRET_KEY) {
                var dataSet = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY);
                dataSet = dataSet.toString();
                return dataSet;
            },
            decrypt: function decrypt(data, SECRET_KEY) {
                data = CryptoJS.AES.decrypt(data, SECRET_KEY);
                data = JSON.parse(data.toString(CryptoJS.enc.Utf8));
                return data;
            },

            ClearForm(ruleForm) {
                for (const key in ruleForm) {
                    ruleForm[key] = ''; // Reset to an empty string
                }
            },

            async CheckLoginStatus() {
                try {
                    this.loginDetails = JSON.parse(this.decrypt(localStorage.getItem('currentUser-client'), this.PlatFormPass));
                    const res = await DataService.IsLoggedin();
                    if (!res.data)
                        this.logout();
                } catch (error) {
                    this.logout();
                }
            },

            async logout() {
                localStorage.removeItem('currentUser-client');
                localStorage.clear();
                document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
                
                this.$http.Logout()
                    .then(() => {
                        window.location.href = "/";
                    })

                window.location.href = "/";
            },

            async GetOffices(queryParams) {
                this.Offices = [],
                    blockUI.Start();
                try {
  
                    const res = await DataService.GetOffices(queryParams);

                    if (res.data && res.data.data && res.data.data.items) {
                        this.Offices = res.data.data;
                        console.warn('Unexpected response structure:', this.Offices);
                    } else {
                        console.warn('Unexpected res ', res);
                        console.warn('Unexpected res.data ', res.data);
                        console.warn('Unexpected  res.data.data ', res.data.data);
                    }
                    this.Offices = res.data.data;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
            
            async GetRegions(queryParams) {
                this.Regions = [],
                    blockUI.Start();
                try {
  
                    const res = await DataService.GetRegions(queryParams);

                    if (res.data && res.data.data && res.data.data.items) {
                        this.Regions = res.data.data;
                        console.warn('Unexpected response structure:', this.Regions);
                    } else {
                        console.warn('Unexpected res ', res);
                        console.warn('Unexpected res.data ', res.data);
                        console.warn('Unexpected  res.data.data ', res.data.data);
                    }
                    this.Regions = res.data.data;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            formatNumber(num) {
                if (num) {
                    if (num <= 0) {
                        return 0;
                    } else {
                        return new Intl.NumberFormat('en-US').format(num); // Format the number
                    }
                } else {
                    return 0;
                }
            },

            moment(date) {
                if (date === null) {
                    return "------";
                }
                // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
                return moment(date).format('DD/MM/YYYY');
            },

            momentTime (date) {
                if (date === null) {
                    return "------";
                }
                // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
                return moment(date).format('DD/MM/YYYY ||  h:mm a');
            },

            ChangeDate(date) {
                    if (date === null) {
                        return "فارغ";
                    }
                    return moment(date).format("YYYY-MM-DD");
            },
             
            //Get Dictionaries
            async GetAllSubscriptionsType() {
                this.SubscriptionsType = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetAllSubscriptionsType();
                    this.SubscriptionsType = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllSchools() {
                this.Schools = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetAllSchools();
                    this.Schools = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllSchoolsProfileYears(Id) {
                this.SchoolsProfileYears = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetSchoolsProfileYears(Id);
                    this.SchoolsProfileYears = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllAcademicSpecializations(Id) {
                this.AcademicSpecializations = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllAcademicSpecializations(Id);
                    this.AcademicSpecializations = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllSubjects() {
                this.Subjects = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllSubjects();
                    this.Subjects = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllTeachers() {
                this.Teachers = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllUsers();
                    this.Teachers = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllStudents() {
                this.Students = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllStudents();
                    this.Students = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllStudentsByClass(Id) {
                this.StudentsByClass = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllStudentsByClass(Id);
                    this.StudentsByClass = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllPaymentsMethods() {
                this.PaymentsMethods = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllPaymentsMethods();
                    this.PaymentsMethods = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetAllShapters(Id) {
                this.Shapters = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllShapters(Id);
                    this.Shapters = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
             
            async GetAllCourses(ClasseId) {
                this.Courses = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllClassesCourses(ClasseId);
                    this.Courses = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
             
            async GetAllUsers() {
                this.Users = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllUsers();
                    this.Users = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
             
            async GetAllDistributors() {
                this.Distributors = [],
                    blockUI.Start();
                try {
                    const res = await DataService.GetAllDistributors();
                    this.Distributors = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
           
        }
    }
</script>
