# تطبيق نمط خالد على صفحة الطلبات (requests.vue)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة الطلبات مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الميزات المطبقة

### 1. التحقق من صحة النماذج
- **الحقول المطلوبة**: جميع الحقول مطلوبة (العنوان، الوصف، الأولوية، الموقع، رقم المرجع، حالة الطلب)
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

### 1. استخدام useFormValidation
```typescript
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
  addError,
} = useFormValidation()
```

### 2. Headers ديناميكية
```typescript
const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.requests.title'), key: 'title' },
  { title: t('tableHeaders.requests.description'), key: 'description' },
  { title: t('tableHeaders.requests.priority'), key: 'priority' },
  { title: t('tableHeaders.requests.location'), key: 'location' },
  { title: t('tableHeaders.requests.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.requests.requestStatus'), key: 'requestStatus' },
  { title: t('tableHeaders.requests.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.requests.actions'), key: 'actions', sortable: false },
])
```

### 3. التحقق من صحة البيانات
```typescript
const addRequest = async () => {
  clearErrors()
  
  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  
  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.requests.title') }))
  }
  
  // ... باقي التحقق
  
  if (hasErrors.value) {
    return
  }
  
  // ... إرسال البيانات
}
```

### 4. معالجة أخطاء الباك إند
```typescript
if (error.response?.data?.errors) {
  setErrorsFromResponse(error.response.data.errors, {
    Title: 'title',
    Description: 'description',
    Priority: 'priority',
    Location: 'location',
    ReferenceNumber: 'referenceNumber',
    RequestStatus: 'requestStatus',
  })
}
```

## ملفات الترجمة المضافة

### العربية (ar.json)
```json
{
  "tableHeaders": {
    "requests": {
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "createdDate": "تاريخ الإنشاء",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "requests": {
      "title": "إدارة الطلبات",
      "addRequest": "إضافة طلب",
      "addNewRequest": "إضافة طلب جديد",
      "editRequest": "تعديل الطلب",
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف هذا الطلب؟"
    }
  },
  "validation": {
    "required": "حقل {field} مطلوب"
  }
}
```

### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "requests": {
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "createdDate": "Created Date",
      "actions": "Actions"
    }
  },
  "pages": {
    "requests": {
      "title": "Requests Management",
      "addRequest": "Add Request",
      "addNewRequest": "Add New Request",
      "editRequest": "Edit Request",
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this request?"
    }
  },
  "validation": {
    "required": "Field {field} is required"
  }
}
```

## API Endpoints المستخدمة

### 1. جلب الطلبات
```typescript
const response = await $api('/Requests/requests', {
  headers: {
    'Accept-Language': locale.value
  }
})
```

### 2. إضافة طلب جديد
```typescript
await $api('/Requests', {
  method: 'POST',
  body: newRequest.value,
  headers: {
    'Accept-Language': locale.value
  }
})
```

### 3. تحديث طلب
```typescript
await $api(`/Requests/${editRequest.value.id}`, {
  method: 'PUT',
  body: editRequest.value,
  headers: {
    'Accept-Language': locale.value
  }
})
```

### 4. حذف طلب
```typescript
await $api(`/Requests/${selectedRequest.value.id}`, {
  method: 'DELETE',
  headers: {
    'Accept-Language': locale.value
  }
})
```

## الميزات المطبقة

### 1. إدارة الطلبات
- **عرض الطلبات**: جدول مع جميع الطلبات
- **إضافة طلب جديد**: نموذج لإضافة طلب جديد
- **تعديل طلب**: نموذج لتعديل طلب موجود
- **حذف طلب**: تأكيد حذف الطلب

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
تم تطبيق نمط خالد بنجاح على صفحة الطلبات مع:
- ✅ تحقق شامل من صحة البيانات
- ✅ دعم كامل للترجمة العربية والإنجليزية
- ✅ تكامل سلس مع FluentValidation في الباك إند
- ✅ تحسين تجربة المستخدم
- ✅ إدارة شاملة للطلبات (إضافة، تعديل، حذف) 