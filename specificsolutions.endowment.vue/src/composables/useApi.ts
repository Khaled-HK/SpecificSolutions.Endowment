import { ofetch } from 'ofetch'
import Cookies from 'js-cookie'

export const useApi = () => {
  const resolvedBaseURL = (import.meta as any).env?.VITE_API_BASE_URL as string | undefined
  if (!resolvedBaseURL) {
    throw new Error('VITE_API_BASE_URL is not set. Please define it in your environment (.env/.env.local).')
  }

  const client = ofetch.create({
    baseURL: resolvedBaseURL,
    credentials: 'include',
    async onRequest({ options }) {
      const accessTokenCookie = useCookie('accessToken').value as string | null | undefined
      const accessToken = accessTokenCookie || Cookies.get('accessToken')

      const rawUserDataCookie = useCookie('userData').value as unknown
      let userData: any = null
      if (rawUserDataCookie) {
        if (typeof rawUserDataCookie === 'string') {
          try {
            userData = JSON.parse(rawUserDataCookie)
          } catch {
            userData = null
          }
        } else if (typeof rawUserDataCookie === 'object') {
          userData = rawUserDataCookie
        }
      }

      // Normalize headers to a Headers instance for consistent setting
      const headers = new Headers(options.headers as any)

      if (accessToken) {
        headers.set('Authorization', `Bearer ${accessToken}`)
      } else if (userData && (userData.token || userData.Token || userData.accessToken)) {
        const fallbackToken = userData.token || userData.Token || userData.accessToken
        headers.set('Authorization', `Bearer ${fallbackToken}`)
      } else if (userData && (userData.id || userData.Id)) {
        headers.set('X-User-Id', String(userData.id ?? userData.Id))
      }

      // No debug logs in production; keep headers minimal

      options.headers = headers
    },
    async onResponseError({ response }) {
      // لا نقوم بإعادة التوجيه تلقائياً. نسمح للمكونات بالتعامل مع 401/403 وعرض الرسالة دون إنهاء الجلسة.
      // إذا رغبت بإعادة التوجيه تلقائياً عند 401، يمكن إعادة تفعيل ذلك لاحقاً.
      return
    },
  })

  return client
}