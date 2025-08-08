<script setup>
import { useFormValidation } from '@/composables/useFormValidation'

// استخدام نظام التحقق الجديد
const {
  validationState,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateEmail,
  addError,
  setErrorsFromResponse,
} = useFormValidation()

const route = useRoute()
const router = useRouter()

const form = reactive({
  email: route.query.email || '',
  token: route.query.token || '',
})

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  if (!validateRequired(form.token, 'token', 'رمز التحقق مطلوب')) {
    isValid = false
  }
  
  setFieldTouched('email')
  setFieldTouched('token')
  
  if (!isValid) return
  
  try {
    const response = await $fetch('/api/auth/confirm-email', {
      method: 'POST',
      body: {
        email: form.email,
        token: form.token
      }
    })
    
    if (response.isSuccess) {
      // إظهار رسالة نجاح والانتقال إلى صفحة تسجيل الدخول
      console.log('تم تأكيد البريد الإلكتروني بنجاح')
      router.push('/login')
    } else {
      // معالجة الأخطاء
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else if (response.message) {
        addError('general', response.message)
      }
    }
  } catch (error) {
    console.error('خطأ في تأكيد البريد الإلكتروني:', error)
    addError('general', 'حدث خطأ أثناء تأكيد البريد الإلكتروني')
  }
}

definePage({
  meta: {
    layout: 'blank',
    unauthenticatedOnly: true,
  },
})
</script>

<template>
  <VCard class="mx-auto" max-width="500">
    <VCardTitle class="text-center">
      تأكيد البريد الإلكتروني ✉️
    </VCardTitle>
    
    <VCardText>
      <p class="text-center mb-4">
        أدخل رمز التحقق المرسل إلى بريدك الإلكتروني
      </p>
      
      <VForm @submit.prevent="handleSubmit">
        <VRow>
          <!-- Email -->
          <VCol cols="12">
            <AppTextField
              v-model="form.email"
              label="البريد الإلكتروني"
              type="email"
              placeholder="johndoe@email.com"
              :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
              :error-messages="validationState.errors.email || []"
              @blur="setFieldTouched('email')"
            />
          </VCol>

          <!-- Token -->
          <VCol cols="12">
            <AppTextField
              v-model="form.token"
              label="رمز التحقق"
              placeholder="أدخل رمز التحقق"
              :error="validationState.errors.token && validationState.errors.token.length > 0 && validationState.touched.token"
              :error-messages="validationState.errors.token || []"
              @blur="setFieldTouched('token')"
            />
          </VCol>

          <!-- Confirm Email Button -->
          <VCol cols="12">
            <VBtn
              block
              type="submit"
              :disabled="hasErrors"
            >
              تأكيد البريد الإلكتروني
            </VBtn>
          </VCol>

          <!-- Back to login -->
          <VCol cols="12" class="text-center">
            <RouterLink :to="{ name: 'login' }">
              العودة إلى تسجيل الدخول
            </RouterLink>
          </VCol>
        </VRow>
      </VForm>
    </VCardText>
  </VCard>
</template> 