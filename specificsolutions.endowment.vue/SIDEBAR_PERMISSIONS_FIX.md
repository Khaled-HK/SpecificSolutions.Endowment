# حل مشكلة اختفاء عناصر الأوقاف من القائمة الجانبية

## المشكلة
عند تحديث الصفحة، كانت عناصر الأوقاف (Buildings, Mosques, Cities, Regions, Offices, Products, Decisions) تختفي من القائمة الجانبية بعد تسجيل الدخول.

## السبب
كانت المشكلة تكمن في عدم تطابق أسماء الكوكيز المستخدمة لحفظ قواعد الصلاحيات:
- في `login.vue`: يتم حفظ الصلاحيات في `user-ability-rules`
- في `casl/index.js`: كان يحاول قراءة الصلاحيات من `userAbilityRules`

## الحل المطبق

### 1. توحيد أسماء الكوكيز
تم تحديث جميع الملفات لاستخدام نفس اسم الكوكي `user-ability-rules`:

#### ملف `src/plugins/casl/index.js`
```javascript
const userAbilityRules = useCookie('user-ability-rules', {
  default: () => [],
  maxAge: 60 * 60 * 24 * 7, // 7 days
  path: '/',
  secure: true,
  sameSite: 'strict'
})
```

### 2. إضافة صلاحية `otherview` تلقائياً
تم إضافة صلاحية `otherview` لجميع المستخدمين في `login.vue`:

```javascript
// Add otherview permission for all users (required for non-endowment elements)
rules.push(
  { action: 'View', subject: 'otherview' }
)
```

### 3. إضافة دالة إعادة تحميل الصلاحيات
تم إنشاء دالة `reloadAbilityFromCookie` في `src/plugins/casl/ability.js`:

```javascript
export const reloadAbilityFromCookie = () => {
  try {
    const userAbilityRules = useCookie('user-ability-rules', {
      default: () => [],
      maxAge: 60 * 60 * 24 * 7,
      path: '/',
      secure: true,
      sameSite: 'strict'
    }).value

    if (userAbilityRules && Array.isArray(userAbilityRules)) {
      ability.update(userAbilityRules)
      console.log('✅ Reloaded ability from cookie:', userAbilityRules)
      return true
    } else {
      console.warn('❌ No valid ability rules found in cookie')
      return false
    }
  } catch (error) {
    console.error('❌ Error reloading ability from cookie:', error)
    return false
  }
}
```

### 4. إعادة تحميل الصلاحيات عند تحميل التطبيق
تم إضافة استدعاء لإعادة تحميل الصلاحيات في:

#### ملف `src/plugins/casl/index.js`
```javascript
// إعادة تحميل الصلاحيات عند تحميل التطبيق
if (typeof window !== 'undefined') {
  setTimeout(() => {
    reloadAbilityFromCookie()
  }, 100)
}
```

#### ملف `src/App.vue`
```javascript
// إعادة تحميل الصلاحيات عند تحميل التطبيق
onMounted(() => {
  setTimeout(() => {
    reloadAbilityFromCookie()
  }, 200)
})
```

#### ملف `src/plugins/1.router/guards.js`
```javascript
router.beforeEach((to, from, next) => {
  try {
    // إعادة تحميل الصلاحيات من الكوكيز في كل مرة
    reloadAbilityFromCookie()
    // ... باقي الكود
  }
})
```

### 5. تحديث useAbility composable
تم تحديث `src/plugins/casl/composables/useAbility.js` لإضافة دالة إعادة تحميل الصلاحيات:

```javascript
export const useAbility = () => {
  const ability = useCaslAbility()
  
  // إضافة دالة إعادة تحميل الصلاحيات
  ability.reloadFromCookie = reloadAbilityFromCookie
  
  return ability
}
```

## النتيجة
بعد تطبيق هذه التحديثات:
- ✅ عناصر الأوقاف تظهر في القائمة الجانبية بعد تسجيل الدخول
- ✅ العناصر تبقى ظاهرة بعد تحديث الصفحة
- ✅ الصلاحيات يتم حفظها واستعادتها بشكل صحيح
- ✅ جميع العناصر الأخرى تعمل بشكل طبيعي

## الملفات المحدثة
1. `src/plugins/casl/index.js`
2. `src/plugins/casl/ability.js`
3. `src/plugins/casl/composables/useAbility.js`
4. `src/pages/login.vue`
5. `src/App.vue`
6. `src/plugins/1.router/guards.js` 