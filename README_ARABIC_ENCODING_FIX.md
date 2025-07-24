# حل مشاكل الترميز العربي
## Arabic Encoding Issues Fix

---

## 🚨 المشكلة المكتشفة

### ما تراه الآن:
```
ظ…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط´ط­ط§طھ
```

### ما يجب أن تراه:
```
مكتب الأوقاف - شحات
```

---

## 🔍 سبب المشكلة

المشكلة تحدث بسبب:
- **عدم ضبط Collation** صحيح لقاعدة البيانات
- **عدم استخدام NVARCHAR** مع Unicode
- **عدم استخدام N''** في النصوص العربية
- **إعدادات خاطئة** في SQL Server لدعم العربية

---

## 🛠️ الحل السريع

### 📌 الطريقة الأولى: التشغيل التلقائي
```batch
# اضغط دبل كليك على:
fix_arabic_encoding.bat
```

### 📌 الطريقة الثانية: SSMS
1. افتح **SQL Server Management Studio**
2. اتصل بقاعدة البيانات
3. افتح ملف `fix_arabic_encoding.sql`
4. اضغط **Execute** (F5)

---

## ⚙️ ما يحدث عند التصليح

### 🔧 التحديثات التقنية:
1. **تغيير Collation** قاعدة البيانات إلى `Arabic_CI_AS`
2. **تحديث جميع الأعمدة** النصية إلى `NVARCHAR` مع `Arabic_CI_AS`
3. **حذف البيانات** المشوهة
4. **إعادة إدراج البيانات** بترميز صحيح `N''`
5. **اختبار النتائج** بعرض عينة من البيانات

### 📋 الجداول المشمولة:
- ✅ **Banks** (البنوك)
- ✅ **Cities** (المدن)  
- ✅ **Regions** (المناطق)
- ✅ **Products** (المنتجات)
- ✅ **Offices** (المكاتب)
- ✅ **Facilities** (المرافق)
- ✅ **Buildings** (المباني)
- ✅ **Accounts** (الحسابات)

---

## 📊 النتيجة المتوقعة

### قبل الإصلاح:
```sql
ظ…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط´ط­ط§طھ
ظ…طµط±ظپ ط§ظ„ط¬ظ…ظ‡ظˆط±ظٹط©
ط§ظ„ط¨ظ†ظƒ ط§ظ„طھط¬ط§ط±ظٹ ط§ظ„ظˆط·ظ†ظٹ
```

### بعد الإصلاح:
```sql
مكتب الأوقاف - شحات
مصرف الجمهورية  
البنك التجاري الوطني
مكتب الأوقاف - طرابلس
مكتب الأوقاف - بنغازي
```

---

## 🎯 خطوات التحقق من النجاح

### 1️⃣ فحص البيانات:
```sql
SELECT TOP 5 Name FROM [Banks]
SELECT TOP 5 Name FROM [Cities] 
SELECT TOP 5 Name FROM [Offices]
```

### 2️⃣ فحص Collation:
```sql
SELECT name, collation_name 
FROM sys.databases 
WHERE name = 'Swagger_Endowment22'
```

### 3️⃣ فحص نوع الأعمدة:
```sql
SELECT COLUMN_NAME, DATA_TYPE, COLLATION_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Banks' AND COLUMN_NAME = 'Name'
```

---

## ❌ إذا لم يعمل الحل

### 🔧 خطوات إضافية:

#### 1. إعدادات SQL Server Management Studio:
```
Tools > Options > Query Results > SQL Server > Results to Grid
☑️ Include column headers
☑️ Include Unicode characters
```

#### 2. تغيير Font في SSMS:
```
Tools > Options > Environment > Fonts and Colors
Font: Arial Unicode MS
```

#### 3. إعدادات المتصفح:
```
• Character Encoding: UTF-8
• في Chrome: Settings > Advanced > Languages
• في Firefox: View > Text Encoding > Unicode (UTF-8)
```

#### 4. فحص Connection String:
```csharp
"Server=.;Database=Swagger_Endowment22;Trusted_Connection=True;MultipleActiveResultSets=true;trustservercertificate=true;Charset=utf8;"
```

