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
      // Get language from localStorage or default to Arabic
      const savedLanguage = typeof window !== 'undefined' ? localStorage.getItem('preferredLanguage') : null
      const currentLanguage = savedLanguage === 'en' ? 'en-US' : 'ar-LY'
      
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
        console.error('Error parsing response data:', error)
        // Return original data if parsing fails
        parsedData = data
      }

      return { data: parsedData, response }
    },
    onFetchError(ctx) {
      console.error('Fetch error:', ctx.error)
      return ctx
    },
  },
}) 