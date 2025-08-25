# دليل تطبيق نظام رموز التحقق في الباك إند

## نظرة عامة

تم إنشاء نظام شامل لإدارة رموز التحقق الرقمية (6 أرقام) في الباك إند مع دعم كامل للأمان والتتبع.

## الملفات المطلوبة

### 1. **Entity** ✅
- `SpecificSolutions.Endowment.Core/Entities/VerificationCode.cs`

### 2. **Repository Interface** ✅
- `SpecificSolutions.Endowment.Application/Abstractions/IRepositories/IVerificationCodeRepository.cs`

### 3. **Service** ✅
- `SpecificSolutions.Endowment.Application/Services/VerificationCodeService.cs`

### 4. **DTOs** ✅
- `SpecificSolutions.Endowment.Application/Models/DTOs/VerificationCodeDtos.cs`

### 5. **Handlers** ✅
- `SpecificSolutions.Endowment.Application/Handlers/Authentications/SendVerificationCodeHandler.cs`
- `SpecificSolutions.Endowment.Application/Handlers/Authentications/VerifyCodeHandler.cs`

### 6. **Controller** ✅
- `SpecificSolutions.Endowment.Api/Controllers/VerificationCodeController.cs`

## الخطوات المطلوبة للتطبيق

### 1. **إنشاء جدول قاعدة البيانات**

```sql
CREATE TABLE VerificationCodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(256) NOT NULL,
    Code NVARCHAR(6) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ExpiresAt DATETIME2 NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    IsExpired BIT NOT NULL DEFAULT 0,
    Purpose NVARCHAR(50) NULL,
    IpAddress NVARCHAR(50) NULL,
    UserAgent NVARCHAR(500) NULL,
    UserId NVARCHAR(450) NULL,
    CONSTRAINT FK_VerificationCodes_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- إنشاء Indexes للأداء
CREATE INDEX IX_VerificationCodes_Email_Purpose ON VerificationCodes(Email, Purpose);
CREATE INDEX IX_VerificationCodes_Code ON VerificationCodes(Code);
CREATE INDEX IX_VerificationCodes_ExpiresAt ON VerificationCodes(ExpiresAt);
CREATE INDEX IX_VerificationCodes_IsUsed_IsExpired ON VerificationCodes(IsUsed, IsExpired);
```

### 2. **إنشاء Repository Implementation**

```csharp
// SpecificSolutions.Endowment.Infrastructure/Persistence/Repositories/VerificationCodeRepository.cs
public class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly ApplicationDbContext _context;

    public VerificationCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VerificationCode?> GetValidCodeAsync(string email, string code, string purpose)
    {
        return await _context.VerificationCodes
            .Where(vc => vc.Email == email && 
                        vc.Code == code && 
                        vc.Purpose == purpose &&
                        vc.ExpiresAt > DateTime.UtcNow &&
                        !vc.IsUsed &&
                        !vc.IsExpired)
            .FirstOrDefaultAsync();
    }

    public async Task<VerificationCode> CreateAsync(VerificationCode verificationCode)
    {
        _context.VerificationCodes.Add(verificationCode);
        await _context.SaveChangesAsync();
        return verificationCode;
    }

    public async Task<bool> UpdateAsync(VerificationCode verificationCode)
    {
        _context.VerificationCodes.Update(verificationCode);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteExpiredCodesAsync()
    {
        var expiredCodes = await _context.VerificationCodes
            .Where(vc => vc.ExpiresAt <= DateTime.UtcNow || vc.IsUsed || vc.IsExpired)
            .ToListAsync();

        _context.VerificationCodes.RemoveRange(expiredCodes);
        return await _context.SaveChangesAsync() > 0;
    }

    // ... باقي الطرق
}
```

### 3. **تسجيل الخدمات في DI Container**

```csharp
// SpecificSolutions.Endowment.Application/ApplicationContainer.cs
public static class ApplicationContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // ... الخدمات الموجودة

        // إضافة خدمات رموز التحقق
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<VerificationCodeService>();

        return services;
    }
}
```

### 4. **تحديث RegisterHandler لإرسال رمز التحقق**

```csharp
// في RegisterHandler
public async Task<Response<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
{
    // ... الكود الموجود

    // بعد إنشاء المستخدم بنجاح
    if (userCreated)
    {
        // إرسال رمز التحقق
        await _verificationCodeService.SendVerificationCodeAsync(
            request.Email,
            "EmailConfirmation",
            user.Id);

        return Response<RegisterResponse>.SuccessResponse(
            new RegisterResponse
            {
                IsSuccess = true,
                Message = "تم التسجيل بنجاح. يرجى التحقق من بريدك الإلكتروني لتأكيد الحساب.",
                RequiresEmailConfirmation = true
            },
            "تم التسجيل بنجاح");
    }

    // ... باقي الكود
}
```

