# دمج خدمة البريد الإلكتروني (Email Service Integration)

## المشكلة الأصلية
كانت `IEmailService` موجودة ومُعرَّفة ولكنها **غير مُسجلة** في حاوية DI، وغير **مُستخدمة** في الخدمات التي تحتاج إليها.

## الحلول المطبقة

### 1. تسجيل IEmailService في حاوية DI
تم إضافة تسجيل `IEmailService` في `InfrastructureContainer.cs`:

```csharp
// Register Email Service
services.AddScoped<IEmailService, EmailService>();
```

### 2. تحديث PasswordService
تم تحديث `PasswordService` لاستخدام `IEmailService` في دالة `ForgotPasswordAsync`:

```csharp
public class PasswordService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISessionService _sessionService;
    private readonly IEmailService _emailService; // إضافة جديدة

    public PasswordService(UserManager<ApplicationUser> userManager, 
                          ISessionService sessionService, 
                          IEmailService emailService) // إضافة parameter جديد
    {
        _userManager = userManager;
        _sessionService = sessionService;
        _emailService = emailService;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new Exception("User not found with this email address.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        // إرسال بريد إعادة تعيين كلمة المرور
        var resetLink = $"https://yourapp.com/reset-password?email={email}&token={token}";
        var emailSent = await _emailService.SendPasswordResetAsync(email, resetLink);
        
        if (!emailSent)
        {
            throw new Exception("Failed to send password reset email.");
        }
        
        return true;
    }
}
```

### 3. تحديث RegistrationService
تم تحديث `RegistrationService` لاستخدام `IEmailService` في دالة `SendEmailConfirmationAsync`:

```csharp
public class RegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _dbContext;
    private readonly IEmailService _emailService; // إضافة جديدة

    public RegistrationService(UserManager<ApplicationUser> userManager, 
                              AppDbContext dbContext, 
                              IEmailService emailService) // إضافة parameter جديد
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _emailService = emailService;
    }

    private async Task SendEmailConfirmationAsync(ApplicationUser user)
    {
        try
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://yourapp.com/confirm-email?email={user.Email}&token={token}";

            // إرسال بريد تأكيد البريد الإلكتروني
            var emailSent = await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);
            
            if (!emailSent)
            {
                Console.WriteLine($"Failed to send confirmation email to {user.Email}");
            }
        }
        catch (Exception ex)
        {
            // لا نريد أن يفشل التسجيل بسبب مشكلة في إرسال البريد
            Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
        }
    }
}
```

### 4. إضافة using directive
تم إضافة using directive المطلوب في `RegistrationService.cs`:

```csharp
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
```

## الوظائف المتاحة في EmailService

### 1. SendEmailAsync
إرسال بريد إلكتروني عام:
```csharp
Task<bool> SendEmailAsync(string to, string subject, string body)
```

### 2. SendEmailConfirmationAsync
إرسال بريد تأكيد البريد الإلكتروني:
```csharp
Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink)
```

### 3. SendPasswordResetAsync
إرسال بريد إعادة تعيين كلمة المرور:
```csharp
Task<bool> SendPasswordResetAsync(string email, string resetLink)
```

### 4. ValidateEmailAsync
التحقق من صحة البريد الإلكتروني:
```csharp
Task<bool> ValidateEmailAsync(string email)
```

## ملاحظات مهمة

1. **التنفيذ الحالي**: `EmailService` يستخدم محاكاة لإرسال البريد (Console.WriteLine) مع `Task.Delay(100)`.
2. **التنفيذ الفعلي**: يمكن استبدال المحاكاة بـ:
   - SendGrid
   - MailKit
   - SMTP Server
   - أي خدمة بريد إلكتروني أخرى

3. **معالجة الأخطاء**: تم إضافة معالجة أخطاء مناسبة لضمان عدم فشل العمليات الرئيسية بسبب مشاكل البريد الإلكتروني.

4. **نمط خالد**: تم تطبيق نمط خالد في تنظيم الكود وفصل المسؤوليات.

## الخطوات التالية

1. تكوين خدمة بريد إلكتروني فعلية (SendGrid, MailKit, إلخ)
2. إضافة إعدادات البريد الإلكتروني في `appsettings.json`
3. اختبار وظائف البريد الإلكتروني
4. إضافة logging مناسب لعمليات البريد الإلكتروني
