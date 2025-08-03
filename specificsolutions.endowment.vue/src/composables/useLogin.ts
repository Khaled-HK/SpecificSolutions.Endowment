import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAbility } from '@/plugins/casl/composables/useAbility'
import { useApi } from '@/utils/api'
import { BASE_PERMISSIONS, mapPermissionToAction, mapPermissionToSubject } from './authPermissions'
import { useFormValidation } from './useFormValidation'
import Cookies from 'js-cookie'

export function useLogin() {
  const router = useRouter()
  const route = useRoute()
  const ability = useAbility()
  const api = useApi()
  
  const {
    validationState,
    clearErrors,
    setFieldTouched,
    addError,
    setErrorsFromResponse,
  } = useFormValidation()

  const credentials = reactive({
    email: 'admin@demo.com',
    password: 'admin',
  })

  const rememberMe = ref(false)
  const isLoading = ref(false)

  const login = async (t: (key: string) => string) => {
    isLoading.value = true
    
    try {
      const res = await api('/auth/login', {
        method: 'POST',
        body: {
          email: credentials.email,
          password: credentials.password,
          rememberMe: rememberMe.value,
        },
      })

      // معالجة الاستجابة الفاشلة
      if (res.isSuccess === false) {
        if (res.errors && Array.isArray(res.errors)) {
          setErrorsFromResponse(res)
          setFieldTouched('email')
          setFieldTouched('password')
        } else if (res.message?.trim()) {
          addError('general', res.message.trim())
        } else {
          addError('general', t('login.tryAgain'))
        }
        return
      }
      
      const user = res.data
      
      // التحقق من وجود token
      if (!user?.token) {
        addError('general', t('login.tryAgain'))
        return
      }
      
      // حفظ بيانات المستخدم
      Cookies.set('accessToken', user.token)
      Cookies.set('userData', JSON.stringify(user))

      // معالجة الصلاحيات
      const userPermissions = user.permissions || user.userAbilityRules || []
      
      if (!userPermissions.length) {
        addError('email', t('login.noPermissions'))
        return
      }
      
      // إنشاء قواعد الصلاحيات
      const rules = [...BASE_PERMISSIONS]
      userPermissions.forEach(permission => {
        const action = mapPermissionToAction(permission)
        const subject = mapPermissionToSubject(permission)
        
        if (action && subject) {
          rules.push({ action, subject })
        }
      })

      // حفظ الصلاحيات
      try {
        Cookies.set('user-ability-rules', JSON.stringify(rules), { 
          expires: 7,
          path: '/',
          secure: false,
          sameSite: 'lax'
        })
      } catch (error) {
        console.error('Error saving to cookie:', error)
      }
      
      ability.update(rules)

      // إعادة تحميل الصلاحيات
      const { reloadAbilityFromCookie } = await import('@/plugins/casl/ability')
      reloadAbilityFromCookie()

      // التوجيه للصفحة المطلوبة
      const target = route.query.to ? String(route.query.to) : '/dashboard'
      router.replace(target)
    } catch (err: any) {
      // معالجة أخطاء التحقق في catch block
      if (err.data?.errors && Array.isArray(err.data.errors)) {
        setErrorsFromResponse(err.data)
        setFieldTouched('email')
        setFieldTouched('password')
      } else {
        addError('general', t('login.tryAgain'))
      }
    } finally {
      isLoading.value = false
    }
  }

  const onSubmit = async (t: (key: string) => string) => {
    clearErrors()
    setFieldTouched('email')
    setFieldTouched('password')
    await login(t)
  }

  return {
    credentials,
    rememberMe,
    isLoading,
    validationState,
    login,
    onSubmit
  }
} 