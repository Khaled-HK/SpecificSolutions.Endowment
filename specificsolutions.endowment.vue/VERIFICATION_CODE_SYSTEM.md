# نظام رمز التحقق الرقمي - Verification Code System

## نظرة عامة

تم تحديث نظام تأكيد البريد الإلكتروني ليدعم **رمز التحقق الرقمي** (6 أرقام) بدلاً من الرابط التقليدي.

## الميزات الجديدة

### 🔢 **رمز التحقق الرقمي**
- **6 أرقام** (000000 - 999999)
- **صالح لمدة 15 دقيقة**
- **يمكن إعادة إرساله** عند انتهاء الصلاحية

### 📧 **البريد الإلكتروني الجديد**
```
مرحباً [اسم المستخدم]،

رمز التحقق الخاص بك هو: 123456

هذا الرمز صالح لمدة 15 دقيقة فقط.
إذا لم تطلب هذا الرمز، يمكنك تجاهل هذا البريد.

شكراً لك،
فريق [اسم التطبيق]
```

## الصفحات المحدثة

### 1. **صفحة تأكيد البريد الإلكتروني** (`/confirm-email`)
- **حقل رمز التحقق**: إدخال 6 أرقام
- **التحقق التلقائي**: من الرابط (للنظام القديم)
- **إعادة إرسال الرمز**: زر لإرسال رمز جديد

### 2. **صفحة إعادة إرسال الرمز** (`/resend-verification-code`)
- **إدخال البريد الإلكتروني**
- **إرسال رمز جديد**
- **التوجيه لصفحة التأكيد**

### 3. **صفحة عدم التأكيد** (`/email-not-confirmed`)
- **روابط محدثة** تشير للنظام الجديد

## API Endpoints المطلوبة

### 1. **إرسال رمز التحقق** (عند التسجيل)
```http
POST /api/auth/send-verification-code
Content-Type: application/json

{
  "email": "user@example.com"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "تم إرسال رمز التحقق بنجاح"
}
```

### 2. **تأكيد البريد الإلكتروني بالرمز**
```http
POST /api/auth/confirm-email-with-code
Content-Type: application/json

{
  "email": "user@example.com",
  "verificationCode": "123456"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "تم تأكيد البريد الإلكتروني بنجاح"
}
```

### 3. **إعادة إرسال رمز التحقق**
```http
POST /api/auth/resend-verification-code
Content-Type: application/json

{
  "email": "user@example.com"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "تم إرسال رمز التحقق الجديد بنجاح"
}
```

## التطبيق في الباك إند

### 1. **إنشاء رمز التحقق**
```csharp
public class VerificationCodeService
{
    public string GenerateCode()
    {
        // إنشاء رمز عشوائي من 6 أرقام
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }
    
    public async Task<bool> SendVerificationCode(string email, string code)
    {
        // إرسال البريد الإلكتروني مع الرمز
        var emailContent = GenerateEmailContent(code);
        return await _emailService.SendEmailAsync(email, "رمز التحقق", emailContent);
    }
}
```

### 2. **تخزين الرمز**
```csharp
public class VerificationCode
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}
```

### 3. **التحقق من الرمز**
```csharp
public async Task<bool> VerifyCode(string email, string code)
{
    var verificationCode = await _context.VerificationCodes
        .Where(vc => vc.Email == email && 
                     vc.Code == code && 
                     vc.ExpiresAt > DateTime.UtcNow && 
                     !vc.IsUsed)
        .FirstOrDefaultAsync();
        
    if (verificationCode != null)
    {
        verificationCode.IsUsed = true;
        await _context.SaveChangesAsync();
        return true;
    }
    
    return false;
}
```

## المزايا

### ✅ **أمان أعلى**
- رمز مؤقت (15 دقيقة)
- استخدام مرة واحدة فقط
- لا يمكن تخمينه بسهولة

### ✅ **تجربة مستخدم أفضل**
- رمز سهل التذكر (6 أرقام)
- لا حاجة لنسخ/لصق الروابط
- يعمل على جميع الأجهزة

### ✅ **مرونة أكبر**
- يمكن إعادة الإرسال
- دعم للهواتف المحمولة
- لا يعتمد على متصفح معين

## التوافق مع النظام القديم

النظام الجديد **يدعم كلا الطريقتين**:

1. **النظام الجديد**: رمز التحقق الرقمي
2. **النظام القديم**: رابط التأكيد (للتوافق)

## الاختبار

### 1. **اختبار التسجيل**
- سجل حساب جديد
- تحقق من استلام رمز التحقق
- أدخل الرمز في صفحة التأكيد

### 2. **اختبار إعادة الإرسال**
- انتظر انتهاء صلاحية الرمز
- استخدم زر "إعادة إرسال"
- تحقق من استلام رمز جديد

### 3. **اختبار الخطأ**
- أدخل رمز خاطئ
- تحقق من رسالة الخطأ
- جرب إعادة الإرسال

## ملاحظات مهمة

- **مدة الصلاحية**: 15 دقيقة (قابلة للتعديل)
- **عدد المحاولات**: غير محدود (قابل للتقييد)
- **التخزين**: في قاعدة البيانات مع تاريخ الانتهاء
- **الأمان**: حذف الرموز المنتهية تلقائياً
