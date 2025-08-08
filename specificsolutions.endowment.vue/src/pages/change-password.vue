<script setup>
import { useFormValidation } from '@/composables/useFormValidation'

// استخدام نظام التحقق الجديد
const {
  validationState,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateLength,
  addError,
  setErrorsFromResponse,
} = useFormValidation()

const form = reactive({
  currentPassword: '',
  newPassword: '',
  confirmPassword: '',
})

const isCurrentPasswordVisible = ref(false)
const isNewPasswordVisible = ref(false)
const isConfirmPasswordVisible = ref(false)

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.currentPassword, 'currentPassword', 'كلمة المرور الحالية مطلوبة')) {
    isValid = false
  }
  
  if (!validateRequired(form.newPassword, 'newPassword', 'كلمة المرور الجديدة مطلوبة')) {
    isValid = false
  } else if (!validateLength(form.newPassword, 'newPassword', 6, 100, 'كلمة المرور يجب أن تكون بين 6 و 100 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(form.confirmPassword, 'confirmPassword', 'تأكيد كلمة المرور مطلوب')) {
    isValid = false
  } else if (form.newPassword !== form.confirmPassword) {
    addError('confirmPassword', 'كلمة المرور وتأكيدها غير متطابقين')
    isValid = false
  }
  
  setFieldTouched('currentPassword')
  setFieldTouched('newPassword')
  setFieldTouched('confirmPassword')
  
  if (!isValid) return
  
  try {
    const response = await $fetch('/api/auth/change-password', {
      method: 'POST',
      body: {
        currentPassword: form.currentPassword,
        newPassword: form.newPassword,
        confirmPassword: form.confirmPassword
      }
    })
    
    if (response.isSuccess) {
      // إظهار رسالة نجاح
      console.log('تم تغيير كلمة المرور بنجاح')
      // يمكن إضافة إشعار نجاح هنا
    } else {
      // معالجة الأخطاء
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else if (response.message) {
        addError('general', response.message)
      }
    }
  } catch (error) {
    console.error('خطأ في تغيير كلمة المرور:', error)
    addError('general', 'حدث خطأ أثناء تغيير كلمة المرور')
  }
}

definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})
</script>

<template>
  <VCard>
    <VCardTitle>
      تغيير كلمة المرور
    </VCardTitle>
    
    <VCardText>
      <VForm @submit.prevent="handleSubmit">
        <VRow>
          <!-- Current Password -->
          <VCol cols="12">
            <AppTextField
              v-model="form.currentPassword"
              label="كلمة المرور الحالية"
              placeholder="············"
              :type="isCurrentPasswordVisible ? 'text' : 'password'"
              autocomplete="current-password"
              :error="validationState.errors.currentPassword && validationState.errors.currentPassword.length > 0 && validationState.touched.currentPassword"
              :error-messages="validationState.errors.currentPassword || []"
              :append-inner-icon="isCurrentPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
              @click:append-inner="isCurrentPasswordVisible = !isCurrentPasswordVisible"
              @blur="setFieldTouched('currentPassword')"
            />
          </VCol>

          <!-- New Password -->
          <VCol cols="12">
            <AppTextField
              v-model="form.newPassword"
              label="كلمة المرور الجديدة"
              placeholder="············"
              :type="isNewPasswordVisible ? 'text' : 'password'"
              autocomplete="new-password"
              :error="validationState.errors.newPassword && validationState.errors.newPassword.length > 0 && validationState.touched.newPassword"
              :error-messages="validationState.errors.newPassword || []"
              :append-inner-icon="isNewPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
              @click:append-inner="isNewPasswordVisible = !isNewPasswordVisible"
              @blur="setFieldTouched('newPassword')"
            />
          </VCol>

          <!-- Confirm Password -->
          <VCol cols="12">
            <AppTextField
              v-model="form.confirmPassword"
              label="تأكيد كلمة المرور الجديدة"
              placeholder="············"
              :type="isConfirmPasswordVisible ? 'text' : 'password'"
              autocomplete="new-password"
              :error="validationState.errors.confirmPassword && validationState.errors.confirmPassword.length > 0 && validationState.touched.confirmPassword"
              :error-messages="validationState.errors.confirmPassword || []"
              :append-inner-icon="isConfirmPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
              @click:append-inner="isConfirmPasswordVisible = !isConfirmPasswordVisible"
              @blur="setFieldTouched('confirmPassword')"
            />
          </VCol>

          <!-- Change Password Button -->
          <VCol cols="12">
            <VBtn
              type="submit"
              :disabled="hasErrors"
            >
              تغيير كلمة المرور
            </VBtn>
          </VCol>
        </VRow>
      </VForm>
    </VCardText>
  </VCard>
</template> 