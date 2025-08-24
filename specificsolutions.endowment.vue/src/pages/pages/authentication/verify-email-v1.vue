<script setup>
import authV1BottomShape from '@images/svg/auth-v1-bottom-shape.svg?raw'
import authV1TopShape from '@images/svg/auth-v1-top-shape.svg?raw'
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'
import { useAppAlerts } from '@/composables/useAppAlerts'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/composables/useApi'

const { success: showSuccess, error: showError } = useAppAlerts()
const api = useApi()
const router = useRouter()
const route = useRoute()

// استخدام نظام التحقق
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

const form = reactive({
  email: route.query.email || 'hello@example.com', // استقبال البريد من query parameters أو استخدام القيمة الافتراضية
})

const isLoading = ref(false)
const isResendSuccess = ref(false)
const showResendForm = ref(false)

const handleResendEmail = () => {
  showResendForm.value = true
}

const handleSubmitResend = async () => {
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
      isResendSuccess.value = true
      showSuccess('تم إرسال بريد التأكيد بنجاح. تحقق من صندوق الوارد الخاص بك.')
      // إخفاء النموذج بعد النجاح
      setTimeout(() => {
        showResendForm.value = false
        isResendSuccess.value = false
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

const handleSkipForNow = () => {
  router.push('/')
}

definePage({
  meta: {
    layout: 'blank',
    public: true,
  },
})
</script>

<template>
  <div class="auth-wrapper d-flex align-center justify-center pa-4">
    <div class="position-relative my-sm-16">
      <!-- 👉 Top shape -->
      <VNodeRenderer
        :nodes="h('div', { innerHTML: authV1TopShape })"
        class="text-primary auth-v1-top-shape d-none d-sm-block"
      />

      <!-- 👉 Bottom shape -->
      <VNodeRenderer
        :nodes="h('div', { innerHTML: authV1BottomShape })"
        class="text-primary auth-v1-bottom-shape d-none d-sm-block"
      />

      <!-- 👉 Auth card -->
      <VCard
        class="auth-card"
        max-width="460"
        :class="$vuetify.display.smAndUp ? 'pa-6' : 'pa-2'"
      >
        <VCardItem class="justify-center">
          <VCardTitle>
            <RouterLink to="/">
              <div class="app-logo">
                <VNodeRenderer :nodes="themeConfig.app.logo" />
                <h1 class="app-logo-title">
                  {{ themeConfig.app.title }}
                </h1>
              </div>
            </RouterLink>
          </VCardTitle>
        </VCardItem>

        <VCardText>
          <h4 class="text-h4 mb-1 text-center">
            تحقق من بريدك الإلكتروني ✉️
          </h4>
          <p class="text-body-1 mb-4 text-center">
            تم إرسال رابط تفعيل الحساب إلى بريدك الإلكتروني:
            <br>
            <span class="font-weight-medium text-high-emphasis">{{ form.email }}</span>
            <br>
            يرجى متابعة الرابط الموجود داخل البريد للمتابعة.
          </p>

          <!-- Success Message for Resend -->
          <VAlert
            v-if="isResendSuccess"
            type="success"
            variant="tonal"
            class="mb-4"
          >
            <template #title>
              تم الإرسال بنجاح! ✅
            </template>
            <template #text>
              تم إرسال بريد التأكيد إلى بريدك الإلكتروني. تحقق من صندوق الوارد أو مجلد الرسائل غير المرغوب فيها.
            </template>
          </VAlert>

          <!-- Resend Email Form -->
          <VExpansionPanels v-if="showResendForm" class="mb-4">
            <VExpansionPanel>
              <VExpansionPanelTitle>
                <VIcon start icon="tabler-refresh" />
                إعادة إرسال بريد التأكيد
              </VExpansionPanelTitle>
              <VExpansionPanelText>
                <VForm @submit.prevent="handleSubmitResend">
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
                        :disabled="isLoading"
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
                        :disabled="hasErrors || isLoading"
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
                  </VRow>
                </VForm>
              </VExpansionPanelText>
            </VExpansionPanel>
          </VExpansionPanels>

          <!-- Action Buttons -->
          <div class="d-flex flex-column gap-3">
            <VBtn
              block
              color="primary"
              @click="handleSkipForNow"
              class="mb-2"
            >
              <VIcon start icon="tabler-arrow-right" />
              تخطي الآن
            </VBtn>

            <div class="d-flex align-center justify-center">
              <span class="me-2">لم تستلم البريد؟</span>
              <VBtn
                variant="text"
                color="primary"
                @click="handleResendEmail"
                :disabled="isLoading"
              >
                <VIcon start icon="tabler-refresh" />
                إعادة إرسال
              </VBtn>
            </div>

            <VDivider class="my-2" />

            <VBtn
              variant="text"
              color="secondary"
              @click="handleBackToLogin"
              :disabled="isLoading"
              class="text-center"
            >
              <VIcon start icon="tabler-arrow-left" />
              العودة إلى تسجيل الدخول
            </VBtn>
          </div>

          <!-- Help Text -->
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
        </VCardText>
      </VCard>
    </div>
  </div>
</template>

<style lang="scss">
@use "@core/scss/template/pages/page-auth";

.app-logo {
  .app-logo-title {
    font-size: 1.5rem;
    font-weight: 600;
    margin: 0;
  }
}
</style>
