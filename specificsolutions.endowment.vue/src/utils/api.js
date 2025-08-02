import { ofetch } from 'ofetch'
import Cookies from 'js-cookie'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

// Create a composable for API with proper Vue 3 setup
export const useApi = () => {
  // قراءة البيانات من الكوكيز
  const getAccessToken = () => Cookies.get('accessToken')
  const getUserData = () => {
    const userDataString = Cookies.get('userData')
    if (userDataString) {
      try {
        return JSON.parse(userDataString)
      } catch (error) {
        console.error('Error parsing userData from cookie:', error)
        return null
      }
    }
    return null
  }
  
  const { locale } = useI18n()
  const router = useRouter()

  const api = ofetch.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    async onRequest({ options }) {
      const accessToken = getAccessToken()
      const userData = getUserData()
      
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
      const languageMap = {
        'ar': 'ar-LY',
        'en': 'en-US'
      }
      const currentLanguage = languageMap[locale.value] || 'ar-LY'
      
      options.headers = {
        ...options.headers,
        'Accept-Language': currentLanguage,
      }
    },
    async onResponseError({ response }) {
      // إذا كان الخطأ 401 (غير مخول) أو 403 (ممنوع)
      if (response.status === 401 || response.status === 403) {
        console.warn('Token expired or unauthorized, clearing cookies...')
        
        // Only clear cookies if we have a token but it's invalid
        const accessToken = Cookies.get('accessToken')
        if (accessToken) {
          // تنظيف الكوكيز
          Cookies.remove('accessToken')
          Cookies.remove('userData')
          Cookies.remove('user-ability-rules')
          
          // إعادة توجيه لصفحة تسجيل الدخول
          if (process.client) {
            router.push('/login')
          }
        }
      }
    },
  })

  return api
}
