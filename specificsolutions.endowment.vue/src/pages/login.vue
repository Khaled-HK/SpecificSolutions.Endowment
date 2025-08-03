<script setup lang="ts">
import { VForm } from 'vuetify/components/VForm'
import AuthProvider from '@/views/pages/authentication/AuthProvider.vue'
import { useGenerateImageVariant } from '@core/composable/useGenerateImageVariant'
import authV2LoginIllustrationBorderedDark from '@images/pages/auth-v2-login-illustration-bordered-dark.png'
import authV2LoginIllustrationBorderedLight from '@images/pages/auth-v2-login-illustration-bordered-light.png'
import authV2LoginIllustrationDark from '@images/pages/auth-v2-login-illustration-dark.png'
import authV2LoginIllustrationLight from '@images/pages/auth-v2-login-illustration-light.png'
import authV2MaskDark from '@images/pages/misc-mask-dark.png'
import authV2MaskLight from '@images/pages/misc-mask-light.png'
import { VNodeRenderer } from '@layouts/components/VNodeRenderer'
import { themeConfig } from '@themeConfig'
import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAbility } from '@/plugins/casl/composables/useAbility'
import { useFormValidation } from '@/composables/useFormValidation'
import { useApi } from '@/utils/api'
import Cookies from 'js-cookie'

const authThemeImg = useGenerateImageVariant(authV2LoginIllustrationLight, authV2LoginIllustrationDark, authV2LoginIllustrationBorderedLight, authV2LoginIllustrationBorderedDark, true)
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark)

definePage({
  meta: {
    layout: 'blank',
    unauthenticatedOnly: true,
  },
})

const isPasswordVisible = ref(false)
const route = useRoute()
const router = useRouter()
const ability = useAbility()
const api = useApi()

// استخدام نظام التحقق الجديد
const {
  validationState,
  setErrorsFromResponse,
  clearErrors,
  hasErrors,
  setFieldTouched,
  validateRequired,
  validateEmail,
  addError,
} = useFormValidation()

const refVForm = ref()

const credentials = reactive({
  email: 'admin@demo.com',
  password: 'admin',
})

const rememberMe = ref(false)
const isLoading = ref(false)

// متغيرات اللغة
const currentLanguage = ref('ar') // ar للعربية، en للإنجليزية
const isRTL = computed(() => currentLanguage.value === 'ar')

// نصوص متعددة اللغات
const texts = {
  ar: {
    welcome: 'مرحباً بك في',
    signInMessage: 'يرجى تسجيل الدخول إلى حسابك وابدأ المغامرة',
    adminEmail: 'بريد المدير',
    clientEmail: 'بريد العميل',
    password: 'كلمة المرور',
    email: 'البريد الإلكتروني',
    passwordField: 'كلمة المرور',
    rememberMe: 'تذكرني',
    forgotPassword: 'نسيت كلمة المرور؟',
    signIn: 'تسجيل الدخول',
    newUser: 'جديد على منصتنا؟',
    createAccount: 'إنشاء حساب',
    or: 'أو',
    loginFailed: 'فشل تسجيل الدخول. يرجى التحقق من بيانات الاعتماد.',
    noPermissions: 'فشل تسجيل الدخول: لم يتم العثور على صلاحيات.',
    emailRequired: 'البريد الإلكتروني مطلوب',
    invalidEmail: 'البريد الإلكتروني غير صحيح',
    passwordRequired: 'كلمة المرور مطلوبة',
    tryAgain: 'فشل تسجيل الدخول. يرجى المحاولة مرة أخرى.'
  },
  en: {
    welcome: 'Welcome to',
    signInMessage: 'Please sign-in to your account and start the adventure',
    adminEmail: 'Admin Email',
    clientEmail: 'Client Email',
    password: 'Password',
    email: 'Email',
    passwordField: 'Password',
    rememberMe: 'Remember me',
    forgotPassword: 'Forgot Password?',
    signIn: 'Sign In',
    newUser: 'New on our platform?',
    createAccount: 'Create an account',
    or: 'or',
    loginFailed: 'Login failed. Please check your credentials.',
    noPermissions: 'Login failed: No permissions found.',
    emailRequired: 'Email is required',
    invalidEmail: 'Invalid email format',
    passwordRequired: 'Password is required',
    tryAgain: 'Login failed. Please try again.'
  }
}

// دالة تبديل اللغة
const toggleLanguage = () => {
  currentLanguage.value = currentLanguage.value === 'ar' ? 'en' : 'ar'
  // حفظ اللغة في localStorage
  localStorage.setItem('preferredLanguage', currentLanguage.value)
}

// دالة الحصول على النص الحالي
const t = (key: string) => {
  return texts[currentLanguage.value][key] || key
}

// تحميل اللغة المحفوظة عند تحميل الصفحة
onMounted(() => {
  const savedLanguage = localStorage.getItem('preferredLanguage')
  if (savedLanguage) {
    currentLanguage.value = savedLanguage
  }
})

