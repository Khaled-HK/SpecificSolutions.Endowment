import { createMongoAbility } from '@casl/ability'
import Cookies from 'js-cookie'

export const ability = createMongoAbility()

// دالة لضمان تحميل الصلاحيات عند بدء التطبيق
const initializePermissions = () => {
  // محاولة تحميل الصلاحيات من الكوكيز
  const loaded = reloadAbilityFromCookie()
  
  // إذا لم يتم تحميل الصلاحيات، إضافة صلاحيات أساسية محدودة
  if (!loaded) {
    const basicRules = [
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' }
    ]
    ability.update(basicRules)
    console.log('⚠️ Using basic permissions as fallback')
  }
  
  // التأكد من أن الصلاحيات محملة
  if (ability.rules.length === 0) {
    const basicRules = [
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' }
    ]
    ability.update(basicRules)
    console.log('⚠️ No rules found, adding basic permissions')
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
      
      if (userAbilityRules && Array.isArray(userAbilityRules) && userAbilityRules.length > 0) {
        // إضافة الصلاحيات الأساسية إلى الصلاحيات المحملة
        const rulesWithBasics = [
          // صلاحيات أساسية مطلوبة
          { action: 'View', subject: 'Dashboard' },
          { action: 'read', subject: 'Auth' },
          // الصلاحيات المحملة من الكوكيز
          ...userAbilityRules
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
          
          // حفظ الصلاحيات في الكوكيز
          Cookies.set('user-ability-rules', JSON.stringify(rules), { 
            expires: 7,
            path: '/',
            secure: false,
            sameSite: 'lax'
          })
          
          ability.update(rules)
          console.log('✅ Permissions loaded from userData and saved to cookies')
          return true
        }
      } catch (error) {
        console.error('❌ Error parsing userData:', error)
      }
    }
    
    // إذا لم تكن هناك صلاحيات، إضافة صلاحيات أساسية محدودة
    const basicRules = [
      { action: 'View', subject: 'Dashboard' },
      { action: 'read', subject: 'Auth' }
    ]
    ability.update(basicRules)
    console.log('⚠️ No permissions found, using basic fallback')
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
  if (permission.startsWith('Account_')) return 'Account'
  if (permission.startsWith('Building_')) return 'Building'
  if (permission.startsWith('Mosque_')) return 'Mosque'
  if (permission.startsWith('City_')) return 'City'
  if (permission.startsWith('Region_')) return 'Region'
  if (permission.startsWith('Office_')) return 'Office'
  if (permission.startsWith('Product_')) return 'Product'
  if (permission.startsWith('Decision_')) return 'Decision'
  if (permission.startsWith('Request_')) return 'Request'
  
  // Handle fake API format (e.g., { action: 'manage', subject: 'all' })
  if (typeof permission === 'object' && permission.subject) {
    return permission.subject
  }
  
  return null // Return null for unknown permissions
}

// Export for backward compatibility
export const reloadAbilityFromLocalStorage = reloadAbilityFromCookie
