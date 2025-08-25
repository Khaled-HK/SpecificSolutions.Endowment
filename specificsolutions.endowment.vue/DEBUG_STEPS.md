# خطوات التشخيص المحدثة - Updated Debug Steps

## المشكلة الحالية
النظام يعرض تنبيه على صفحة تسجيل الدخول بدلاً من التوجيه لصفحة `email-not-confirmed`.

## خطوات التشخيص المحدثة

### 1. إعادة تشغيل التطبيق بالكامل
```bash
# أوقف التطبيق (Ctrl+C)
# ثم أعد تشغيله
npm run dev
```

### 2. مسح Cache المتصفح بالكامل
- اضغط `Ctrl + Shift + Delete`
- اختر "All time" و "All data"
- أعد تشغيل المتصفح

### 3. التحقق من Console Logs الجديدة
ابحث عن هذه الرسائل في Developer Tools:

```
🔍 DEBUG: Checking error data structure...
📊 err.data: {...}
📊 err.data?.message: ...
📊 err.status: 401
🔍 401 Unauthorized error detected, checking if it might be email confirmation issue
✅ Redirect to email-not-confirmed page successful
```

### 4. إذا لم تظهر الرسائل الجديدة
هذا يعني أن الكود المحدث لم يتم تطبيقه. جرب:

#### أ) تحقق من حفظ الملف
تأكد من حفظ ملف `useLogin.ts` بعد التحديث.

#### ب) تحقق من Hot Reload
إذا كان Hot Reload لا يعمل، أعد تشغيل التطبيق.

#### ج) تحقق من الملف الصحيح
تأكد من أنك تعمل على الملف الصحيح:
`specificsolutions.endowment.vue/src/composables/useLogin.ts`

### 5. اختبار سريع
1. افتح Developer Tools (F12)
2. انتقل إلى Console tab
3. حاول تسجيل الدخول بحساب غير مؤكد
4. تحقق من الرسائل في Console

### 6. إذا استمرت المشكلة

#### أ) تحقق من وجود الصفحة
```bash
# تأكد من وجود الملف
ls src/pages/email-not-confirmed.vue
```

#### ب) تحقق من Routes
```bash
# تأكد من تسجيل الرابط
grep -r "email-not-confirmed" src/plugins/router/
```

#### ج) إضافة Test مؤقت
أضف هذا الكود في بداية دالة `login`:

```typescript
const login = async (t: (key: string) => string) => {
  console.log('🚀 LOGIN FUNCTION STARTED - TEST')
  isLoading.value = true
  
  // ... rest of the code
```

### 7. النتيجة المتوقعة
بعد الإصلاح، يجب أن ترى في Console:

```
🚀 LOGIN FUNCTION STARTED - TEST
🔍 Attempting login with: { email: "...", password: "***", rememberMe: false }
❌ Login error: FetchError: [POST] "http://localhost:7140/api/auth/login": 401 Unauthorized
🔍 DEBUG: Checking error data structure...
📊 err.data: {}
📊 err.status: 401
🔍 401 Unauthorized error detected, checking if it might be email confirmation issue
✅ Redirect to email-not-confirmed page successful
```

### 8. إذا لم يعمل التوجيه
- تحقق من أن `router.push` يعمل بشكل صحيح
- تأكد من أن الصفحة `email-not-confirmed` موجودة
- تحقق من أن الراوتر مسجل بشكل صحيح

## ملاحظات مهمة
- الكود المحدث يحتوي على debug مفصل
- جميع الخطوات مسجلة في Console
- التوجيه يجب أن يعمل مع خطأ 401