const login = async () => {
  isLoading.value = true
  try {
    const res = await api('/auth/login', {
      method: 'POST',
      body: {
        email: credentials.email,
        password: credentials.password,
      },
      onResponseError({ response }) {
        if (response._data.errors) {
          setErrorsFromResponse(response._data)
        } else {
          // إضافة خطأ عام إذا لم تكن هناك أخطاء محددة
          addError('email', t('loginFailed'))
        }
      },
    })

         const user = res.data
     // حفظ بيانات المستخدم في الكوكيز
     Cookies.set('accessToken', user.token)
     Cookies.set('userData', JSON.stringify(user))

    // Check if user has abilityRules (from fake API) or permissions (from real API)
    const userPermissions = user.permissions || user.userAbilityRules || []
    
    if (!userPermissions || userPermissions.length === 0) {
      console.error('User object does not have permissions or abilityRules:', user)
      addError('email', t('noPermissions'))
      return
    }
    
    const rules = []

    // Add general permissions for Auth subject (required for page access)
    rules.push(
      { action: 'read', subject: 'Auth' },
      { action: 'write', subject: 'Auth' },
      { action: 'delete', subject: 'Auth' }
     )

    // Add Dashboard permissions (required for email and other dashboard pages)
    rules.push(
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Dashboard' },
      { action: 'write', subject: 'Dashboard' }
     )

    // Add otherview permission for all users (required for non-endowment elements)
   // rules.push(
   //     { action: 'View', subject: 'otherview' }
   //   )

    // Convert specific permissions from backend
    userPermissions.forEach(permission => {
      // Handle camelCase permissions from backend (e.g., "cityView", "accountAdd")
      const action = mapPermissionToAction(permission)
      const subject = mapPermissionToSubject(permission)
      
      if (action && subject) {
        rules.push({ action, subject })
      }
    })

    // حفظ قواعد الصلاحيات في الكوكيز
    try {
      Cookies.set('user-ability-rules', JSON.stringify(rules), { 
        expires: 7, // 7 days
        path: '/',
        secure: false, // Set to true in production with HTTPS
        sameSite: 'lax'
      })
    } catch (error) {
      console.error('❌ Error saving to cookie:', error)
    }
    
    ability.update(rules)

    // Immediately reload abilities to ensure they are available
    const { reloadAbilityFromCookie } = await import('@/plugins/casl/ability')
    reloadAbilityFromCookie()

    const target = route.query.to ? String(route.query.to) : '/dashboard'
    await nextTick(() => {
      router.replace(target)
    })
  } catch (err) {
    console.error('Login error:', err)
    
    // إذا كان هناك استجابة من الخادم مع أخطاء محددة
    if (err.response?.data?.errors && err.response.data.errors.length > 0) {
      const errorMessage = err.response.data.errors[0].errorMessage
      if (errorMessage) {
        addError('general', errorMessage)
      } else {
        addError('general', t('tryAgain'))
      }
    } else if (err.response?.data?.message) {
      addError('general', err.response.data.message)
    } else {
      // خطأ عام إذا لم تكن هناك استجابة محددة
      addError('general', t('tryAgain'))
    }
  } finally {
    isLoading.value = false
  }
}

const onSubmit = async () => {
  clearErrors()
  
  let isValid = true
  
  if (!validateRequired(credentials.email, 'email', t('emailRequired'))) {
    isValid = false
  } else if (!validateEmail(credentials.email, 'email', t('invalidEmail'))) {
    isValid = false
  }
  
  if (!validateRequired(credentials.password, 'password', t('passwordRequired'))) {
    isValid = false
  }
  
  setFieldTouched('email')
  setFieldTouched('password')
  
  if (!isValid) return
  
  await login()
}

// Helper functions to map backend permissions to CASL actions/subjects
function mapPermissionToAction(permission) {
  // Handle underscore format from backend (e.g., "City_View", "Account_Add")
  if (permission.endsWith('_View')) return 'View'
  if (permission.endsWith('_Add')) return 'Add'
  if (permission.endsWith('_Edit')) return 'Edit'
  if (permission.endsWith('_Delete')) return 'Delete'
  
  // Handle fake API format (e.g., { action: 'manage', subject: 'all' })
  if (typeof permission === 'object' && permission.action) {
    return permission.action
  }
  
  return null // Return null for unknown permissions
}

