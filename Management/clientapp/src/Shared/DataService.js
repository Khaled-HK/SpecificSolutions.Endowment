import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.cookie.split("=")[1];

//const baseUrl = 'http://localhost:4810/Api';

export default {




    //********************************************************************| Authintecations |***********************************************************

    login(bodyObjeect) {
        return axios.post(`/Security/login`, bodyObjeect);
    },

    IsLoggedin() {
        return axios.get(`/Security/IsLoggedin`);
    },

    Logout() {
        return axios.post(`/Security/Logout`);
    },



    //// ********************************| Mosque |********************************

    CreateMosque(command) {
        return axios.post(`api/Mosque`, command);
    },
     
    FilterMosque(queryParams) {
        return axios.get(`api/Mosque/filter`, {
            params: queryParams // Axios converts this to `?Name=...&City=...`
        });
    },
    
    //// ********************************| BuildingDetail |********************************

    CreateBuildingDetail(command) {
        return axios.post(`api/BuildingDetail`, command,{
            headers: {
                'Content-Type': 'application/json'
            }
        });
    },
     
    FilterBuildingDetail(queryParams) {
        return axios.get(`api/BuildingDetail/filter`, {
            params: queryParams // Axios converts this to `?Name=...&City=...`
        });
    },

    //// ********************************| Offices |********************************


    GetOffices(queryParams) {
        return axios.get(`api/Office/GetOffices`, {
            params: queryParams // Axios converts this to `?Name=...&City=...`
        });
    },
    
    //// ********************************| Regions |********************************
     
    GetRegions(queryParams) {
        return axios.get(`api/Region/GetRegions`, {
            params: queryParams // Axios converts this to `?Name=...&City=...`
        });
    },

    //// ********************************| Products |********************************

    getProducts(queryParams) {
        return axios.get(`api/Product/filter`, {
            params: queryParams // Axios converts this to `?PageNumber=...&PageSize=...&SearchTerm=...`
        });
    },

    createProduct(productData) {
        return axios.post(`api/Product`, productData);
    },

    updateProduct(id, productData) {
        return axios.put(`api/Product/${id}`, productData);
    },

    deleteProduct(id) {
        return axios.delete(`api/Product/${id}`);
    },

    getProductById(id) {
        return axios.get(`api/Product/${id}`);
    },

    //// ********************************| Schools |********************************


    GetSchools(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Schools/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },
   

    GetAllSchools() {
        return axios.get(`api/admin/Schools/GetAll`);
    },

    AddSchools(bodyObject) {
        return axios.post(`api/admin/Schools/Add`, bodyObject);
    },

    EditSchools(bodyObject) {
        return axios.post(`api/admin/Schools/Edit`, bodyObject);
    },

    ChangeStatusSchools(Id) {
        return axios.post(`api/admin/Schools/${Id}/ChangeStatus`);
    },

    ResetDeviceSchools(Id) {
        return axios.post(`api/admin/Schools/${Id}/ResetDevice`);
    },

    ResetPasswordSchools(Id) {
        return axios.post(`api/admin/Schools/${Id}/ResetPassword`);
    },

    DeleteSchools(Id) {
        return axios.post(`api/admin/Schools/${Id}/Delete`);
    },

    //Suspends
    SuspendsSchools(bodyObject) {
        return axios.post(`api/admin/Schools/Suspends/Add`, bodyObject);
    },

    GetSuspendsSchools(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Schools/Suspends/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    CanselSuspendsSchools(Id) {
        return axios.post(`api/admin/Schools/${Id}/Suspends/Cansel`);
    },



    //Studetns Subscriptions
    GetSchoolsSubscriptions(Id, Search) {
        return axios.get(`api/admin/Schools/Subscriptions/Get?Id=${Id}&Search=${Search}`);
    },

    AddSchoolsSubscriptions(bodyObject) {
        return axios.post(`api/admin/Schools/Subscriptions/Add`, bodyObject);
    },

    AddSchoolsSubscriptionsValue(bodyObject) {
        return axios.post(`api/admin/Schools/Subscriptions/AddValue`, bodyObject);
    },

    DropSchoolsSubscriptions(bodyObject) {
        return axios.post(`api/admin/Schools/Subscriptions/Drop`, bodyObject);
    },

     
    //Wallet
    GetSchoolsWallet(Id) {
        return axios.get(`api/admin/Schools/Wallet/Get?Id=${Id}`);
    },

    AddSchoolsWalletValue(bodyObject) {
        return axios.post(`api/admin/Schools/Wallet/Charge`, bodyObject);
    },




    //ProfileYears
    GetSchoolsProfileYears(Id) {
        return axios.get(`api/admin/Schools/ProfileYears/Get?Id=${Id}`);
    },

    AddProfileYears(bodyObject) {
        return axios.post(`api/admin/Schools/ProfileYears/Add`, bodyObject);
    },

    ActiveProfileYears(Id) {
        return axios.post(`api/admin/Schools/${Id}/ProfileYears/Active`);
    },

    CloseProfileYears(Id) {
        return axios.post(`api/admin/Schools/${Id}/ProfileYears/Close`);
    },




    //Chart
    GetAllSchoolsChartInfo() {
        return axios.get(`api/admin/Schools/Chart/GetAll`);
    },



    //// ********************************| End Of Schools |********************************





    //// ********************************| Classes |********************************


    GetClasses(pageNo, pageSize, Search, SchoolsId, ProfileYearsId) {
        return axios.get(`api/admin/Classes/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &SchoolsId=${SchoolsId}&ProfileYearsId=${ProfileYearsId}`);
    },

    AddClasses(bodyObject) {
        return axios.post(`api/admin/Classes/Add`, bodyObject);
    },

    EditClasses(bodyObject) {
        return axios.post(`api/admin/Classes/Edit`, bodyObject);
    },

    ChangeStatusClasses(Id) {
        return axios.post(`api/admin/Classes/${Id}/ChangeStatus`);
    },

    DeleteClasses(Id) {
        return axios.post(`api/admin/Classes/${Id}/Delete`);
    },




    //Courses
    GetClassesCourses(pageNo, pageSize, Search, ClasseId) {
        return axios.get(`api/admin/Classes/Courses/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &ClasseId=${ClasseId}`);
    },

    GetAllClassesCourses(ClasseId) {
        return axios.get(`api/admin/Classes/Courses/GetAll?ClasseId=${ClasseId}`);
    },

    AddClassesCourses(bodyObject) {
        return axios.post(`api/admin/Classes/Courses/Add`, bodyObject);
    },

    EditClassesCourses(bodyObject) {
        return axios.post(`api/admin/Classes/Courses/Edit`, bodyObject);
    },

    ChangeStatusClassesCourses(Id) {
        return axios.post(`api/admin/Classes/${Id}/Courses/ChangeStatus`);
    },

    DeleteClassesCourses(Id) {
        return axios.post(`api/admin/Classes/${Id}/Courses/Delete`);
    },


    //CoursesSchedules
    GetCoursesSchedules(pageNo, pageSize, Search, ClasseId) {
        return axios.get(`api/admin/Classes/CoursesSchedules/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &ClasseId=${ClasseId}`);
    },

    AddCoursesSchedules(bodyObject) {
        return axios.post(`api/admin/Classes/CoursesSchedules/Add`, bodyObject);
    },

    EditCoursesSchedules(bodyObject) {
        return axios.post(`api/admin/Classes/CoursesSchedules/Edit`, bodyObject);
    },

    ChangeStatusCoursesSchedules(Id) {
        return axios.post(`api/admin/Classes/${Id}/CoursesSchedules/ChangeStatus`);
    },

    DeleteCoursesSchedules(Id) {
        return axios.post(`api/admin/Classes/${Id}/CoursesSchedules/Delete`);
    },



    //Students
    GetClassesStudents(Search, ClasseId) {
        return axios.get(`api/admin/Classes/Students/Get?Search=${Search}&ClasseId=${ClasseId}`);
    },

    AddClassesStudents(bodyObject) {
        return axios.post(`api/admin/Classes/Students/Add`, bodyObject);
    },

    EditClassesStudents(bodyObject) {
        return axios.post(`api/admin/Classes/Students/Edit`, bodyObject);
    },

    DeleteClassesStudents(Id) {
        return axios.post(`api/admin/Classes/${Id}/Students/Delete`);
    },


    //Certificates
    GetClassesCertificates(pageNo, pageSize, Search, ClasseId) {
        return axios.get(`api/admin/Classes/Certificates/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}&ClasseId=${ClasseId}`);
    },

    AddClassesCertificates(bodyObject) {
        return axios.post(`api/admin/Classes/Certificates/Add`, bodyObject);
    },

    EditClassesCertificates(bodyObject) {
        return axios.post(`api/admin/Classes/Certificates/Edit`, bodyObject);
    },

    DeleteClassesCertificates(Id) {
        return axios.post(`api/admin/Classes/${Id}/Certificates/Delete`);
    },



    GetClassesEnrollments(pageNo, pageSize, Search, ClasseId,ByDate) {
        return axios.get(`api/admin/Classes/Enrollments/Get?pageno=${pageNo}
            &pagesize=${pageSize}&Search=${Search}&ClasseId=${ClasseId}&ByDate=${ByDate}`);
    },

    AddClassesEnrollments(bodyObject) {
        return axios.post(`api/admin/Classes/Enrollments/Add`, bodyObject);
    },

    AddClassesEnrollmentsList(bodyObject) {
        return axios.post(`api/admin/Classes/Enrollments/AddList`, bodyObject);
    },

    

    EditClassesEnrollments(bodyObject) {
        return axios.post(`api/admin/Classes/Enrollments/Edit`, bodyObject);
    },

    DeleteClassesEnrollments(Id) {
        return axios.post(`api/admin/Classes/${Id}/Enrollments/Delete`);
    },




    //CoursesExams
    GetCoursesExams(pageNo, pageSize, Search, ClasseId, CoursesId) {
        return axios.get(`api/admin/Classes/CoursesExams/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &ClasseId=${ClasseId}&CoursesId=${CoursesId}`);
    },

    AddCoursesExams(bodyObject) {
        return axios.post(`api/admin/Classes/CoursesExams/Add`, bodyObject);
    },

    EditCoursesExams(bodyObject) {
        return axios.post(`api/admin/Classes/CoursesExams/Edit`, bodyObject);
    },

    ChangeStatusCoursesExams(Id) {
        return axios.post(`api/admin/Classes/${Id}/CoursesExams/ChangeStatus`);
    },

    DeleteCoursesExams(Id) {
        return axios.post(`api/admin/Classes/${Id}/CoursesExams/Delete`);
    },


    //ExamsGrades
    GetExamsGrades(ExamId, Search) {
        return axios.get(`api/admin/Classes/ExamsGrades/Get?ExamId=${ExamId}&Search=${Search}`);
    },

    AddExamsGrades(bodyObject) {
        return axios.post(`api/admin/Classes/ExamsGrades/Add`, bodyObject);
    },

    EditExamsGrades(bodyObject) {
        return axios.post(`api/admin/Classes/ExamsGrades/Edit`, bodyObject);
    },

    ChangeStatusExamsGrades(Id) {
        return axios.post(`api/admin/Classes/${Id}/ExamsGrades/ChangeStatus`);
    },

    DeleteExamsGrades(Id) {
        return axios.post(`api/admin/Classes/${Id}/ExamsGrades/Delete`);
    },




    


    //// ********************************| End Of Classes |********************************






















    // ********************************| Start Dictionaries |******************************

    GetAllStudents() {
        return axios.get(`api/admin/Dictionaries/Students/GetAll`);
    },

    GetAllStudentsByClassForEnrollments(Id,EnrollmentDate) {
        return axios.get(`api/admin/Dictionaries/Students/GetAllForEnrollments?ClassesId=${Id}&EnrollmentDate=${EnrollmentDate}`);
    },

    GetAllStudentsByClassForDegree(Id) {
        return axios.get(`api/admin/Dictionaries/Students/GetAllStudentsForDegree?ExamId=${Id}`);
    },


    GetAllStudentsByClass(Id) {
        return axios.get(`api/admin/Dictionaries/Students/GetAllByClass?ClassesId=${Id}`);
    },



    // PaymentsMethods
    GetPaymentsMethods(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/PaymentsMethods/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllPaymentsMethods() {
        return axios.get(`api/admin/Dictionaries/PaymentsMethods/GetAll`);
    },

    AddPaymentsMethods(bodyObject) {
        return axios.post(`api/admin/Dictionaries/PaymentsMethods/Add`, bodyObject);
    },

    EditPaymentsMethods(bodyObject) {
        return axios.post(`api/admin/Dictionaries/PaymentsMethods/Edit`, bodyObject);
    },

    DeletePaymentsMethods(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/PaymentsMethods/Delete`);
    },

    ChangeStatusPaymentsMethods(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/PaymentsMethods/ChangeStatus`);
    },



    // Subjects
    GetSubjects(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/Subjects/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllSubjects() {
        return axios.get(`api/admin/Dictionaries/Subjects/GetAll`);
    },

    AddSubjects(bodyObject) {
        return axios.post(`api/admin/Dictionaries/Subjects/Add`, bodyObject);
    },

    EditSubjects(bodyObject) {
        return axios.post(`api/admin/Dictionaries/Subjects/Edit`, bodyObject);
    },

    DeleteSubjects(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/Subjects/Delete`);
    },

    ChangeStatusSubjects(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/Subjects/ChangeStatus`);
    },





    // SubscriptionsType
    GetSubscriptionsType(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/SubscriptionsType/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllSubscriptionsType() {
        return axios.get(`api/admin/Dictionaries/SubscriptionsType/GetAll`);
    },

    AddSubscriptionsType(bodyObject) {
        return axios.post(`api/admin/Dictionaries/SubscriptionsType/Add`, bodyObject);
    },

    EditSubscriptionsType(bodyObject) {
        return axios.post(`api/admin/Dictionaries/SubscriptionsType/Edit`, bodyObject);
    },

    DeleteSubscriptionsType(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/SubscriptionsType/Delete`);
    },

    ChangeStatusSubscriptionsType(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/SubscriptionsType/ChangeStatus`);
    },





    // AcademicLevels
    GetAcademicLevels(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/AcademicLevels/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllAcademicLevels() {
        return axios.get(`api/admin/Dictionaries/AcademicLevels/GetAll`);
    },

    AddAcademicLevels(bodyObject) {
        return axios.post(`api/admin/Dictionaries/AcademicLevels/Add`, bodyObject);
    },

    EditAcademicLevels(bodyObject) {
        return axios.post(`api/admin/Dictionaries/AcademicLevels/Edit`, bodyObject);
    },

    DeleteAcademicLevels(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/AcademicLevels/Delete`);
    },

    ChangeStatusAcademicLevels(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/AcademicLevels/ChangeStatus`);
    },




    // AcademicSpecializations
    GetAcademicSpecializations(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/AcademicSpecializations/Get?pageno=${pageNo}&pagesize=${pageSize}
        &Search=${Search}`);
    },

    GetAllAcademicSpecializations(Id) {
        return axios.get(`api/admin/Dictionaries/AcademicSpecializations/GetAll?AcademicLevelId=${Id}`);
    },

    AddAcademicSpecializations(bodyObject) {
        return axios.post(`api/admin/Dictionaries/AcademicSpecializations/Add`, bodyObject);
    },

    EditAcademicSpecializations(bodyObject) {
        return axios.post(`api/admin/Dictionaries/AcademicSpecializations/Edit`, bodyObject);
    },

    DeleteAcademicSpecializations(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/AcademicSpecializations/Delete`);
    },

    ChangeStatusAcademicSpecializations(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/AcademicSpecializations/ChangeStatus`);
    },








    // Faq
    GetFaq(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Dictionaries/Faq/Get?pageno=${pageNo}&pagesize=${pageSize}
        &Search=${Search}`);
    },

    DeleteFaq(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/Faq/Delete`);
    },

    ChangeStatusFaq(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/Faq/ChangeStatus`);
    },




    GetDashboardInfo() {
        return axios.get(`api/admin/Dictionaries/Dashboard/Get`);
    },



    // ********************************| End Dictionaries |********************************
























    // ********************************| Subscriptions |************************************

    GetSubscriptions(pageNo, pageSize, Search, IsFree, IsDiscount, SalesStatus, ViewStatus) {
        return axios.get(`api/admin/Subscriptions/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
            &IsFree=${IsFree}&IsDiscount=${IsDiscount}&SalesStatus=${SalesStatus}&Status=${ViewStatus}`);
    },

    GetAllSubscriptions() {
        return axios.get(`api/admin/Subscriptions/GetAll`);
    },

    GetAllSubscriptionsBySubscriptionsTypepecialization(SubjectId, AcademicSpecializationId) {
        return axios.get(`api/admin/Subscriptions/GetBySubscriptionsTypepecialization?SubjectId=${SubjectId}&AcademicSpecializationId=${AcademicSpecializationId}`);
    },

    AddSubscriptions(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Add`, bodyObject);
    },

    EditSubscriptions(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Edit`, bodyObject);
    },

    DeleteSubscriptions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Delete`);
    },

    ChangeStatusSubscriptions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/ChangeStatus`);
    },

    ChangeSalesStatusSubscriptions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/ChangeSalesStatus`);
    },

    CloseSubscriptions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Close`);
    },




    //Closed Course
    GetClosedSubscriptions(pageNo, pageSize, Search, IsFree, IsDiscount) {
        return axios.get(`api/admin/Subscriptions/GetClosed?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
            &IsFree=${IsFree}&IsDiscount=${IsDiscount}`);
    },

    OpenSubscriptions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Open`);
    },


    //ChartInfo
    GetAllSubscriptionsChartInfo() {
        return axios.get(`api/admin/Subscriptions/Chart/GetAll`);
    },


    //ChartInfo
    GetSubscriptionsChartInfo(Id) {
        return axios.get(`api/admin/Subscriptions/Chart/Get?Id=${Id}`);
    },


   





    //Shapters
    GetSubscriptionshapters(Id, Search) {
        return axios.get(`api/admin/Subscriptions/Shapters/Get?Id=${Id}&Search=${Search}`);
    },

    GetAllShapters(Id) {
        return axios.get(`api/admin/Subscriptions/Shapters/GetAll?CourseId=${Id}`);
    },

    AddShapters(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Shapters/Add`, bodyObject);
    },

    EditShapters(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Shapters/Edit`, bodyObject);
    },

    DeleteShapters(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Shapters/Delete`);
    },

    ChangeStatusShapters(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Shapters/ChangeStatus`);
    },






    //Lectures
    GetCourseLectures(CourseId,ShapterId, Search) {
        return axios.get(`api/admin/Subscriptions/Lectures/Get?CourseId=${CourseId}&ShapterId=${ShapterId}&Search=${Search}`);
    },

    AddLectures(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Lectures/Add`, bodyObject);
    },

    EditLectures(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Lectures/Edit`, bodyObject);
    },

    DeleteLectures(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Lectures/Delete`);
    },

    ChangeStatusLectures(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Lectures/ChangeStatus`);
    },






    //LecturesAttashments
    GetCourseLecturesAttashments(LectureId, Search) {
        return axios.get(`api/admin/Subscriptions/LecturesAttashments/Get?LectureId=${LectureId}&Search=${Search}`);
    },

    AddLecturesAttashments(bodyObject) {
        return axios.post(`api/admin/Subscriptions/LecturesAttashments/Add`, bodyObject);
    },

    EditLecturesAttashments(bodyObject) {
        return axios.post(`api/admin/Subscriptions/LecturesAttashments/Edit`, bodyObject);
    },

    DeleteLecturesAttashments(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/LecturesAttashments/Delete`);
    },

    ChangeStatusLecturesAttashments(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/LecturesAttashments/ChangeStatus`);
    },






    //Exams
    GetCourseExams(CourseId,ShapterId, Search) {
        return axios.get(`api/admin/Subscriptions/Exams/Get?CourseId=${CourseId}&ShapterId=${ShapterId}&Search=${Search}`);
    },

    AddExams(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Exams/Add`, bodyObject);
    },

    EditExams(bodyObject) {
        return axios.post(`api/admin/Subscriptions/Exams/Edit`, bodyObject);
    },

    DeleteExams(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Exams/Delete`);
    },

    ChangeStatusExams(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/Exams/ChangeStatus`);
    },






    //ExamsQuestions
    GetCourseExamsQuestions(ExamId, Search) {
        return axios.get(`api/admin/Subscriptions/ExamsQuestions/Get?ExamId=${ExamId}&Search=${Search}`);
    },

    AddExamsQuestions(bodyObject) {
        return axios.post(`api/admin/Subscriptions/ExamsQuestions/Add`, bodyObject);
    },

    EditExamsQuestions(bodyObject) {
        return axios.post(`api/admin/Subscriptions/ExamsQuestions/Edit`, bodyObject);
    },

    DeleteExamsQuestions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/ExamsQuestions/Delete`);
    },

    ChangeStatusExamsQuestions(Id) {
        return axios.post(`api/admin/Subscriptions/${Id}/ExamsQuestions/ChangeStatus`);
    },


    //Schools
    GetSubscriptionschools(Id, Search) {
        return axios.get(`api/admin/Subscriptions/Schools/Get?Id=${Id}&Search=${Search}`);
    },


    //Review
    GetCourseReview(Id, Search) {
        return axios.get(`api/admin/Subscriptions/Review/Get?Id=${Id}&Search=${Search}`);
    },





    //// ********************************| End Subscriptions |********************************


























    // ********************************| Instructors |********************************



    GetInstructors(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Instructors/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllInstructors() {
        return axios.get(`api/admin/Instructors/GetAll`);
    },

    AddInstructors(bodyObject) {
        return axios.post(`api/admin/Instructors/Add`, bodyObject);
    },

    EditInstructors(bodyObject) {
        return axios.post(`api/admin/Instructors/Edit`, bodyObject);
    },

    ChangeStatusInstructors(Id) {
        return axios.post(`api/admin/Instructors/${Id}/ChangeStatus`);
    },

    ResetDeviceInstructors(Id) {
        return axios.post(`api/admin/Instructors/${Id}/ResetDevice`);
    },

    ResetPasswordInstructors(Id) {
        return axios.post(`api/admin/Instructors/${Id}/ResetPassword`);
    },

    DeleteInstructors(Id) {
        return axios.post(`api/admin/Instructors/${Id}/Delete`);
    },

    //Suspends
    SuspendsInstructors(bodyObject) {
        return axios.post(`api/admin/Instructors/Suspends/Add`, bodyObject);
    },

    GetSuspendsInstructors(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Instructors/Suspends/Get?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    CanselInstructors(Id) {
        return axios.post(`api/admin/Instructors/${Id}/Suspends/Cansel`);
    },



    // ********************************| End Of Instructors |********************************












    // ********************************| Financial |********************************

    //GetSubscriptions(pageNo, pageSize, Search, From, To) {
    //    return axios.get(`api/admin/Financial/GetSubscriptions?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
    //    &From=${From}&To=${To}`);
    //},

    GetRecharge(pageNo, pageSize, Search, From, To, UserId, DistributorsId, PaymentMethodId) {
        return axios.get(`api/admin/Financial/GetRecharge?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &From=${From}&To=${To}&UserId=${UserId}&DistributorsId=${DistributorsId}&PaymentMethodId=${PaymentMethodId}`);
    },

    GetWalletTracker(pageNo, pageSize, Search, From, To, ProcessType, Channel) {
        return axios.get(`api/admin/Financial/GetWalletTracker?pageno=${pageNo}&pagesize=${pageSize}&Search=${Search}
        &From=${From}&To=${To}&ProcessType=${ProcessType}&Channel=${Channel}`);
    },





    // ********************************| End Of Financial |********************************














    // ********************************| VoucherCards |********************************

    GetVoucherCards(pageNo, pageSize, Search, DistributorsId) {
        return axios.get(`api/admin/VoucherCards/Get?pageNo=${pageNo}&pagesize=${pageSize}&Search=${Search}&DistributorsId=${DistributorsId}`);
    },

    AddVoucherCards(bodyObject) {
        return axios.post('api/admin/VoucherCards/Add', bodyObject);
    },

    SaleVoucherCards(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Sale`, { responseType: 'blob' });
    },

    ForzePackage(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Freeze`, { responseType: 'blob' });
    },

    DeletePackage(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Delete`);
    },



    //Card
    GetCards() {
        return axios.get(`api/admin/VoucherCards/Cards/Get`);
    },

    GetCardInfoBySerialNumber(SerialNumber) {
        return axios.get(`api/admin/VoucherCards/Cards/GetCardInfoBySerialNumber?SerialNumber=${SerialNumber}`);
    },

    GetCardInfoByVoucherNumber(VoucherNumber) {
        return axios.get(`api/admin/VoucherCards/Cards/GetCardInfoByVoucherNumber?SerialNumber=${VoucherNumber}`);
    },


    //TryAttemp
    GetVoucherCardsTryAttemp(pageNo, pageSize, Search) {
        return axios.get(`api/admin/VoucherCards/TryAttemp/Get?pageNo=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },



    //Chart
    GetVoucherCardsChart() {
        return axios.get(`api/admin/VoucherCards/Chart/Get`);
    },



    


    //Distributors
    GetDistributors(pageNo, pageSize, Search) {
        return axios.get(`api/admin/VoucherCards/Distributors/Get?pageNo=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetAllDistributors() {
        return axios.get(`api/admin/VoucherCards/Distributors/GetAll`);
    },

    AddDistributors(bodyObject) {
        return axios.post('api/admin/VoucherCards/Distributors/Add', bodyObject);
    },

    EditDistributors(bodyObject) {
        return axios.post('api/admin/VoucherCards/Distributors/Edit', bodyObject);
    },

    ResetPasswordDistributors(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Distributors/ResetPassword`);
    },

    ChangeStatusDistributors(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Distributors/ChangeStatus`);
    },

    DeleteDistributors(Id) {
        return axios.post(`api/admin/VoucherCards/${Id}/Distributors/Delete`);
    },




    ActivePackgeDistributors(bodyObject) {
        return axios.post('api/admin/VoucherCards/Distributors/ActivePackge', bodyObject);
    },

    GetDistributorsPackages(pageNo, pageSize) {
        return axios.get(`api/admin/VoucherCards/Distributors/Packages/Get?pageNo=${pageNo}&pagesize=${pageSize}`);
    },

    // ********************************| End Of VoucherCards |********************************












    // ********************************| Users |********************************
    GetUsers(pageNo, pageSize, Search) {
        return axios.get(`api/admin/Users/Get?pageNo=${pageNo}&pagesize=${pageSize}&Search=${Search}`);
    },

    GetUsersTransactions(pageNoT, pageSizeT, Id) {
        return axios.get(`api/admin/Users/GetTransactions?pageNo=${pageNoT}&pagesize=${pageSizeT}&Id=${Id}`);
    },

    GetAllUsers() {
        return axios.get(`api/admin/Users/GetAll`);
    },

    AddUsers(bodyObject) {
        return axios.post('api/admin/Users/Add', bodyObject);
    },

    EditUsers(bodyObject) {
        return axios.post('api/admin/Users/Edit', bodyObject);
    },

    ResetPasswordUsers(Id) {
        return axios.post(`api/admin/Users/${Id}/ResetPassword`);
    },

    ChangeStatusUsers(Id) {
        return axios.post(`api/admin/Users/${Id}/ChangeStatus`);
    },

    DeleteUsers(Id) {
        return axios.post(`api/admin/Users/${Id}/Delete`);
    },


    //USerProfile 
    ChangeProfilePicture(bodyObject) {
        return axios.post('api/admin/Users/Profile/ChangePicture', bodyObject);
    },

    ChangeProfileInfo(bodyObject) {
        return axios.post('api/admin/Users/Profile/ChangeInfo', bodyObject);
    },

    ChangeProfilePassword(bodyObject) {
        return axios.post('api/admin/Users/Profile/ChangePassword', bodyObject);
    },




    // ********************************| End Of Users |********************************




    
    
}