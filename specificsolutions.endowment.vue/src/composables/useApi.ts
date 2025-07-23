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
      const userData = useCookie('userData').value

      if (accessToken) {
        options.headers = {
          ...options.headers,
          Authorization: `Bearer ${accessToken}`,
        }
      } else if (userData && userData.id) {
        // فقط إذا لم يوجد توكن
        options.headers = {
          ...options.headers,
          'X-User-Id': userData.id,
        }
      }

      // Add Accept-Language header based on current locale
      const { locale } = useI18n()
      const languageMap: Record<string, string> = {
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
  },
}) 