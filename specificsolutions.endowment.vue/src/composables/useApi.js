import { createFetch } from '@vueuse/core'
import { destr } from 'destr'

export const useApi = () => {
  // قراءة البيانات من localStorage
  const getAccessToken = () => localStorage.getItem('accessToken')
  const getUserData = () => {
    const userDataString = localStorage.getItem('userData')
    if (userDataString) {
      try {
        return JSON.parse(userDataString)
      } catch (error) {
        console.error('Error parsing userData from localStorage:', error)
        return null
      }
    }
    return null
  }
  
  const { locale } = useI18n()
  const router = useRouter()

  return createFetch({
    baseUrl: 'http://localhost:5173/api',
    fetchOptions: {
      headers: {
        Accept: 'application/json',
      },
    },
    options: {
      refetch: true,
      async beforeFetch({ options }) {
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
          console.warn('Token expired or unauthorized, clearing localStorage...')
          
          // تنظيف localStorage
          localStorage.removeItem('accessToken')
          localStorage.removeItem('userData')
          localStorage.removeItem('user-ability-rules')
          
          // إعادة توجيه لصفحة تسجيل الدخول
          if (process.client) {
            router.push('/login')
          }
        }
      },
    },
  })
}
