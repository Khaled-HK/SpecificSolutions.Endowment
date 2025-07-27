// Ported from [Nuxt](https://github.com/nuxt/nuxt/blob/main/packages/nuxt/src/app/composables/cookie.ts)
import { parse, serialize } from 'cookie-es'
import { destr } from 'destr'

const CookieDefaults = {
  path: '/',
  watch: true,
  decode: val => destr(decodeURIComponent(val)),
  encode: val => encodeURIComponent(typeof val === 'string' ? val : JSON.stringify(val)),
}

export const useCookie = (name, _opts) => {
  // Validate cookie name
  if (!name || typeof name !== 'string') {
    console.error('useCookie: Invalid cookie name provided:', name)
    return ref(null)
  }
  
  // Sanitize cookie name to ensure it's valid for cookies
  const sanitizedName = name.replace(/[^a-zA-Z0-9_-]/g, '_')
  if (sanitizedName !== name) {
    console.warn(`useCookie: Cookie name "${name}" was sanitized to "${sanitizedName}"`)
  }
  
  const opts = { ...CookieDefaults, ..._opts || {} }
  const cookies = parse(document.cookie, opts)
  const cookie = ref(cookies[sanitizedName] ?? opts.default?.())

  watch(cookie, () => {
    document.cookie = serializeCookie(sanitizedName, cookie.value, opts)
  })
  
  return cookie
}
function serializeCookie(name, value, opts = {}) {
  if (value === null || value === undefined)
    return serialize(name, value, { ...opts, maxAge: -1 })
  
  return serialize(name, value, { ...opts, maxAge: 60 * 60 * 24 * 30 })
}
