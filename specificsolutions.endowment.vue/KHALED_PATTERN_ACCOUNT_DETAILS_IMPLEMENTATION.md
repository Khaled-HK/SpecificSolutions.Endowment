# تنفيذ نمط خالد - صفحة تفاصيل الحسابات (Account Details)

## نظرة عامة
تم تطبيق نمط خالد بنجاح على صفحة تفاصيل الحسابات (`account-details.vue`) مع دعم كامل للتحقق من صحة البيانات والترجمة.

## الملفات المطلوبة

### 1. صفحة تفاصيل الحسابات
**الملف**: `src/pages/apps/account-details.vue`

**الميزات المطبقة**:
- ✅ إدارة CRUD كاملة (إنشاء، قراءة، تحديث، حذف)
- ✅ التحقق من صحة البيانات باستخدام `useFormValidation`
- ✅ دعم الترجمة العربية والإنجليزية
- ✅ تكامل مع API الباك إند (`/AccountDetails`)
- ✅ إدارة الأخطاء من FluentValidation
- ✅ تحسين UX مع تعطيل الأزرار عند وجود أخطاء
- ✅ تنسيق العملة والتواريخ حسب اللغة
- ✅ عرض نوع العملية باستخدام VChip ملون

### 2. ملفات الترجمة

#### العربية (ar.json)
```json
{
  "tableHeaders": {
    "accountDetails": {
      "debtor": "مدين",
      "creditor": "دائن",
      "date": "التاريخ",
      "operationType": "نوع العملية",
      "operationNumber": "رقم العملية",
      "balance": "الرصيد",
      "note": "ملاحظة",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "accountDetails": {
      "title": "إدارة تفاصيل الحسابات",
      "addAccountDetail": "إضافة تفصيل حساب",
      "addNewAccountDetail": "إضافة تفصيل حساب جديد",
      "editAccountDetail": "تعديل تفصيل الحساب",
      "debtor": "مدين",
      "creditor": "دائن",
      "date": "التاريخ",
      "operationType": "نوع العملية",
      "operationNumber": "رقم العملية",
      "balance": "الرصيد",
      "note": "ملاحظة",
      "operationTypeCredit": "دائن",
      "operationTypeDebit": "مدين",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف تفصيل الحساب هذا؟"
    }
  }
}
```

#### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "accountDetails": {
      "debtor": "Debtor",
      "creditor": "Creditor",
      "date": "Date",
      "operationType": "Operation Type",
      "operationNumber": "Operation Number",
      "balance": "Balance",
      "note": "Note",
      "actions": "Actions"
    }
  },
  "pages": {
    "accountDetails": {
      "title": "Account Details Management",
      "addAccountDetail": "Add Account Detail",
      "addNewAccountDetail": "Add New Account Detail",
      "editAccountDetail": "Edit Account Detail",
      "debtor": "Debtor",
      "creditor": "Creditor",
      "date": "Date",
      "operationType": "Operation Type",
      "operationNumber": "Operation Number",
      "balance": "Balance",
      "note": "Note",
      "operationTypeCredit": "Credit",
      "operationTypeDebit": "Debit",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this account detail?"
    }
  }
}
```

## الحقول المطلوبة

### 1. الحقول الأساسية
- **مدين (Debtor)**: مبلغ المدين - رقم عشري
- **دائن (Creditor)**: مبلغ الدائن - رقم عشري
- **نوع العملية (Operation Type)**: Credit أو Debit
- **رقم العملية (Operation Number)**: رقم صحيح
- **ملاحظة (Note)**: نص اختياري

### 2. الحقول المحسوبة
- **الرصيد (Balance)**: يتم حسابه تلقائياً
- **التاريخ (Date)**: تاريخ إنشاء العملية

## التحقق من صحة البيانات

### 1. التحقق من الحقول المطلوبة
```typescript
// التحقق من المدين
if (!validateNumeric(newAccountDetail.value.debtor.toString(), 'debtor')) {
  addError('debtor', t('validation.required', { field: t('pages.accountDetails.debtor') }))
}

// التحقق من الدائن
if (!validateNumeric(newAccountDetail.value.creditor.toString(), 'creditor')) {
  addError('creditor', t('validation.required', { field: t('pages.accountDetails.creditor') }))
}

// التحقق من نوع العملية
if (!newAccountDetail.value.operationType) {
  addError('operationType', t('validation.required', { field: t('pages.accountDetails.operationType') }))
}

