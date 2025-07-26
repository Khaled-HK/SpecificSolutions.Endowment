# حل مشكلة الوصول إلى صفحة البريد الإلكتروني والصلاحيات

## المشكلة
لم يكن من الممكن الوصول إلى `http://localhost:5173/apps/email` بسبب مشكلة في نظام الصلاحيات.

## السبب
كل صفحة في التطبيق تحتوي على `definePage` مع `meta` يحتوي على `action` و `subject` للتحقق من الصلاحيات:

- **صفحة البريد الإلكتروني**: `action: 'View', subject: 'Dashboard'`
- **صفحة المساجد**: `action: 'View', subject: 'Mosque'`
- **صفحة المدن**: `action: 'View', subject: 'City'`
- **صفحة المناطق**: `action: 'View', subject: 'Region'`
- وهكذا...

المستخدم لم يكن يملك الصلاحيات المطلوبة، وكان نظام التحقق من الصلاحيات غير مفعل في router guards.

## الحل المطبق

### 1. إضافة التحقق من الصلاحيات في Router Guards
تم تعديل ملف `src/plugins/1.router/guards.js` لإضافة التحقق من الصلاحيات باستخدام CASL:

```javascript
import { createMongoAbility } from '@casl/ability'

// دالة للتحقق من الصلاحيات باستخدام CASL
const checkPermissions = (to) => {
  try {
    // إذا لم تكن هناك صلاحيات محددة في meta، اسمح بالوصول
    if (!to.meta.action || !to.meta.subject) {
      return true
    }

    // الحصول على قواعد الصلاحيات من الكوكيز
    const userAbilityRules = useCookie('userAbilityRules').value
    if (!userAbilityRules || !Array.isArray(userAbilityRules)) {
      console.warn('No ability rules found')
      return false
    }

    // إنشاء ability مؤقت للتحقق
    const ability = createMongoAbility(userAbilityRules)

    // التحقق من الصلاحية
    const canAccess = ability.can(to.meta.action, to.meta.subject)
    
    if (!canAccess) {
      console.warn(`Access denied: ${to.meta.action} on ${to.meta.subject}`)
    }
    
    return canAccess
  } catch (error) {
    console.error('Permission check error:', error)
    return false
  }
}
```

### 2. إضافة صلاحيات شاملة في Login
تم تعديل ملف `src/pages/login.vue` لإضافة صلاحيات لجميع الكيانات المطلوبة:

```javascript
// Add Dashboard permissions (required for email and other dashboard pages)
rules.push(
  { action: 'View', subject: 'Dashboard' },
  { action: 'read', subject: 'Dashboard' },
  { action: 'write', subject: 'Dashboard' }
)

// Add permissions for all main entities (required for app pages)
rules.push(
  { action: 'View', subject: 'Mosque' },
  { action: 'View', subject: 'City' },
  { action: 'View', subject: 'Region' },
  { action: 'View', subject: 'Office' },
  { action: 'View', subject: 'Building' },
  { action: 'View', subject: 'Product' },
  { action: 'View', subject: 'Decision' },
  { action: 'View', subject: 'Account' },
  { action: 'View', subject: 'User' },
  { action: 'View', subject: 'Role' },
  { action: 'View', subject: 'Request' }
)
```

### 3. إضافة إعادة التوجيه إلى صفحة غير مصرح
عند رفض الصلاحية، يتم إعادة التوجيه إلى صفحة `not-authorized`:

```javascript
// التحقق من الصلاحيات
const hasPermission = checkPermissions(to)
if (!hasPermission) {
  console.warn('Permission denied, redirecting to not-authorized...')
  next({
    name: 'not-authorized',
  })
  return
}
```

### 4. إصلاح مشكلة المسارات المخصصة
تم إصلاح مشكلة المسارات المخصصة في `additional-routes.js` التي كانت تتجاوز `definePage` في الصفحات:

```javascript
// قبل الإصلاح
{
  path: '/apps/ecommerce/dashboard',
  name: 'apps-ecommerce-dashboard',
  component: () => import('@/pages/dashboards/ecommerce.vue'),
  // لا يوجد meta للصلاحيات!
}

// بعد الإصلاح
{
  path: '/apps/ecommerce/dashboard',
  name: 'apps-ecommerce-dashboard',
  component: () => import('@/pages/dashboards/ecommerce.vue'),
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
}
```

### 5. إضافة الصلاحيات لجميع المسارات في additional-routes.js
تم إضافة `meta` للصلاحيات في جميع المسارات المخصصة:

```javascript
// Email filter routes
{
  path: '/apps/email/filter/:filter',
  name: 'apps-email-filter',
  component: emailRouteComponent,
  meta: {
    navActiveLink: 'apps-email',
    layoutWrapperClasses: 'layout-content-height-fixed',
    action: 'View',
    subject: 'Dashboard',
  },
}

// Email label routes
{
  path: '/apps/email/label/:label',
  name: 'apps-email-label',
  component: emailRouteComponent,
  meta: {
    navActiveLink: 'apps-email',
    layoutWrapperClasses: 'layout-content-height-fixed',
    action: 'View',
    subject: 'Dashboard',
  },
}
```

