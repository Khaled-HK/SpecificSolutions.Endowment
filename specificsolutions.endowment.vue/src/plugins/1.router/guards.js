import { createMongoAbility } from '@casl/ability'

export const setupGuards = router => {
  // دالة لفك تشفير JWT Token محلياً
  const decodeJWT = (token) => {
    try {
      // JWT Token يتكون من 3 أجزاء مفصولة بـ .
      const parts = token.split('.')
      if (parts.length !== 3) return null
      
      // فك تشفير الجزء الثاني (payload)
      const payload = parts[1]
      const decoded = JSON.parse(atob(payload))
      
      return decoded
    } catch (error) {
      console.warn('JWT decode failed:', error)
      return null
    }
  }

  // دالة للتحقق المحلي من صلاحية Token
  const validateTokenLocally = () => {
    try {
      const accessToken = useCookie('accessToken').value
      if (!accessToken) return false
      
      // فك تشفير Token
      const decoded = decodeJWT(accessToken)
      if (!decoded) return false
      
      // التحقق من انتهاء الصلاحية
      const currentTime = Math.floor(Date.now() / 1000)
      if (decoded.exp && decoded.exp < currentTime) {
        console.warn('Token expired locally')
        return false
      }
      
      return true
    } catch (error) {
      console.warn('Local token validation failed:', error)
      return false
    }
  }

  // دالة للتحقق من الصلاحيات باستخدام CASL
  const checkPermissions = (to) => {
    try {
      // إذا لم تكن هناك صلاحيات محددة في meta، اسمح بالوصول
      if (!to.meta.action || !to.meta.subject) {
        return true
      }

          // الحصول على قواعد الصلاحيات من الكوكيز
    const userAbilityRules = useCookie('user-ability-rules').value
      if (!userAbilityRules || !Array.isArray(userAbilityRules)) {
        console.warn('No ability rules found')
        return false
      }

      // إنشاء ability مؤقت للتحقق
      const ability = createMongoAbility(userAbilityRules)

      // التحقق من الصلاحية
      const canAccess = ability.can(to.meta.action, to.meta.subject)
      
      if (!canAccess) {
        console.warn(`Access denied: ${to.meta.action} on ${to.meta.subject}`)
      }
      
      return canAccess
    } catch (error) {
      console.error('Permission check error:', error)
      return false
    }
  }

  // 👉 router.beforeEach
  // Docs: https://router.vuejs.org/guide/advanced/navigation-guards.html#global-before-guards
  router.beforeEach((to, from, next) => {
    try {
      /*
       * If it's a public route, continue navigation. This kind of pages are allowed to visited by login & non-login users. Basically, without any restrictions.
       * Examples of public routes are, 404, under maintenance, etc.
       */
      if (to.meta.public) {
        next()
        return
      }

      /**
       * Check if user is logged in by checking if token & user data exists in local storage
       * Feel free to update this logic to suit your needs
       */
      const userData = useCookie('userData')
      const accessToken = useCookie('accessToken')
      const hasToken = !!(userData.value && accessToken.value)

      /*
        If user is logged in and is trying to access login like page, redirect to home
        else allow visiting the page
        (WARN: Don't allow executing further by return statement because next code will check for permissions)
       */
      if (to.meta.unauthenticatedOnly) {
        if (hasToken) {
          next('/')
          return
        } else {
          next()
          return
        }
      }

      // If user is not logged in and trying to access protected route
      if (!hasToken) {
        next({
          name: 'login',
          query: {
            ...to.query,
            to: to.fullPath !== '/' ? to.path : undefined,
          },
        })
        return
      }

      // التحقق المحلي من Token (بدون الاتصال بالباك إند)
      const isTokenValid = validateTokenLocally()
      if (!isTokenValid) {
        console.warn('Invalid token detected locally, redirecting to login...')
        
        // تنظيف الكوكيز
        userData.value = null
        accessToken.value = null
        
        next({
          name: 'login',
          query: {
            ...to.query,
            to: to.fullPath !== '/' ? to.path : undefined,
          },
        })
        return
      }

      // التحقق من الصلاحيات
      const hasPermission = checkPermissions(to)
      if (!hasPermission) {
        console.warn('Permission denied, redirecting to not-authorized...')
        next({
          name: 'not-authorized',
        })
        return
      }

      // User is logged in, token is valid, and has permissions, allow access
      next()
    } catch (error) {
      console.error('Navigation guard error:', error)
      // Fallback to login page
      next({
        name: 'login',
        query: {
          ...to.query,
          to: to.fullPath !== '/' ? to.path : undefined,
        },
      })
    }
  })
}
