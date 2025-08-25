# API Requirements for Email Confirmation - Backend

## نظرة عامة

يجب أن يعيد الباك إند حقل محدد في الاستجابة لتحديد حالة عدم تأكيد البريد الإلكتروني، بدلاً من الاعتماد على تحليل النص في الفرونت إند.

## التوقعات من الباك إند

### عند تسجيل الدخول - حالة عدم تأكيد البريد الإلكتروني

**الاستجابة المتوقعة من الباك إند:**

```json
{
  "isSuccess": false,
  "state": 400,
  "message": "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول",
  "requiresEmailConfirmation": true,  // ← الحقل المطلوب
  "emailNotConfirmed": true,          // ← بديل للحقل السابق
  "data": {
    "requiresEmailConfirmation": true  // ← بديل آخر
  }
}
```

### الحقول المقبولة

الفرونت إند يتوقع أحد هذه الحقول:

1. **`requiresEmailConfirmation: true`** (الأولوية الأولى)
2. **`emailNotConfirmed: true`** (بديل)
3. **`data.requiresEmailConfirmation: true`** (بديل)

### مثال على الاستجابة الكاملة

```json
{
  "isSuccess": false,
  "state": 400,
  "message": "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول",
  "requiresEmailConfirmation": true,
  "errors": [],
  "data": null
}
```

## التنفيذ في الباك إند

### في Login Handler

```csharp
// في حالة عدم تأكيد البريد الإلكتروني
if (!user.EmailConfirmed)
{
    return Response.FailureResponse(
        message: "يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول",
        requiresEmailConfirmation: true  // ← إضافة هذا الحقل
    );
}
```

### في Response Model

```csharp
public class LoginResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public bool RequiresEmailConfirmation { get; set; }  // ← إضافة هذا الحقل
    public object Data { get; set; }
    public List<string> Errors { get; set; }
}
```

## المزايا

1. **نمط خالد**: الباك إند يحدد الحالة، الفرونت إند يعالج فقط
2. **دقة أعلى**: لا يعتمد على تحليل النص
3. **سهولة الصيانة**: تغيير الرسالة لا يؤثر على المنطق
4. **مرونة**: يمكن إضافة حالات أخرى بسهولة

## الحالات الأخرى

### تسجيل الدخول الناجح

```json
{
  "isSuccess": true,
  "state": 200,
  "message": "تم تسجيل الدخول بنجاح",
  "requiresEmailConfirmation": false,
  "data": {
    "token": "...",
    "user": { ... }
  }
}
```

### خطأ آخر (غير عدم التأكيد)

```json
{
  "isSuccess": false,
  "state": 400,
  "message": "كلمة المرور غير صحيحة",
  "requiresEmailConfirmation": false,
  "errors": ["كلمة المرور غير صحيحة"]
}
```

## التطوير المستقبلي

يمكن إضافة حقول أخرى للحالات المختلفة:

```json
{
  "isSuccess": false,
  "requiresEmailConfirmation": false,
  "accountLocked": false,
  "passwordExpired": false,
  "twoFactorRequired": false,
  "message": "رسالة الخطأ"
}
```

## ملاحظات مهمة

1. **الفرونت إند لا يحلل النص**: يعتمد فقط على الحقول المحددة
2. **الباك إند يحدد الحالة**: مسؤولية تحديد نوع الخطأ
3. **سهولة التوسع**: إضافة حالات جديدة لا يتطلب تغيير الفرونت إند
4. **التوافق مع نمط خالد**: فصل المسؤوليات بشكل واضح