---

## 🚨 حلول متقدمة

### إذا استمرت المشكلة:

#### حل 1: إعادة بناء قاعدة البيانات
```sql
-- حفظ البيانات أولاً
BACKUP DATABASE [Swagger_Endowment22] TO DISK = 'C:\Backup\EndowmentBackup.bak'

-- إعادة إنشاء قاعدة البيانات
DROP DATABASE [Swagger_Endowment22]
CREATE DATABASE [Swagger_Endowment22] COLLATE Arabic_CI_AS

-- استرداد البيانات
RESTORE DATABASE [Swagger_Endowment22] FROM DISK = 'C:\Backup\EndowmentBackup.bak'
```

#### حل 2: تغيير Server Collation
```sql
-- يتطلب إعادة تشغيل SQL Server
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
```

#### حل 3: إنشاء جداول جديدة
```sql
-- إنشاء جداول جديدة بـ collation صحيح
CREATE TABLE [Banks_New] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(255) COLLATE Arabic_CI_AS NOT NULL,
    [Address] NVARCHAR(500) COLLATE Arabic_CI_AS,
    [ContactNumber] NVARCHAR(50) COLLATE Arabic_CI_AS
);

-- نقل البيانات
INSERT INTO [Banks_New] SELECT * FROM [Banks];
```

---

## 📱 فحص في تطبيقات مختلفة

### في C# / ASP.NET:
```csharp
// تأكد من استخدام
using System.Text;
Encoding.UTF8.GetString(data);

// في Entity Framework
[Column(TypeName = "nvarchar(255)")]
public string Name { get; set; }
```

### في JavaScript/Frontend:
```javascript
// تأكد من
<meta charset="UTF-8">
Content-Type: application/json; charset=UTF-8
```

---

## 🎯 البيانات المثبتة

بعد تشغيل الإصلاح بنجاح، ستجد:

### 🏦 البنوك الليبية:
- مصرف الجمهورية
- البنك التجاري الوطني  
- مصرف الوحدة
- البنك الأهلي الليبي
- مصرف الصحاري

### 🏙️ المدن الليبية:
- طرابلس
- بنغازي
- مصراتة  
- الزاوية
- شحات

### 🏢 مكاتب الأوقاف:
- مكتب الأوقاف - طرابلس
- مكتب الأوقاف - بنغازي
- مكتب الأوقاف - مصراتة
- مكتب الأوقاف - الزاوية
- مكتب الأوقاف - شحات

---

## ⚠️ تحذيرات مهمة

### 🔒 النسخ الاحتياطية:
```sql
-- اعمل backup قبل التشغيل
BACKUP DATABASE [Swagger_Endowment22] 
TO DISK = 'C:\Backup\BeforeEncodingFix.bak'
```

### 🔄 إعادة التشغيل:
- يمكن تشغيل الإصلاح عدة مرات
- لن يؤثر على البيانات الموجودة
- سيعيد إنشاء البيانات بترميز صحيح

### 🚫 تجنب:
- ❌ تشغيل الإصلاح أثناء استخدام النظام
- ❌ تعديل الملف أثناء التشغيل
- ❌ إغلاق النافذة أثناء العملية

---

## 📞 الدعم

### إذا استمرت المشكلة:
1. **فحص ملف اللوج**: `encoding_fix_output.log`
2. **تأكد من الصلاحيات**: تشغيل كـ Administrator
3. **فحص SQL Server**: تأكد من التشغيل
4. **فحص قاعدة البيانات**: تأكد من الوجود

---

## 🎉 النتيجة النهائية

بعد تطبيق الإصلاح بنجاح:
- ✅ **النصوص العربية** تظهر بشكل صحيح
- ✅ **جميع الجداول** محدثة بترميز Unicode
- ✅ **البيانات الليبية** جاهزة للاستخدام
- ✅ **التطبيق** جاهز للعمل بالعربية

---

*تم إنشاؤه خصيصاً لحل مشاكل الترميز العربي في نظام الأوقاف الليبية* 🇱🇾