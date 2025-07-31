# نظام التنبيهات مع خاصية الاختفاء عند الضغط

## 🎯 نظرة عامة

تم تطوير نظام تنبيهات متقدم يدعم خاصية **الاختفاء عند الضغط** في أي مكان على التنبيه. النظام مصمم ليكون سهل الاستخدام ومتجاوب مع تجربة المستخدم.

## ✨ الميزات الرئيسية

- 🔥 **الاختفاء عند الضغط** - التنبيه يختفي عند الضغط عليه في أي مكان
- 🎨 **تصميم جميل** - ألوان وأيقونات مختلفة لكل نوع من التنبيهات
- ⏰ **إخفاء تلقائي** - إمكانية تحديد وقت للإخفاء التلقائي
- 🎭 **انتقالات سلسة** - تأثيرات بصرية عند الظهور والاختفاء
- 📱 **استجابة للضغط** - تأثيرات بصرية عند الضغط
- 🔧 **قابل للتخصيص** - خيارات متعددة للتحكم في السلوك

## 🏗️ المكونات

### 1. useAlert Composable
```typescript
// src/composables/useAlert.ts
export const useAlert = () => {
  // إدارة حالة التنبيهات
  // دوال إظهار التنبيهات
  // خيارات التخصيص
}
```

### 2. GlobalAlert Component
```vue
<!-- src/components/GlobalAlert.vue -->
<!-- مكون تنبيه عام يظهر في أعلى الصفحة -->
<!-- يدعم Teleport للظهور في أي مكان -->
```

### 3. ErrorAlert Component
```vue
<!-- src/components/ErrorAlert.vue -->
<!-- مكون تنبيه محسن مع دعم الاختفاء عند الضغط -->
<!-- يمكن استخدامه في أي مكان في التطبيق -->
```

## 🚀 كيفية الاستخدام

### 1. استخدام التنبيه العام (GlobalAlert)

```vue
<script setup>
import { useAlert } from '@/composables/useAlert'

const { showSuccess, showError, showWarning, showInfo } = useAlert()

// إظهار تنبيه نجاح
const handleSuccess = () => {
  showSuccess('تم حفظ البيانات بنجاح!', {
    timeout: 5000, // يختفي تلقائياً بعد 5 ثوان
    clickToDismiss: true // يختفي عند الضغط
  })
}

// إظهار تنبيه خطأ
const handleError = () => {
  showError('حدث خطأ أثناء حفظ البيانات', {
    timeout: 0, // لا يختفي تلقائياً
    clickToDismiss: true
  })
}

// إظهار تنبيه تحذير
const handleWarning = () => {
  showWarning('يرجى التحقق من البيانات المدخلة', {
    timeout: 3000,
    clickToDismiss: true
  })
}

// إظهار تنبيه معلومات
const handleInfo = () => {
  showInfo('تم تحديث النظام بنجاح', {
    timeout: 4000,
    clickToDismiss: true
  })
}
</script>
```

### 2. استخدام مكون ErrorAlert المحسن

```vue
<template>
  <ErrorAlert
    v-model="showAlert"
    :message="alertMessage"
    :type="alertType"
    :timeout="5000"
    :click-to-dismiss="true"
  />
</template>

<script setup>
const showAlert = ref(false)
const alertMessage = ref('')
const alertType = ref('success')

const showCustomAlert = (message, type = 'success') => {
  alertMessage.value = message
  alertType.value = type
  showAlert.value = true
}
</script>
```

### 3. خيارات متقدمة

```javascript
// خيارات كاملة للتنبيه
showAlert({
  message: 'رسالة التنبيه',
  type: 'success', // 'success' | 'error' | 'warning' | 'info'
  timeout: 5000, // الوقت بالمللي ثانية (0 = لا يختفي تلقائياً)
  closable: true, // إظهار زر الإغلاق
  clickToDismiss: true // الاختفاء عند الضغط
})
```

## 📋 الخصائص المتاحة

### useAlert Composable

| الدالة | الوصف | المعاملات |
|--------|--------|------------|
| `showAlert(options)` | إظهار تنبيه مع خيارات مخصصة | `AlertOptions` |
| `showSuccess(message, options?)` | إظهار تنبيه نجاح | `string, Partial<AlertOptions>` |
| `showError(message, options?)` | إظهار تنبيه خطأ | `string, Partial<AlertOptions>` |
| `showWarning(message, options?)` | إظهار تنبيه تحذير | `string, Partial<AlertOptions>` |
| `showInfo(message, options?)` | إظهار تنبيه معلومات | `string, Partial<AlertOptions>` |
| `hideAlert()` | إخفاء التنبيه يدوياً | - |
| `dismissAlert()` | إخفاء التنبيه عند الضغط | - |

### ErrorAlert Component Props

| الخاصية | النوع | الافتراضي | الوصف |
|---------|-------|-----------|-------|
| `modelValue` | Boolean | - | حالة إظهار التنبيه |
| `message` | String | - | رسالة التنبيه |
| `type` | String | 'error' | نوع التنبيه |
| `closable` | Boolean | true | إظهار زر الإغلاق |
| `timeout` | Number | 0 | وقت الإخفاء التلقائي |
| `clickToDismiss` | Boolean | true | الاختفاء عند الضغط |

## 🎨 التصميم والألوان

### ألوان التنبيهات

| النوع | اللون الرئيسي | لون الخلفية | لون النص |
|-------|---------------|--------------|----------|
| **نجاح (Success)** | #4caf50 | #e8f5e8 | #2e7d32 |
| **خطأ (Error)** | #f44336 | #ffebee | #c62828 |
| **تحذير (Warning)** | #ff9800 | #fff8e1 | #ef6c00 |
| **معلومات (Info)** | #2196f3 | #e3f2fd | #1565c0 |

