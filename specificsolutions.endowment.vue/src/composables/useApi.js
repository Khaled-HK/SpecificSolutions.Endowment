import { createFetch } from '@vueuse/core'
import { destr } from 'destr'

export const useApi = createFetch({
  baseUrl: import.meta.env.VITE_API_BASE_URL || '/api',
  fetchOptions: {
    headers: {
      Accept: 'application/json',
    },
  },
  options: {
    refetch: true,
    async beforeFetch({ options }) {
      const accessToken = useCookie('accessToken').value
      if (accessToken) {
        options.headers = {
          ...options.headers,
          Authorization: `Bearer ${accessToken}`,
        }
      }

      // Add Accept-Language header based on current locale
      const { locale } = useI18n()
      const languageMap = {
        'ar': 'ar-LY',
        'en': 'en-US'
      }
      const currentLanguage = languageMap[locale.value] || 'ar-LY'
      
      options.headers = {
        ...options.headers,
        'Accept-Language': currentLanguage,
      }
      
      return { options }
    },
    afterFetch(ctx) {
      const { data, response } = ctx

      // Parse data if it's JSON
      let parsedData = null
      try {
        parsedData = destr(data)
      }
      catch (error) {
        console.error(error)
      }
      
      return { data: parsedData, response }
    },
    onFetchError(ctx) {
      const { response } = ctx
      
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
          const router = useRouter()
          router.push('/login')
        }
      }
    },
  },
})
