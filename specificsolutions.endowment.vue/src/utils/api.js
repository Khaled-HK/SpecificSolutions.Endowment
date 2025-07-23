import { ofetch } from 'ofetch'

export const $api = ofetch.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  async onRequest({ options }) {
    const accessToken = useCookie('accessToken').value
    const userData = useCookie('userData').value

    if (accessToken) {
      options.headers = {
        ...options.headers,
        Authorization: `Bearer ${accessToken}`,
      }
    }

    // Add UserId to headers if available
    if (userData && userData.id) {
      options.headers = {
        ...options.headers,
        'X-User-Id': userData.id,
      }
    }

    // Add Accept-Language header based on current locale
    // Use cookie or localStorage instead of useI18n to avoid composition API error
    const currentLocale = useCookie('i18n_redirected').value || 'ar'
    const languageMap = {
      'ar': 'ar-LY',
      'en': 'en-US'
    }
    const currentLanguage = languageMap[currentLocale] || 'ar-LY'
    
    options.headers = {
      ...options.headers,
      'Accept-Language': currentLanguage,
    }
  },
  async onResponseError({ response }) {
    // إذا كان الخطأ 401 (غير مخول) أو 403 (ممنوع)
    if (response.status === 401 || response.status === 403) {
      console.warn('Token expired or unauthorized, clearing cookies...')
      
      // تنظيف الكوكيز
      const accessToken = useCookie('accessToken')
      const userData = useCookie('userData')
      
      accessToken.value = null
      userData.value = null
      
      // إعادة توجيه لصفحة تسجيل الدخول
      if (process.client) {
        // Use window.location instead of useRouter to avoid composition API error
        window.location.href = '/login'
      }
    }
  },
})
