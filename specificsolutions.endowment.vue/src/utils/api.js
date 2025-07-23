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
        const router = useRouter()
        router.push('/login')
      }
    }
  },
})
