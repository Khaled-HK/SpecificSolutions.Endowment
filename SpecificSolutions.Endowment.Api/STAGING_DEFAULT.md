# البيئة الافتراضية: Staging

## التغيير الجديد

تم تغيير البيئة الافتراضية من **Development** إلى **Staging**.

## ما يعنيه هذا:

### ✅ **عند تشغيل التطبيق**:
- سيستخدم `appsettings.Staging.json` تلقائياً
- `UseSmtp: true` - سيحاول إرسال بريد فعلي
- `MaxEmailsPerDay: 500` - حد Gmail المجاني
- `EmailRateLimit: 10` - 10 رسائل/دقيقة

### ⚠️ **تحذير مهم**:
- يجب إعداد حساب Gmail صحيح في `appsettings.Staging.json`
- أو تغيير `UseSmtp` إلى `false` إذا كنت تريد التطوير المحلي

## كيفية التبديل للبيئات الأخرى:

### للتطوير المحلي (بدون إرسال بريد):
```bash
dotnet run --environment Development
```

### للاختبار:
```bash
dotnet run --environment Testing
```

### للإنتاج:
```bash
dotnet run --environment Production
```

## إعداد Gmail للـ Staging:

1. أنشئ حساب Gmail: `staging-endowment@gmail.com`
2. فعّل المصادقة الثنائية
3. أنشئ كلمة مرور التطبيق
4. حدث `SmtpPassword` في `appsettings.Staging.json`

## أو استخدم User Secrets (أكثر أماناً):

```bash
dotnet user-secrets set "EmailSettings:SmtpPassword" "your-staging-password"
```
