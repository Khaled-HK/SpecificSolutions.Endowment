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
})
