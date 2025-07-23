# نمط خالد - Khaled Pattern

## نظرة عامة
نمط خالد هو نظام شامل للتحقق من صحة النماذج وإدارة البيانات في تطبيق Vue.js مع .NET backend. يعتمد على تنفيذ الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات (LINQ to Entities) داخل الـ Repository.

## المكونات الأساسية

### 1. نمط الفلترة في Backend
- **الموقع**: Repository Layer
- **الطريقة**: `GetByFilterAsync` في كل Repository
- **الميزات**: 
  - فلترة على مستوى قاعدة البيانات
  - تحويل إلى DTO مباشرة
  - دعم Paging
  - استدعاء مباشر من Handler

### 2. نظام التحقق من صحة النماذج (Frontend)
- **الملف الأساسي**: `useFormValidation.ts`
- **المكونات**: `ValidatedTextField.vue`
- **الميزات**:
  - تحقق من الحقول المطلوبة
  - تحقق من البريد الإلكتروني
  - تحقق من أرقام الهواتف
  - دعم الرسائل المخصصة
  - تكامل مع FluentValidation backend

### 3. إدارة العناوين الديناميكية
- **النظام**: i18n (Vue I18n)
- **الملفات**: 
  - `ar.json` (العربية)
  - `en.json` (الإنجليزية)
- **الهيكل**: `tableHeaders.{page}.{field}`

### 4. إدارة اللغة المتعددة (Frontend ↔ Backend)
- **الفرونت إند**: إرسال `Accept-Language` header مع كل طلب API
- **الباك إند**: `ThreadCultureMiddleware` يقرأ اللغة من الـ header
- **التكامل**: رسائل الخطأ تأتي باللغة المختارة في الفرونت إند
- **الملفات المحدثة**:
  - `src/utils/api.js`
  - `src/composables/useApi.ts`
  - `src/composables/useApi.js`

## الصفحات المطبقة

### 1. صفحات المصادقة
- **Login**: تسجيل الدخول
- **Register**: التسجيل
- **Forgot Password**: نسيان كلمة المرور

### 2. صفحات إدارة البيانات
- **Decisions**: القرارات (النموذج الأساسي)
- **Regions**: المناطق
- **Cities**: المدن ✅ (محدث بنمط خالد)
- **Products**: المنتجات ✅ (محدث بنمط خالد)
- **Offices**: المكاتب
- **Mosques**: المساجد

## هيكل العناوين الديناميكية

### القرارات (Decisions)
```json
{
  "title": "عنوان القرار",
  "referenceNumber": "رقم المرجع",
  "description": "الوصف",
  "createdDate": "تاريخ الإنشاء",
  "actions": "الإجراءات"
}
```

### المناطق (Regions)
```json
{
  "name": "المنطقة",
  "country": "الدولة",
  "actions": "الإجراءات"
}
```

### المدن (Cities)
```json
{
  "name": "المدينة",
  "regionName": "المنطقة",
  "country": "الدولة",
  "actions": "الإجراءات"
}
```

### المنتجات (Products)
```json
{
  "name": "اسم المادة",
  "description": "الوصف",
  "price": "السعر",
  "quantity": "الكمية",
  "actions": "الإجراءات"
}
```

### المكاتب (Offices)
```json
{
  "name": "المكتب",
  "regionName": "المنطقة",
  "phoneNumber": "رقم الهاتف",
  "actions": "الإجراءات"
}
```

### المساجد (Mosques)
```json
{
  "name": "اسم المسجد",
  "fileNumber": "رقم الملف",
  "region": "المنطقة",
  "office": "المكتب",
  "actions": "الإجراءات"
}
```

## الممارسات المطبقة

### 1. تصميم موحد
- استخدام Snackbar للإشعارات
- تصميم موحد للجداول
- أزرار موحدة (إضافة، تعديل، حذف)
- عرض أخطاء التحقق بشكل موحد

### 2. التحقق من الصحة
- تحقق فوري من الحقول
- رسائل خطأ واضحة
- تعطيل الأزرار عند وجود أخطاء
- تنظيف حالة التحقق قبل التحقق الجديد

