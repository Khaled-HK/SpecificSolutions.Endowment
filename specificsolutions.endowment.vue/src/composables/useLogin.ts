import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAbility } from '@/plugins/casl/composables/useAbility'
import { useApi } from '@/composables/useApi'
import { BASE_PERMISSIONS, mapPermissionToAction, mapPermissionToSubject } from './authPermissions'
import { useFormValidation } from './useFormValidation'
import { useAppAlerts } from '@/composables/useAppAlerts'
import Cookies from 'js-cookie'

export function useLogin() {
  const router = useRouter()
  const route = useRoute()
  const ability = useAbility()
  const api = useApi()
  
  // نظام التنبيهات - نمط خالد
  const { error: showError } = useAppAlerts()
  
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
      const res: any = await api('/auth/login', {
        method: 'POST',
        body: {
          email: credentials.email,
          password: credentials.password,
          rememberMe: rememberMe.value,
        },
      })

      // معالجة الاستجابة الفاشلة - نمط خالد
      if (res.isSuccess === false || res.state === 400 || res.state === "400" || !res.data) {
        if (res.errors && Array.isArray(res.errors) && res.errors.length > 0) {
          setErrorsFromResponse(res)
          setFieldTouched('email')
          setFieldTouched('password')
        } else if (res.message?.trim()) {
          // عرض رسالة الباك اند مباشرة - نمط خالد (Toast فقط)
          showError(res.message.trim(), { timeout: 5000 })
          setFieldTouched('email')
          setFieldTouched('password')
        } else {
          showError(t('login.tryAgain'), { timeout: 5000 })
        }
        return
      }
      
      const user = res.data
      
      // التحقق من وجود token أو accessToken من الخادم
      const token = user?.token || user?.Token || user?.accessToken
      if (!token) {
        addError('general', t('login.tryAgain'))
        return
      }
      
      // حفظ بيانات المستخدم على المسار الجذري لضمان توافرها في جميع الصفحات
      Cookies.set('accessToken', token, {
        expires: 7,
        path: '/',
        secure: false,
        sameSite: 'lax',
      })
      Cookies.set('userData', JSON.stringify(user), {
        expires: 7,
        path: '/',
        secure: false,
        sameSite: 'lax',
      })

      // معالجة الصلاحيات
      const userPermissions = user.permissions || user.userAbilityRules || []
      
      if (!userPermissions.length) {
        showError(t('login.noPermissions'), { timeout: 5000 })
        return
      }
      
      // إنشاء قواعد الصلاحيات
      const rules = [...BASE_PERMISSIONS]
      userPermissions.forEach((permission: string) => {
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
      // معالجة أخطاء التحقق في catch block - نمط خالد
      if (err.data?.errors && Array.isArray(err.data.errors)) {
        setErrorsFromResponse(err.data)
        setFieldTouched('email')
        setFieldTouched('password')
      } else if (err.data?.message?.trim()) {
        // عرض رسالة الخطأ من الباك اند - نمط خالد (Toast فقط)
        showError(err.data.message.trim(), { timeout: 5000 })
      } else if (err.message?.trim()) {
        // عرض رسالة الخطأ العامة (Toast فقط)
        showError(err.message.trim(), { timeout: 5000 })
      } else {
        showError(t('login.tryAgain'), { timeout: 5000 })
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