// التحقق من رقم العملية
if (!validateNumeric(newAccountDetail.value.operationNumber.toString(), 'operationNumber')) {
  addError('operationNumber', t('validation.required', { field: t('pages.accountDetails.operationNumber') }))
}
```

### 2. إدارة الأخطاء من الباك إند
```typescript
if (error.response?.data?.errors) {
  setErrorsFromResponse(error.response.data.errors, {
    Debtor: 'debtor',
    Creditor: 'creditor',
    Note: 'note',
    OperationType: 'operationType',
    OperationNumber: 'operationNumber',
    Balance: 'balance'
  })
}
```

## الميزات الخاصة

### 1. تنسيق العملة
```typescript
const formatCurrency = (value: number) => {
  return new Intl.NumberFormat(locale.value === 'ar' ? 'ar-LY' : 'en-US', {
    style: 'currency',
    currency: 'LYD'
  }).format(value)
}
```

### 2. تنسيق التواريخ
```typescript
const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString(locale.value === 'ar' ? 'ar-LY' : 'en-US')
}
```

### 3. عرض نوع العملية
```vue
<template #item.operationType="{ item }">
  <VChip
    :color="item.operationType === 'Credit' ? 'success' : 'error'"
    size="small"
  >
    {{ t(`pages.accountDetails.operationType${item.operationType}`) }}
  </VChip>
</template>
```

## API Endpoints

### 1. الحصول على البيانات
- **GET** `/AccountDetails/filter` - الحصول على قائمة تفاصيل الحسابات

### 2. إدارة البيانات
- **POST** `/AccountDetails` - إنشاء تفصيل حساب جديد
- **PUT** `/AccountDetails/{id}` - تحديث تفصيل حساب موجود
- **DELETE** `/AccountDetails/{id}` - حذف تفصيل حساب

## الباك إند المطلوب

### 1. Controller
```csharp
[HttpGet("filter")]
public async Task<EndowmentResponse<PagedList<FilterAccountDetailDTO>>> Filter([FromQuery] FilterAccountDetailQuery query, CancellationToken cancellationToken)
    => await _mediator.Send(query, cancellationToken);

[HttpPost]
public async Task<EndowmentResponse> Create(CreateAccountDetailCommand command, CancellationToken cancellationToken)
    => await _mediator.Send(command, cancellationToken);

[HttpPut("{id}")]
public async Task<EndowmentResponse> Update(Guid id, UpdateAccountDetailCommand command, CancellationToken cancellationToken)
    => await _mediator.Send(command, cancellationToken);

[HttpDelete("{id}")]
public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
    => await _mediator.Send(new DeleteAccountDetailCommand(id), cancellationToken);
```

### 2. DTOs
```csharp
public class FilterAccountDetailDTO
{
    public Guid Id { get; set; }
    public decimal Debtor { get; set; }
    public decimal Creditor { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public OperationType OperationType { get; set; }
    public int? OperationNumber { get; set; }
    public decimal Balance { get; set; }
    public Guid AccountId { get; set; }
}
```

### 3. Commands
```csharp
public class CreateAccountDetailCommand : ICommand, ICreateAccountDetailCommand
{
    public Guid Id { get; set; }
    public decimal Debtor { get; set; }
    public decimal Creditor { get; set; }
    public string Note { get; set; }
    public OperationType OperationType { get; set; }
    public int OperationNumber { get; set; }
    public decimal Balance { get; set; }
}
```

### 4. Enums
```csharp
public enum OperationType
{
    Credit,
    Debit
}
```

## النتيجة النهائية

✅ **صفحة تفاصيل الحسابات مكتملة** مع:
- إدارة كاملة للبيانات (CRUD)
- تحقق من صحة البيانات في الواجهة والباك إند
- دعم الترجمة العربية والإنجليزية
- تنسيق العملة والتواريخ حسب اللغة
- عرض نوع العملية بألوان مميزة
- تحسين تجربة المستخدم مع رسائل خطأ واضحة
- تكامل سلس مع FluentValidation في الباك إند

## الخطوات التالية

1. **اختبار الوظائف**: التأكد من عمل جميع العمليات (إضافة، تعديل، حذف)
2. **اختبار التحقق**: التأكد من عمل التحقق من صحة البيانات
3. **اختبار الترجمة**: التأكد من عمل الترجمة العربية والإنجليزية
4. **اختبار التنسيق**: التأكد من تنسيق العملة والتواريخ
5. **اختبار الأداء**: التأكد من سرعة تحميل البيانات

## ملاحظات التطوير

- تم استخدام `validateNumeric` للتحقق من الحقول الرقمية
- تم استخدام `VChip` لعرض نوع العملية بألوان مميزة
- تم استخدام `Intl.NumberFormat` لتنسيق العملة حسب اللغة
- تم استخدام `toLocaleDateString` لتنسيق التواريخ حسب اللغة
- تم تطبيق نفس نمط التحقق المستخدم في الصفحات السابقة 