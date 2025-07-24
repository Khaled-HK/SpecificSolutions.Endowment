# 🚨 إصلاح عاجل: مشكلة الترميز العربي في التطبيق
# 🚨 Urgent Fix: Arabic Encoding Issue in Application

## 📋 **المشكلة المكتشفة**

### ❌ **ما يظهر حالياً:**
```
ط…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط·ط±ط§ط¨ظ„ط³
ط…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط¨ظ†ط؛ط§ط²ظٹ
ط…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ظ…طµط±ط§طھط©
```

### ✅ **ما يجب أن يظهر:**
```
مكتب الأوقاف - طرابلس
مكتب الأوقاف - بنغازي
مكتب الأوقاف - مصراتة
```

---

## 🚀 **الإصلاح السريع**

### **الطريقة الأولى: تشغيل تلقائي**
```bash
# Windows
double-click on: run_urgent_fix.bat

# أو من Command Prompt
run_urgent_fix.bat
```

### **الطريقة الثانية: يدوياً**
1. افتح **SQL Server Management Studio**
2. اتصل بقاعدة البيانات `Swagger_Endowment22`
3. افتح ملف `urgent_arabic_fix_complete.sql`
4. اضغط **Execute** (F5)

---

## 📋 **ما يفعله هذا الإصلاح**

### **1. إصلاح قاعدة البيانات:**
- ✅ تغيير Database Collation إلى `Arabic_CI_AS`
- ✅ تحويل أعمدة النصوص من `VARCHAR` إلى `NVARCHAR`
- ✅ تطبيق `Arabic_CI_AS` على جميع الأعمدة النصية

### **2. تنظيف البيانات:**
- ✅ حذف البيانات المشوهة/المعطوبة
- ✅ إعادة إدراج بيانات ليبية صحيحة
- ✅ استخدام `N''` prefix للنصوص العربية

### **3. الجداول المتأثرة:**
- 🏦 `Banks` - البنوك
- 🏙️ `Cities` - المدن  
- 🗺️ `Regions` - المناطق
- 🏢 `Offices` - المكاتب
- 🏗️ `Buildings` - المباني
- 🕌 `Mosques` - المساجد

---

## 📊 **البيانات الليبية الجديدة**

### **🏦 البنوك:**
- مصرف الجمهورية
- البنك التجاري الوطني
- مصرف الوحدة
- البنك الأهلي الليبي
- مصرف الصحاري

### **🏙️ المدن:**
- طرابلس
- بنغازي
- مصراتة
- الزاوية
- شحات

### **🏢 مكاتب الأوقاف:**
- مكتب الأوقاف - طرابلس (0218-84-1234579)
- مكتب الأوقاف - بنغازي (0218-84-1234584)
- مكتب الأوقاف - مصراتة (0218-18-1234572)
- مكتب الأوقاف - الزاوية (0218-18-1234568)
- مكتب الأوقاف - شحات (0218-84-1234568)

---

## 🔧 **خطوات ما بعد الإصلاح**

### **1. إعادة تشغيل التطبيق:**
```bash
# إيقاف IIS/Application Pool
iisreset /stop

# إعادة تشغيل
iisreset /start
```

### **2. تنظيف Browser Cache:**
- Chrome: `Ctrl+Shift+Delete`
- Firefox: `Ctrl+Shift+Delete`
- Edge: `Ctrl+Shift+Delete`

### **3. فحص النتائج:**
```sql
-- جرب هذه الاستعلامات في SSMS:
SELECT TOP 5 Name FROM Offices
SELECT TOP 5 Name FROM Regions  
SELECT TOP 5 Name FROM Cities
```

---

## ⚠️ **إذا لم يعمل الإصلاح**

### **مشاكل محتملة إضافية:**

#### **1. مشكلة في Connection String:**
```xml
<!-- web.config أو appsettings.json -->
<add name="DefaultConnection" 
     connectionString="Server=.;Database=Swagger_Endowment22;Trusted_Connection=True;
     MultipleActiveResultSets=true;trustservercertificate=true;charset=utf8;" />
```

#### **2. مشكلة في الكود C#:**
```csharp
// تأكد من استخدام UTF-8 في API Controller
[ApiController]
[Produces("application/json; charset=utf-8")]
public class OfficesController : ControllerBase
{
    // ...
}
```

#### **3. مشكلة في Frontend:**
```html
<!-- تأكد من وجود هذا في HTML head -->
<meta charset="UTF-8">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
```

#### **4. مشكلة في HTTP Headers:**
```csharp
// في Startup.cs أو Program.cs
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
    await next();
});
```

---

## 🔍 **اختبار النتائج**

### **في التطبيق:**
1. افتح صفحة المكاتب
2. تحقق من أن النصوص تظهر كالتالي:
   ```
   ✅ مكتب الأوقاف - طرابلس
   ✅ مكتب الأوقاف - بنغازي
   ✅ مكتب الأوقاف - مصراتة
   ```

### **في قاعدة البيانات:**
```sql
-- يجب أن تظهر النتائج بالعربية الصحيحة
SELECT o.Name as OfficeName, r.Name as RegionName, o.PhoneNumber
FROM Offices o
LEFT JOIN Regions r ON o.RegionId = r.Id
ORDER BY o.Name
```

---

## 📁 **الملفات المتضمنة**

- ✅ `urgent_arabic_fix_complete.sql` - الإصلاح الرئيسي
- ✅ `run_urgent_fix.bat` - تشغيل تلقائي
- ✅ `README_URGENT_FIX.md` - هذا الدليل

---

## 💡 **نصائح مهمة**

### **للمطورين:**
- 🔤 استخدم `NVARCHAR` بدلاً من `VARCHAR` للنصوص العربية
- 🔤 استخدم `N''` prefix عند إدراج البيانات العربية
- 🔤 تأكد من `Arabic_CI_AS` collation في جميع الأعمدة النصية

### **للمدراء:**
- 🖥️ أعد تشغيل الخادم بعد الإصلاح
- 🖥️ تأكد من أن جميع المستخدمين خرجوا من النظام أثناء الإصلاح
- 🖥️ اختبر التطبيق في متصفحات مختلفة

### **للمستخدمين:**
- 🌐 امسح cache المتصفح
- 🌐 أعد تحميل الصفحة بـ `Ctrl+F5`
- 🌐 تأكد من أن المتصفح يدعم UTF-8

---

## 🆘 **الحصول على المساعدة**

إذا استمرت المشكلة بعد تطبيق هذا الإصلاح:

1. **تحقق من ملف السجل:** `urgent_fix_output.log`
2. **جرب الإصلاح اليدوي** في SQL Server Management Studio
3. **تأكد من صلاحيات المستخدم** لتعديل قاعدة البيانات
4. **فحص Connection String** في ملف الإعدادات

---

## 🎯 **النتيجة المتوقعة**

بعد تطبيق هذا الإصلاح، يجب أن يعرض التطبيق البيانات العربية بشكل صحيح:

```
✅ مكتب الأوقاف - طرابلس
✅ مكتب الأوقاف - بنغازي  
✅ مكتب الأوقاف - مصراتة
✅ مكتب الأوقاف - الزاوية
✅ مكتب الأوقاف - شحات
```

**بدلاً من النص المشوه الحالي! 🇱🇾**