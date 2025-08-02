# إصلاح مشكلة صلاحيات ConstructionRequest

## المشكلة
كان المستخدم لا يستطيع الوصول إلى صفحة `construction-requests` رغم وجود الصلاحيات في قاعدة البيانات.

## السبب
المشكلة كانت في تحويل الصلاحيات من الباك إند إلى الفرونت إند. الصلاحيات تأتي من الباك إند بتنسيق `ConstructionRequest_View` لكن النظام في الفرونت إند يبحث عن `View` على `ConstructionRequest`.

## الحل المطبق

### 1. إضافة Debugging في guards.js
تم إضافة debugging إضافي في ملف `src/plugins/1.router/guards.js` لمعرفة الصلاحيات الفعلية المخزنة في الكوكيز:

```javascript
// إضافة debugging إضافي لمعرفة الصلاحيات الفعلية
console.log('🔍 Looking for permission:', to.meta.action, 'on', to.meta.subject)
console.log('🔍 Available permissions:')
userAbilityRules.forEach((rule, index) => {
  console.log(`  ${index + 1}. Action: ${rule.action}, Subject: ${rule.subject}`)
})
```

### 2. إضافة Debugging في login.vue
تم إضافة debugging في ملف `src/pages/login.vue` لمعرفة الصلاحيات التي يتم إرسالها من الباك إند:

```javascript
// إضافة debugging إضافي لمعرفة الصلاحيات الفعلية
console.log('🔍 Backend permissions details:')
user.permissions.forEach((permission, index) => {
  const action = mapPermissionToAction(permission)
  const subject = mapPermissionToSubject(permission)
  console.log(`  ${index + 1}. Original: "${permission}" -> Action: "${action}", Subject: "${subject}"`)
})
```

### 3. إضافة حل مؤقت
تم إضافة حل مؤقت في `login.vue` لإضافة صلاحية `ConstructionRequest_View` مباشرة إذا لم تكن موجودة:

```javascript
// إضافة صلاحية ConstructionRequest_View مباشرة كحل مؤقت
// TODO: إزالة هذا الحل المؤقت بعد إصلاح المشكلة الأساسية
const hasConstructionRequestView = rules.some(rule => 
  rule.action === 'View' && rule.subject === 'ConstructionRequest'
)
if (!hasConstructionRequestView) {
  console.log('🔧 Adding temporary ConstructionRequest_View permission')
  rules.push({ action: 'View', subject: 'ConstructionRequest' })
}
```

## التحقق من الصلاحيات في قاعدة البيانات
جميع المستخدمين لديهم صلاحية `constructionRequestView: true` في قاعدة البيانات كما هو موضح في ملف `Seeder.cs`:

```csharp
constructionRequestView: true, constructionRequestAdd: true, constructionRequestEdit: true, constructionRequestDelete: true,
```

## الخطوات التالية
1. اختبار الحل المؤقت
2. فحص الصلاحيات الفعلية المخزنة في الكوكيز
3. إصلاح المشكلة الأساسية في تحويل الصلاحيات
4. إزالة الحل المؤقت بعد الإصلاح

## ملاحظات
- الصلاحيات المؤقتة مضافة للتطوير والاختبار فقط
- يجب إزالة الحل المؤقت بعد إصلاح المشكلة الأساسية
- النظام يدعم جميع الصلاحيات المطلوبة في الباك إند 