### 3. إدارة الحالة
- إدارة حالة اللمس (touched state)
- عرض الأخطاء فقط للحقول الملموسة
- تنظيف الأخطاء عند التعديل

### 4. إدارة اللغة المتعددة
- إرسال اللغة المختارة مع كل طلب API
- تكامل مع ThreadCultureMiddleware في الباك إند
- رسائل خطأ متعددة اللغات من FluentValidation

## الاستخدام في الكود

### استخدام العناوين الديناميكية
```vue
<template>
  <v-data-table
    :headers="headers"
    :items="items"
  >
  </v-data-table>
</template>

<script setup>
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const headers = computed(() => [
  { title: t('tableHeaders.decisions.title'), key: 'title' },
  { title: t('tableHeaders.decisions.referenceNumber'), key: 'referenceNumber' },
  { title: t('tableHeaders.decisions.description'), key: 'description' },
  { title: t('tableHeaders.decisions.createdDate'), key: 'createdDate' },
  { title: t('tableHeaders.decisions.actions'), key: 'actions' }
])
</script>
```

### استخدام نظام التحقق
```vue
<template>
  <ValidatedTextField
    v-model="form.name"
    :label="t('form.name')"
    :error="validationErrors.name"
    :touched="touchedFields.name"
    @blur="setFieldTouched('name')"
  />
</template>

<script setup>
import { useFormValidation } from '@/composables/useFormValidation'

const {
  validateRequired,
  validateEmail,
  addError,
  clearErrors,
  setFieldTouched,
  validationErrors,
  touchedFields,
  hasErrors
} = useFormValidation()
</script>
```

### إرسال اللغة مع طلبات API
```javascript
// في useApi.ts و api.js
async beforeFetch({ options }) {
  // ... existing code ...

  // Add Accept-Language header based on current locale
  const { locale } = useI18n()
  const languageMap = {
    'ar': 'ar-LY',
    'en': 'en-US'
  }
  const currentLanguage = languageMap[locale.value] || 'ar-LY'
  
  options.headers = {
    ...options.headers,
    'Accept-Language': currentLanguage,
  }

  return { options }
}
```

## الملفات الأساسية

### Frontend
- `src/composables/useFormValidation.ts` - نظام التحقق الأساسي
- `src/components/ValidatedTextField.vue` - مكون حقل النص مع التحقق
- `src/plugins/i18n/locales/ar.json` - الترجمات العربية
- `src/plugins/i18n/locales/en.json` - الترجمات الإنجليزية
- `src/utils/api.js` - إعدادات API مع إرسال اللغة
- `src/composables/useApi.ts` - composable API مع إرسال اللغة

### Backend
- Repository Pattern مع `GetByFilterAsync`
- Handler Pattern مع استدعاء مباشر للـ Repository
- FluentValidation للتحقق من صحة البيانات
- `ThreadCultureMiddleware` لقراءة اللغة من الـ headers

## المزايا

1. **أداء عالي**: فلترة على مستوى قاعدة البيانات
2. **مرونة**: دعم متعدد اللغات
3. **قابلية الصيانة**: كود موحد ومنظم
4. **تجربة مستخدم محسنة**: تحقق فوري ورسائل واضحة
5. **قابلية التوسع**: سهولة إضافة صفحات جديدة
6. **تكامل متعدد اللغات**: رسائل خطأ باللغة المختارة

## التطبيق المستقبلي

يجب تطبيق هذا النمط على جميع الصفحات الجديدة والمتبقية في المشروع لضمان الاتساق والجودة العالية.

## التحديثات المطبقة على صفحة المدن

### ✅ الميزات المطبقة:
1. **العناوين الديناميكية**: استخدام i18n للترجمات
2. **نظام التحقق المحسن**: استخدام `useFormValidation` composable
3. **تحقق فوري**: تحقق من الحقول المطلوبة قبل الإرسال
4. **تكامل مع Backend**: معالجة أخطاء FluentValidation
5. **إدارة الحالة**: تنظيف الأخطاء عند فتح النماذج
6. **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
7. **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة

