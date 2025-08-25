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
      console.log('🔍 Attempting login with:', {
        email: credentials.email,
        password: credentials.password ? '***' : 'empty',
        rememberMe: rememberMe.value
      })
      
      const res: any = await api('/auth/login', {
        method: 'POST',
        body: {
          email: credentials.email,
          password: credentials.password,
          rememberMe: rememberMe.value,
        },
      })

      console.log('✅ Login response received:', res)

      // معالجة الاستجابة الفاشلة - نمط خالد
      if (res.isSuccess === false || res.state === 400 || res.state === "400" || !res.data) {
        // التحقق من حالة عدم تأكيد البريد الإلكتروني - نمط خالد: الاعتماد على الباك إند
        if (res.requiresEmailConfirmation === true || res.emailNotConfirmed === true || res.data?.requiresEmailConfirmation === true) {
          
          console.log('🔍 Email not confirmed detected from backend, redirecting to email-not-confirmed page')
          console.log('📧 Email:', credentials.email)
          console.log('📝 Backend response:', res)
          
          // توجيه المستخدم إلى صفحة عدم تأكيد البريد الإلكتروني
          try {
            await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
            console.log('✅ Redirect successful')
          } catch (redirectError) {
            console.error('❌ Redirect failed:', redirectError)
            // Fallback: عرض رسالة للمستخدم
            showError('يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول', { timeout: 5000 })
          }
          return
        }
        
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

      // معالجة الصلاحيات - تحسين معالجة الصلاحيات من الباك إند
      const userPermissions = user.permissions || user.userAbilityRules || []
      
      console.log('🔍 User permissions from backend:', userPermissions)
      
      // إنشاء قواعد الصلاحيات مع الصلاحيات الأساسية
      const rules = [...BASE_PERMISSIONS]
      
      // تحويل صلاحيات المستخدم من الباك إند
      if (userPermissions.length > 0) {
        const processedPermissions = new Set() // لتجنب التكرار
        
        userPermissions.forEach((permission: string) => {
          const action = mapPermissionToAction(permission)
          const subject = mapPermissionToSubject(permission)
          
          if (action && subject) {
            const ruleKey = `${action}:${subject}`
            if (!processedPermissions.has(ruleKey)) {
              rules.push({ action, subject })
              processedPermissions.add(ruleKey)
              console.log(`✅ Mapped permission: ${permission} -> { action: '${action}', subject: '${subject}' }`)
            } else {
              console.log(`⏭️ Skipped duplicate permission: ${permission}`)
            }
          } else {
            console.warn(`⚠️ Could not map permission: ${permission}`)
          }
        })
        
        console.log('✅ Processed permissions:', rules.length, 'total rules')
        console.log('📋 Final rules:', rules)
      } else {
        console.warn('⚠️ No user permissions received from backend')
      }

      // حفظ الصلاحيات في الكوكيز
      try {
        const rulesToSave = rules.filter(rule => rule.action && rule.subject) // التأكد من صحة القواعد
        Cookies.set('user-ability-rules', JSON.stringify(rulesToSave), { 
          expires: 7,
          path: '/',
          secure: false,
          sameSite: 'lax'
        })
        console.log('✅ Permissions saved to cookies:', rulesToSave.length, 'rules')
      } catch (error) {
        console.error('❌ Error saving permissions to cookie:', error)
      }
      
      // تحديث الصلاحيات في النظام
      const validRules = rules.filter(rule => rule.action && rule.subject)
      ability.update(validRules)
      console.log('✅ Permissions updated in ability system:', validRules.length, 'rules')

      // إعادة تحميل الصلاحيات للتأكد
      const { reloadAbilityFromCookie } = await import('@/plugins/casl/ability')
      reloadAbilityFromCookie()

      // التوجيه للصفحة المطلوبة
      const target = route.query.to ? String(route.query.to) : '/dashboard'
      router.replace(target)
    } catch (err: any) {
      console.error('❌ Login error:', err)
      console.error('❌ Error details:', {
        status: err.status,
        statusText: err.statusText,
        data: err.data,
        message: err.message
      })
      
      // DEBUG: تحقق مفصل من البيانات
      console.log('🔍 DEBUG: Checking error data structure...')
      console.log('📊 err.data:', err.data)
      console.log('📊 err.data?.message:', err.data?.message)
      console.log('📊 err.data?.message?.trim():', err.data?.message?.trim())
      console.log('📊 err.status:', err.status)
      
      // معالجة أخطاء التحقق في catch block - نمط خالد
      if (err.data?.errors && Array.isArray(err.data.errors)) {
        console.log('🔍 Processing validation errors...')
        setErrorsFromResponse(err.data)
        setFieldTouched('email')
        setFieldTouched('password')
      } else if (err.data?.message?.trim()) {
        console.log('🔍 Processing error message...')
        
        // التحقق من حالة عدم تأكيد البريد الإلكتروني في catch block - نمط خالد: الاعتماد على الباك إند
        if (err.data.requiresEmailConfirmation === true || err.data.emailNotConfirmed === true || err.data.data?.requiresEmailConfirmation === true) {
          
          console.log('🔍 Email not confirmed detected from backend (catch block), redirecting to email-not-confirmed page')
          console.log('📧 Email:', credentials.email)
          console.log('📝 Backend error response:', err.data)
          
          // توجيه المستخدم إلى صفحة عدم تأكيد البريد الإلكتروني
          try {
            await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
            console.log('✅ Redirect successful')
          } catch (redirectError) {
            console.error('❌ Redirect failed:', redirectError)
            // Fallback: عرض رسالة للمستخدم
            showError('يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول', { timeout: 5000 })
          }
          return
        }
        
        // Fallback: التحقق من النص في الرسالة (للتوافق مع الباك إند الحالي)
        const message = err.data.message.toLowerCase()
        console.log('🔍 DEBUG: Checking message for email confirmation keywords...')
        console.log('📝 Original message:', err.data.message)
        console.log('📝 Lowercase message:', message)
        console.log('🔍 Contains "email":', message.includes('email'))
        console.log('🔍 Contains "بريدك":', message.includes('بريدك'))
        console.log('🔍 Contains "تأكيد":', message.includes('تأكيد'))
        console.log('🔍 Contains "confirm":', message.includes('confirm'))
        
        if ((message.includes('email') || message.includes('بريدك') || message.includes('بريدك الإلكتروني')) && 
            (message.includes('confirm') || 
             message.includes('verify') ||
             message.includes('تأكيد') ||
             message.includes('تحقق') ||
             message.includes('يرجى'))) {
          
          console.log('🔍 Email not confirmed detected from message text (catch block), redirecting to email-not-confirmed page')
          console.log('📧 Email:', credentials.email)
          console.log('📝 Message:', err.data.message)
          
          // توجيه المستخدم إلى صفحة عدم تأكيد البريد الإلكتروني
          try {
            await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
            console.log('✅ Redirect successful')
          } catch (redirectError) {
            console.error('❌ Redirect failed:', redirectError)
            // Fallback: عرض رسالة للمستخدم
            showError('يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول', { timeout: 5000 })
          }
          return
        }
        
        console.log('🔍 No email confirmation detected, showing error message...')
        // عرض رسالة الخطأ من الباك اند - نمط خالد (Toast فقط)
        showError(err.data.message.trim(), { timeout: 5000 })
      } else if (err.status === 401) {
        // معالجة خاصة لخطأ 401 - قد يكون عدم تأكيد البريد الإلكتروني
        console.log('🔍 401 Unauthorized error detected, checking if it might be email confirmation issue')
        console.log('📧 Email:', credentials.email)
        console.log('📝 Full error object:', err)
        
        // توجيه المستخدم إلى صفحة عدم تأكيد البريد الإلكتروني كـ fallback
        try {
          await router.push(`/email-not-confirmed?email=${encodeURIComponent(credentials.email)}`)
          console.log('✅ Redirect to email-not-confirmed page successful')
        } catch (redirectError) {
          console.error('❌ Redirect failed:', redirectError)
          // Fallback: عرض رسالة عامة
          showError('يرجى التحقق من بيانات تسجيل الدخول أو تأكيد بريدك الإلكتروني', { timeout: 5000 })
        }
      } else if (err.message?.trim()) {
        console.log('🔍 Processing general error message...')
        // عرض رسالة الخطأ العامة (Toast فقط)
        showError(err.message.trim(), { timeout: 5000 })
      } else {
        console.log('🔍 No specific error handling, showing generic message...')
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

 