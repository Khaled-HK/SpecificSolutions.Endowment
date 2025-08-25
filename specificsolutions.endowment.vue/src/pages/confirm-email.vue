<script setup>
import { useFormValidation } from '@/composables/useFormValidation'
import { useAppAlerts } from '@/composables/useAppAlerts'
import { useApi } from '@/composables/useApi'
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'

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

const { success: showSuccess, error: showError } = useAppAlerts()
const api = useApi()
const route = useRoute()
const router = useRouter()

const form = reactive({
  email: route.query.email || '',
  verificationCode: '', // رمز التحقق الرقمي (6 أرقام)
  token: route.query.token || '', // للتوافق مع النظام القديم
})

const isLoading = ref(false)
const isSuccess = ref(false)
const isAutoConfirming = ref(false)
const showWelcomeMessage = ref(false)
const showCodeInput = ref(true) // إظهار حقل رمز التحقق

// التحقق التلقائي عند تحميل الصفحة (للنظام القديم)
onMounted(async () => {
  if (form.email && form.token && !form.verificationCode) {
    isAutoConfirming.value = true
    await handleSubmit()
  } else {
    // إذا لم يكن هناك معاملات، قد يكون المستخدم جاء من صفحة التسجيل
    showWelcomeMessage.value = true
  }
})

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  // التحقق من رمز التحقق الرقمي
  if (showCodeInput.value) {
    if (!validateRequired(form.verificationCode, 'verificationCode', 'رمز التحقق مطلوب')) {
      isValid = false
    } else if (form.verificationCode.length !== 6) {
      addError('verificationCode', 'رمز التحقق يجب أن يكون 6 أرقام')
      isValid = false
    } else if (!/^\d{6}$/.test(form.verificationCode)) {
      addError('verificationCode', 'رمز التحقق يجب أن يحتوي على أرقام فقط')
      isValid = false
    }
  }
  
  // التحقق من token (للنظام القديم)
  if (!showCodeInput.value && !validateRequired(form.token, 'token', 'رمز التحقق مطلوب')) {
    isValid = false
  }
  
  setFieldTouched('email')
  if (showCodeInput.value) {
    setFieldTouched('verificationCode')
  } else {
    setFieldTouched('token')
  }
  
  if (!isValid) return
  
  isLoading.value = true
  
  try {
    let response
    
    if (showCodeInput.value) {
      // استخدام رمز التحقق الرقمي
      response = await api('/auth/confirm-email-with-code', {
        method: 'POST',
        body: {
          email: form.email,
          verificationCode: form.verificationCode
        }
      })
    } else {
      // استخدام token (للنظام القديم)
      response = await api('/auth/confirm-email', {
        method: 'POST',
        body: {
          email: form.email,
          token: form.token
        }
      })
    }
    
    if (response.isSuccess) {
      isSuccess.value = true
      showSuccess('تم تأكيد البريد الإلكتروني بنجاح! يمكنك الآن تسجيل الدخول.')
      // إعادة توجيه إلى صفحة تسجيل الدخول بعد ثانيتين
      setTimeout(() => {
        router.push('/login')
      }, 2000)
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
  } finally {
    isLoading.value = false
    isAutoConfirming.value = false
  }
}

const handleResendCode = async () => {
  clearErrors()
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    setFieldTouched('email')
    return
  }
  
  try {
    const response = await api('/auth/resend-verification-code', {
      method: 'POST',
      body: {
        email: form.email
      }
    })
    
    if (response.isSuccess) {
      showSuccess('تم إرسال رمز التحقق الجديد بنجاح. تحقق من بريدك الإلكتروني.')
    } else {
      if (response.message) {
        addError('general', response.message)
      }
    }
  } catch (error) {
    console.error('خطأ في إعادة إرسال رمز التحقق:', error)
    addError('general', 'حدث خطأ أثناء إعادة إرسال رمز التحقق')
  }
}

const handleBackToLogin = () => {
  router.push('/login')
}

definePage({
  meta: {
    layout: 'blank',
    unauthenticatedOnly: true,
  },
})
</script>

