# دليل الإصلاح السريع - Quick Fix Guide

## المشكلة الحالية
النظام يعرض تنبيه على صفحة تسجيل الدخول بدلاً من التوجيه لصفحة `email-not-confirmed`.

## الحل السريع

### 1. إعادة تشغيل التطبيق
```bash
# في مجلد specificsolutions.endowment.vue
npm run dev
# أو
yarn dev
```

### 2. مسح Cache المتصفح
- اضغط `Ctrl + Shift + R` (أو `Cmd + Shift + R` على Mac)
- أو اذهب إلى Developer Tools > Network tab > Disable cache

### 3. التحقق من Console
افتح Developer Tools (F12) وتحقق من وجود هذه الرسائل:
```
🔍 401 Unauthorized error detected, checking if it might be email confirmation issue
✅ Redirect to email-not-confirmed page successful
```

### 4. إذا لم يعمل التوجيه

#### أ) تحقق من وجود الصفحة
تأكد من أن ملف `src/pages/email-not-confirmed.vue` موجود.

#### ب) تحقق من Routes
تأكد من أن الرابط `/email-not-confirmed` مسجل في `src/plugins/router/routes.ts`.

#### ج) إضافة Debug مؤقت
أضف هذا الكود في `useLogin.ts` في بداية دالة `login`:

```typescript
const login = async (t: (key: string) => string) => {
  console.log('🚀 Login function started')
  isLoading.value = true
  
  try {
    // ... existing code ...
  } catch (err: any) {
    console.log('🔥 Error caught:', err.status, err.data)
    
    if (err.status === 401) {
      console.log('🎯 401 detected, redirecting...')
      await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
      console.log('✅ Redirect completed')
      return
    }
    // ... rest of error handling ...
  }
}
```

### 5. اختبار سريع
1. حاول تسجيل الدخول بحساب غير مؤكد
2. تحقق من Console للرسائل
3. تأكد من التوجيه لصفحة `email-not-confirmed`

### 6. إذا استمرت المشكلة
- تحقق من أن الباك إند يعيد خطأ 401
- تأكد من أن `err.data` فارغ أو لا يحتوي على رسالة
- تحقق من أن `router.push` يعمل بشكل صحيح

## النتيجة المتوقعة
بعد الإصلاح، عند محاولة تسجيل الدخول بحساب غير مؤكد:
1. الباك إند يعيد خطأ 401
2. النظام يكشف الخطأ ويتوجه لصفحة `email-not-confirmed`
3. لا تظهر أي تنبيهات على صفحة تسجيل الدخول
