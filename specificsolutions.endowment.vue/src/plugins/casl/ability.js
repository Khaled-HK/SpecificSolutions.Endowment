import { createMongoAbility } from '@casl/ability'
import Cookies from 'js-cookie'
import { mapPermissionToAction, mapPermissionToSubject } from '@/composables/authPermissions'

export const ability = createMongoAbility()

// دالة لضمان تحميل الصلاحيات عند بدء التطبيق
const initializePermissions = () => {
  // محاولة تحميل الصلاحيات من الكوكيز
  const loaded = reloadAbilityFromCookie()
  
  // إذا لم يتم تحميل الصلاحيات، إضافة صلاحيات أساسية شاملة
  if (!loaded) {
    const basicRules = [
      // صلاحيات أساسية للوحة القيادة والمصادقة
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' },
      { action: 'write', subject: 'Auth' },
      
      // صلاحيات للصفحات العامة
      { action: 'View', subject: 'Login' },
      { action: 'View', subject: 'Register' },
      { action: 'View', subject: 'ForgotPassword' },
      { action: 'View', subject: 'ResendEmailConfirmation' },
      { action: 'View', subject: 'ConfirmEmail' },
      
      // صلاحيات أساسية لإدارة الأوقاف
      { action: 'View', subject: 'Building' },
      { action: 'View', subject: 'Mosque' },
      { action: 'View', subject: 'City' },
      { action: 'View', subject: 'Region' },
      { action: 'View', subject: 'Office' },
      { action: 'View', subject: 'Product' },
      { action: 'View', subject: 'Account' },
      { action: 'View', subject: 'User' },
      { action: 'View', subject: 'Request' },
      { action: 'View', subject: 'Decision' },
      
      // صلاحيات القراءة الأساسية
      { action: 'read', subject: 'Building' },
      { action: 'read', subject: 'Mosque' },
      { action: 'read', subject: 'City' },
      { action: 'read', subject: 'Region' },
      { action: 'read', subject: 'Office' },
      { action: 'read', subject: 'Product' },
      { action: 'read', subject: 'Account' },
      { action: 'read', subject: 'User' },
      { action: 'read', subject: 'Request' },
      { action: 'read', subject: 'Decision' }
    ]
    ability.update(basicRules)
    console.log('✅ Using comprehensive basic permissions as fallback')
  }
  
  // التأكد من أن الصلاحيات محملة
  if (ability.rules.length === 0) {
    const basicRules = [
      // صلاحيات أساسية للوحة القيادة والمصادقة
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' },
      { action: 'write', subject: 'Auth' },
      
      // صلاحيات للصفحات العامة
      { action: 'View', subject: 'Login' },
      { action: 'View', subject: 'Register' },
      { action: 'View', subject: 'ForgotPassword' },
      { action: 'View', subject: 'ResendEmailConfirmation' },
      { action: 'View', subject: 'ConfirmEmail' },
      
      // صلاحيات أساسية لإدارة الأوقاف
      { action: 'View', subject: 'Building' },
      { action: 'View', subject: 'Mosque' },
      { action: 'View', subject: 'City' },
      { action: 'View', subject: 'Region' },
      { action: 'View', subject: 'Office' },
      { action: 'View', subject: 'Product' },
      { action: 'View', subject: 'Account' },
      { action: 'View', subject: 'User' },
      { action: 'View', subject: 'Request' },
      { action: 'View', subject: 'Decision' },
      
      // صلاحيات القراءة الأساسية
      { action: 'read', subject: 'Building' },
      { action: 'read', subject: 'Mosque' },
      { action: 'read', subject: 'City' },
      { action: 'read', subject: 'Region' },
      { action: 'read', subject: 'Office' },
      { action: 'read', subject: 'Product' },
      { action: 'read', subject: 'Account' },
      { action: 'read', subject: 'User' },
      { action: 'read', subject: 'Request' },
      { action: 'read', subject: 'Decision' }
    ]
    ability.update(basicRules)
    console.log('✅ No rules found, adding comprehensive basic permissions')
  }
}

// تحميل الصلاحيات تلقائياً عند بدء التطبيق
setTimeout(() => {
  initializePermissions()
}, 100)