<template>
  <div class="auth-wrapper d-flex align-center justify-center pa-4">
    <VCard class="mx-auto" max-width="500">
      <!-- Header -->
      <VCardItem class="justify-center">
        <VCardTitle>
          <RouterLink to="/">
            <div class="app-logo d-flex align-center gap-x-3">
              <VNodeRenderer :nodes="themeConfig.app.logo" />
              <h1 class="app-logo-title">
                {{ themeConfig.app.title }}
              </h1>
            </div>
          </RouterLink>
        </VCardTitle>
      </VCardItem>

      <VCardTitle class="text-center">
        تأكيد البريد الإلكتروني ✉️
      </VCardTitle>
      
      <VCardText>
        <!-- Auto Confirming Message -->
        <VAlert
          v-if="isAutoConfirming"
          type="info"
          variant="tonal"
          class="mb-4"
        >
          <template #title>
            جاري التأكيد التلقائي... ⏳
          </template>
          <template #text>
            يتم التحقق من رمز التأكيد تلقائياً. يرجى الانتظار.
          </template>
        </VAlert>

        <!-- Success Message -->
        <VAlert
          v-if="isSuccess"
          type="success"
          variant="tonal"
          class="mb-4"
        >
          <template #title>
            تم التأكيد بنجاح! ✅
          </template>
          <template #text>
            تم تأكيد بريدك الإلكتروني بنجاح. يمكنك الآن تسجيل الدخول إلى حسابك.
            <br>
            سيتم توجيهك تلقائياً إلى صفحة تسجيل الدخول خلال ثانيتين.
          </template>
        </VAlert>

        <!-- Welcome Message (when coming from registration) -->
        <VAlert
          v-if="showWelcomeMessage"
          type="success"
          variant="tonal"
          class="mb-4"
        >
          <template #title>
            مرحباً بك في نظام الأوقاف! 🎉
          </template>
          <template #text>
            تم تسجيل حسابك بنجاح! يرجى التحقق من بريدك الإلكتروني وتأكيد حسابك للبدء في استخدام النظام.
          </template>
        </VAlert>

        <!-- Manual Form (only show if no auto-confirmation) -->
        <div v-if="!form.email || (!form.token && !showCodeInput)">
          <VAlert
            type="info"
            variant="tonal"
            class="mb-4"
          >
            <template #title>
              تأكيد البريد الإلكتروني 📧
            </template>
            <template #text>
              تم إرسال رابط تأكيد إلى بريدك الإلكتروني. يرجى التحقق من صندوق الوارد والضغط على الرابط.
              <br>
              أو يمكنك إدخال رمز التحقق يدوياً أدناه.
            </template>
          </VAlert>
          
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
                  :disabled="isLoading || isSuccess"
                  prepend-inner-icon="tabler-mail"
                />
              </VCol>

              <!-- Verification Code -->
              <VCol cols="12" v-if="showCodeInput">
                <AppTextField
                  v-model="form.verificationCode"
                  label="رمز التحقق"
                  placeholder="أدخل رمز التحقق"
                  :error="validationState.errors.verificationCode && validationState.errors.verificationCode.length > 0 && validationState.touched.verificationCode"
                  :error-messages="validationState.errors.verificationCode || []"
                  @blur="setFieldTouched('verificationCode')"
                  :disabled="isLoading || isSuccess"
                  prepend-inner-icon="tabler-key"
                  type="number"
                  min="000000"
                  max="999999"
                  pattern="[0-9]*"
                />
              </VCol>

              <!-- Token -->
              <VCol cols="12" v-if="!showCodeInput">
                <AppTextField
                  v-model="form.token"
                  label="رمز التحقق"
                  placeholder="أدخل رمز التحقق"
                  :error="validationState.errors.token && validationState.errors.token.length > 0 && validationState.touched.token"
                  :error-messages="validationState.errors.token || []"
                  @blur="setFieldTouched('token')"
                  :disabled="isLoading || isSuccess"
                  prepend-inner-icon="tabler-key"
                />
              </VCol>

              <!-- General Error -->
              <VCol cols="12" v-if="validationState.errors.general && validationState.errors.general.length > 0">
                <VAlert
                  type="error"
                  variant="tonal"
                  :text="validationState.errors.general[0]"
                />
              </VCol>

              <!-- Confirm Email Button -->
              <VCol cols="12">
                <VBtn
                  block
                  type="submit"
                  :disabled="hasErrors || isLoading || isSuccess"
                  :loading="isLoading"
                  color="primary"
                  size="large"
                >
                  <VIcon
                    v-if="!isLoading"
                    start
                    icon="tabler-check"
                  />
                  {{ isLoading ? 'جاري التأكيد...' : 'تأكيد البريد الإلكتروني' }}
                </VBtn>
              </VCol>
            </VRow>
          </VForm>
        </div>

        <!-- Action Buttons -->
        <div class="d-flex flex-column gap-3 mt-4">
          <!-- Back to login -->
          <VCol cols="12" class="text-center">
            <VBtn
              variant="text"
              color="primary"
              @click="handleBackToLogin"
              :disabled="isLoading"
            >
              <VIcon
                start
                icon="tabler-arrow-left"
              />
              العودة إلى تسجيل الدخول
            </VBtn>
          </VCol>

          <!-- Resend email confirmation -->
          <VCol cols="12" class="text-center">
            <VBtn
              variant="text"
              color="secondary"
              @click="handleResendCode"
              :disabled="isLoading || isSuccess"
            >
              <VIcon
                start
                icon="tabler-refresh"
              />
              إعادة إرسال بريد التأكيد
            </VBtn>
          </VCol>

          <!-- Help Text -->
          <VCol cols="12">
            <VAlert
              type="info"
              variant="tonal"
              class="mt-4"
            >
              <template #title>
                نصائح مهمة:
              </template>
              <template #text>
                • رمز التحقق صالح لمدة 24 ساعة فقط
                <br>
                • إذا انتهت صلاحية الرمز، استخدم "إعادة إرسال بريد التأكيد"
                <br>
                • تحقق من مجلد الرسائل غير المرغوب فيها (Spam)
              </template>
            </VAlert>
          </VCol>
        </div>
      </VCardText>
    </VCard>
  </div>
</template>

<style lang="scss" scoped>
.app-logo {
  .app-logo-title {
    font-size: 1.5rem;
    font-weight: 600;
    margin: 0;
  }
}

.auth-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}
</style> 