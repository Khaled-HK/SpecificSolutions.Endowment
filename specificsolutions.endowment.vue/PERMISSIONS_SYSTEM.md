# نظام الصلاحيات الجديد

## نظرة عامة

تم تطبيق نظام صلاحيات جديد يقسم العناصر إلى فئتين:

### 1. عناصر الأوقاف (Endowment Elements)
تأخذ صلاحيات من الباك إند مباشرة:
- **Buildings** - `action: 'View', subject: 'Building'`
- **Mosques** - `action: 'View', subject: 'Mosque'`
- **Cities** - `action: 'View', subject: 'City'`
- **Regions** - `action: 'View', subject: 'Region'`
- **Offices** - `action: 'View', subject: 'Office'`
- **Products** - `action: 'View', subject: 'Product'`
- **Decisions** - `action: 'View', subject: 'Decision'`

### 2. باقي العناصر (Other Elements)
تأخذ صلاحية واحدة موحدة:
- **otherview** - `action: 'View', subject: 'otherview'`

## كيفية عمل النظام

### عند تسجيل الدخول:
1. يتم إضافة صلاحية `otherview` تلقائياً لجميع المستخدمين
2. يتم إضافة صلاحيات عناصر الأوقاف من الباك إند
3. يتم حفظ جميع الصلاحيات في الكوكيز

### عند الوصول للصفحات:
1. يتم التحقق من الصلاحيات المطلوبة للصفحة
2. إذا كانت الصفحة تحتاج صلاحية عنصر أوقاف، يتم التحقق من الباك إند
3. إذا كانت الصفحة تحتاج صلاحية `otherview`، يتم السماح بالوصول

## الملفات المحدثة

### ملفات التنقل العمودي:
- `src/navigation/vertical/apps-and-pages.js`
- `src/navigation/vertical/others.js`
- `src/navigation/vertical/dashboard.js`

### ملفات التنقل الأفقي:
- `src/navigation/horizontal/apps.js`
- `src/navigation/horizontal/misc.js`

### ملفات النظام:
- `src/pages/login.vue` - إضافة صلاحية `otherview`
- `src/plugins/1.router/guards.js` - إعادة تفعيل نظام الصلاحيات

## أمثلة على الصلاحيات

### عناصر الأوقاف (تأخذ من الباك إند):
```javascript
{
  title: 'Buildings',
  to: 'apps-buildings',
  action: 'View',
  subject: 'Building',
}
```

### باقي العناصر (صلاحية موحدة):
```javascript
{
  title: 'Email',
  to: 'apps-email',
  action: 'View',
  subject: 'otherview',
}
```

## ملاحظات مهمة

1. **صلاحية `otherview`** تُمنح تلقائياً لجميع المستخدمين بعد تسجيل الدخول
2. **صلاحيات عناصر الأوقاف** تأتي من الباك إند ويجب أن تكون موجودة في `user.permissions`
3. **نظام الصلاحيات** مفعل بالكامل الآن
4. **جميع العناصر** يجب أن تظهر في القائمة الجانبية مع الصلاحيات المناسبة

## اختبار النظام

1. تسجيل الدخول بالتطبيق
2. التحقق من ظهور جميع العناصر في القائمة الجانبية
3. محاولة الوصول لصفحات عناصر الأوقاف (تحتاج صلاحيات من الباك إند)
4. محاولة الوصول لباقي الصفحات (تحتاج صلاحية `otherview` فقط) 