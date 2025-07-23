<template>
  <VCard>
    <VCardTitle class="text-h4 mb-4">
      مثال على التحقق من النماذج
    </VCardTitle>
    
    <VCardText>
      <p class="mb-4">
        هذه الصفحة توضح كيفية استخدام نظام التحقق من النماذج مع إظهار الإطار الأحمر ورسائل الخطأ.
      </p>
      
      <VForm @submit.prevent="handleSubmit">
        <VRow>
          <VCol cols="12" md="6">
            <VTextField
              v-model="form.email"
              label="البريد الإلكتروني"
              type="email"
              placeholder="johndoe@email.com"
              required
              :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
              :error-messages="validationState.errors.email || []"
              @blur="setFieldTouched('email')"
            />
          </VCol>
          
          <VCol cols="12" md="6">
            <VTextField
              v-model="form.name"
              label="الاسم"
              required
              :error="validationState.errors.name && validationState.errors.name.length > 0 && validationState.touched.name"
              :error-messages="validationState.errors.name || []"
              @blur="setFieldTouched('name')"
            />
          </VCol>
          
          <VCol cols="12" md="6">
            <VTextField
              v-model="form.phone"
              label="رقم الهاتف"
              type="tel"
              :error="validationState.errors.phone && validationState.errors.phone.length > 0 && validationState.touched.phone"
              :error-messages="validationState.errors.phone || []"
              @blur="setFieldTouched('phone')"
            />
          </VCol>
          
          <VCol cols="12" md="6">
            <VTextField
              v-model="form.age"
              label="العمر"
              type="number"
              :error="validationState.errors.age && validationState.errors.age.length > 0 && validationState.touched.age"
              :error-messages="validationState.errors.age || []"
              @blur="setFieldTouched('age')"
            />
          </VCol>
          
          <VCol cols="12">
            <VTextarea
              v-model="form.description"
              label="الوصف"
              rows="3"
              :error="validationState.errors.description && validationState.errors.description.length > 0 && validationState.touched.description"
              :error-messages="validationState.errors.description || []"
              @blur="setFieldTouched('description')"
            />
          </VCol>
        </VRow>
        
        <VRow class="mt-4">
          <VCol cols="12">
            <VBtn
              type="submit"
              color="primary"
              :loading="loading"
              :disabled="hasErrors"
            >
              إرسال النموذج
            </VBtn>
            
            <VBtn
              variant="outlined"
              class="ms-2"
              @click="clearForm"
            >
              مسح النموذج
            </VBtn>
          </VCol>
        </VRow>
      </VForm>
    </VCardText>
    
    <!-- عرض حالة التحقق -->
    <VDivider />
    <VCardText>
      <h5 class="text-h6 mb-2">حالة التحقق:</h5>
      <VAlert
        v-if="hasErrors"
        type="error"
        variant="tonal"
        class="mb-2"
      >
        يوجد أخطاء في النموذج
      </VAlert>
      <VAlert
        v-else
        type="success"
        variant="tonal"
      >
        النموذج صحيح
      </VAlert>
    </VCardText>
  </VCard>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useFormValidation } from '@/composables/useFormValidation'

definePage({
  meta: {
    action: 'View',
    subject: 'ValidationExample',
  },
})

const loading = ref(false)

// نموذج البيانات
const form = reactive({
  email: '',
  name: '',
  phone: '',
  age: '',
  description: '',
})

// استخدام نظام التحقق
const {
  validationState,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateEmail,
  validateLength,
  addError,
} = useFormValidation()

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

// دالة إرسال النموذج
const handleSubmit = async () => {
  loading.value = true
  
  try {
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
    
    // التحقق من رقم الهاتف
    if (!validatePhone(form.phone)) {
      isValid = false
    }
    
    // التحقق من العمر
    if (!validateAge(form.age)) {
      isValid = false
    }
    
    // التحقق من الوصف
    if (!validateLength(form.description, 'description', 0, 500, 'الوصف يجب أن لا يتجاوز 500 حرف')) {
      isValid = false
    }
    
    // تعيين جميع الحقول كملموسة
    setFieldTouched('email')
    setFieldTouched('name')
    setFieldTouched('phone')
    setFieldTouched('age')
    setFieldTouched('description')
    
    if (!isValid) {
      loading.value = false
      return
    }
    
    // محاكاة إرسال البيانات
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    // نجاح الإرسال
    alert('تم إرسال النموذج بنجاح!')
    clearForm()
    
  } catch (error) {
    console.error('Error submitting form:', error)
    alert('حدث خطأ أثناء إرسال النموذج')
  } finally {
    loading.value = false
  }
}

// دالة مسح النموذج
const clearForm = () => {
  form.email = ''
  form.name = ''
  form.phone = ''
  form.age = ''
  form.description = ''
  clearErrors()
}
</script>

<style scoped>
/* تخصيص مظهر الحقول عند وجود خطأ */
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
</style> 