// دالة لإعادة تحميل الصلاحيات من الكوكيز
export const reloadAbilityFromCookie = () => {
  try {
    const storedRules = Cookies.get('user-ability-rules')
    
    if (storedRules) {
      const userAbilityRules = JSON.parse(storedRules)
      const normalized = normalizeRules(userAbilityRules)
      
      if (normalized && Array.isArray(normalized) && normalized.length > 0) {
        // إضافة الصلاحيات الأساسية إلى الصلاحيات المحملة
        const rulesWithBasics = [
          // صلاحيات أساسية مطلوبة
          { action: 'View', subject: 'Dashboard' },
          { action: 'read', subject: 'Auth' },
          // الصلاحيات المحملة من الكوكيز
          ...normalized
        ]
        
        ability.update(rulesWithBasics)
        console.log('✅ Permissions loaded successfully:', rulesWithBasics.length, 'rules')
        return true
      }
    }
    
    // Fallback: تحميل من userData إذا لم تكن الصلاحيات محفوظة
    const userData = Cookies.get('userData')
    
    if (userData) {
      try {
        const user = JSON.parse(userData)
        
        if (user.permissions && user.permissions.length > 0) {
          // تحويل الصلاحيات من real API إلى تنسيق CASL
          const rules = []
          
          // إضافة صلاحيات أساسية مطلوبة
          rules.push(
            { action: 'View', subject: 'Dashboard' },
            { action: 'read', subject: 'Auth' }
          )
          
          // تحويل صلاحيات المستخدم المحددة
          user.permissions.forEach(permission => {
            const action = mapPermissionToAction(permission)
            const subject = mapPermissionToSubject(permission)
            
            if (action && subject) {
              rules.push({ action, subject })
            }
          })
          
          const normalized = normalizeRules(rules)
          // حفظ الصلاحيات في الكوكيز
          Cookies.set('user-ability-rules', JSON.stringify(normalized), { 
            expires: 7,
            path: '/',
            secure: false,
            sameSite: 'lax'
          })
          
          ability.update(normalized)
          console.log('✅ Permissions loaded from userData and saved to cookies')
          return true
        }
      } catch (error) {
        console.error('❌ Error parsing userData:', error)
      }
    }
    
    // إذا لم تكن هناك صلاحيات، إضافة صلاحيات أساسية شاملة
    const basicRules = [
      // صلاحيات أساسية للوحة القيادة والمصادقة
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' },
      { action: 'write', subject: 'Auth' },
      
      // صلاحيات للصفحات العامة
      { action: 'View', subject: 'Login' },
      { action: 'View', subject: 'Register' },
      { action: 'View', subject: 'ForgotPassword' },
      { action: 'View', subject: 'ResendEmailConfirmation' },
      { action: 'View', subject: 'ConfirmEmail' },
      
      // صلاحيات أساسية لإدارة الأوقاف
      { action: 'View', subject: 'Building' },
      { action: 'View', subject: 'Mosque' },
      { action: 'View', subject: 'City' },
      { action: 'View', subject: 'Region' },
      { action: 'View', subject: 'Office' },
      { action: 'View', subject: 'Product' },
      { action: 'View', subject: 'Account' },
      { action: 'View', subject: 'User' },
      { action: 'View', subject: 'Request' },
      { action: 'View', subject: 'Decision' },
      
      // صلاحيات القراءة الأساسية
      { action: 'read', subject: 'Building' },
      { action: 'read', subject: 'Mosque' },
      { action: 'read', subject: 'City' },
      { action: 'read', subject: 'Region' },
      { action: 'read', subject: 'Office' },
      { action: 'read', subject: 'Product' },
      { action: 'read', subject: 'Account' },
      { action: 'read', subject: 'User' },
      { action: 'read', subject: 'Request' },
      { action: 'read', subject: 'Decision' }
    ]
    ability.update(basicRules)
    console.log('✅ No permissions found, using comprehensive basic fallback')
    return true
  } catch (error) {
    console.error('❌ Error loading permissions:', error)
    // في حالة الخطأ، إضافة صلاحيات أساسية
    const basicRules = [
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' }
    ]
    ability.update(basicRules)
    return true
  }
}

// Normalize mixed rule formats to CASL-compatible { action, subject }
function normalizeRules(rules) {
  try {
    return (rules || []).map(r => {
      if (typeof r === 'string') {
        // e.g. "ChangeOfPathRequest_View"
        if (r.endsWith('_View')) return { action: 'View', subject: r.replace(/_View$/, '') }
        if (r.endsWith('_Add')) return { action: 'Add', subject: r.replace(/_Add$/, '') }
        if (r.endsWith('_Edit')) return { action: 'Edit', subject: r.replace(/_Edit$/, '') }
        if (r.endsWith('_Delete')) return { action: 'Delete', subject: r.replace(/_Delete$/, '') }
        return null
      }
      if (typeof r === 'object' && r && (r.action || r.Action) && (r.subject || r.Subject)) {
        return { action: r.action || r.Action, subject: r.subject || r.Subject }
      }
      return r
    }).filter(Boolean)
  } catch {
    return []
  }
}

// Helper functions to map backend permissions to CASL actions/subjects
// Using imported functions from authPermissions.ts for consistency

// Export for backward compatibility
export const reloadAbilityFromLocalStorage = reloadAbilityFromCookie