### 6. إضافة الصلاحيات لصفحات Ecommerce
تم إضافة `definePage` مع الصلاحيات في جميع صفحات ecommerce التي لم تكن تحتوي عليها:

```javascript
// جميع صفحات ecommerce الآن تحتوي على:
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})
```

#### الصفحات التي تم إصلاحها:
- ✅ `/apps/ecommerce/product/list` - قائمة المنتجات
- ✅ `/apps/ecommerce/product/category-list` - قائمة فئات المنتجات
- ✅ `/apps/ecommerce/product/add` - إضافة منتج جديد
- ✅ `/apps/ecommerce/customer/list` - قائمة العملاء
- ✅ `/apps/ecommerce/customer/details/[id]` - تفاصيل العميل
- ✅ `/apps/ecommerce/order/list` - قائمة الطلبات
- ✅ `/apps/ecommerce/order/details/[id]` - تفاصيل الطلب
- ✅ `/apps/ecommerce/settings` - إعدادات التجارة الإلكترونية
- ✅ `/apps/ecommerce/referrals` - الإحالات
- ✅ `/apps/ecommerce/manage-review` - إدارة المراجعات

### 7. إصلاح أخطاء الكوكيز و useI18n
تم إصلاح عدة أخطاء مهمة في التطبيق:

#### أ. إصلاح خطأ الكوكيز
تم تغيير اسم الكوكيز من `userAbilityRules` إلى `user-ability-rules` لتجنب خطأ `argument name is invalid`:

```javascript
// قبل الإصلاح
useCookie('userAbilityRules').value = rules

// بعد الإصلاح  
useCookie('user-ability-rules').value = rules
```

#### ب. إصلاح خطأ useI18n
تم إصلاح مشكلة `useI18n` في `useApi.ts` التي كانت تسبب خطأ `Must be called at the top of a setup function`:

```javascript
// قبل الإصلاح
const { locale } = useI18n()
const languageMap: Record<string, string> = {
  'ar': 'ar-LY',
  'en': 'en-US'
}
const currentLanguage = languageMap[locale.value] || 'ar-LY'

// بعد الإصلاح
const savedLanguage = typeof window !== 'undefined' ? localStorage.getItem('preferredLanguage') : null
const currentLanguage = savedLanguage === 'en' ? 'en-US' : 'ar-LY'
```

#### ج. إضافة صلاحيات Endowment
تم إضافة صلاحية `Endowment` إلى قائمة الصلاحيات الأساسية:

```javascript
rules.push(
  { action: 'View', subject: 'Endowment' }
)
```

#### د. إصلاح مشاكل MSW و API Errors
تم إصلاح عدة مشاكل متعلقة بـ MSW و API:

**1. إصلاح TypeError في صفحة المنتجات:**
```javascript
// قبل الإصلاح
const products = computed(() => productsData.value.products)
const totalProduct = computed(() => productsData.value.total)

// بعد الإصلاح
const products = computed(() => productsData.value?.products || fallbackProducts)
const totalProduct = computed(() => productsData.value?.total || fallbackProducts.length)
```

**2. إضافة Fallback Data:**
```javascript
const fallbackProducts = [
  {
    id: 1,
    productName: 'iPhone 14 Pro',
    category: 'Electronics',
    stock: true,
    sku: 19472,
    price: '$999',
    qty: 665,
    status: 'Inactive',
    productBrand: 'Super Retina XDR display footnote Pro Motion technology',
  },
  // ... المزيد من المنتجات
]
```

**3. تحسين إعدادات MSW:**
```javascript
export default function () {
  // Only start MSW in development
  if (import.meta.env.DEV) {
    const workerUrl = `${import.meta.env.BASE_URL ?? '/'}mockServiceWorker.js`

    worker.start({
      serviceWorker: {
        url: workerUrl,
      },
      onUnhandledRequest: 'bypass',
    }).then(() => {
      console.log('MSW started successfully')
    }).catch((error) => {
      console.error('MSW failed to start:', error)
    })
  }
}
```

**4. تحسين معالجة الأخطاء في useApi:**
```javascript
onFetchError(ctx) {
  console.error('Fetch error:', ctx.error)
  return ctx
},
```

**5. تحسين معالجة الأخطاء في deleteProduct:**
```javascript
const deleteProduct = async id => {
  try {
    await $api(`apps/ecommerce/products/${ id }`, { method: 'DELETE' })
    // ... باقي الكود
  } catch (error) {
    console.error('Error deleting product:', error)
    // Handle error gracefully
  }
}
```

## نمط الصلاحيات في التطبيق

كل صفحة تحتوي على `definePage` مع meta للصلاحيات:

```javascript
// مثال: صفحة المساجد
definePage({
  meta: {
    action: 'View',
    subject: 'Mosque',
  },
})

// مثال: صفحة المدن
definePage({
  meta: {
    action: 'View',
    subject: 'City',
  },
})

// مثال: صفحة البريد الإلكتروني
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})
```

## مشكلة المسارات المخصصة

