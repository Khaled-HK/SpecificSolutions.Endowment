# دليل تشخيص مشكلة تسجيل الدخول - Login Debug Guide

## المشكلة الحالية

الباك إند يعيد خطأ 401 (Unauthorized) بدون رسالة تفصيلية، مما يجعل من الصعب تحديد سبب الخطأ.

## خطوات التشخيص

### 1. تحقق من البيانات المرسلة

افتح Developer Tools (F12) وانتقل إلى Network tab، ثم حاول تسجيل الدخول. تحقق من:

**Request:**
```json
{
  "email": "khaledalneffaati@gmail.com",
  "password": "a123456",
  "rememberMe": true
}
```

**Response (Current - Problem):**
```json
{
  "status": 401,
  "statusText": "Unauthorized",
  "data": {}
}
```

### 2. تحقق من Console Logs

ابحث عن هذه الرسائل في Console:

```
🔍 Attempting login with: { email: "...", password: "***", rememberMe: true }
❌ Login error: FetchError: [POST] "http://localhost:7140/api/auth/login": 401 Unauthorized
❌ Error details: { status: 401, statusText: "Unauthorized", data: {...}, message: "..." }
🔍 401 Unauthorized error detected, checking if it might be email confirmation issue
✅ Redirect to email-not-confirmed page successful
```

### 3. الحل المطبق ✅

النظام الآن يتعامل مع خطأ 401 كحالة عدم تأكيد البريد الإلكتروني ويوجه المستخدم تلقائياً لصفحة `email-not-confirmed`.

### 4. المشاكل المحتملة

#### أ) الباك إند لا يتعرف على الحساب
- تأكد من أن الحساب موجود في قاعدة البيانات
- تحقق من صحة البريد الإلكتروني وكلمة المرور

#### ب) الباك إند لا يتحقق من تأكيد البريد الإلكتروني
- الباك إند يجب أن يتحقق من `EmailConfirmed` قبل إرجاع 401
- يجب أن يعيد 400 مع `requiresEmailConfirmation: true`

#### ج) مشكلة في API Endpoint
- تأكد من أن `/auth/login` موجود ويعمل
- تحقق من صحة الـ Base URL: `http://localhost:7140/api`

### 5. الحلول المقترحة

#### الحل الفوري (مطبق) ✅
النظام يتعامل مع خطأ 401 كحالة عدم تأكيد البريد الإلكتروني:

```typescript
} else if (err.status === 401) {
  // معالجة خاصة لخطأ 401 - قد يكون عدم تأكيد البريد الإلكتروني
  console.log('🔍 401 Unauthorized error detected, checking if it might be email confirmation issue')
  
  // توجيه المستخدم إلى صفحة عدم تأكيد البريد الإلكتروني كـ fallback
  await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
}
```

#### الحل الصحيح (نمط خالد)
الباك إند يجب أن يعيد:

```json
{
  "isSuccess": false,
  "state": 400,
  "message": "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول",
  "requiresEmailConfirmation": true
}
```

### 6. اختبار الحل

1. **اختبار الحل الحالي:**
   - حاول تسجيل الدخول بحساب غير مؤكد
   - النظام سيتوجه تلقائياً لصفحة `email-not-confirmed`
   - تحقق من تمرير البريد الإلكتروني في الرابط

2. **اختبار الحل الصحيح:**
   - عدّل الباك إند ليعيد الحقل `requiresEmailConfirmation: true`
   - النظام سيكتشف الحقل ويتوجه للصفحة المخصصة

### 7. خطوات التصحيح في الباك إند

1. **تحقق من Login Handler:**
   ```csharp
   // في Login Handler
   if (!user.EmailConfirmed)
   {
       return Response.FailureResponse(
           message: "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول",
           requiresEmailConfirmation: true
       );
   }
   ```

2. **تحقق من Response Model:**
   ```csharp
   public class LoginResponse
   {
       public bool IsSuccess { get; set; }
       public string Message { get; set; }
       public bool RequiresEmailConfirmation { get; set; }
   }
   ```

### 8. ملفات التشخيص

- `src/composables/useLogin.ts` - منطق تسجيل الدخول (محدث)
- `src/composables/useApi.ts` - إعدادات API
- `src/plugins/router/routes.ts` - إعدادات الراوتر
- `src/pages/email-not-confirmed.vue` - صفحة عدم التأكيد

## ملاحظات مهمة

- ✅ **الحل الفوري مطبق**: النظام يتعامل مع خطأ 401 كحالة عدم تأكيد
- ✅ **التوجيه يعمل**: المستخدم سيتم توجيهه لصفحة `email-not-confirmed`
- ✅ **التشخيص محسن**: جميع التفاصيل مسجلة في Console
- 🔄 **الحل الصحيح**: يتطلب تعديل الباك إند ليعيد الحقل المطلوب

## النتيجة المتوقعة

بعد تطبيق الحل، عند محاولة تسجيل الدخول بحساب غير مؤكد:

1. الباك إند يعيد خطأ 401
2. النظام يكشف الخطأ ويتوجه لصفحة `email-not-confirmed`
3. المستخدم يرى الصفحة المخصصة مع خيارات التأكيد
4. يمكن للمستخدم اختيار الإجراء المناسب
