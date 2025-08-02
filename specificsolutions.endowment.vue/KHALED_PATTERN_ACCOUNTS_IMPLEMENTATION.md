# تنفيذ نمط خالد - صفحة الحسابات (Accounts)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة الحسابات (`accounts.vue`) مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الملفات المطلوبة

### 1. صفحة الحسابات
**الملف**: `src/pages/apps/accounts.vue`

**الميزات المطبقة**:
- ✅ إدارة CRUD كاملة (إنشاء، قراءة، تحديث، حذف)
- ✅ التحقق من صحة البيانات باستخدام `useFormValidation`
- ✅ دعم الترجمة العربية والإنجليزية
- ✅ تكامل مع API الباك إند (`/Accounts`)
- ✅ إدارة الأخطاء من FluentValidation
- ✅ تحسين UX مع تعطيل الأزرار عند وجود أخطاء

### 2. ترجمات عربية
**الملف**: `src/plugins/i18n/locales/ar.json`

**المضاف**:
```json
"tableHeaders": {
  "accounts": {
    "name": "الاسم",
    "motherName": "اسم الأم",
    "birthDate": "تاريخ الميلاد",
    "gender": "الجنس",
    "barcode": "الباركود",
    "status": "الحالة",
    "lockerFileNumber": "رقم ملف الخزانة",
    "socialStatus": "الحالة الاجتماعية",
    "bookNumber": "رقم الكتاب",
    "paperNumber": "رقم الورقة",
    "registrationNumber": "رقم التسجيل",
    "accountNumber": "رقم الحساب",
    "type": "النوع",
    "lookOver": "مراجعة",
    "note": "ملاحظة",
    "nid": "الرقم الوطني",
    "isActive": "نشط",
    "balance": "الرصيد",
    "actions": "الإجراءات"
  }
},
"pages": {
  "accounts": {
    "title": "إدارة الحسابات",
    "addAccount": "إضافة حساب",
    "addNewAccount": "إضافة حساب جديد",
    "editAccount": "تعديل الحساب",
    "name": "الاسم",
    "motherName": "اسم الأم",
    "birthDate": "تاريخ الميلاد",
    "gender": "الجنس",
    "barcode": "الباركود",
    "status": "الحالة",
    "lockerFileNumber": "رقم ملف الخزانة",
    "socialStatus": "الحالة الاجتماعية",
    "bookNumber": "رقم الكتاب",
    "paperNumber": "رقم الورقة",
    "registrationNumber": "رقم التسجيل",
    "accountNumber": "رقم الحساب",
    "type": "النوع",
    "lookOver": "مراجعة",
    "note": "ملاحظة",
    "nid": "الرقم الوطني",
    "isActive": "نشط",
    "balance": "الرصيد",
    "address": "العنوان",
    "city": "المدينة",
    "country": "البلد",
    "contactNumber": "رقم الاتصال",
    "floors": "عدد الطوابق",
    "genderMale": "ذكر",
    "genderFemale": "أنثى",
    "statusActive": "نشط",
    "statusInactive": "غير نشط",
    "socialStatusMarried": "متزوج",
    "socialStatusWidower": "أرمل",
    "socialStatusDivorced": "مطلق",
    "socialStatusSingle": "أعزب",
    "socialStatusChild": "طفل",
    "accountTypeMartyr": "شهيد",
    "accountTypeDisability": "إعاقة",
    "accountTypeBeneficiary": "مستفيد",
    "accountTypeTreasury": "خزينة",
    "accountTypePaymentMethod": "طريقة دفع",
    "accountTypeMarket": "سوق",
    "accountTypeMarketer": "مسوق",
    "accountTypeCustomer": "عميل",
    "accountTypeSupplierCompany": "شركة مورد",
    "accountTypeAccounting": "محاسبة",
    "accountTypeRevenues": "إيرادات",
    "accountTypeProducts": "منتجات",
    "accountTypeLiabilities": "التزامات",
    "accountTypeExpenses": "مصروفات",
    "isInactive": "غير نشط",
    "confirmDelete": "تأكيد الحذف",
    "deleteConfirmation": "هل أنت متأكد من حذف هذا الحساب؟"
  }
}
```

### 3. ترجمات إنجليزية
**الملف**: `src/plugins/i18n/locales/en.json`