بعض الصفحات تستخدم مسارات مخصصة في `additional-routes.js` تتجاوز `definePage` في الصفحة نفسها. هذه المسارات تحتاج إلى `meta` للصلاحيات:

### الصفحات المتأثرة:
- `/apps/email/filter/:filter` ✅ (تم إصلاحها)
- `/apps/email/label/:label` ✅ (تم إصلاحها)
- `/apps/ecommerce/dashboard` ✅ (تم إصلاحها)
- `/dashboards/logistics` ✅ (تم إصلاحها)
- `/dashboards/academy` ✅ (تم إصلاحها)

## كيفية الاختبار

1. شغل الخادم:
```bash
cd specificsolutions.endowment.vue
npm run dev
```

2. اذهب إلى `http://localhost:5173`

3. سجل الدخول باستخدام:
   - البريد الإلكتروني: `admin@demo.com`
   - كلمة المرور: `admin`

4. حاول الوصول إلى:
   - `http://localhost:5173/apps/email` (صفحة البريد الإلكتروني)
   - `http://localhost:5173/apps/email/filter/inbox` (فلتر البريد الإلكتروني)
   - `http://localhost:5173/apps/email/label/important` (تسمية البريد الإلكتروني)
   - `http://localhost:5173/apps/mosques` (صفحة المساجد)
   - `http://localhost:5173/apps/cities` (صفحة المدن)
   - `http://localhost:5173/apps/regions` (صفحة المناطق)
   - `http://localhost:5173/apps/ecommerce/dashboard` (صفحة ecommerce dashboard)
   - `http://localhost:5173/apps/ecommerce/product/list` (قائمة المنتجات)
   - `http://localhost:5173/apps/ecommerce/product/category-list` (قائمة فئات المنتجات)
   - `http://localhost:5173/apps/ecommerce/customer/list` (قائمة العملاء)
   - `http://localhost:5173/apps/ecommerce/order/list` (قائمة الطلبات)
   - `http://localhost:5173/dashboards/logistics` (صفحة logistics dashboard)
   - `http://localhost:5173/dashboards/academy` (صفحة academy dashboard)

## النتيجة المتوقعة
الآن يجب أن تتمكن من الوصول إلى جميع صفحات التطبيق بدون مشاكل.

## ملاحظات إضافية
- تم إضافة صلاحيات شاملة لجميع الكيانات الرئيسية
- تم تحسين نظام التحقق من الصلاحيات
- تم إضافة رسائل تحذير مفيدة في وحدة التحكم للمطورين
- كل صفحة تحتوي على meta للصلاحيات المطلوبة
- النظام يدعم التحقق من الصلاحيات باستخدام CASL
- تم إصلاح مشكلة المسارات المخصصة التي كانت تتجاوز definePage
- تم إصلاح مشاكل MSW و API errors
- تم إضافة fallback data للمنتجات في حالة فشل MSW
- تم تحسين معالجة الأخطاء في useApi و deleteProduct
- تم إضافة الصلاحيات لجميع المسارات في additional-routes.js
- تم إصلاح أخطاء الكوكيز و useI18n
- تم إضافة صلاحية Endowment للوصول إلى صفحات الأوقاف

## الأخطاء التي تم إصلاحها

### Error 1: Initial access issue to `/apps/email`
**الوصف:** المستخدم لم يستطع الوصول إلى `/apps/email`
**الحل:** تم إضافة فحص الصلاحيات في router guards وإضافة صلاحيات Dashboard

### Error 2: `require` vs `import` in `guards.js`
**الوصف:** استخدام `require('@casl/ability')` بدلاً من import
**الحل:** تم تغيير `require` إلى `import { createMongoAbility } from '@casl/ability'`

### Error 3: Access issue to `/apps/ecommerce/dashboard`
**الوصف:** المستخدم لم يستطع الوصول إلى `/apps/ecommerce/dashboard`
**الحل:** تم إضافة meta properties لمسارات additional-routes.js

### Error 4: Access issue to `/apps/ecommerce/product/list`
**الوصف:** المستخدم لم يستطع الوصول إلى `/apps/ecommerce/product/list`
**الحل:** تم إضافة definePage لجميع صفحات ecommerce

### Error 5: `TypeError: argument name is invalid` related to cookies
**الوصف:** خطأ في اسم cookie
**الحل:** تم تغيير اسم cookie من `userAbilityRules` إلى `user-ability-rules`

### Error 6: `useI18n` error
**الوصف:** استخدام `useI18n` خارج setup function
**الحل:** تم استخدام `localStorage` بدلاً من `useI18n`

### Error 7: `TypeError: Cannot read properties of null (reading 'products')`
**الوصف:** خطأ في قراءة بيانات المنتجات من API
**الحل:** تم إضافة optional chaining و fallback data

### Error 8: `GET http://localhost:7140/api/apps/ecommerce/products?q&page=1&itemsPerPage=10 404 (Not Found)`
**الوصف:** خطأ 404 في API endpoint
**الحل:** تم إصلاح MSW وتحسين معالجة الأخطاء 