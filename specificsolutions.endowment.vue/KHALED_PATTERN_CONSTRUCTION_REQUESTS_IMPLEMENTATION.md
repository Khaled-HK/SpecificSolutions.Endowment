# تطبيق نمط خالد على صفحة طلبات البناء (construction-requests.vue)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة طلبات البناء مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الميزات المطبقة
### 1. التحقق من صحة النماذج
- **الحقول المطلوبة**: جميع الحقول مطلوبة (العنوان، الوصف، الأولوية، الموقع، رقم المرجع، حالة الطلب، نوع المبنى، الموقع المقترح، المساحة المقترحة، التكلفة المقدرة، اسم المقاول)
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
  { title: t('tableHeaders.constructionRequests.title'), key: 'title' },
  { title: t('tableHeaders.constructionRequests.description'), key: 'description' },
  { title: t('tableHeaders.constructionRequests.priority'), key: 'priority' },
  { title: t('tableHeaders.constructionRequests.location'), key: 'location' },
  { title: t('tableHeaders.constructionRequests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.constructionRequests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.constructionRequests.buildingType'), key: 'buildingType' },
  { title: t('tableHeaders.constructionRequests.proposedLocation'), key: 'proposedLocation' },
  { title: t('tableHeaders.constructionRequests.proposedArea'), key: 'proposedArea' },
  { title: t('tableHeaders.constructionRequests.estimatedCost'), key: 'estimatedCost' },
  { title: t('tableHeaders.constructionRequests.contractorName'), key: 'contractorName' },
  { title: t('tableHeaders.constructionRequests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.constructionRequests.actions'), key: 'actions', sortable: false },
])
```

### دالة إضافة طلب بناء
```typescript
const addConstructionRequest = async () => {
  clearErrors()
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('buildingType')
  setFieldTouched('proposedLocation')
  setFieldTouched('proposedArea')
  setFieldTouched('estimatedCost')
  setFieldTouched('contractorName')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.constructionRequests.title') }))
  }
  // ... باقي التحققات

  if (hasErrors.value) {
    return
  }

  try {
    await $api('/ConstructionRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
    dialog.value = false
    resetNewRequest()
    await loadConstructionRequests()
  } catch (error) {
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        BuildingType: 'buildingType',
        ProposedLocation: 'proposedLocation',
        ProposedArea: 'proposedArea',
        EstimatedCost: 'estimatedCost',
        ContractorName: 'contractorName',
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
  BuildingType: 'buildingType',
  ProposedLocation: 'proposedLocation',
  ProposedArea: 'proposedArea',
  EstimatedCost: 'estimatedCost',
  ContractorName: 'contractorName',
})
```

## ملفات الترجمة المضافة

### العربية (ar.json)
```json
{
  "tableHeaders": {
    "constructionRequests": {
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "buildingType": "نوع المبنى",
      "proposedLocation": "الموقع المقترح",
      "proposedArea": "المساحة المقترحة",
      "estimatedCost": "التكلفة المقدرة",
      "contractorName": "اسم المقاول",
      "createdDate": "تاريخ الإنشاء",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "constructionRequests": {
      "title": "إدارة طلبات البناء",
      "addRequest": "إضافة طلب بناء",
      "addNewRequest": "إضافة طلب بناء جديد",
      "editRequest": "تعديل طلب البناء",
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "buildingType": "نوع المبنى",
      "proposedLocation": "الموقع المقترح",
      "proposedArea": "المساحة المقترحة",
      "estimatedCost": "التكلفة المقدرة",
      "contractorName": "اسم المقاول",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف طلب البناء هذا؟"
    }
  }
}
```

### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "constructionRequests": {
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "buildingType": "Building Type",
      "proposedLocation": "Proposed Location",
      "proposedArea": "Proposed Area",
      "estimatedCost": "Estimated Cost",
      "contractorName": "Contractor Name",
      "createdDate": "Created Date",
      "actions": "Actions"
    }
  },
  "pages": {
    "constructionRequests": {
      "title": "Construction Requests Management",
      "addRequest": "Add Construction Request",
      "addNewRequest": "Add New Construction Request",
      "editRequest": "Edit Construction Request",
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "buildingType": "Building Type",
      "proposedLocation": "Proposed Location",
      "proposedArea": "Proposed Area",
      "estimatedCost": "Estimated Cost",
      "contractorName": "Contractor Name",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this construction request?"
    }
  }
}
```

## API Endpoints المستخدمة
```typescript
// تحميل طلبات البناء
GET /ConstructionRequests/GetConstructionRequests

// إضافة طلب بناء جديد
POST /ConstructionRequests

// تحديث طلب بناء
PUT /ConstructionRequests/{id}

// حذف طلب بناء
DELETE /ConstructionRequests/{id}
```

## الميزات المطبقة
### 1. إدارة طلبات البناء
- **عرض طلبات البناء**: جدول مع جميع طلبات البناء
- **إضافة طلب بناء جديد**: نموذج لإضافة طلب بناء جديد
- **تعديل طلب بناء**: نموذج لتعديل طلب بناء موجود
- **حذف طلب بناء**: تأكيد حذف طلب البناء

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
تم تطبيق نمط خالد بنجاح على صفحة طلبات البناء مع:
- ✅ تحقق شامل من صحة البيانات
- ✅ دعم كامل للترجمة العربية والإنجليزية
- ✅ تكامل سلس مع FluentValidation في الباك إند
- ✅ تحسين تجربة المستخدم
- ✅ إدارة شاملة لطلبات البناء (إضافة، تعديل، حذف) 