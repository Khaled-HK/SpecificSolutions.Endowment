# تصحيح مشكلة الصلاحيات - Ecommerce Dashboard

## المشكلة:
عند الوصول إلى `http://localhost:5173/apps/ecommerce/Dashboard` تظهر صفحة "not-authorized"

## خطوات التصحيح:

### 1. تحقق من الرابط الصحيح:
الرابط الصحيح هو: `http://localhost:5173/apps/ecommerce/dashboard` (dashboard بحروف صغيرة)

### 2. تحقق من تسجيل الدخول:
- اذهب إلى `http://localhost:5173`
- سجل الدخول باستخدام:
  - البريد الإلكتروني: `admin@demo.com`
  - كلمة المرور: `admin`

### 3. تحقق من وحدة التحكم (Console):
افتح Developer Tools (F12) وتحقق من الرسائل التالية:

#### عند تسجيل الدخول يجب أن ترى:
```
🍪 Saved ability rules to cookie: [Array]
✅ Updated CASL ability with rules
```

#### عند الوصول للصفحة يجب أن ترى:
```
🔍 Checking permissions for route: /apps/ecommerce/dashboard
🔍 Route meta: {action: "View", subject: "Dashboard"}
🔍 User ability rules from cookie: [Array]
🔍 Checking: View on Dashboard
🔍 Can access: true
✅ Permission granted
```

### 4. إذا لم تظهر الرسائل أعلاه:

#### أ. تحقق من الكوكيز:
في Developer Tools > Application > Cookies > localhost:5173
يجب أن تجد:
- `accessToken`
- `userData`
- `user-ability-rules`

#### ب. إذا لم توجد الكوكيز:
- امسح جميع الكوكيز
- أعد تسجيل الدخول

#### ج. إذا وجدت الكوكيز ولكن لا تعمل:
- تحقق من أن `user-ability-rules` تحتوي على مصفوفة
- يجب أن تحتوي على: `{action: "View", subject: "Dashboard"}`

### 5. الصلاحيات المطلوبة:
يجب أن تكون موجودة في الكوكيز:
```javascript
[
  { action: 'View', subject: 'Dashboard' },
  { action: 'read', subject: 'Dashboard' },
  { action: 'write', subject: 'Dashboard' }
]
```

### 6. إذا استمرت المشكلة:
1. أعد تشغيل الخادم
2. امسح جميع الكوكيز
3. أعد تسجيل الدخول
4. تحقق من وحدة التحكم للأخطاء

## المسارات البديلة:
إذا لم يعمل الرابط أعلاه، جرب:
```
http://localhost:5173/dashboards/ecommerce
``` 