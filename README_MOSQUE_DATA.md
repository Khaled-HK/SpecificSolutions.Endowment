# دليل إضافة بيانات المساجد والمكاتب والمواد

## نظرة عامة
هذا الدليل يوضح كيفية إضافة بيانات شاملة للمساجد والمكاتب والمواد المرتبطة بها في نظام الأوقاف.

## الملفات المطلوبة

### 1. الملفات الأساسية (يجب تشغيلها أولاً)
- `run_all_seed_data.sql` - البيانات الأساسية (المدن، المناطق، المستخدمين، المنتجات العامة)

### 2. ملفات إضافة البيانات الجديدة
- `seed_mosque_products.sql` - إضافة منتجات خاصة بالمساجد
- `seed_complete_mosque_data.sql` - إضافة المساجد والبنايات التفصيلية والمواد
- `seed_additional_mosques.sql` - إضافة مساجد إضافية في مدن أخرى
- `seed_offices.sql` - إضافة بيانات المكاتب في مختلف المدن

## ترتيب التشغيل

### الخطوة 1: تشغيل البيانات الأساسية
```sql
-- تشغيل الملف الأساسي أولاً
EXEC run_all_seed_data.sql
```

### الخطوة 2: إضافة المنتجات الخاصة بالمساجد (اختياري)
```sql
-- إضافة منتجات خاصة بالمساجد
EXEC seed_mosque_products.sql
```

### الخطوة 3: إضافة بيانات المساجد والبنايات
```sql
-- إضافة المساجد والبنايات التفصيلية والمواد
EXEC seed_complete_mosque_data.sql
```

### الخطوة 4: إضافة مساجد إضافية (اختياري)
```sql
-- إضافة مساجد في مدن أخرى
EXEC seed_additional_mosques.sql
```

### الخطوة 5: إضافة بيانات المكاتب
```sql
-- إضافة بيانات المكاتب في مختلف المدن
EXEC seed_offices.sql
```

## البيانات المضافة

### 1. المنتجات الخاصة بالمساجد (seed_mosque_products.sql)
- **عدد المنتجات**: 50 منتج
- **أمثلة**: سجاد مصلى، ثريات مساجد، مكبرات صوت، أحواض وضوء، كتب دينية، مصاحف شريفة

### 2. المساجد والبنايات (seed_complete_mosque_data.sql)
- **عدد المساجد**: 10 مساجد في طرابلس
- **عدد البنايات التفصيلية**: 110 تفصيل (11 لكل مسجد)
- **عدد المواد المرتبطة**: 330 مادة (3 لكل تفصيل)

#### أنواع البنايات التفصيلية:
- المصلى الرئيسي
- المئذنة
- المكتبة
- غرف الإمام
- دورات المياه
- مرافق الوضوء
- المطبخ
- المخزن
- الساحة الخارجية
- قاعة اجتماعات
- غرف إدارية

### 3. مساجد إضافية (seed_additional_mosques.sql)
- **عدد المساجد**: 10 مساجد إضافية
- **المواقع**: الزاوية، البيضاء، سرت، طبرق
- **عدد البنايات التفصيلية**: 110 تفصيل إضافي

### 4. المكاتب (seed_offices.sql)
- **عدد المكاتب**: 85 مكتب
- **التوزيع الجغرافي**:
  - طرابلس: 11 مكتب
  - بنغازي: 11 مكتب
  - مصراتة: 9 مكاتب
  - الزاوية: 11 مكتب
  - البيضاء: 11 مكتب
  - سرت: 9 مكاتب
  - طبرق: 9 مكاتب
  - مدن أخرى: 14 مكتب

## العلاقات بين الكيانات

```
City (مدينة)
  ↓ (one-to-many)
Region (منطقة)
  ↓ (one-to-many)
Office (مكتب)
  ↓ (one-to-many)
Building (بناية)
  ↓ (one-to-one)
Mosque (مسجد)
  ↓ (one-to-many)
BuildingDetail (تفصيل البناية)
  ↓ (many-to-many)
FacilityDetail (تفصيل المرافق)
  ↓ (many-to-one)
Product (منتج)
```