## API Endpoints

### 1. **إرسال رمز التحقق**
```http
POST /api/verificationcode/send
Content-Type: application/json

{
  "email": "user@example.com",
  "purpose": "EmailConfirmation"
}
```

### 2. **التحقق من الرمز**
```http
POST /api/verificationcode/verify
Content-Type: application/json

{
  "email": "user@example.com",
  "code": "123456",
  "purpose": "EmailConfirmation"
}
```

### 3. **إعادة إرسال الرمز**
```http
POST /api/verificationcode/resend
Content-Type: application/json

{
  "email": "user@example.com",
  "purpose": "EmailConfirmation"
}
```

### 4. **تأكيد البريد الإلكتروني (للتوافق مع الفرونت إند)**
```http
POST /api/verificationcode/confirm-email-with-code
Content-Type: application/json

{
  "email": "user@example.com",
  "code": "123456"
}
```

## إعدادات التكوين

### appsettings.json
```json
{
  "VerificationCode": {
    "ExpirationMinutes": 15,
    "MaxCodesPerEmail": 3,
    "CodeLength": 6,
    "CleanupExpiredCodesIntervalMinutes": 60
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "noreply@endowment.com",
    "FromName": "نظام الأوقاف"
  }
}
```

## ميزات الأمان

### ✅ **حماية من التخمين**
- رمز عشوائي من 6 أرقام (1,000,000 احتمال)
- صلاحية محدودة (15 دقيقة)
- استخدام مرة واحدة فقط

### ✅ **حماية من الإساءة**
- حد أقصى 3 رموز نشطة لكل بريد إلكتروني
- تتبع IP Address و User Agent
- حذف الرموز المنتهية تلقائياً

### ✅ **تتبع الأمان**
- تسجيل جميع المحاولات
- تتبع IP Address
- تتبع User Agent

## البريد الإلكتروني

### محتوى البريد الإلكتروني
```html
<div dir='rtl' style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
    <div style='background-color: #f8f9fa; padding: 20px; border-radius: 10px; text-align: center;'>
        <h2 style='color: #333; margin-bottom: 20px;'>رمز التحقق</h2>
        
        <div style='background-color: #fff; padding: 30px; border-radius: 8px; border: 2px solid #e9ecef; margin: 20px 0;'>
            <h1 style='color: #007bff; font-size: 48px; margin: 0; letter-spacing: 10px; font-weight: bold;'>123456</h1>
        </div>
        
        <p style='color: #666; font-size: 16px; line-height: 1.6;'>
            رمز التحقق الخاص بك لـ <strong>تأكيد البريد الإلكتروني</strong> هو:
        </p>
        
        <div style='background-color: #e7f3ff; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p style='color: #0056b3; margin: 0; font-weight: bold;'>
                ⏰ هذا الرمز صالح لمدة 15 دقيقة فقط
            </p>
        </div>
        
        <div style='background-color: #fff3cd; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p style='color: #856404; margin: 0; font-size: 14px;'>
                🔒 إذا لم تطلب هذا الرمز، يمكنك تجاهل هذا البريد الإلكتروني بأمان
            </p>
        </div>
    </div>
</div>
```

## الاختبار

### 1. **اختبار إرسال الرمز**
```bash
curl -X POST "https://localhost:7140/api/verificationcode/send" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "purpose": "EmailConfirmation"
  }'
```

### 2. **اختبار التحقق من الرمز**
```bash
curl -X POST "https://localhost:7140/api/verificationcode/verify" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "code": "123456",
    "purpose": "EmailConfirmation"
  }'
```

## ملاحظات مهمة

- **مدة الصلاحية**: 15 دقيقة (قابلة للتعديل)
- **عدد المحاولات**: حد أقصى 3 رموز نشطة لكل بريد
- **التخزين**: في قاعدة البيانات مع تاريخ الانتهاء
- **الأمان**: حذف الرموز المنتهية تلقائياً
- **التتبع**: تسجيل IP Address و User Agent

## الخطوات التالية

1. **إنشاء جدول قاعدة البيانات**
2. **إنشاء Repository Implementation**
3. **تسجيل الخدمات في DI Container**
4. **تحديث RegisterHandler**
5. **اختبار النظام**

النظام جاهز للاستخدام مع الفرونت إند المحدث! 🚀
