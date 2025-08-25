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
const router = useRouter()
const route = useRoute()

const form = reactive({
  email: route.query.email || '', // استقبال البريد من query parameters
})

const isLoading = ref(false)
const isSuccess = ref(false)

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  setFieldTouched('email')
  
  if (!isValid) return
  
  isLoading.value = true
  
  try {
    const response = await api('/auth/resend-verification-code', {
      method: 'POST',
      body: {
        email: form.email
      }
    })
    
    if (response.isSuccess) {
      isSuccess.value = true
      showSuccess('تم إرسال رمز التحقق الجديد بنجاح. تحقق من صندوق الوارد الخاص بك.')
      // إعادة توجيه إلى صفحة تأكيد البريد الإلكتروني بعد 3 ثوان
      setTimeout(() => {
        router.push(`/confirm-email?email=${encodeURIComponent(form.email)}`)
      }, 3000)
    } else {
      // معالجة الأخطاء
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
      } else if (response.message) {
        addError('general', response.message)
      }
    }
  } catch (error) {
    console.error('خطأ في إعادة إرسال رمز التحقق:', error)
    addError('general', 'حدث خطأ أثناء إعادة إرسال رمز التحقق')
  } finally {
    isLoading.value = false
  }
}

const handleBackToLogin = () => {
  router.push('/login')
}

const handleGoToConfirmEmail = () => {
  router.push(`/confirm-email?email=${encodeURIComponent(form.email)}`)
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
        إعادة إرسال رمز التحقق
      </VCardTitle>

      <VCardText>
        <!-- Success Message -->
        <div v-if="isSuccess">
          <VAlert
            type="success"
            variant="tonal"
            class="mb-6"
          >
            <template #title>
              تم إرسال رمز التحقق بنجاح! ✅
            </template>
            <template #text>
              <div class="mb-3">
                تم إرسال رمز التحقق الجديد إلى بريدك الإلكتروني.
              </div>
              <div class="text-body-2">
                <strong>البريد الإلكتروني:</strong> {{ form.email }}
              </div>
            </template>
          </VAlert>

          <!-- Action Buttons -->
          <div class="d-flex flex-column gap-3">
            <VBtn
              block
              color="primary"
              size="large"
              @click="handleGoToConfirmEmail"
              prepend-icon="tabler-mail-check"
            >
              الذهاب لصفحة تأكيد البريد الإلكتروني
            </VBtn>

            <VBtn
              block
              variant="text"
              color="primary"
              @click="handleBackToLogin"
              prepend-icon="tabler-arrow-left"
            >
              العودة إلى تسجيل الدخول
            </VBtn>
          </div>
        </div>

        <!-- Form -->
        <div v-else>
          <VAlert
            type="info"
            variant="tonal"
            class="mb-6"
          >
            <template #title>
              رمز التحقق الرقمي 🔢
            </template>
            <template #text>
              <div class="mb-3">
                أدخل بريدك الإلكتروني لإعادة إرسال رمز التحقق المكون من 6 أرقام.
              </div>
              <div class="text-body-2">
                رمز التحقق صالح لمدة 15 دقيقة فقط.
              </div>
            </template>
          </VAlert>

          <VForm @submit.prevent="handleSubmit">
            <VRow>
              <!-- Email -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.email"
                  label="البريد الإلكتروني"
                  placeholder="أدخل بريدك الإلكتروني"
                  type="email"
                  autofocus
                  :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
                  :error-messages="validationState.errors.email || []"
                  @blur="setFieldTouched('email')"
                  :disabled="isLoading"
                  prepend-inner-icon="tabler-mail"
                />
              </VCol>

              <!-- Submit Button -->
              <VCol cols="12">
                <VBtn
                  block
                  type="submit"
                  :loading="isLoading"
                  :disabled="!form.email.trim()"
                >
                  إرسال رمز التحقق
                </VBtn>
              </VCol>

              <!-- Action Links -->
              <VCol cols="12" class="text-center">
                <VBtn
                  variant="text"
                  color="primary"
                  @click="handleBackToLogin"
                  prepend-icon="tabler-arrow-left"
                >
                  العودة إلى تسجيل الدخول
                </VBtn>
              </VCol>
            </VRow>
          </VForm>
        </div>
      </VCardText>
    </VCard>
  </div>
</template>
