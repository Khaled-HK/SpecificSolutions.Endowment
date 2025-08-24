# 🔒 إرشادات الأمان - نظام الأوقاف

## ⚠️ **تحذير مهم**

**لا تقم أبدًا برفع كلمات المرور أو البيانات الحساسة إلى GitHub!**

## 🛠️ **إعداد المتغيرات البيئية**

### 1. إنشاء ملف `.env` محلي
```bash
# Email Settings
SMTP_PASSWORD=your_gmail_app_password_here
SMTP_USERNAME=your_gmail_username_here
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587

# Database Connection
CONNECTION_STRING=your_database_connection_string_here

# JWT Settings
JWT_SECRET=your_jwt_secret_here
JWT_ISSUER=your_jwt_issuer_here
JWT_AUDIENCE=your_jwt_audience_here
```

### 2. إعداد متغيرات البيئة في Windows
```powershell
# إعداد كلمة مرور SMTP
[Environment]::SetEnvironmentVariable("SMTP_PASSWORD", "your_gmail_app_password", "User")

# إعداد متغيرات أخرى
[Environment]::SetEnvironmentVariable("JWT_SECRET", "your_jwt_secret", "User")
```

### 3. إعداد متغيرات البيئة في Linux/Mac
```bash
# إضافة إلى ~/.bashrc أو ~/.zshrc
export SMTP_PASSWORD="your_gmail_app_password"
export JWT_SECRET="your_jwt_secret"
```

## 🔐 **إعداد Gmail App Password**

1. اذهب إلى [Google Account Settings](https://myaccount.google.com/)
2. اختر "Security" → "2-Step Verification"
3. اختر "App passwords"
4. أنشئ كلمة مرور جديدة للتطبيق
5. استخدم هذه الكلمة في متغير `SMTP_PASSWORD`

## 🚨 **ما يجب فعله الآن**

1. **غير كلمة مرور Gmail App Password** فورًا
2. **احذف البيانات الحساسة من Git History**
3. **أضف `.env` إلى `.gitignore`**
4. **استخدم متغيرات البيئة بدلاً من القيم المباشرة**

## 📋 **قائمة الملفات المحمية**

- `.env` - متغيرات البيئة المحلية
- `appsettings.Production.json` - إعدادات الإنتاج
- أي ملف يحتوي على كلمات مرور أو مفاتيح API

## 🔍 **فحص الأمان**

```bash
# فحص الملفات للبيانات الحساسة
grep -r "password\|secret\|key" . --exclude-dir=node_modules --exclude-dir=bin --exclude-dir=obj
```
