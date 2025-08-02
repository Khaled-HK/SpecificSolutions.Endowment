# تطبيق نمط خالد على صفحة المباني (Buildings)

## نظرة عامة
تم تطبيق نمط خالد (Khaled Pattern) بنجاح على صفحة المباني في الفرونت إند، مما يوفر تجربة مستخدم محسنة مع دعم كامل للغة العربية والإنجليزية.

## الميزات المطبقة

### 1. التحقق من صحة النماذج ✅
- **الحقول المطلوبة**: التحقق من وجود القيم
- **طول النص**: التحقق من الحد الأدنى والأقصى
- **التحقق من الاختيارات**: التحقق من اختيار المدينة والمنطقة

### 2. دعم الترجمة (i18n) ✅
- **Headers ديناميكية**: عناوين الجدول قابلة للترجمة
- **رسائل التحقق**: رسائل الخطأ باللغة المحددة
- **نصوص الواجهة**: جميع النصوص قابلة للترجمة

### 3. إدارة الأخطاء ✅
- **أخطاء الباك إند**: تطبيق أخطاء FluentValidation
- **أخطاء الواجهة**: عرض أخطاء التحقق المحلية
- **مسح الأخطاء**: مسح الأخطاء عند فتح النوافذ

### 4. تحسين تجربة المستخدم ✅
- **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
- **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة
- **رسائل واضحة**: رسائل خطأ واضحة ومفيدة

## التغييرات المطبقة

### 1. تحديث صفحة المباني (buildings.vue)
```typescript
// إضافة استخدام نظام التحقق
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

// استخدام i18n للترجمة
const { t, locale } = useI18n()

// Headers ديناميكية مع دعم الترجمة
const headers = computed(() => [
  { title: 'ID', key: 'id' },
  { title: t('tableHeaders.buildings.name'), key: 'name' },
  { title: t('tableHeaders.buildings.address'), key: 'address' },
  { title: t('tableHeaders.buildings.city'), key: 'cityName' },
  { title: t('tableHeaders.buildings.region'), key: 'regionName' },
  { title: t('tableHeaders.buildings.actions'), key: 'actions', sortable: false },
])
```

### 2. إضافة التحقق من صحة البيانات
```typescript
const addBuilding = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  // تعيين الحقول كملموسة لعرض الأخطاء
  setFieldTouched('name')
  setFieldTouched('address')
  setFieldTouched('cityId')
  setFieldTouched('regionId')
  
  // التحقق من صحة البيانات
  let isValid = true
  
  if (!validateRequired(newBuilding.value.name, 'name', locale.value === 'ar' ? 'اسم المبنى مطلوب' : 'Building name is required')) {
    isValid = false
  } else if (!validateLength(newBuilding.value.name, 'name', 2, 200, locale.value === 'ar' ? 'اسم المبنى يجب أن يكون بين 2 و 200 حرف' : 'Building name must be between 2 and 200 characters')) {
    isValid = false
  }
  
  // ... باقي التحقق
  
  if (!isValid) {
    return
  }
  
  // إرسال البيانات
}
```

### 3. إضافة الترجمات

#### العربية (ar.json)
```json
{
  "tableHeaders": {
    "buildings": {
      "name": "اسم المبنى",
      "address": "العنوان",
      "city": "المدينة",
      "region": "المنطقة",
      "actions": "الإجراءات"
    }
  },
  "pages": {
    "buildings": {
      "title": "إدارة المباني",
      "addBuilding": "إضافة مبنى",
      "addNewBuilding": "إضافة مبنى جديد",
      "editBuilding": "تعديل المبنى",
      "buildingName": "اسم المبنى",
      "address": "العنوان",
      "city": "المدينة",
      "region": "المنطقة",
      "description": "الوصف",
      "confirmDelete": "تأكيد الحذف",
      "deleteConfirmation": "هل أنت متأكد من حذف هذا المبنى؟"
    }
  },
  "common": {
    "cancel": "إلغاء",
    "save": "حفظ",
    "update": "تحديث",
    "delete": "حذف"
  }
}
```

