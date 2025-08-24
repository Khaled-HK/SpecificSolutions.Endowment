# إعدادات البيئات المختلفة

## نظرة عامة

هذا المشروع يدعم 4 بيئات مختلفة مع إعدادات مخصصة لكل منها:

> **⚠️ ملاحظة مهمة**: البيئة الافتراضية هي **Development** - أي أن التطبيق سيستخدم `appsettings.Development.json` تلقائياً

### 1. **Development (التطوير)** ⭐ **الافتراضي**
- **الملف**: `appsettings.Development.json`
- **الاستخدام**: للتطوير المحلي
- **المميزات**:
  - ✅ `UseSmtp: false` - لا يرسل بريد فعلي
  - ✅ `EnableFallbackLogging: true` - يسجل في Console
  - ✅ `MaxEmailsPerDay: 1000` - حد مرتفع للتطوير
  - ✅ `EmailRateLimit: 20` - معدل مرتفع
  - ✅ `QueueProcessingIntervalSeconds: 5` - معالجة سريعة

### 2. **Testing (الاختبار)**
- **الملف**: `appsettings.Testing.json`
- **الاستخدام**: لاختبار الوحدة والتكامل
- **المميزات**:
  - ✅ `UseSmtp: false` - لا يرسل بريد فعلي
  - ✅ `EnableEmailQueue: false` - معالجة فورية
  - ✅ `EnableEmailTemplates: false` - نصوص بسيطة
  - ✅ `MaxEmailsPerDay: 2000` - حد عالي للاختبارات

### 3. **Staging (الانتقالية)**
- **الملف**: `appsettings.Staging.json`
- **الاستخدام**: للاختبار قبل الإنتاج
- **المميزات**:
  - ✅ `UseSmtp: true` - يرسل بريد فعلي
  - ✅ `MaxEmailsPerDay: 500` - حد Gmail المجاني
  - ✅ `EmailRateLimit: 10` - معدل آمن
  - ✅ `EnableFallbackLogging: false` - لا تسجيل إضافي

### 4. **Production (الإنتاج)**
- **الملف**: `appsettings.Production.json`
- **الاستخدام**: للبيئة الإنتاجية
- **المميزات**:
  - ✅ `UseSmtp: true` - يرسل بريد فعلي
  - ✅ `RetryAttempts: 5` - محاولات أكثر
  - ✅ `RetryDelaySeconds: 3` - تأخير أطول
  - ✅ `QueueProcessingIntervalSeconds: 15` - معالجة أبطأ

## كيفية تحديد البيئة

### 1. **متغير البيئة**
```bash
# Windows
set ASPNETCORE_ENVIRONMENT=Development

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Development
```

### 2. **في Visual Studio**
- Properties → Debug → Environment Variables
- أضف: `ASPNETCORE_ENVIRONMENT = Development`

### 3. **في launchSettings.json**
```json
{
  "profiles": {
    "Development": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Staging": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Staging"
      }
    },
    "Production": {
      "commandName": "Project",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    }
  }
}
```

### 4. **في Docker**
```dockerfile
ENV ASPNETCORE_ENVIRONMENT=Production
```

### 5. **في Azure/AWS**
- Environment Variables → ASPNETCORE_ENVIRONMENT = Production

## ترتيب تحميل الإعدادات

.NET Core يحمل الإعدادات بالترتيب التالي (الأولوية للأخير):

1. `appsettings.json` (الإعدادات الأساسية)
2. `appsettings.{Environment}.json` (إعدادات البيئة)
3. متغيرات البيئة (Environment Variables)
4. User Secrets (للتطوير فقط)

## مثال على الإعدادات

### Development
```json
{
  "EmailSettings": {
    "UseSmtp": false,
    "EnableFallbackLogging": true,
    "MaxEmailsPerDay": 1000
  }
}
```

### Production
```json
{
  "EmailSettings": {
    "UseSmtp": true,
    "EnableFallbackLogging": false,
    "MaxEmailsPerDay": 500
  }
}
```

## نصائح مهمة

### 1. **كلمات المرور**
- لا تضع كلمات مرور حقيقية في ملفات الإعدادات
- استخدم User Secrets للتطوير
- استخدم متغيرات البيئة للإنتاج

### 2. **User Secrets (للتطوير)**
```bash
dotnet user-secrets set "EmailSettings:SmtpPassword" "your-password"
```

### 3. **متغيرات البيئة (للإنتاج)**
```bash
export EmailSettings__SmtpPassword="your-production-password"
```

### 4. **Azure Key Vault (للإنتاج)**
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{vaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

## التحقق من البيئة الحالية

```csharp
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"Current Environment: {environment}");
```

## تشغيل التطبيق

```bash
# التطوير
dotnet run --environment Development

# الاختبار
dotnet run --environment Testing

# الانتقالية
dotnet run --environment Staging

# الإنتاج
dotnet run --environment Production
```
