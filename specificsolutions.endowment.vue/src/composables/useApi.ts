import { ofetch } from 'ofetch'
import Cookies from 'js-cookie'

export const useApi = () => {
  const client = ofetch.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    async onRequest({ options }) {
      const accessTokenCookie = useCookie('accessToken').value as string | null | undefined
      const accessToken = accessTokenCookie || Cookies.get('accessToken')
      const userDataCookie = useCookie('userData').value as any
      let userData = userDataCookie
      if (!userData && typeof document !== 'undefined') {
        const userDataString = Cookies.get('userData')
        try { userData = userDataString ? JSON.parse(userDataString) : null } catch { userData = null }
      }

      // Merge headers as plain object
      const currentHeaders: Record<string, string> = {}
      const hdrs: any = options.headers as any
      if (hdrs && typeof hdrs.forEach === 'function') {
        hdrs.forEach((v: string, k: string) => { currentHeaders[k] = v })
      } else if (Array.isArray(hdrs)) {
        hdrs.forEach(([k, v]: [string, string]) => { currentHeaders[k] = v })
      } else if (hdrs && typeof hdrs === 'object') {
        Object.assign(currentHeaders, hdrs as Record<string, string>)
      }
      currentHeaders['Accept'] = 'application/json'

      if (accessToken) {
        currentHeaders['Authorization'] = `Bearer ${accessToken}`
      } else if (userData && (userData.id || userData.Id)) {
        currentHeaders['X-User-Id'] = String(userData.id ?? userData.Id)
      }

      const savedLanguage = typeof window !== 'undefined' ? localStorage.getItem('preferredLanguage') : null
      const currentLanguage = savedLanguage === 'en' ? 'en-US' : 'ar-LY'
      currentHeaders['Accept-Language'] = currentLanguage
      options.headers = currentHeaders as any
    },
    async onResponseError({ response }) {
      if (response && (response.status === 401 || response.status === 403)) {
        Cookies.remove('accessToken')
        Cookies.remove('userData')
        Cookies.remove('user-ability-rules')
        if (typeof window !== 'undefined') window.location.href = '/login'
      }
    },
  })

  return client
}