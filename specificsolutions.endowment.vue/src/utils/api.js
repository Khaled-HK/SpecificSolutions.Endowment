import { ofetch } from 'ofetch'
import Cookies from 'js-cookie'

// Create a unified global API instance
const globalApi = ofetch.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  async onRequest({ options }) {
    const accessToken = Cookies.get('accessToken')
    const userDataString = Cookies.get('userData')
    let userData = null
    
    if (userDataString) {
      try {
        userData = JSON.parse(userDataString)
      } catch (error) {
        console.error('Error parsing userData from cookie:', error)
      }
    }
    
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

    // Add Accept-Language header (default to Arabic)
    options.headers = {
      ...options.headers,
      'Accept-Language': 'ar-LY',
    }
  },
  async onResponseError({ response }) {
    if (response.status === 401 || response.status === 403) {
      console.warn('Token expired or unauthorized, clearing cookies...')
      
      const accessToken = Cookies.get('accessToken')
      if (accessToken) {
        Cookies.remove('accessToken')
        Cookies.remove('userData')
        Cookies.remove('user-ability-rules')
        
        if (process.client) {
          window.location.href = '/login'
        }
      }
    }
  },
})

// Export unified $api for all usage
export const $api = globalApi

// For backward compatibility and Vue 3 composable pattern
export const useApi = () => globalApi
