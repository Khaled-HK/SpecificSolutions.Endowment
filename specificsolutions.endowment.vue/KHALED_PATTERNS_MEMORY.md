# نمط خالد - Khaled Pattern

## نظرة عامة
نمط خالد هو نظام شامل لإدارة النماذج والتحقق من صحة البيانات في تطبيق Vue.js مع دعم كامل للغة العربية والإنجليزية، متكامل مع FluentValidation في الباك إند.

## المكونات الأساسية

### 1. useFormValidation Composable
```typescript
// composables/useFormValidation.ts
export const useFormValidation = () => {
  // إدارة حالة التحقق
  const validationState = ref({
    errors: {} as Record<string, string[]>,
    touched: {} as Record<string, boolean>
  })

  // دوال التحقق
  const validateRequired = (value: string, field: string, message: string): boolean
  const validateLength = (value: string, field: string, min: number, max: number, message: string): boolean
  const validatePattern = (value: string, field: string, pattern: RegExp, message: string): boolean
  const validateNumeric = (value: string, field: string, message: string): boolean

  // إدارة الأخطاء
  const setErrorsFromResponse = (response: any): void
  const clearErrors = (): void
  const setFieldTouched = (field: string): void
  const hasErrors = computed(() => Object.keys(validationState.value.errors).length > 0)

  return {
    validationState,
    setErrorsFromResponse,
    clearErrors,
    hasErrors,
    setFieldTouched,
    validateRequired,
    validateLength,
    validatePattern,
    validateNumeric
  }
}
```

### 2. دعم الترجمة (i18n)
```typescript
// استخدام i18n للترجمة
const { t, locale } = useI18n()

// Headers ديناميكية
const headers = computed(() => [
  {
    title: t('tableHeaders.entities.fieldName'),
    key: 'fieldName',
  }
])

// رسائل التحقق متعددة اللغات
const errorMessage = locale.value === 'ar' ? 'رسالة الخطأ بالعربية' : 'Error message in English'
```

### 3. إدارة حالة التحقق
```typescript
// مسح الأخطاء عند فتح النوافذ
const openEditDialog = (item: Entity) => {
  editItem.value = { ...item }
  editDialog.value = true
  clearErrors() // مسح أخطاء التحقق
}

// تعيين الحقول كملموسة لعرض الأخطاء
setFieldTouched('fieldName')
```

### 4. دعم الباك إند
```typescript
// إرسال Accept-Language header
const response = await $api('/endpoint', {
  headers: {
    'Accept-Language': locale.value
  }
})

// تطبيق أخطاء FluentValidation
if (response && response.isSuccess === false) {
  if (response.errors && response.errors.length > 0) {
    setErrorsFromResponse(response)
    setFieldTouched('fieldName')
    return
  }
}
```

## الصفحات المطبقة

### 1. المنتجات (products.vue) ✅
- **التحقق من صحة النماذج**: اسم المنتج، الوصف، السعر، الكمية
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء

### 2. المدن (cities.vue) ✅
- **التحقق من صحة النماذج**: اسم المدينة، المنطقة
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء

### 3. المناطق (regions.vue) ✅
- **التحقق من صحة النماذج**: اسم المنطقة، الدولة
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء

### 4. المساجد (mosques.vue) ✅
- **التحقق من صحة النماذج**: اسم المسجد، رقم الملف، المنطقة، المكتب
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء
- **إدارة تفاصيل المباني**: إضافة وتعديل وحذف تفاصيل المباني
- **إدارة المنتجات**: إضافة وتعديل وحذف المنتجات المرتبطة

### 5. القرارات (decisions.vue) ✅
- **التحقق من صحة النماذج**: عنوان القرار، الوصف، رقم المرجع
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء
- **تنسيق التواريخ**: عرض التواريخ باللغة المحددة

