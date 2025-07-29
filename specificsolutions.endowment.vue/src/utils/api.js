import { ofetch } from 'ofetch'

// Create a base API instance without composables
export const $api = ofetch.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
})

// Create a composable for API with proper Vue 3 setup
export const useApi = () => {
  const accessToken = useCookie('accessToken')
  const userData = useCookie('userData')
  const { locale } = useI18n()
  const router = useRouter()

  const api = ofetch.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    async onRequest({ options }) {
      if (accessToken.value) {
        options.headers = {
          ...options.headers,
          Authorization: `Bearer ${accessToken.value}`,
        }
      }

      // Add UserId to headers if available
      if (userData.value && userData.value.id) {
        options.headers = {
          ...options.headers,
          'X-User-Id': userData.value.id,
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
        
        // تنظيف الكوكيز
        accessToken.value = null
        userData.value = null
        
        // إعادة توجيه لصفحة تسجيل الدخول
        if (process.client) {
          router.push('/login')
        }
      }
    },
  })

  return api
}
