# تطبيق نمط خالد على صفحة طلبات الصيانة (maintenance-requests.vue)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة طلبات الصيانة مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الميزات المطبقة
### 1. التحقق من صحة النماذج
- **الحقول المطلوبة**: جميع الحقول مطلوبة (العنوان، الوصف، الأولوية، الموقع، رقم المرجع، حالة الطلب، نوع الصيانة، التكلفة المقدرة، تاريخ بداية متوقع، تاريخ نهاية متوقع)
- **التحقق المحلي**: استخدام `validateRequired` للتحقق من الحقول المطلوبة
- **عرض الأخطاء**: عرض رسائل خطأ واضحة باللغة المحددة

### 2. دعم الترجمة (i18n)
- **ترجمة ديناميكية**: جميع النصوص قابلة للترجمة
- **Headers الجدول**: ترجمة أعمدة الجدول
- **رسائل التحقق**: رسائل التحقق باللغة المحددة
- **أزرار الإجراءات**: ترجمة أزرار الإضافة والتعديل والحذف

### 3. إدارة الأخطاء
- **أخطاء الباك إند**: تطبيق أخطاء FluentValidation من الباك إند
- **أخطاء الواجهة**: عرض أخطاء التحقق المحلية
- **مسح الأخطاء**: مسح الأخطاء عند فتح النوافذ أو إعادة تعيين النماذج

### 4. تحسين تجربة المستخدم
- **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
- **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة
- **رسائل واضحة**: رسائل خطأ واضحة ومفيدة

## الكود المطبق

### استخدام useFormValidation
```typescript
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  addError,
} = useFormValidation()
```

### Headers ديناميكية
```typescript
const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.maintenanceRequests.title'), key: 'title' },
  { title: t('tableHeaders.maintenanceRequests.description'), key: 'description' },
  { title: t('tableHeaders.maintenanceRequests.priority'), key: 'priority' },
  { title: t('tableHeaders.maintenanceRequests.location'), key: 'location' },
  { title: t('tableHeaders.maintenanceRequests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.maintenanceRequests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.maintenanceRequests.maintenanceType'), key: 'maintenanceType' },
  { title: t('tableHeaders.maintenanceRequests.estimatedCost'), key: 'estimatedCost' },
  { title: t('tableHeaders.maintenanceRequests.expectedStartDate'), key: 'expectedStartDate' },
  { title: t('tableHeaders.maintenanceRequests.expectedEndDate'), key: 'expectedEndDate' },
  { title: t('tableHeaders.maintenanceRequests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.maintenanceRequests.actions'), key: 'actions', sortable: false },
])
```

### دالة إضافة طلب صيانة
```typescript
const addMaintenanceRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('maintenanceType')
  setFieldTouched('estimatedCost')
  setFieldTouched('expectedStartDate')
  setFieldTouched('expectedEndDate')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.maintenanceRequests.title') }))
  }
  // ... باقي التحقق من الحقول

  if (hasErrors.value) {
    return
  }

  try {
    await $api('/MaintenanceRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: { 'Accept-Language': locale.value }
    })
    dialog.value = false
    resetNewRequest()
    await loadMaintenanceRequests()
  } catch (error) {
    console.error('Error adding maintenance request:', error)
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        MaintenanceType: 'maintenanceType',
        EstimatedCost: 'estimatedCost',
        ExpectedStartDate: 'expectedStartDate',
        ExpectedEndDate: 'expectedEndDate',
      })
    }
  }
}
```

### تطبيق أخطاء الباك إند
```typescript
setErrorsFromResponse(error.response.data.errors, {
  Title: 'title',
  Description: 'description',
  Priority: 'priority',
  Location: 'location',
  ReferenceNumber: 'referenceNumber',
  RequestStatus: 'requestStatus',
  MaintenanceType: 'maintenanceType',
  EstimatedCost: 'estimatedCost',
  ExpectedStartDate: 'expectedStartDate',
  ExpectedEndDate: 'expectedEndDate',
})
```