**المضاف**:
```json
"tableHeaders": {
  "accounts": {
    "name": "Name",
    "motherName": "Mother Name",
    "birthDate": "Birth Date",
    "gender": "Gender",
    "barcode": "Barcode",
    "status": "Status",
    "lockerFileNumber": "Locker File Number",
    "socialStatus": "Social Status",
    "bookNumber": "Book Number",
    "paperNumber": "Paper Number",
    "registrationNumber": "Registration Number",
    "accountNumber": "Account Number",
    "type": "Type",
    "lookOver": "Look Over",
    "note": "Note",
    "nid": "NID",
    "isActive": "Is Active",
    "balance": "Balance",
    "actions": "Actions"
  }
},
"pages": {
  "accounts": {
    "title": "Accounts Management",
    "addAccount": "Add Account",
    "addNewAccount": "Add New Account",
    "editAccount": "Edit Account",
    "name": "Name",
    "motherName": "Mother Name",
    "birthDate": "Birth Date",
    "gender": "Gender",
    "barcode": "Barcode",
    "status": "Status",
    "lockerFileNumber": "Locker File Number",
    "socialStatus": "Social Status",
    "bookNumber": "Book Number",
    "paperNumber": "Paper Number",
    "registrationNumber": "Registration Number",
    "accountNumber": "Account Number",
    "type": "Type",
    "lookOver": "Look Over",
    "note": "Note",
    "nid": "NID",
    "isActive": "Is Active",
    "balance": "Balance",
    "address": "Address",
    "city": "City",
    "country": "Country",
    "contactNumber": "Contact Number",
    "floors": "Floors",
    "genderMale": "Male",
    "genderFemale": "Female",
    "statusActive": "Active",
    "statusInactive": "Inactive",
    "socialStatusMarried": "Married",
    "socialStatusWidower": "Widower",
    "socialStatusDivorced": "Divorced",
    "socialStatusSingle": "Single",
    "socialStatusChild": "Child",
    "accountTypeMartyr": "Martyr",
    "accountTypeDisability": "Disability",
    "accountTypeBeneficiary": "Beneficiary",
    "accountTypeTreasury": "Treasury",
    "accountTypePaymentMethod": "Payment Method",
    "accountTypeMarket": "Market",
    "accountTypeMarketer": "Marketer",
    "accountTypeCustomer": "Customer",
    "accountTypeSupplierCompany": "Supplier Company",
    "accountTypeAccounting": "Accounting",
    "accountTypeRevenues": "Revenues",
    "accountTypeProducts": "Products",
    "accountTypeLiabilities": "Liabilities",
    "accountTypeExpenses": "Expenses",
    "isInactive": "Inactive",
    "confirmDelete": "Confirm Delete",
    "deleteConfirmation": "Are you sure you want to delete this account?"
  }
}
```

## الميزات المطبقة

### 1. التحقق من صحة البيانات
- ✅ التحقق من الحقول المطلوبة (name, motherName, birthDate, gender, barcode, status, etc.)
- ✅ استخدام `validateRequired` للتحقق من الحقول النصية
- ✅ استخدام `setFieldTouched` لتعيين الحقول كملموسة
- ✅ استخدام `addError` لإضافة أخطاء مخصصة
- ✅ استخدام `hasErrors` لتعطيل الأزرار عند وجود أخطاء

### 2. تكامل FluentValidation
- ✅ استخدام `setErrorsFromResponse` لمعالجة أخطاء الباك إند
- ✅ ربط أخطاء الباك إند مع الحقول الأمامية
- ✅ دعم `Accept-Language` header للترجمة

### 3. واجهة المستخدم
- ✅ جدول بيانات مع `VDataTable`
- ✅ نوافذ حوارية للإضافة والتعديل والحذف
- ✅ رقائق ملونة لعرض الحالات (الجنس، الحالة، النوع الاجتماعي، نوع الحساب)
- ✅ أزرار تعطيل عند وجود أخطاء
- ✅ رسائل خطأ واضحة

### 4. الحقول المدعومة
- ✅ الاسم (name)
- ✅ اسم الأم (motherName)
- ✅ تاريخ الميلاد (birthDate)
- ✅ الجنس (gender) - ذكر/أنثى
- ✅ الباركود (barcode)
- ✅ الحالة (status) - نشط/غير نشط
- ✅ رقم ملف الخزانة (lockerFileNumber)
- ✅ الحالة الاجتماعية (socialStatus) - متزوج/أرمل/مطلق/أعزب/طفل
- ✅ رقم الكتاب (bookNumber)
- ✅ رقم الورقة (paperNumber)
- ✅ رقم التسجيل (registrationNumber)
- ✅ رقم الحساب (accountNumber)
- ✅ النوع (type) - شهيد/إعاقة/مستفيد/خزينة/طريقة دفع/سوق/مسوق/عميل/شركة مورد/محاسبة/إيرادات/منتجات/التزامات/مصروفات
- ✅ مراجعة (lookOver) - مفتاح/إيقاف
- ✅ ملاحظة (note)
- ✅ الرقم الوطني (nid)
- ✅ نشط (isActive) - مفتاح/إيقاف
- ✅ الرصيد (balance)
- ✅ العنوان (address)
- ✅ المدينة (city)
- ✅ البلد (country)
- ✅ رقم الاتصال (contactNumber)
- ✅ عدد الطوابق (floors)

### 5. العمليات المدعومة
- ✅ عرض قائمة الحسابات
- ✅ إضافة حساب جديد
- ✅ تعديل حساب موجود
- ✅ حذف حساب
- ✅ تصفية وبحث (من خلال API الباك إند)

## API Endpoints المستخدمة
- `GET /Accounts/filter` - جلب قائمة الحسابات
- `POST /Accounts` - إضافة حساب جديد
- `PUT /Accounts/{id}` - تحديث حساب موجود
- `DELETE /Accounts/{id}` - حذف حساب

## ملاحظات تقنية
- تم استخدام `computed` properties للـ headers لدعم الترجمة
- تم استخدام `ref` للبيانات التفاعلية
- تم استخدام `onMounted` لتحميل البيانات عند بدء الصفحة
- تم استخدام `VSwitch` للحقول البولينية (lookOver, isActive)
- تم استخدام `VSelect` للحقول المحددة مسبقاً (gender, status, socialStatus, type)
- تم استخدام `VTextarea` لحقل الملاحظة
- تم استخدام `type="number"` للحقول الرقمية
- تم استخدام `type="date"` لتاريخ الميلاد

## الحالة
✅ **مكتمل** - تم تطبيق نمط خالد بنجاح على صفحة الحسابات 