# إصلاح مشكلة التحقق من صحة بيانات تسجيل الدخول

## المشكلة
كانت الاستجابة من الباك إند تحتوي على `"errors": []` فارغة ورسالة عامة `"Validation failed"`، بينما يجب أن تكون الأخطاء محددة لكل حقل حسب نمط خالد.

## السبب الجذري
`SecurityController` كان يستخدم `BodyObject` بدلاً من `LoginCommand` مباشرة، مما يعني أن `ValidationPipelineBehavior` لم يكن يعمل لأن `BodyObject` ليس `IBaseCommand`.

## الحلول المطبقة

### 1. تحديث SecurityController
- تغيير `loginUser` method ليستقبل `LoginCommand` مباشرة بدلاً من `BodyObject`
- إزالة التحقق اليدوي من الحقول لأن `FluentValidation` سيتعامل مع ذلك
- إزالة `BodyObject` class لأنه لم يعد مطلوباً

### 2. تحديث LoginCommand
- إضافة `RememberMe` property إلى `LoginCommand`
- إضافة `RememberMe` property إلى `ILoginCommand` interface

### 3. تحديث الفرونت إند
- إضافة `rememberMe: rememberMe.value` إلى البيانات المرسلة في طلب تسجيل الدخول
- **إصلاح معالجة الأخطاء**: تحديث login function لمعالجة الأخطاء المحددة للحقول في الاستجابة المباشرة

## الملفات المعدلة

### الباك إند
1. `Management/Controllers/SecurityController.cs`
   - تغيير parameter من `BodyObject` إلى `LoginCommand`
   - إزالة التحقق اليدوي من الحقول
   - إزالة `BodyObject` class

2. `SpecificSolutions.Endowment.Application/Handlers/Authentications/Commands/Login/LoginCommand.cs`
   - إضافة `RememberMe` property

3. `SpecificSolutions.Endowment.Core/Models/Authentications/ILoginCommand.cs`
   - إضافة `RememberMe` property

### الفرونت إند
1. `specificsolutions.endowment.vue/src/pages/login.vue`
   - إضافة `rememberMe: rememberMe.value` إلى request body
   - **إصلاح معالجة الأخطاء**: تحديث login function لمعالجة `res.errors` مباشرة
   - إزالة الكود المكرر في catch block

## النتيجة المتوقعة
الآن عند إرسال بيانات تسجيل دخول غير صحيحة، ستظهر أخطاء محددة لكل حقل:

```json
{
  "errors": [
    {
      "propertyName": "Email",
      "errorMessage": "البريد الإلكتروني مطلوب"
    },
    {
      "propertyName": "Email",
      "errorMessage": "البريد الإلكتروني غير صحيح"
    },
    {
      "propertyName": "Password",
      "errorMessage": "كلمة المرور مطلوبة"
    },
    {
      "propertyName": "Password",
      "errorMessage": "كلمة المرور يجب أن تكون 3 أحرف على الأقل"
    }
  ],
  "message": "Validation failed",
  "isSuccess": false,
  "state": 400
}
```

## الاختبار
1. تشغيل الباك إند: `dotnet run` في مجلد `Management`
2. تشغيل الفرونت إند: `npm run dev` في مجلد `specificsolutions.endowment.vue`
3. محاولة تسجيل دخول ببيانات غير صحيحة
4. التحقق من أن الأخطاء محددة لكل حقل في الاستجابة وفي الواجهة

## ملاحظات
- تم الحفاظ على `LoginCommandValidator` كما هو لأنه يحتوي على قواعد التحقق الصحيحة
- `GlobalExceptionHandler` يتعامل مع `ValidationException` ويعيد الأخطاء بالشكل الصحيح
- `ValidationPipelineBehavior` يعمل الآن بشكل صحيح مع `LoginCommand`
- الفرونت إند الآن يعالج الأخطاء المحددة للحقول في الاستجابة المباشرة بدلاً من الاعتماد على catch block 