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
    const response = await api('/auth/resend-email-confirmation', {
      method: 'POST',
      body: {
        email: form.email
      }
    })
    
    if (response.isSuccess) {
      isSuccess.value = true
      showSuccess('تم إرسال بريد التأكيد بنجاح. تحقق من صندوق الوارد الخاص بك.')
      // إعادة توجيه إلى صفحة تسجيل الدخول بعد 3 ثوان
      setTimeout(() => {
        router.push('/login')
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
    console.error('خطأ في إعادة إرسال بريد التأكيد:', error)
    addError('general', 'حدث خطأ أثناء إعادة إرسال بريد التأكيد')
  } finally {
    isLoading.value = false
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
        إعادة إرسال بريد التأكيد ✉️
      </VCardTitle>
      
      <VCardText>
        <p class="text-center mb-4">
          أدخل بريدك الإلكتروني لإعادة إرسال رابط تأكيد البريد الإلكتروني
        </p>
        
        <!-- Success Message -->
        <VAlert
          v-if="isSuccess"
          type="success"
          variant="tonal"
          class="mb-4"
        >
          <template #title>
            تم الإرسال بنجاح! ✅
          </template>
          <template #text>
            تم إرسال بريد التأكيد إلى بريدك الإلكتروني. تحقق من صندوق الوارد أو مجلد الرسائل غير المرغوب فيها.
            <br>
            سيتم توجيهك تلقائياً إلى صفحة تسجيل الدخول خلال 3 ثوان.
          </template>
        </VAlert>
        
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

            <!-- General Error -->
            <VCol cols="12" v-if="validationState.errors.general && validationState.errors.general.length > 0">
              <VAlert
                type="error"
                variant="tonal"
                :text="validationState.errors.general[0]"
              />
            </VCol>

            <!-- Resend Email Button -->
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
                  icon="tabler-send"
                />
                {{ isLoading ? 'جاري الإرسال...' : 'إعادة إرسال بريد التأكيد' }}
              </VBtn>
            </VCol>

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
                  • تحقق من مجلد الرسائل غير المرغوب فيها (Spam)
                  <br>
                  • تأكد من صحة عنوان البريد الإلكتروني
                  <br>
                  • انتظر بضع دقائق قبل إعادة المحاولة
                </template>
              </VAlert>
            </VCol>
          </VRow>
        </VForm>
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