function mapPermissionToSubject(permission) {
  // Handle underscore format from backend (e.g., "City_View", "Account_Add")
  if (permission.startsWith('AccountDetail_')) return 'AccountDetail'
  if (permission.startsWith('ConstructionRequest_')) return 'ConstructionRequest'
  if (permission.startsWith('MaintenanceRequest_')) return 'MaintenanceRequest'
  if (permission.startsWith('ChangeRequest_')) return 'ChangeRequest'
  if (permission.startsWith('DemolitionRequest_')) return 'DemolitionRequest'
  if (permission.startsWith('NameChangeRequest_')) return 'NameChangeRequest'
  if (permission.startsWith('NeedsRequest_')) return 'NeedsRequest'
  if (permission.startsWith('ExpenditureChangeRequest_')) return 'ExpenditureChangeRequest'
  if (permission.startsWith('ChangeOfPathRequest_')) return 'ChangeOfPathRequest'
  if (permission.startsWith('Account_')) return 'Account'
  if (permission.startsWith('User_')) return 'User'
  if (permission.startsWith('Role_')) return 'Role'
  if (permission.startsWith('Decision_')) return 'Decision'
  if (permission.startsWith('Request_')) return 'Request'
  if (permission.startsWith('Office_')) return 'Office'
  if (permission.startsWith('Endowment_')) return 'Endowment'
  if (permission.startsWith('City_')) return 'City'
  if (permission.startsWith('Region_')) return 'Region'
  if (permission.startsWith('Building_')) return 'Building'
  if (permission.startsWith('Mosque_')) return 'Mosque'
  
  // Handle fake API format (e.g., { action: 'manage', subject: 'all' })
  if (typeof permission === 'object' && permission.subject) {
    return permission.subject
  }
  
  return null // Return null for unknown permissions
}
</script>

<template>
  <!-- زر تبديل اللغة -->
  <div class="language-toggle">
    <VBtn
      variant="text"
      size="small"
      @click="toggleLanguage"
      class="language-btn"
    >
      {{ currentLanguage === 'ar' ? 'English' : 'العربية' }}
    </VBtn>
  </div>

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
          style="padding-inline: 6.25rem;"
        >
          <VImg
            max-width="613"
            src="/Mosque.png"
            class="auth-illustration mt-0 mb-2"
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
    >
      <VCard
        flat
        :max-width="500"
        class="mt-12 mt-sm-0 pa-4"
      >
        <VCardText>
          <h4 class="text-h4 mb-1">
            {{ t('welcome') }} <span class="text-capitalize"> {{ themeConfig.app.title }} </span>! 👋🏻
          </h4>
          <p class="mb-0">
            {{ t('signInMessage') }}
          </p>
        </VCardText>
   
        <VCardText>
          <VForm
            ref="refVForm"
            @submit.prevent="onSubmit"
          >
            <VRow>
              <!-- email -->
              <VCol cols="12">
                <AppTextField
                  v-model="credentials.email"
                  :label="t('email')"
                  placeholder="johndoe@email.com"
                  type="email"
                  autofocus
                  :error="validationState.errors.email && validationState.errors.email.length > 0 && validationState.touched.email"
                  :error-messages="validationState.errors.email || []"
                  @blur="setFieldTouched('email')"
                />
              </VCol>

              <!-- password -->
              <VCol cols="12">
                <AppTextField
                  v-model="credentials.password"
                  :label="t('passwordField')"
                  placeholder="············"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  autocomplete="password"
                  :error="validationState.errors.password && validationState.errors.password.length > 0 && validationState.touched.password"
                  :error-messages="validationState.errors.password || []"
                  :append-inner-icon="isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
                  @blur="setFieldTouched('password')"
                />

                <div class="d-flex align-center flex-wrap justify-space-between my-6">
                  <VCheckbox
                    v-model="rememberMe"
                    :label="t('rememberMe')"
                  />
                  <RouterLink
                    class="text-primary ms-2 mb-1"
                    :to="{ name: 'forgot-password' }"
                  >
                    {{ t('forgotPassword') }}
                  </RouterLink>
                </div>

                <!-- عرض الأخطاء العامة -->
                <div v-if="validationState.errors.general && validationState.errors.general.length > 0" class="mb-4">
                  <VAlert
                    type="error"
                    variant="tonal"
                    :text="validationState.errors.general[0]"
                    class="mb-4"
                  />
                </div>

                <VBtn
                  block
                  type="submit"
                  :loading="isLoading"
                >
                  {{ t('signIn') }}
                </VBtn>
              </VCol>

              <!-- create account -->
              <VCol
                cols="12"
                class="text-center"
              >
                <span>{{ t('newUser') }}</span>
                <RouterLink
                  class="text-primary ms-1"
                  :to="{ name: 'register' }"
                >
                  {{ t('createAccount') }}
                </RouterLink>
              </VCol>
              <VCol
                cols="12"
                class="d-flex align-center"
              >
                <VDivider />
                <span class="mx-4">{{ t('or') }}</span>
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

// تصميم زر تبديل اللغة
.language-toggle {
  position: absolute;
  top: 2rem;
  right: 2rem;
  z-index: 10;
}

.language-btn {
  background: rgba(255, 255, 255, 0.9) !important;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
  
  &:hover {
    background: rgba(255, 255, 255, 1) !important;
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
}

// تحسينات للاستجابة
@media (max-width: 960px) {
  .language-toggle {
    position: relative;
    top: auto;
    right: auto;
    margin-bottom: 1rem;
    text-align: center;
  }
}
</style>