#### الإنجليزية (en.json)
```json
{
  "tableHeaders": {
    "buildings": {
      "name": "Building Name",
      "address": "Address",
      "city": "City",
      "region": "Region",
      "actions": "Actions"
    }
  },
  "pages": {
    "buildings": {
      "title": "Buildings Management",
      "addBuilding": "Add Building",
      "addNewBuilding": "Add New Building",
      "editBuilding": "Edit Building",
      "buildingName": "Building Name",
      "address": "Address",
      "city": "City",
      "region": "Region",
      "description": "Description",
      "confirmDelete": "Confirm Delete",
      "deleteConfirmation": "Are you sure you want to delete this building?"
    }
  },
  "common": {
    "cancel": "Cancel",
    "save": "Save",
    "update": "Update",
    "delete": "Delete"
  }
}
```

### 4. تحسين واجهة المستخدم
```vue
<template>
  <!-- إضافة التحقق من الأخطاء -->
  <VTextField
    v-model="newBuilding.name"
    :label="t('pages.buildings.buildingName')"
    required
    :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
    :error-messages="validationState.errors.name || []"
    @blur="setFieldTouched('name')"
  />
  
  <!-- تعطيل الأزرار عند وجود أخطاء -->
  <VBtn
    color="blue darken-1"
    text
    @click="addBuilding"
    :disabled="hasErrors"
  >
    {{ t('common.save') }}
  </VBtn>
</template>

<style scoped>
/* تخصيص مظهر الحقل عند وجود خطأ */
:deep(.v-field--error) {
  border-color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-field__outline) {
  color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-label) {
  color: rgb(var(--v-theme-error)) !important;
}

/* تخصيص مظهر رسائل الخطأ */
:deep(.v-messages__message) {
  color: rgb(var(--v-theme-error)) !important;
  font-size: 0.75rem;
  margin-top: 4px;
}
</style>
```

## الصفحات المطبقة حالياً

### ✅ الصفحات المكتملة
1. **المنتجات (products.vue)** - مطبق
2. **المدن (cities.vue)** - مطبق
3. **المناطق (regions.vue)** - مطبق
4. **المساجد (mosques.vue)** - مطبق
5. **القرارات (decisions.vue)** - مطبق
6. **المكاتب (offices.vue)** - مطبق
7. **المباني (buildings.vue)** - مطبق ✅

### ❌ الصفحات المتبقية
- الطلبات (requests.vue) - غير موجودة
- طلبات البناء (construction-requests.vue) - غير موجودة
- طلبات الصيانة (maintenance-requests.vue) - غير موجودة
- طلبات التغيير (change-requests.vue) - غير موجودة
- الحسابات (accounts.vue) - غير موجودة
- تفاصيل الحسابات (account-details.vue) - غير موجودة

## الميزات المشتركة

### 1. التحقق من صحة النماذج
- **الحقول المطلوبة**: التحقق من وجود القيم
- **طول النص**: التحقق من الحد الأدنى والأقصى
- **التنسيق**: التحقق من صحة التنسيق
- **الأرقام**: التحقق من صحة القيم الرقمية

### 2. إدارة الأخطاء
- **أخطاء الباك إند**: تطبيق أخطاء FluentValidation
- **أخطاء الواجهة**: عرض أخطاء التحقق المحلية
- **مسح الأخطاء**: مسح الأخطاء عند فتح النوافذ

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
// BuildingValidator.cs
public class BuildingValidator : AbstractValidator<BuildingDto>
{
    public BuildingValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("اسم المبنى مطلوب")
            .MaximumLength(200).WithMessage("اسم المبنى يجب أن يكون أقل من 200 حرف");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("عنوان المبنى مطلوب")
            .MaximumLength(500).WithMessage("عنوان المبنى يجب أن يكون أقل من 500 حرف");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("المدينة مطلوبة");

        RuleFor(x => x.RegionId)
            .NotEmpty().WithMessage("المنطقة مطلوبة");
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

## الخلاصة

تم تطبيق نمط خالد بنجاح على صفحة المباني، مما يوفر:

- ✅ **تحقق شامل من صحة النماذج**
- ✅ **دعم كامل للغة العربية والإنجليزية**
- ✅ **إدارة متقدمة للأخطاء**
- ✅ **تحسين تجربة المستخدم**
- ✅ **تكامل سلس مع الباك إند**

الصفحة الآن جاهزة للاستخدام مع جميع الميزات المطلوبة مطبقة بنجاح. 