## ملفات الترجمة المضافة

### ar.json
```json
{
  "tableHeaders": {
    "maintenanceRequests": {
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "maintenanceType": "نوع الصيانة",
      "estimatedCost": "التكلفة المقدرة",
      "expectedStartDate": "تاريخ بداية متوقع",
      "expectedEndDate": "تاريخ نهاية متوقع",
      "createdDate": "تاريخ الإنشاء",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "maintenanceRequests": {
      "title": "إدارة طلبات الصيانة",
      "addRequest": "إضافة طلب صيانة",
      "addNewRequest": "إضافة طلب صيانة جديد",
      "editRequest": "تعديل طلب الصيانة",
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "maintenanceType": "نوع الصيانة",
      "estimatedCost": "التكلفة المقدرة",
      "expectedStartDate": "تاريخ بداية متوقع",
      "expectedEndDate": "تاريخ نهاية متوقع",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف طلب الصيانة هذا؟"
    }
  }
}
```

### en.json
```json
{
  "tableHeaders": {
    "maintenanceRequests": {
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "maintenanceType": "Maintenance Type",
      "estimatedCost": "Estimated Cost",
      "expectedStartDate": "Expected Start Date",
      "expectedEndDate": "Expected End Date",
      "createdDate": "Created Date",
      "actions": "Actions"
    }
  },
  "pages": {
    "maintenanceRequests": {
      "title": "Maintenance Requests Management",
      "addRequest": "Add Maintenance Request",
      "addNewRequest": "Add New Maintenance Request",
      "editRequest": "Edit Maintenance Request",
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "maintenanceType": "Maintenance Type",
      "estimatedCost": "Estimated Cost",
      "expectedStartDate": "Expected Start Date",
      "expectedEndDate": "Expected End Date",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this maintenance request?"
    }
  }
}
```

## API Endpoints المستخدمة
```typescript
// تحميل طلبات الصيانة
GET /MaintenanceRequests/GetMaintenanceRequests

// إضافة طلب صيانة جديد
POST /MaintenanceRequests

// تحديث طلب صيانة
PUT /MaintenanceRequests/{id}

// حذف طلب صيانة
DELETE /MaintenanceRequests/{id}
```

## الميزات المطبقة
### 1. إدارة طلبات الصيانة
- **عرض طلبات الصيانة**: جدول مع جميع طلبات الصيانة
- **إضافة طلب صيانة جديد**: نموذج لإضافة طلب صيانة جديد
- **تعديل طلب صيانة**: نموذج لتعديل طلب صيانة موجود
- **حذف طلب صيانة**: تأكيد حذف طلب الصيانة

### 2. التحقق من صحة البيانات
- **الحقول المطلوبة**: جميع الحقول مطلوبة
- **عرض الأخطاء**: رسائل خطأ واضحة
- **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء

### 3. دعم متعدد اللغات
- **ترجمة ديناميكية**: جميع النصوص قابلة للترجمة
- **رسائل التحقق**: رسائل التحقق باللغة المحددة
- **تنسيق التواريخ**: عرض التواريخ باللغة المحددة

### 4. تحسين تجربة المستخدم
- **تحميل البيانات**: مؤشر تحميل أثناء جلب البيانات
- **رسائل خطأ واضحة**: رسائل خطأ مفيدة ومفهومة
- **تأكيد الحذف**: نافذة تأكيد قبل الحذف

## النتيجة النهائية
تم تطبيق نمط خالد بنجاح على صفحة طلبات الصيانة مع:
- ✅ تحقق شامل من صحة البيانات
- ✅ دعم كامل للترجمة العربية والإنجليزية
- ✅ تكامل سلس مع FluentValidation في الباك إند
- ✅ تحسين تجربة المستخدم
- ✅ إدارة شاملة لطلبات الصيانة (إضافة، تعديل، حذف) 