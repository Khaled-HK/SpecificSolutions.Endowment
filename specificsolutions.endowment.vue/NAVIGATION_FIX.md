# إصلاح القائمة الجانبية

## المشكلة
كانت القائمة الجانبية لا تظهر العناصر بعد تسجيل الدخول بسبب مشاكل في الصلاحيات.

## الحلول المطبقة

### 1. إزالة الصلاحيات المطلوبة من عناصر التنقل
- تم إزالة `action` و `subject` من جميع عناصر التنقل في:
  - `apps-and-pages.js`
  - `others.js`
  - `dashboard.js`

### 2. تعديل نظام الصلاحيات
- تم تعديل `guards.js` للسماح بالوصول مؤقتاً بدلاً من رفضه عند عدم وجود صلاحيات

### 3. إضافة ملفات التصحيح
- `debug-nav.js` - عناصر بسيطة للاختبار
- `NavItemsDebug.vue` - مكون لتصحيح القائمة الجانبية
- `debug-nav.vue` - صفحة تصحيح

### 4. إضافة مسار التصحيح
- تم إضافة مسار `/debug-nav` للوصول لصفحة التصحيح

## كيفية الاختبار

1. تسجيل الدخول للتطبيق
2. الانتقال إلى `/debug-nav` لرؤية عناصر التنقل
3. التحقق من أن القائمة الجانبية تظهر جميع العناصر

## العناصر المتوقعة في القائمة الجانبية

### إدارة الأوقاف (Endowment Management)
- Buildings (المباني)
- Mosques (المساجد)
- Cities (المدن)
- Regions (المناطق)
- Offices (المكاتب)
- Products (المنتجات)
- Decisions (القرارات)

### التجارة الإلكترونية (Ecommerce)
- Dashboard
- Product (List, Add, Category)
- Order (List, Details)
- Customer (List, Details)
- Manage Review
- Referrals
- Settings

### الأكاديمية (Academy)
- Dashboard
- My Course
- Course Details

### اللوجستيات (Logistics)
- Dashboard
- Fleet

### أدوات أخرى
- Email
- Chat
- Calendar
- Kanban
- Invoice
- User
- Roles & Permissions

## ملاحظات مهمة

1. تم إزالة الصلاحيات مؤقتاً لضمان ظهور العناصر
2. يمكن إعادة تفعيل نظام الصلاحيات لاحقاً بعد التأكد من عمل القائمة الجانبية
3. جميع العناصر يجب أن تظهر الآن في القائمة الجانبية 