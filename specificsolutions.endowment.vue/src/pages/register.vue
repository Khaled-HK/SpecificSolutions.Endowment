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
import { useApi } from '@/composables/useApi'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'

const imageVariant = useGenerateImageVariant(authV2RegisterIllustrationLight, authV2RegisterIllustrationDark, authV2RegisterIllustrationBorderedLight, authV2RegisterIllustrationBorderedDark, true)
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark)

// استخدام API composable مثل الصفحات الأخرى
const api = useApi()
const router = useRouter()
const { t, te } = useI18n()

// استخدام نظام التنبيهات الموحّد
import { useAppAlerts } from '@/composables/useAppAlerts'
const { success: showSuccess, error: showError, warning: showWarning } = useAppAlerts()

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
  firstName: '',
  lastName: '',
  username: '',
  email: '',
  password: '',
  phoneNumber: '',
  address: '',
  city: '',
  country: '',
  officeId: '',
  privacyPolicies: false,
})

const isPasswordVisible = ref(false)

// computed property لتحسين حالة الزر
const isFormValid = computed(() => {
  return form.firstName && 
         form.lastName && 
         form.username && 
         form.email && 
         form.phoneNumber && 
         form.address && 
         form.city && 
         form.country && 
         form.password && 
         form.privacyPolicies
})

const handleSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(form.firstName, 'firstName', 'الاسم الأول مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(form.lastName, 'lastName', 'اسم العائلة مطلوب')) {
    isValid = false
  }
  
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
  
  if (!validateRequired(form.phoneNumber, 'phoneNumber', 'رقم الهاتف مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(form.address, 'address', 'العنوان مطلوب')) {
    isValid = false
  }
  
  if (!validateRequired(form.city, 'city', 'المدينة مطلوبة')) {
    isValid = false
  }
  
  if (!validateRequired(form.country, 'country', 'البلد مطلوب')) {
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
  
  setFieldTouched('firstName')
  setFieldTouched('lastName')
  setFieldTouched('username')
  setFieldTouched('email')
  setFieldTouched('phoneNumber')
  setFieldTouched('address')
  setFieldTouched('city')
  setFieldTouched('country')
  setFieldTouched('password')
  setFieldTouched('privacyPolicies')
  
  if (!isValid) return
  
  try {
    const response = await api('/auth/register', {
      method: 'POST',
      body: {
        firstName: form.firstName || '',
        lastName: form.lastName || '',
        email: form.email,
        userName: form.username,
        password: form.password,
        confirmPassword: form.password,
        phoneNumber: form.phoneNumber || '',
        address: form.address || '',
        city: form.city || '',
        country: form.country || '',
        officeId: form.officeId || 'DDEC6E9E-7628-4623-9A94-4E4EFC02187C'
      }
    })
    
    if (response && response.isSuccess === false) {
      // معالجة أخطاء التحقق من الباك إند
      if (response.errors && Array.isArray(response.errors)) {
        setErrorsFromResponse(response)
        showWarning('⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
          timeout: 5000,
          clickToDismiss: true
        })
      } else if (response.message) {
        showError(response.message, {
          timeout: 0,
          clickToDismiss: true
        })
      }
      return
    }
    
    // نجاح التسجيل
    showSuccess('تم التسجيل بنجاح! سيتم توجيهك إلى صفحة تسجيل الدخول', {
      timeout: 3000,
      clickToDismiss: true
    })
    router.push('/login')
    
  } catch (error) {
    console.error('خطأ في التسجيل:', error)
    
    // التحقق من أن الخطأ يحتوي على أخطاء FluentValidation
    if (error?.data?.errors && Array.isArray(error.data.errors)) {
      setErrorsFromResponse(error.data)
      showWarning('⚠️ يرجى تصحيح الأخطاء المميزة باللون الأحمر أدناه', {
        timeout: 5000,
        clickToDismiss: true
      })
    } else {
      showError('حدث خطأ أثناء التسجيل', {
        timeout: 0,
        clickToDismiss: true
      })
    }
  }
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
              <!-- First Name -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="form.firstName"
                  autofocus
                  label="الاسم الأول"
                  placeholder="أحمد"
                  :error="validationState.errors.firstName && validationState.errors.firstName.length > 0 && validationState.touched.firstName"
                  :error-messages="validationState.errors.firstName || []"
                  @blur="setFieldTouched('firstName')"
                />
              </VCol>

              <!-- Last Name -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="form.lastName"
                  label="اسم العائلة"
                  placeholder="محمد"
                  :error="validationState.errors.lastName && validationState.errors.lastName.length > 0 && validationState.touched.lastName"
                  :error-messages="validationState.errors.lastName || []"
                  @blur="setFieldTouched('lastName')"
                />
              </VCol>

              <!-- Username -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.username"
                  label="اسم المستخدم"
                  placeholder="ahmed.mohamed"
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

              <!-- Phone Number -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.phoneNumber"
                  label="رقم الهاتف"
                  placeholder="+966501234567"
                  :error="validationState.errors.phoneNumber && validationState.errors.phoneNumber.length > 0 && validationState.touched.phoneNumber"
                  :error-messages="validationState.errors.phoneNumber || []"
                  @blur="setFieldTouched('phoneNumber')"
                />
              </VCol>

              <!-- Address -->
              <VCol cols="12">
                <AppTextField
                  v-model="form.address"
                  label="العنوان"
                  placeholder="شارع الملك فهد"
                  :error="validationState.errors.address && validationState.errors.address.length > 0 && validationState.touched.address"
                  :error-messages="validationState.errors.address || []"
                  @blur="setFieldTouched('address')"
                />
              </VCol>

              <!-- City -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="form.city"
                  label="المدينة"
                  placeholder="الرياض"
                  :error="validationState.errors.city && validationState.errors.city.length > 0 && validationState.touched.city"
                  :error-messages="validationState.errors.city || []"
                  @blur="setFieldTouched('city')"
                />
              </VCol>

              <!-- Country -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="form.country"
                  label="البلد"
                  placeholder="المملكة العربية السعودية"
                  :error="validationState.errors.country && validationState.errors.country.length > 0 && validationState.touched.country"
                  :error-messages="validationState.errors.country || []"
                  @blur="setFieldTouched('country')"
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
                  :disabled="!isFormValid"
                >
                  إنشاء حساب
                </VBtn>
              </VCol>

              <!-- create account -->
              <VCol
                cols="12"
                class="text-center text-base"
              >
                <span class="d-inline-block">لديك حساب بالفعل؟</span>
                <RouterLink
                  class="text-primary ms-1 d-inline-block"
                  :to="{ name: 'login' }"
                >
                  تسجيل الدخول بدلاً من ذلك
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
