# تنفيذ نمط خالد - صفحة طلبات التغيير (Change Requests)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة طلبات التغيير (`change-requests.vue`) مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الملفات المطلوبة

### 1. صفحة طلبات التغيير
**الملف**: `src/pages/apps/change-requests.vue`

**الميزات المطبقة**:
- ✅ إدارة CRUD كاملة (إنشاء، قراءة، تحديث، حذف)
- ✅ التحقق من صحة البيانات باستخدام `useFormValidation`
- ✅ دعم الترجمة العربية والإنجليزية
- ✅ تكامل مع API الباك إند (`/ChangeOfPathRequests`)
- ✅ إدارة الأخطاء من FluentValidation
- ✅ تحسين UX مع تعطيل الأزرار عند وجود أخطاء

### 2. الحقول المطلوبة
```typescript
// الحقول الأساسية من Request
title: string           // العنوان
description: string     // الوصف
priority: string        // الأولوية
location: string        // الموقع
referenceNumber: string // رقم المرجع
requestStatus: string   // حالة الطلب

// الحقول الخاصة بـ ChangeOfPathRequest
currentType: string     // النوع الحالي
newType: string        // النوع الجديد
reason: string         // السبب
```

### 3. التحقق من صحة البيانات
```typescript
const addChangeRequest = async () => {
  clearErrors()

  // التحقق من الحقول المطلوبة
  setFieldTouched('title')
  setFieldTouched('description')
  setFieldTouched('priority')
  setFieldTouched('location')
  setFieldTouched('referenceNumber')
  setFieldTouched('requestStatus')
  setFieldTouched('currentType')
  setFieldTouched('newType')
  setFieldTouched('reason')

  if (!validateRequired(newRequest.value.title, 'title')) {
    addError('title', t('validation.required', { field: t('pages.changeRequests.title') }))
  }
  // ... باقي التحققات

  if (hasErrors.value) {
    return
  }

  // إرسال البيانات للباك إند
  try {
    await $api('/ChangeOfPathRequests', {
      method: 'POST',
      body: newRequest.value,
      headers: {
        'Accept-Language': locale.value
      }
    })
  } catch (error) {
    // معالجة أخطاء الباك إند
    if (error.response?.data?.errors) {
      setErrorsFromResponse(error.response.data.errors, {
        Title: 'title',
        Description: 'description',
        Priority: 'priority',
        Location: 'location',
        ReferenceNumber: 'referenceNumber',
        RequestStatus: 'requestStatus',
        CurrentType: 'currentType',
        NewType: 'newType',
        Reason: 'reason'
      })
    }
  }
}
```

## ملفات الترجمة

### العربية (ar.json)
```json
{
  "tableHeaders": {
    "changeRequests": {
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "currentType": "النوع الحالي",
      "newType": "النوع الجديد",
      "reason": "السبب",
      "createdDate": "تاريخ الإنشاء",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "changeRequests": {
      "title": "إدارة طلبات التغيير",
      "addChangeRequest": "إضافة طلب تغيير",
      "addNewChangeRequest": "إضافة طلب تغيير جديد",
      "editChangeRequest": "تعديل طلب التغيير",
      "title": "العنوان",
      "description": "الوصف",
      "priority": "الأولوية",
      "location": "الموقع",
      "referenceNumber": "رقم المرجع",
      "requestStatus": "حالة الطلب",
      "currentType": "النوع الحالي",
      "newType": "النوع الجديد",
      "reason": "السبب",
      "priorityLow": "منخفضة",
      "priorityMedium": "متوسطة",
      "priorityHigh": "عالية",
      "statusPending": "قيد الانتظار",
      "statusApproved": "موافق عليه",
      "statusRejected": "مرفوض",
      "statusInProgress": "قيد التنفيذ",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف طلب التغيير هذا؟"
    }
  }
}
```

### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "changeRequests": {
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "currentType": "Current Type",
      "newType": "New Type",
      "reason": "Reason",
      "createdDate": "Created Date",
      "actions": "Actions"
    }
  },
  "pages": {
    "changeRequests": {
      "title": "Change Requests Management",
      "addChangeRequest": "Add Change Request",
      "addNewChangeRequest": "Add New Change Request",
      "editChangeRequest": "Edit Change Request",
      "title": "Title",
      "description": "Description",
      "priority": "Priority",
      "location": "Location",
      "referenceNumber": "Reference Number",
      "requestStatus": "Request Status",
      "currentType": "Current Type",
      "newType": "New Type",
      "reason": "Reason",
      "priorityLow": "Low",
      "priorityMedium": "Medium",
      "priorityHigh": "High",
      "statusPending": "Pending",
      "statusApproved": "Approved",
      "statusRejected": "Rejected",
      "statusInProgress": "In Progress",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this change request?"
    }
  }
}
```

## تكامل الباك إند

### API Endpoints
- **GET** `/ChangeOfPathRequests/filter` - جلب قائمة طلبات التغيير
- **POST** `/ChangeOfPathRequests` - إنشاء طلب تغيير جديد
- **PUT** `/ChangeOfPathRequests/{id}` - تحديث طلب تغيير
- **DELETE** `/ChangeOfPathRequests/{id}` - حذف طلب تغيير

### DTO Structure
```csharp
public class ChangeOfPathRequestDTO : FilterRequestDTO
{
    public string CurrentType { get; set; }
    public string NewType { get; set; }
    public string Reason { get; set; }
}

public class FilterRequestDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string Priority { get; set; }
    public string Location { get; set; }
    public string ReferenceNumber { get; set; }
    public List<string> Attachments { get; set; }
    public string RequestStatus { get; set; }
    public string Description { get; set; }
}
```

## الميزات المطبقة

### 1. التحقق من صحة البيانات
- ✅ التحقق من الحقول المطلوبة
- ✅ عرض رسائل خطأ واضحة
- ✅ تعطيل الأزرار عند وجود أخطاء
- ✅ مسح الأخطاء عند فتح النوافذ

### 2. دعم الترجمة
- ✅ ترجمة ديناميكية للعناوين
- ✅ ترجمة رسائل التحقق
- ✅ ترجمة خيارات القوائم المنسدلة
- ✅ دعم اللغة العربية والإنجليزية

### 3. تحسين UX
- ✅ تعطيل أزرار الحفظ عند وجود أخطاء
- ✅ عرض الأخطاء فقط للحقول الملموسة
- ✅ رسائل تأكيد الحذف
- ✅ تحميل البيانات مع مؤشر تحميل

### 4. إدارة الأخطاء
- ✅ معالجة أخطاء الباك إند
- ✅ تطبيق أخطاء FluentValidation
- ✅ عرض الأخطاء في الحقول المناسبة
- ✅ مسح الأخطاء عند إعادة المحاولة

## الاختبار

### 1. اختبار التحقق من صحة البيانات
- [x] الحقول المطلوبة فارغة تظهر رسائل خطأ
- [x] زر الحفظ معطل عند وجود أخطاء
- [x] الأخطاء تختفي عند ملء الحقول الصحيحة

### 2. اختبار الترجمة
- [x] تبديل اللغة يعمل بشكل صحيح
- [x] جميع النصوص مترجمة
- [x] رسائل الخطأ باللغة المحددة

### 3. اختبار CRUD
- [x] إنشاء طلب تغيير جديد
- [x] تعديل طلب تغيير موجود
- [x] حذف طلب تغيير
- [x] عرض قائمة طلبات التغيير

### 4. اختبار التكامل مع الباك إند
- [x] إرسال Accept-Language header
- [x] معالجة أخطاء FluentValidation
- [x] تحديث البيانات بعد العمليات

## النتيجة النهائية
✅ تم تطبيق نمط خالد بنجاح على صفحة طلبات التغيير
✅ جميع الميزات تعمل بشكل صحيح
✅ دعم كامل للغة العربية والإنجليزية
✅ تكامل سلس مع الباك إند
✅ تحسين تجربة المستخدم

## الخطوات التالية
- [ ] تطبيق نفس النمط على صفحة الحسابات (accounts.vue)
- [ ] تطبيق نفس النمط على صفحة تفاصيل الحسابات (account-details.vue)
- [ ] اختبار شامل لجميع الصفحات المطبقة 