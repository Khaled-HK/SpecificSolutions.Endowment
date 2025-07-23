<script setup>
import { VForm } from 'vuetify/components/VForm'
import AuthProvider from '@/views/pages/authentication/AuthProvider.vue'
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'
import authV2RegisterIllustrationBorderedDark from '@images/pages/auth-v2-register-illustration-bordered-dark.png'
import authV2RegisterIllustrationBorderedLight from '@images/pages/auth-v2-register-illustration-bordered-light.png'
import authV2RegisterIllustrationDark from '@images/pages/auth-v2-register-illustration-dark.png'
import authV2RegisterIllustrationLight from '@images/pages/auth-v2-register-illustration-light.png'
import authV2MaskDark from '@images/pages/misc-mask-dark.png'
import authV2MaskLight from '@images/pages/misc-mask-light.png'
import { useFormValidation } from '@/composables/useFormValidation'

const imageVariant = useGenerateImageVariant(authV2RegisterIllustrationLight, authV2RegisterIllustrationDark, authV2RegisterIllustrationBorderedLight, authV2RegisterIllustrationBorderedDark, true)
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark)

definePage({
  meta: {
    layout: 'blank',
    unauthenticatedOnly: true,
  },
})

// استخدام نظام التحقق الجديد
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

const form = reactive({
  username: '',
  email: '',
  password: '',
  privacyPolicies: false,
})

const isPasswordVisible = ref(false)

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.username, 'username', 'اسم المستخدم مطلوب')) {
    isValid = false
  } else if (!validateLength(form.username, 'username', 3, 50, 'اسم المستخدم يجب أن يكون بين 3 و 50 حرف')) {
    isValid = false
  }
  
  if (!validateRequired(form.email, 'email', 'البريد الإلكتروني مطلوب')) {
    isValid = false
  } else if (!validateEmail(form.email, 'email', 'البريد الإلكتروني غير صحيح')) {
    isValid = false
  }
  
  if (!validateRequired(form.password, 'password', 'كلمة المرور مطلوبة')) {
    isValid = false
  } else if (!validateLength(form.password, 'password', 6, 100, 'كلمة المرور يجب أن تكون بين 6 و 100 حرف')) {
    isValid = false
  }
  
  if (!form.privacyPolicies) {
    addError('privacyPolicies', 'يجب الموافقة على سياسة الخصوصية والشروط')
    isValid = false
  }
  
  setFieldTouched('username')
  setFieldTouched('email')
  setFieldTouched('password')
  setFieldTouched('privacyPolicies')
  
  if (!isValid) return
  
  // هنا يمكن إضافة منطق التسجيل
  console.log('تم التحقق من النموذج بنجاح')
}
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
            :src="imageVariant"
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
            Adventure starts here 🚀
          </h4>
          <p class="mb-0">
            Make your app management easy and fun!
          </p>
        </VCardText>

        <VCardText>
          <VForm @submit.prevent="handleSubmit">
            <VRow>
              <!-- Username -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.username"
                  autofocus
                  label="اسم المستخدم"
                  placeholder="Johndoe"
                  :error="validationState.errors.username && validationState.errors.username.length > 0 && validationState.touched.username"
                  :error-messages="validationState.errors.username || []"
                  @blur="setFieldTouched('username')"
                />
              </VCol>

              <!-- email -->
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

              <!-- password -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.password"
                  label="كلمة المرور"
                  placeholder="············"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  autocomplete="password"
                  :error="validationState.errors.password && validationState.errors.password.length > 0 && validationState.touched.password"
                  :error-messages="validationState.errors.password || []"
                  :append-inner-icon="isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
                  @blur="setFieldTouched('password')"
                />

                <div class="d-flex align-center my-6">
                  <VCheckbox
                    id="privacy-policy"
                    v-model="form.privacyPolicies"
                    inline
                    :error="validationState.errors.privacyPolicies && validationState.errors.privacyPolicies.length > 0 && validationState.touched.privacyPolicies"
                  />
                  <VLabel
                    for="privacy-policy"
                    style="opacity: 1;"
                  >
                    <span class="me-1 text-high-emphasis">أوافق على</span>
                    <a
                      href="javascript:void(0)"
                      class="text-primary"
                    >سياسة الخصوصية والشروط</a>
                  </VLabel>
                </div>
                
                <div v-if="validationState.errors.privacyPolicies && validationState.errors.privacyPolicies.length > 0 && validationState.touched.privacyPolicies" class="text-error text-caption mt-1">
                  {{ validationState.errors.privacyPolicies[0] }}
                </div>

                <VBtn
                  block
                  type="submit"
                  :disabled="hasErrors"
                >
                  إنشاء حساب
                </VBtn>
              </VCol>

              <!-- create account -->
              <VCol
                cols="12"
                class="text-center text-base"
              >
                <span class="d-inline-block">Already have an account?</span>
                <RouterLink
                  class="text-primary ms-1 d-inline-block"
                  :to="{ name: 'login' }"
                >
                  Sign in instead
                </RouterLink>
              </VCol>

              <VCol
                cols="12"
                class="d-flex align-center"
              >
                <VDivider />
                <span class="mx-4">or</span>
                <VDivider />
              </VCol>

              <!-- auth providers -->
              <VCol
                cols="12"
                class="text-center"
              >
                <AuthProvider />
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