### 🔧 التحديثات التقنية:
- تحديث العناوين لتستخدم `t('tableHeaders.cities.*')`
- استبدال `validationState` بـ `shouldShowFieldError` و `getFieldErrors`
- إضافة تحقق من الحقول المطلوبة في `addCity` و `updateCity`
- تحسين معالجة أخطاء Backend
- إضافة تنظيف الأخطاء عند فتح النماذج

## التحديثات المطبقة على صفحة المواد

### ✅ الميزات المطبقة:
1. **العناوين الديناميكية**: استخدام i18n للترجمات
2. **نظام التحقق المحسن**: استخدام `useFormValidation` composable
3. **تحقق فوري**: تحقق من الحقول المطلوبة قبل الإرسال
4. **تكامل مع Backend**: معالجة أخطاء FluentValidation
5. **إدارة الحالة**: تنظيف الأخطاء عند فتح النماذج
6. **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
7. **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة
8. **تحقق من الأرقام**: تحقق من السعر والكمية (لا يمكن أن تكون سالبة)

### 🔧 التحديثات التقنية:
- تحديث العناوين لتستخدم `t('tableHeaders.products.*')`
- استبدال `validationState` بـ `shouldShowFieldError` و `getFieldErrors`
- إضافة تحقق من الحقول المطلوبة في `addProduct` و `updateProduct`
- تحسين معالجة أخطاء Backend
- إضافة تنظيف الأخطاء عند فتح النماذج
- تحقق من القيم الرقمية (السعر والكمية)

## التحديثات المطبقة على إدارة اللغة المتعددة

### ✅ الميزات المطبقة:
1. **إرسال اللغة مع الطلبات**: إضافة `Accept-Language` header
2. **تكامل مع الباك إند**: استخدام `ThreadCultureMiddleware`
3. **رسائل خطأ متعددة اللغات**: رسائل FluentValidation باللغة المختارة
4. **دعم اللغات**: العربية (ar-LY) والإنجليزية (en-US)

### 🔧 التحديثات التقنية:
- تحديث `src/utils/api.js` لإرسال `Accept-Language` header
- تحديث `src/composables/useApi.ts` لإرسال `Accept-Language` header
- تحديث `src/composables/useApi.js` لإرسال `Accept-Language` header
- تكامل مع `ThreadCultureMiddleware` في الباك إند
- إصلاح أخطاء TypeScript في ملفات TypeScript

## التحديثات المطبقة على صفحة المساجد

### ✅ الميزات المطبقة:
1. **العناوين الديناميكية**: استخدام i18n للترجمات
2. **نظام التحقق المحسن**: استخدام `useFormValidation` composable
3. **تحقق فوري**: تحقق من الحقول المطلوبة قبل الإرسال
4. **تكامل مع Backend**: معالجة أخطاء FluentValidation
5. **إدارة الحالة**: تنظيف الأخطاء عند فتح النماذج
6. **تعطيل الأزرار**: تعطيل أزرار الحفظ عند وجود أخطاء
7. **عرض الأخطاء**: عرض الأخطاء فقط للحقول الملموسة
8. **تحقق من الأرقام**: تحقق من المساحات وعدد الطوابق (قيم موجبة)
9. **دعم تفاصيل المبنى**: إدارة تفاصيل المبنى والمواد

### 🔧 التحديثات التقنية:
- تحديث العناوين لتستخدم `t('tableHeaders.mosques.*')`
- تحديث عناوين تفاصيل المبنى لتستخدم `t('tableHeaders.buildingDetails.*')`
- استبدال `validationState` بـ `shouldShowFieldError` و `getFieldErrors`
- إضافة تحقق من الحقول المطلوبة في `addMosque` و `updateMosque`
- تحسين معالجة أخطاء Backend
- إضافة تنظيف الأخطاء عند فتح النماذج
- تحقق من القيم الرقمية (المساحات وعدد الطوابق)
- إضافة دعم إدارة المواد في تفاصيل المبنى 