### الأيقونات

| النوع | الأيقونة | الوصف |
|-------|----------|-------|
| **نجاح** | `tabler-check-circle` | دائرة مع علامة صح |
| **خطأ** | `tabler-alert-circle` | دائرة مع علامة تعجب |
| **تحذير** | `tabler-alert-triangle` | مثلث تحذير |
| **معلومات** | `tabler-info-circle` | دائرة معلومات |

## 🔧 التخصيص

### تخصيص الألوان

```vue
<style scoped>
.v-alert {
  /* تخصيص الألوان */
  --alert-success-color: #4caf50;
  --alert-error-color: #f44336;
  --alert-warning-color: #ff9800;
  --alert-info-color: #2196f3;
}
</style>
```

### تخصيص الموقع

```vue
<template>
  <GlobalAlert 
    :position="'top-right'"
    :offset="{ x: 20, y: 20 }"
  />
</template>
```

## 📱 الاستجابة للضغط

### تأثيرات بصرية

```css
.v-alert:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
}

.v-alert:active {
  transform: translateY(0);
  transition: transform 0.1s ease;
}
```

### سلوك الاختفاء

```javascript
const dismissAlert = () => {
  if (props.clickToDismiss) {
    hideAlert()
  }
}
```

## 🔄 الانتقالات

### تأثيرات الظهور والاختفاء

```css
.alert-fade-enter-active,
.alert-fade-leave-active {
  transition: all 0.3s ease;
}

.alert-fade-enter-from {
  opacity: 0;
  transform: translateX(-50%) translateY(-20px);
}

.alert-fade-leave-to {
  opacity: 0;
  transform: translateX(-50%) translateY(-20px);
}
```

## 📝 أمثلة عملية

### مثال 1: تنبيه نجاح مع إخفاء تلقائي

```javascript
showSuccess('تم حفظ البيانات بنجاح!', {
  timeout: 4000,
  clickToDismiss: true
})
```

### مثال 2: تنبيه خطأ بدون إخفاء تلقائي

```javascript
showError('حدث خطأ في الاتصال بالخادم', {
  timeout: 0, // لا يختفي تلقائياً
  clickToDismiss: true
})
```

### مثال 3: تنبيه تحذير مع خيارات مخصصة

```javascript
showWarning('يرجى التحقق من البيانات المدخلة', {
  timeout: 5000,
  clickToDismiss: true,
  closable: true
})
```

## 🚨 معالجة الأخطاء

### في catch blocks

```javascript
try {
  // كود قد يسبب خطأ
} catch (error) {
  if (error?.data?.errors && Array.isArray(error.data.errors)) {
    showWarning('⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
      timeout: 5000,
      clickToDismiss: true
    })
  } else {
    showError('حدث خطأ أثناء العملية', {
      timeout: 0,
      clickToDismiss: true
    })
  }
}
```

## 🔮 التحديثات المستقبلية

- [ ] دعم التنبيهات المتعددة في نفس الوقت
- [ ] إمكانية تخصيص موقع التنبيه (top-left, top-right, bottom-left, bottom-right)
- [ ] دعم التنبيهات مع أزرار إجراءات
- [ ] دعم التنبيهات مع تقدم زمني
- [ ] دعم التنبيهات مع صوت
- [ ] دعم التنبيهات مع صور
- [ ] دعم التنبيهات مع روابط
- [ ] دعم التنبيهات مع قوائم منسدلة

## 📋 ملاحظات مهمة

1. **GlobalAlert** يظهر تلقائياً في جميع الصفحات
2. **ErrorAlert** يمكن استخدامه في أي مكان في التطبيق
3. خاصية `clickToDismiss` مفعلة افتراضياً
4. التنبيهات تختفي تلقائياً عند تحديد `timeout`
5. يمكن إلغاء خاصية الاختفاء عند الضغط بتعيين `clickToDismiss: false`
6. التنبيهات تستخدم Teleport للظهور في أعلى الصفحة
7. جميع التنبيهات تدعم الانتقالات السلسة
8. التنبيهات متوافقة مع النظام العربي (RTL)

## 🛠️ التثبيت والإعداد

### 1. إضافة المكونات إلى App.vue

```vue
<script setup>
import GlobalAlert from '@/components/GlobalAlert.vue'
</script>

<template>
  <VApp>
    <RouterView />
    <GlobalAlert />
  </VApp>
</template>
```

### 2. استخدام Composable في الصفحات

```vue
<script setup>
import { useAlert } from '@/composables/useAlert'

const { showSuccess, showError } = useAlert()
</script>
```

### 3. استخدام مكون ErrorAlert

```vue
<template>
  <ErrorAlert
    v-model="showAlert"
    :message="alertMessage"
    :type="alertType"
    :click-to-dismiss="true"
  />
</template>
```

## 🎉 الخلاصة

تم تطوير نظام تنبيهات متقدم يدعم خاصية **الاختفاء عند الضغط** مع الميزات التالية:

- ✅ **سهولة الاستخدام** - واجهة بسيطة وواضحة
- ✅ **مرونة عالية** - خيارات تخصيص متعددة
- ✅ **تصميم جميل** - ألوان وأيقونات متناسقة
- ✅ **استجابة ممتازة** - تأثيرات بصرية سلسة
- ✅ **توافق كامل** - يعمل مع جميع المتصفحات
- ✅ **دعم عربي** - متوافق مع النظام العربي

النظام جاهز للاستخدام في جميع أنحاء التطبيق! 🚀 