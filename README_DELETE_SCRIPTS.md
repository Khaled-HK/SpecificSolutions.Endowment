# Scripts حذف البيانات من قاعدة البيانات
## Database Data Deletion Scripts

تم إنشاء مجموعة من الـ scripts لحذف جميع البيانات من كافة الجداول في قاعدة البيانات `Swagger_Endowment22`.

## الملفات المتوفرة

### 1. `delete_all_data_from_all_tables.sql`
**النسخة السريعة والمباشرة**

- يحذف جميع البيانات من كافة الجداول بسرعة
- يعيد تعيين Identity Seeds تلقائياً
- مناسب للبيئات التطويرية
- **تحذير**: لا يوفر إمكانية التراجع

### 2. `delete_all_data_safe_version.sql`
**النسخة الآمنة والموصى بها**

- يتضمن تقرير حالة البيانات قبل وبعد الحذف
- يتحقق من وجود كل جدول قبل محاولة الحذف
- يتضمن معالجة الأخطاء (Error Handling)
- إمكانية استخدام Transactions للتراجع
- رسائل تقدم العملية باللغتين العربية والإنجليزية

## كيفية الاستخدام

### الخطوات الأساسية:

1. **قم بفتح SQL Server Management Studio (SSMS)**

2. **اتصل بالسيرفر وقاعدة البيانات**
   ```sql
   Server: . (Local Instance)
   Database: Swagger_Endowment22
   ```

3. **اختر الـ Script المناسب:**
   - للاستخدام السريع: `delete_all_data_from_all_tables.sql`
   - للاستخدام الآمن: `delete_all_data_safe_version.sql`

4. **قم بتنفيذ الـ Script**

### للاستخدام الآمن مع إمكانية التراجع:

1. **افتح `delete_all_data_safe_version.sql`**

2. **قم بإلغاء التعليق عن أسطر Transaction:**
   ```sql
   BEGIN TRANSACTION DeleteAllData;
   PRINT 'Transaction started - يمكنك استخدام ROLLBACK للتراجع'
   ```

3. **نفذ الـ Script**

4. **إذا كنت راضياً عن النتيجة، قم بإلغاء التعليق وتنفيذ:**
   ```sql
   COMMIT TRANSACTION DeleteAllData;
   ```

5. **إذا كنت تريد التراجع:**
   ```sql
   ROLLBACK TRANSACTION DeleteAllData;
   ```

## الجداول التي يتم حذفها

### جداول الهوية (Identity)
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens
- AspNetRoleClaims
- AspNetUsers
- AspNetRoles

### جداول الطلبات
- BuildingDetailRequests
- ChangeOfPathRequests
- ConstructionRequests
- DemolitionRequests
- ExpenditureChangeRequests
- MaintenanceRequests
- NameChangeRequests
- NeedsRequests
- Requests

### جداول التفاصيل
- AccountDetails
- BuildingDetails
- FacilityDetails

### جداول التدقيق
- AuditLogs

### الكيانات الرئيسية
- Accounts
- Buildings
- Facilities
- Mosques
- QuranicSchools
- Offices
- Products
- Decisions

### الجداول المرجعية
- Branchs
- Cities
- Regions
- Banks

## ميزات الأمان

### في النسخة الآمنة:
- ✅ فحص وجود الجداول قبل الحذف
- ✅ معالجة الأخطاء مع رسائل واضحة
- ✅ تقارير ما قبل وما بعد الحذف
- ✅ إمكانية استخدام Transactions
- ✅ إعادة تفعيل القيود في حالة الخطأ

### في النسختين:
- ✅ تعطيل وإعادة تفعيل Foreign Key Constraints
- ✅ ترتيب الحذف حسب العلاقات
- ✅ تجنب حذف جداول النظام والـ Migrations

## إعادة تعيين Identity Seeds

### تلقائياً (النسخة السريعة):
يتم إعادة تعيين جميع الـ Identity Seeds إلى 0 تلقائياً.

### يدوياً (النسخة الآمنة):
قم بإلغاء التعليق عن القسم المخصص لإعادة تعيين الـ Identity Seeds:

```sql
/*
PRINT 'إعادة تعيين Identity Seeds...'
DBCC CHECKIDENT ('TableName', RESEED, 0);
...
PRINT 'تم إعادة تعيين Identity Seeds'
*/
```

## تحذيرات مهمة

⚠️ **هذه العملية لا يمكن التراجع عنها إلا باستخدام Transactions**

⚠️ **تأكد من أخذ نسخة احتياطية قبل التنفيذ**

⚠️ **لا تستخدم هذه Scripts في بيئة الإنتاج بدون تأكد تام**

⚠️ **تأكد من أن لديك الصلاحيات المناسبة لحذف البيانات**

## استعادة البيانات

بعد حذف البيانات، يمكنك استخدام ملفات الـ Seed الموجودة لاستعادة البيانات الأساسية:

```sql
-- ملفات Seed المتوفرة:
run_all_seed_data.sql
seed_banks.sql
seed_cities.sql
seed_regions.sql
seed_products.sql
seed_offices.sql
seed_mosques_buildings_products.sql
-- ... والمزيد
```

## الدعم والمساعدة

في حالة مواجهة أي مشاكل:
1. تحقق من رسائل الخطأ في Output
2. تأكد من صحة اسم قاعدة البيانات
3. تأكد من وجود الصلاحيات المناسبة
4. راجع Event Log في حالة أخطاء النظام