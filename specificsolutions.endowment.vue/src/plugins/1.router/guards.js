import { createMongoAbility } from '@casl/ability'
import { reloadAbilityFromCookie } from '../casl/ability'
import Cookies from 'js-cookie'

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
      const accessToken = Cookies.get('accessToken')
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

      // Get user ability rules from cookie
      let userAbilityRules = []
      try {
        const storedRules = Cookies.get('user-ability-rules')
        if (storedRules) {
          userAbilityRules = JSON.parse(storedRules)
        } else {
          // إذا لم تكن الصلاحيات محفوظة، حاول تحميلها من userData
          const userData = Cookies.get('userData')
          if (userData) {
            const user = JSON.parse(userData)
            
            if (user.permissions && user.permissions.length > 0) {
              // تحويل الصلاحيات من real API إلى تنسيق CASL
              const rules = []
              
              // Add general permissions for Auth subject (required for page access)
              rules.push(
                { action: 'read', subject: 'Auth' },
                { action: 'write', subject: 'Auth' },
                { action: 'delete', subject: 'Auth' }
              )

              // Add Dashboard permissions (required for email and other dashboard pages)
              rules.push(
                { action: 'View', subject: 'Dashboard' },
                { action: 'read', subject: 'Dashboard' },
                { action: 'write', subject: 'Dashboard' }
              )

              // Convert specific permissions from backend
              user.permissions.forEach(permission => {
                const action = mapPermissionToAction(permission)
                const subject = mapPermissionToSubject(permission)
                
                if (action && subject) {
                  rules.push({ action, subject })
                }
              })

              // حفظ الصلاحيات في الكوكيز
              Cookies.set('user-ability-rules', JSON.stringify(rules), { 
                expires: 7,
                path: '/',
                secure: false,
                sameSite: 'lax'
              })
              
              userAbilityRules = rules
            }
          }
        }
      } catch (error) {
        console.error('❌ Error reading from cookie:', error)
        userAbilityRules = []
      }

      // إنشاء ability مؤقت للتحقق
      const ability = createMongoAbility(userAbilityRules)

      // التحقق من الصلاحية
      const canAccess = ability.can(to.meta.action, to.meta.subject)
      
      if (!canAccess) {
        console.warn(`❌ Access denied: ${to.meta.action} on ${to.meta.subject}`)
        return false
      }
      
      return canAccess
    } catch (error) {
      console.error('❌ Permission check error:', error)
      return false
    }
  }

  // Helper functions to map backend permissions to CASL actions/subjects
  function mapPermissionToAction(permission) {
    // Handle underscore format from backend (e.g., "City_View", "Account_Add")
    if (permission.endsWith('_View')) return 'View'
    if (permission.endsWith('_Add')) return 'Add'
    if (permission.endsWith('_Edit')) return 'Edit'
    if (permission.endsWith('_Delete')) return 'Delete'
    
    // Handle fake API format (e.g., { action: 'manage', subject: 'all' })
    if (typeof permission === 'object' && permission.action) {
      return permission.action
    }
    
    return null // Return null for unknown permissions
  }

  function mapPermissionToSubject(permission) {
    // Handle underscore format from backend (e.g., "City_View", "Account_Add")
    if (permission.startsWith('AccountDetail_')) return 'AccountDetail'
    if (permission.startsWith('ConstructionRequest_')) return 'ConstructionRequest'
    if (permission.startsWith('MaintenanceRequest_')) return 'MaintenanceRequest'
    if (permission.startsWith('ChangeRequest_')) return 'ChangeRequest'
    if (permission.startsWith('DemolitionRequest_')) return 'DemolitionRequest'
    if (permission.startsWith('NameChangeRequest_')) return 'NameChangeRequest'
    if (permission.startsWith('NeedsRequest_')) return 'NeedsRequest'
    if (permission.startsWith('ExpenditureChangeRequest_')) return 'ExpenditureChangeRequest'
    if (permission.startsWith('ChangeOfPathRequest_')) return 'ChangeOfPathRequest'
    if (permission.startsWith('Account_')) return 'Account'
    if (permission.startsWith('User_')) return 'User'
    if (permission.startsWith('Role_')) return 'Role'
    if (permission.startsWith('Decision_')) return 'Decision'
    if (permission.startsWith('Request_')) return 'Request'
    if (permission.startsWith('Office_')) return 'Office'
    if (permission.startsWith('Endowment_')) return 'Endowment'
    if (permission.startsWith('City_')) return 'City'
    if (permission.startsWith('Region_')) return 'Region'
    if (permission.startsWith('Building_')) return 'Building'
    if (permission.startsWith('Mosque_')) return 'Mosque'
    
    // Handle fake API format (e.g., { action: 'manage', subject: 'all' })
    if (typeof permission === 'object' && permission.subject) {
      return permission.subject
    }
    
    return null // Return null for unknown permissions
  }

  // 👉 router.beforeEach
  // Docs: https://router.vuejs.org/guide/advanced/navigation-guards.html#global-before-guards
  router.beforeEach(async (to, from, next) => {
    try {
      // إعادة تحميل الصلاحيات من الكوكيز في كل مرة
      reloadAbilityFromCookie()
      
      // Add a small delay to ensure cookies are properly loaded
      await new Promise(resolve => setTimeout(resolve, 100))
      
      /*
       * If it's a public route, continue navigation. This kind of pages are allowed to visited by login & non-login users. Basically, without any restrictions.
       * Examples of public routes are, 404, under maintenance, etc.
       */
      if (to.meta.public) {
        next()
        return
      }

      /**
       * Check if user is logged in by checking if token & user data exists in cookie
       * Feel free to update this logic to suit your needs
       */
      const userData = Cookies.get('userData')
      const accessToken = Cookies.get('accessToken')
      const hasToken = !!(userData && accessToken)

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
        
        // Only clear cookies if we have a token but it's invalid
        const accessToken = Cookies.get('accessToken')
        if (accessToken) {
          // تنظيف الكوكيز
          Cookies.remove('accessToken')
          Cookies.remove('userData')
          Cookies.remove('user-ability-rules')
        }
        
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