### 6. المكاتب (offices.vue) ✅
- **التحقق من صحة النماذج**: اسم المكتب، الموقع، رقم الهاتف، المنطقة
- **دعم الترجمة**: العربية والإنجليزية
- **إدارة الأخطاء**: عرض أخطاء الباك إند
- **تحسين UX**: تعطيل الأزرار عند وجود أخطاء
- **التحقق من رقم الهاتف**: تنسيق ليبي صحيح (091-1234567 أو 021-1234567)
- **إدارة المناطق**: اختيار المنطقة من قائمة المناطق المتاحة

## ملفات الترجمة

### العربية (ar.json)
```json
{
  "tableHeaders": {
    "products": {
      "name": "اسم المادة",
      "description": "الوصف",
      "price": "السعر",
      "quantity": "الكمية",
      "actions": "الإجراءات"
    },
    "cities": {
      "name": "المدينة",
      "regionName": "المنطقة",
      "country": "الدولة",
      "actions": "الإجراءات"
    },
    "regions": {
      "name": "المنطقة",
      "country": "الدولة",
      "actions": "الإجراءات"
    },
    "mosques": {
      "name": "اسم المسجد",
      "fileNumber": "رقم الملف",
      "region": "المنطقة",
      "office": "المكتب",
      "actions": "الإجراءات"
    },
    "decisions": {
      "title": "عنوان القرار",
      "referenceNumber": "رقم المرجع",
      "description": "الوصف",
      "createdDate": "تاريخ الإنشاء",
      "actions": "الإجراءات"
    },
    "offices": {
      "name": "المكتب",
      "regionName": "المنطقة",
      "phoneNumber": "رقم الهاتف",
      "actions": "الإجراءات"
    }
  }
}
```

### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "products": {
      "name": "Product Name",
      "description": "Description",
      "price": "Price",
      "quantity": "Quantity",
      "actions": "Actions"
    },
    "cities": {
      "name": "City",
      "regionName": "Region",
      "country": "Country",
      "actions": "Actions"
    },
    "regions": {
      "name": "Region",
      "country": "Country",
      "actions": "Actions"
    },
    "mosques": {
      "name": "Mosque Name",
      "fileNumber": "File Number",
      "region": "Region",
      "office": "Office",
      "actions": "Actions"
    },
    "decisions": {
      "title": "Decision Title",
      "referenceNumber": "Reference Number",
      "description": "Description",
      "createdDate": "Created Date",
      "actions": "Actions"
    },
    "offices": {
      "name": "Office",
      "regionName": "Region",
      "phoneNumber": "Phone Number",
      "actions": "Actions"
    }
  }
}
```

## الميزات المشتركة

### 1. التحقق من صحة النماذج
- **الحقول المطلوبة**: التحقق من وجود القيم
- **طول النص**: التحقق من الحد الأدنى والأقصى
- **التنسيق**: التحقق من صحة التنسيق (أرقام الهواتف، البريد الإلكتروني)
- **الأرقام**: التحقق من صحة القيم الرقمية

### 2. إدارة الأخطاء
- **أخطاء الباك إند**: تطبيق أخطاء FluentValidation
- **أخطاء الواجهة**: عرض أخطاء التحقق المحلية
- **مسح الأخطاء**: مسح الأخطاء عند فتح النوافذ أو إعادة تعيين النماذج

### 3. تحسين تجربة المستخدم
- **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
- **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة
- **رسائل واضحة**: رسائل خطأ واضحة ومفيدة

### 4. دعم متعدد اللغات
- **ترجمة ديناميكية**: جميع النصوص قابلة للترجمة
- **تنسيق التواريخ**: عرض التواريخ باللغة المحددة
- **رسائل التحقق**: رسائل التحقق باللغة المحددة

## الباك إند Integration

### 1. Middleware للغة
```csharp
// ThreadCultureMiddleware.cs
public class ThreadCultureMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var acceptLanguage = context.Request.Headers["Accept-Language"].FirstOrDefault();
        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var culture = acceptLanguage.StartsWith("ar") ? "ar-SA" : "en-US";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }
        await next(context);
    }
}
```

### 2. FluentValidation
```csharp
// DecisionValidator.cs
public class DecisionValidator : AbstractValidator<DecisionDto>
{
    public DecisionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان القرار مطلوب")
            .MaximumLength(100).WithMessage("عنوان القرار يجب أن يكون أقل من 100 حرف");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف القرار مطلوب")
            .MaximumLength(500).WithMessage("وصف القرار يجب أن يكون أقل من 500 حرف");

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("رقم المرجع مطلوب")
            .MaximumLength(50).WithMessage("رقم المرجع يجب أن يكون أقل من 50 حرف");
    }
}
```

## أفضل الممارسات

### 1. إدارة الحالة
- استخدام `ref` للبيانات المحلية
- استخدام `computed` للقيم المحسوبة
- استخدام `watch` لمراقبة التغييرات

### 2. التحقق من صحة البيانات
- التحقق من صحة البيانات قبل الإرسال
- تطبيق أخطاء الباك إند على الحقول
- عرض الأخطاء فقط للحقول الملموسة

### 3. تحسين الأداء
- استخدام `computed` للـ headers
- تجنب إعادة الحساب غير الضروري
- استخدام `watch` بكفاءة

### 4. قابلية الصيانة
- فصل منطق التحقق في composable
- استخدام ملفات ترجمة منفصلة
- توحيد أنماط الكود

## حل مشاكل التحقق من صحة البيانات

### مشكلة: زر "حفظ" معطل حتى بعد ملء الحقول
**السبب**: عدم تعيين الحقول كملموسة (`touched`) قبل التحقق من صحة البيانات.

**الحل**:
```typescript
const addCity = async () => {
  // Clear previous errors
  clearErrors()
  
  // Set fields as touched first - مهم جداً!
  setFieldTouched('name')
  setFieldTouched('country')
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(newCity.name, 'name', 'اسم المدينة مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(newCity.country, 'country', 'الدولة مطلوبة')) {
    isValid = false
  }
  
  // Check if there are any validation errors
  if (!isValid) {
    return
  }
  
  // ... باقي الكود
}
```

**النقاط المهمة**:
1. **ترتيب العمليات مهم**: `setFieldTouched` يجب أن يكون قبل `validateRequired`
2. **استخدام `!isValid` بدلاً من `hasErrors.value`**: للاعتماد على متغير التحقق المحلي
3. **مسح الأخطاء في البداية**: `clearErrors()` لضمان حالة نظيفة

### تطبيق نفس الحل على دالة التحديث
```typescript
const updateCity = async () => {
  // Clear previous errors
  clearErrors()
  
  // Set fields as touched first
  setFieldTouched('editName')
  setFieldTouched('editCountry')
  
  // Validate required fields
  let isValid = true
  
  if (!validateRequired(editCity.name, 'editName', 'اسم المدينة مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(editCity.country, 'editCountry', 'الدولة مطلوبة')) {
    isValid = false
  }
  
  // Check if there are any validation errors
  if (!isValid) {
    return
  }
  
  // ... باقي الكود
}
```

### النتيجة المتوقعة
- **الحقول فارغة**: رسائل خطأ تظهر وزر "حفظ" معطل
- **الحقول تحتوي على بيانات صحيحة**: رسائل الخطأ تختفي وزر "حفظ" متاح
- **تجربة مستخدم سلسة**: التحقق يعمل بشكل فوري عند ملء الحقول

## الصفحات المتبقية للتطبيق
- [ ] المباني (buildings.vue)
- [ ] الطلبات (requests.vue)
- [ ] طلبات البناء (construction-requests.vue)
- [ ] طلبات الصيانة (maintenance-requests.vue)
- [ ] طلبات التغيير (change-requests.vue)
- [ ] الحسابات (accounts.vue)
- [ ] تفاصيل الحسابات (account-details.vue)

## ملاحظات التطوير
- جميع الصفحات تطبق نفس النمط الموحد
- التحقق من صحة البيانات يتم على مستوى الواجهة والباك إند
- دعم كامل للغة العربية والإنجليزية
- تحسين تجربة المستخدم مع رسائل خطأ واضحة
- تكامل سلس مع FluentValidation في الباك إند 