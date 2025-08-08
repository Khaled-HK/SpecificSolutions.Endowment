<script setup>
import { useGenerateImageVariant } from '@core/composable/useGenerateImageVariant'
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'
import authV2ForgotPasswordIllustrationDark from '@images/pages/auth-v2-forgot-password-illustration-dark.png'
import authV2ForgotPasswordIllustrationLight from '@images/pages/auth-v2-forgot-password-illustration-light.png'
import authV2MaskDark from '@images/pages/misc-mask-dark.png'
import authV2MaskLight from '@images/pages/misc-mask-light.png'
import { useFormValidation } from '@/composables/useFormValidation'

// استخدام نظام التحقق الجديد
const {
  validationState,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateEmail,
  validateLength,
  addError,
  setErrorsFromResponse,
} = useFormValidation()

const route = useRoute()
const router = useRouter()

const form = reactive({
  email: route.query.email || '',
  token: route.query.token || '',
  newPassword: '',
  confirmPassword: '',
})

const isPasswordVisible = ref(false)
const isConfirmPasswordVisible = ref(false)

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
  
  setFieldTouched('email')
  setFieldTouched('token')
  setFieldTouched('newPassword')
  setFieldTouched('confirmPassword')
  
  if (!isValid) return
  
  try {
    const response = await $fetch('/api/auth/reset-password', {
      method: 'POST',
      body: {
        email: form.email,
        token: form.token,
        newPassword: form.newPassword,
        confirmPassword: form.confirmPassword
      }
    })
    
    if (response.isSuccess) {
      // إظهار رسالة نجاح والانتقال إلى صفحة تسجيل الدخول
      console.log('تم إعادة تعيين كلمة المرور بنجاح')
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
    console.error('خطأ في إعادة تعيين كلمة المرور:', error)
    addError('general', 'حدث خطأ أثناء إعادة تعيين كلمة المرور')
  }
}

const authThemeImg = useGenerateImageVariant(authV2ForgotPasswordIllustrationLight, authV2ForgotPasswordIllustrationDark)
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark)

definePage({
  meta: {
    layout: 'blank',
    unauthenticatedOnly: true,
  },
})
</script>

<template>
  <RouterLink to="/">
    <div class="auth-logo d-flex align-center gap-x-3">
      <VNodeRenderer :nodes="themeConfig.app.logo" />
      <h1 class="auth-title">
        {{ themeConfig.app.title }}
      </h1>
    </div>
  </RouterLink>

  <VRow
    no-gutters
    class="auth-wrapper bg-surface"
  >
    <VCol
      md="8"
      class="d-none d-md-flex"
    >
      <div class="position-relative bg-background w-100 me-0">
        <div
          class="d-flex align-center justify-center w-100 h-100"
          style="padding-inline: 100px;"
        >
          <VImg
            max-width="500"
            :src="authThemeImg"
            class="auth-illustration mt-16 mb-2"
          />
        </div>

        <img
          class="auth-footer-mask"
          :src="authThemeMask"
          alt="auth-footer-mask"
          height="280"
          width="100"
        >
      </div>
    </VCol>

    <VCol
      cols="12"
      md="4"
      class="auth-card-v2 d-flex align-center justify-center"
      style="background-color: rgb(var(--v-theme-surface));"
    >
      <VCard
        flat
        :max-width="500"
        class="mt-12 mt-sm-0 pa-4"
      >
        <VCardText>
          <h4 class="text-h4 mb-1">
            إعادة تعيين كلمة المرور 🔒
          </h4>
          <p class="mb-0">
            أدخل كلمة المرور الجديدة
          </p>
        </VCardText>

        <VCardText>
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

              <!-- New Password -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.newPassword"
                  label="كلمة المرور الجديدة"
                  placeholder="············"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  autocomplete="new-password"
                  :error="validationState.errors.newPassword && validationState.errors.newPassword.length > 0 && validationState.touched.newPassword"
                  :error-messages="validationState.errors.newPassword || []"
                  :append-inner-icon="isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
                  @blur="setFieldTouched('newPassword')"
                />
              </VCol>

              <!-- Confirm Password -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.confirmPassword"
                  label="تأكيد كلمة المرور"
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

              <!-- Reset Password Button -->
              <VCol cols="12">
                <VBtn
                  block
                  type="submit"
                  :disabled="hasErrors"
                >
                  إعادة تعيين كلمة المرور
                </VBtn>
              </VCol>

              <!-- Back to login -->
              <VCol cols="12">
                <RouterLink
                  class="d-flex align-center justify-center"
                  :to="{ name: 'login' }"
                >
                  <VIcon
                    icon="tabler-chevron-left"
                    size="20"
                    class="me-1 flip-in-rtl"
                  />
                  <span>العودة إلى تسجيل الدخول</span>
                </RouterLink>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
</template>

<style lang="scss">
@use "@core/scss/template/pages/page-auth";
</style> 