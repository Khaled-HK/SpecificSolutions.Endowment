<script setup>
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'
import { useRoute, useRouter } from 'vue-router'
import { useAppAlerts } from '@/composables/useAppAlerts'

const route = useRoute()
const router = useRouter()
const { success: showSuccess } = useAppAlerts()

const email = route.query.email || ''

const handleGoToConfirmEmail = () => {
  router.push(`/confirm-email?email=${encodeURIComponent(email)}`)
}

       const handleResendEmailConfirmation = () => {
         router.push(`/resend-verification-code?email=${encodeURIComponent(email)}`)
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
    <VCard class="mx-auto" max-width="600">
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
        ⚠️ يرجى تأكيد بريدك الإلكتروني
      </VCardTitle>
      
      <VCardText>
        <!-- Main Alert -->
        <VAlert
          type="warning"
          variant="tonal"
          class="mb-6"
        >
          <template #title>
            تحقق من بريدك الإلكتروني ✉️
          </template>
          <template #text>
            <div class="mb-3">
              يجب تأكيد بريدك الإلكتروني قبل تسجيل الدخول إلى النظام.
            </div>
            <div v-if="email" class="text-body-2">
              <strong>البريد الإلكتروني:</strong> {{ email }}
            </div>
          </template>
        </VAlert>

        <!-- Instructions -->
        <VAlert
          type="info"
          variant="tonal"
          class="mb-6"
        >
          <template #title>
            خطوات التأكيد:
          </template>
          <template #text>
            <ol class="mt-2">
              <li>تحقق من صندوق الوارد في بريدك الإلكتروني</li>
              <li>ابحث عن رسالة من {{ themeConfig.app.title }}</li>
              <li>اضغط على رابط "تأكيد البريد الإلكتروني" في الرسالة</li>
              <li>أو انسخ رمز التحقق وأدخله في صفحة التأكيد</li>
            </ol>
          </template>
        </VAlert>

        <!-- Action Buttons -->
        <div class="d-flex flex-column gap-3">
          <!-- Go to Confirm Email Page -->
          <VBtn
            block
            color="primary"
            size="large"
            @click="handleGoToConfirmEmail"
            prepend-icon="tabler-mail-check"
          >
            صفحة تأكيد البريد الإلكتروني
          </VBtn>

          <!-- Resend Email Confirmation -->
          <VBtn
            block
            color="secondary"
            variant="outlined"
            size="large"
            @click="handleResendEmailConfirmation"
            prepend-icon="tabler-refresh"
          >
            إعادة إرسال بريد التأكيد
          </VBtn>

          <!-- Back to Login -->
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

        <!-- Help Section -->
        <VAlert
          type="info"
          variant="tonal"
          class="mt-6"
        >
          <template #title>
            نصائح مهمة:
          </template>
          <template #text>
            <ul class="mt-2">
              <li>تحقق من مجلد الرسائل غير المرغوب فيها (Spam)</li>
              <li>تأكد من صحة عنوان البريد الإلكتروني</li>
              <li>انتظر بضع دقائق قبل إعادة المحاولة</li>
              <li>رمز التحقق صالح لمدة 24 ساعة فقط</li>
            </ul>
          </template>
        </VAlert>
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

ol {
  padding-left: 1.5rem;
  
  li {
    margin-bottom: 0.5rem;
  }
}

ul {
  padding-left: 1.5rem;
  
  li {
    margin-bottom: 0.5rem;
  }
}
</style>
