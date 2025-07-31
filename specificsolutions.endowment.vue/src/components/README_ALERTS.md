# نظام التنبيهات مع خاصية الاختفاء عند الضغط

## نظرة عامة

تم إضافة نظام تنبيهات جديد يدعم خاصية الاختفاء عند الضغط في أي مكان على التنبيه. النظام يتكون من:

1. **useAlert Composable** - لإدارة حالة التنبيهات
2. **GlobalAlert Component** - مكون تنبيه عام يظهر في أعلى الصفحة
3. **ErrorAlert Component** - مكون تنبيه محسن مع دعم الاختفاء عند الضغط

## الميزات

- ✅ **الاختفاء عند الضغط** - التنبيه يختفي عند الضغط عليه في أي مكان
- ✅ **أنواع متعددة** - success, error, warning, info
- ✅ **إخفاء تلقائي** - إمكانية تحديد وقت للإخفاء التلقائي
- ✅ **تصميم جميل** - ألوان وأيقونات مختلفة لكل نوع
- ✅ **انتقالات سلسة** - تأثيرات بصرية عند الظهور والاختفاء
- ✅ **استجابة للضغط** - تأثيرات بصرية عند الضغط

## كيفية الاستخدام

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

## الخصائص المتاحة

### useAlert Composable

| الدالة | الوصف |
|--------|--------|
| `showAlert(options)` | إظهار تنبيه مع خيارات مخصصة |
| `showSuccess(message, options?)` | إظهار تنبيه نجاح |
| `showError(message, options?)` | إظهار تنبيه خطأ |
| `showWarning(message, options?)` | إظهار تنبيه تحذير |
| `showInfo(message, options?)` | إظهار تنبيه معلومات |
| `hideAlert()` | إخفاء التنبيه يدوياً |
| `dismissAlert()` | إخفاء التنبيه عند الضغط |

### ErrorAlert Component Props

| الخاصية | النوع | الافتراضي | الوصف |
|---------|-------|-----------|-------|
| `modelValue` | Boolean | - | حالة إظهار التنبيه |
| `message` | String | - | رسالة التنبيه |
| `type` | String | 'error' | نوع التنبيه |
| `closable` | Boolean | true | إظهار زر الإغلاق |
| `timeout` | Number | 0 | وقت الإخفاء التلقائي |
| `clickToDismiss` | Boolean | true | الاختفاء عند الضغط |

## التصميم والألوان

### ألوان التنبيهات

- **نجاح (Success)**: أخضر (#4caf50)
- **خطأ (Error)**: أحمر (#f44336)
- **تحذير (Warning)**: برتقالي (#ff9800)
- **معلومات (Info)**: أزرق (#2196f3)

### الأيقونات

- **نجاح**: `tabler-check-circle`
- **خطأ**: `tabler-alert-circle`
- **تحذير**: `tabler-alert-triangle`
- **معلومات**: `tabler-info-circle`

## التحديثات المستقبلية

- [ ] دعم التنبيهات المتعددة في نفس الوقت
- [ ] إمكانية تخصيص موقع التنبيه
- [ ] دعم التنبيهات مع أزرار إجراءات
- [ ] دعم التنبيهات مع تقدم زمني
- [ ] دعم التنبيهات مع صوت

## ملاحظات مهمة

1. **GlobalAlert** يظهر تلقائياً في جميع الصفحات
2. **ErrorAlert** يمكن استخدامه في أي مكان في التطبيق
3. خاصية `clickToDismiss` مفعلة افتراضياً
4. التنبيهات تختفي تلقائياً عند تحديد `timeout`
5. يمكن إلغاء خاصية الاختفاء عند الضغط بتعيين `clickToDismiss: false` 