# دليل استخدام نظام التحقق من النماذج

## نظرة عامة

تم تطوير نظام شامل للتحقق من النماذج في الفرونت إند يتكامل مع FluentValidation في الباك إند. النظام يدعم:

- ✅ إظهار الإطار الأحمر للحقول التي تحتوي على أخطاء
- ✅ عرض رسائل الخطأ تحت الحقول
- ✅ التحقق في الوقت الفعلي (عند blur)
- ✅ التحقق من الباك إند (FluentValidation)
- ✅ دعم التحقق المخصص
- ✅ إدارة حالة الحقول (touched, dirty)

## الملفات المطلوبة

### 1. Composable للتحقق
```typescript
// src/composables/useFormValidation.ts
import { useFormValidation } from '@/composables/useFormValidation'
```

### 2. مكون VTextField محسن (اختياري)
```vue
// src/components/ValidatedTextField.vue
```

## كيفية الاستخدام

### الخطوة 1: استيراد Composable

```vue
<script setup lang="ts">
import { useFormValidation } from '@/composables/useFormValidation'

// استخدام نظام التحقق
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateEmail,
  validateLength,
  addError,
} = useFormValidation()
</script>
```

### الخطوة 2: إعداد النموذج

```vue
<template>
  <VForm @submit.prevent="handleSubmit">
    <VTextField
      v-model="form.email"
      label="البريد الإلكتروني"
      type="email"
      required
      :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
      :error-messages="validationState.errors.email || []"
      @blur="setFieldTouched('email')"
    />
    
    <VTextField
      v-model="form.name"
      label="الاسم"
      required
      :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
      :error-messages="validationState.errors.name || []"
      @blur="setFieldTouched('name')"
    />
    
    <VBtn
      type="submit"
      :disabled="hasErrors"
    >
      إرسال
    </VBtn>
  </VForm>
</template>
```

### الخطوة 3: التحقق من البيانات

```typescript
const handleSubmit = async () => {
  // مسح الأخطاء السابقة
  clearErrors()
  
  let isValid = true
  
  // التحقق من البريد الإلكتروني
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  // التحقق من الاسم
  if (!validateRequired(form.name, 'name', 'الاسم مطلوب')) {
    isValid = false
  } else if (!validateLength(form.name, 'name', 2, 50, 'الاسم يجب أن يكون بين 2 و 50 حرف')) {
    isValid = false
  }
  
  // تعيين الحقول كملموسة لعرض الأخطاء
  setFieldTouched('email')
  setFieldTouched('name')
  
  if (!isValid) {
    return
  }
  
  // إرسال البيانات
  try {
    const response = await $api('/endpoint', {
      method: 'POST',
      body: form,
    })
    
    // معالجة أخطاء الباك إند
    if (response && response.isSuccess === false) {
      if (response.errors && response.errors.length > 0) {
        setErrorsFromResponse(response)
        setFieldTouched('email')
        setFieldTouched('name')
        return
      }
    }
    
    // نجاح الإرسال
    alert('تم الإرسال بنجاح!')
    
  } catch (error) {
    console.error('Error:', error)
  }
}
```

## الدوال المتاحة

### دوال التحقق الأساسية

```typescript
// التحقق من الحقول المطلوبة
validateRequired(value, fieldName, message)

// التحقق من البريد الإلكتروني
validateEmail(email, fieldName, message)

// التحقق من الطول
validateLength(value, fieldName, min, max, message)
```

### دوال إدارة الأخطاء

```typescript
// إضافة خطأ لحقل معين
addError(fieldName, errorMessage)

// إزالة خطأ من حقل معين
removeError(fieldName, errorMessage?)

// مسح جميع الأخطاء
clearErrors()

// تعيين أخطاء من استجابة الباك إند
setErrorsFromResponse(response)
```

### دوال إدارة الحالة

```typescript
// تعيين حقل كملموس
setFieldTouched(fieldName, touched = true)

// تعيين حقل كمتغير
setFieldDirty(fieldName, dirty = true)

// التحقق من وجود أخطاء
hasErrors

// التحقق من وجود خطأ في حقل معين
hasFieldError(fieldName)

// الحصول على أخطاء حقل معين
getFieldErrors(fieldName)

// الحصول على أول خطأ لحقل معين
getFirstFieldError(fieldName)
```

## التحقق المخصص

يمكنك إنشاء دوال تحقق مخصصة:

```typescript
// دالة التحقق من رقم الهاتف
const validatePhone = (phone: string) => {
  if (!phone) return true // اختياري
  
  const phoneRegex = /^(09[1-5]|02[1-9])-?\d{7}$/
  if (!phoneRegex.test(phone)) {
    addError('phone', 'رقم الهاتف غير صحيح')
    return false
  }
  return true
}

// دالة التحقق من العمر
const validateAge = (age: string) => {
  if (!age) return true // اختياري
  
  const ageNum = parseInt(age)
  if (isNaN(ageNum) || ageNum < 0 || ageNum > 120) {
    addError('age', 'العمر يجب أن يكون بين 0 و 120')
    return false
  }
  return true
}
```

## التكامل مع FluentValidation

النظام يتكامل تلقائياً مع أخطاء FluentValidation من الباك إند:

```typescript
// استجابة الباك إند تحتوي على أخطاء FluentValidation
const response = {
  isSuccess: false,
  errors: [
    {
      propertyName: "email",
      errorMessage: "البريد الإلكتروني مطلوب"
    },
    {
      propertyName: "name", 
      errorMessage: "الاسم يجب أن يكون بين 2 و 50 حرف"
    }
  ]
}

// تطبيق الأخطاء على الحقول
setErrorsFromResponse(response)
setFieldTouched('email')
setFieldTouched('name')
```

## التخصيص

### تخصيص مظهر الحقول

```css
/* تخصيص مظهر الحقل عند وجود خطأ */
:deep(.v-field--error) {
  border-color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-field__outline) {
  color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-label) {
  color: rgb(var(--v-theme-error)) !important;
}

/* تخصيص مظهر رسائل الخطأ */
:deep(.v-messages__message) {
  color: rgb(var(--v-theme-error)) !important;
  font-size: 0.75rem;
  margin-top: 4px;
}
```

## أمثلة عملية

### مثال 1: نموذج تسجيل الدخول

```vue
<template>
  <VForm @submit.prevent="handleLogin">
    <VTextField
      v-model="form.email"
      label="البريد الإلكتروني"
      type="email"
      required
      :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
      :error-messages="validationState.errors.email || []"
      @blur="setFieldTouched('email')"
    />
    
    <VTextField
      v-model="form.password"
      label="كلمة المرور"
      type="password"
      required
      :error="validationState.errors.password && validationState.errors.password.length > 0 && validationState.touched.password"
      :error-messages="validationState.errors.password || []"
      @blur="setFieldTouched('password')"
    />
    
    <VBtn type="submit" :disabled="hasErrors">
      تسجيل الدخول
    </VBtn>
  </VForm>
</template>

<script setup>
const form = reactive({
  email: '',
  password: '',
})

const handleLogin = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  if (!validateRequired(form.password, 'password', 'كلمة المرور مطلوبة')) {
    isValid = false
  } else if (!validateLength(form.password, 'password', 6, 50, 'كلمة المرور يجب أن تكون بين 6 و 50 حرف')) {
    isValid = false
  }
  
  setFieldTouched('email')
  setFieldTouched('password')
  
  if (!isValid) return
  
  // إرسال البيانات...
}
</script>
```

### مثال 2: نموذج إنشاء حساب

```vue
<template>
  <VForm @submit.prevent="handleRegister">
    <VTextField
      v-model="form.name"
      label="الاسم الكامل"
      required
      :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
      :error-messages="validationState.errors.name || []"
      @blur="setFieldTouched('name')"
    />
    
    <VTextField
      v-model="form.phone"
      label="رقم الهاتف"
      type="tel"
      :error="validationState.errors.phone && validationState.errors.phone.length > 0 && validationState.touched.phone"
      :error-messages="validationState.errors.phone || []"
      @blur="setFieldTouched('phone')"
    />
    
    <VBtn type="submit" :disabled="hasErrors">
      إنشاء حساب
    </VBtn>
  </VForm>
</template>

<script setup>
const form = reactive({
  name: '',
  phone: '',
})

const validatePhone = (phone: string) => {
  if (!phone) return true
  
  const phoneRegex = /^(09[1-5]|02[1-9])-?\d{7}$/
  if (!phoneRegex.test(phone)) {
    addError('phone', 'رقم الهاتف غير صحيح')
    return false
  }
  return true
}

const handleRegister = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.name, 'name', 'الاسم مطلوب')) {
    isValid = false
  } else if (!validateLength(form.name, 'name', 2, 100, 'الاسم يجب أن يكون بين 2 و 100 حرف')) {
    isValid = false
  }
  
  if (!validatePhone(form.phone)) {
    isValid = false
  }
  
  setFieldTouched('name')
  setFieldTouched('phone')
  
  if (!isValid) return
  
  // إرسال البيانات...
}
</script>
```

## نصائح مهمة

1. **دائماً امسح الأخطاء السابقة** قبل التحقق الجديد
2. **استخدم setFieldTouched** لعرض الأخطاء بعد التحقق
3. **تعامل مع أخطاء الباك إند** باستخدام setErrorsFromResponse
4. **استخدم hasErrors** لتعطيل زر الإرسال عند وجود أخطاء
5. **أضف CSS مخصص** لتحسين مظهر الحقول عند وجود خطأ

## استكشاف الأخطاء

### المشكلة: لا تظهر رسائل الخطأ
**الحل:** تأكد من استخدام `setFieldTouched` بعد التحقق

### المشكلة: لا يظهر الإطار الأحمر
**الحل:** تأكد من إضافة CSS مخصص للحقول

### المشكلة: لا تعمل أخطاء الباك إند
**الحل:** تأكد من استخدام `setErrorsFromResponse` مع `setFieldTouched`

## الخلاصة

هذا النظام يوفر حلاً شاملاً للتحقق من النماذج في الفرونت إند مع التكامل الكامل مع FluentValidation في الباك إند. النظام سهل الاستخدام وقابل للتخصيص ويدعم جميع أنواع التحقق المطلوبة. 