## التحقق من البيانات

### التحقق من المساجد
```sql
SELECT 
    m.Id as MosqueId,
    m.MosqueDefinition,
    m.MosqueClassification,
    b.Name as BuildingName,
    b.PrayerCapacity,
    r.Name as RegionName,
    c.Name as CityName
FROM Mosques m
INNER JOIN Buildings b ON m.BuildingId = b.Id
INNER JOIN Regions r ON b.RegionId = r.Id
INNER JOIN Cities c ON r.CityId = c.Id
ORDER BY c.Name, b.Name
```

### التحقق من البنايات التفصيلية
```sql
SELECT 
    bd.Name as BuildingDetailName,
    bd.BuildingCategory,
    bd.Floors,
    bd.WithinMosqueArea,
    b.Name as BuildingName,
    m.MosqueDefinition
FROM BuildingDetails bd
INNER JOIN Buildings b ON bd.BuildingId = b.Id
INNER JOIN Mosques m ON b.Id = m.BuildingId
ORDER BY b.Name, bd.Name
```

### التحقق من المواد المرتبطة
```sql
SELECT 
    p.Name as ProductName,
    fd.Quantity,
    bd.Name as BuildingDetailName,
    b.Name as BuildingName
FROM FacilityDetails fd
INNER JOIN Products p ON fd.ProductId = p.Id
INNER JOIN BuildingDetails bd ON fd.BuildingDetailId = bd.Id
INNER JOIN Buildings b ON bd.BuildingId = b.Id
INNER JOIN Mosques m ON b.Id = m.BuildingId
ORDER BY b.Name, bd.Name, p.Name
```

### التحقق من المكاتب
```sql
SELECT 
    o.Name as OfficeName,
    o.Location,
    o.PhoneNumber,
    r.Name as RegionName,
    c.Name as CityName
FROM Offices o
INNER JOIN Regions r ON o.RegionId = r.Id
INNER JOIN Cities c ON r.CityId = c.Id
ORDER BY c.Name, o.Name
```

## إحصائيات شاملة

بعد تشغيل جميع الملفات، ستحصل على:
- **20 مسجد** في 7 مدن مختلفة
- **220 بناية تفصيلية** (11 لكل مسجد)
- **660 مادة مرتبطة** (3 لكل بناية تفصيلية)
- **85 مكتب** في مختلف المدن والمناطق
- **50+ منتج خاص بالمساجد**

## استكشاف الأخطاء

### مشكلة: "خطأ: يجب تشغيل run_all_seed_data.sql أولاً!"
**الحل**: تأكد من تشغيل الملف الأساسي أولاً

### مشكلة: "خطأ في إضافة البيانات"
**الحل**: تحقق من وجود البيانات الأساسية في الجداول التالية:
- Cities
- Regions
- AspNetUsers
- Products

### مشكلة: "خطأ في المفاتيح الأجنبية"
**الحل**: تأكد من ترتيب تشغيل الملفات كما هو موضح أعلاه

## ملاحظات مهمة

1. **ترتيب التشغيل**: يجب الالتزام بالترتيب المذكور أعلاه
2. **البيانات المكررة**: الملفات تتحقق من وجود البيانات قبل الإضافة
3. **الأخطاء**: جميع الملفات تحتوي على معالجة أخطاء شاملة
4. **الإحصائيات**: كل ملف يعرض إحصائيات مفصلة بعد التشغيل

## الدعم

إذا واجهت أي مشاكل، تحقق من:
1. ترتيب تشغيل الملفات
2. وجود البيانات الأساسية
3. صلاحيات قاعدة البيانات
4. سجلات الأخطاء في SQL Server

---

**تم إنشاء هذا الدليل بواسطة نظام الأوقاف** 🏛️✨ 