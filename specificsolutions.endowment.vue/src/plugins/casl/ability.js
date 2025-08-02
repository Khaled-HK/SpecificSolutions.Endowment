import { createMongoAbility } from '@casl/ability'
import Cookies from 'js-cookie'

export const ability = createMongoAbility()

// دالة لإعادة تحميل الصلاحيات من الكوكيز
export const reloadAbilityFromCookie = () => {
  try {
    const storedRules = Cookies.get('user-ability-rules')
    
    if (storedRules) {
      const userAbilityRules = JSON.parse(storedRules)
      
      if (userAbilityRules && Array.isArray(userAbilityRules) && userAbilityRules.length > 0) {
        ability.update(userAbilityRules)
        return true
      } else {
        return false
      }
    } else {
      // إذا لم تكن الصلاحيات محفوظة، حاول تحميلها من userData
      const userData = Cookies.get('userData')
      if (userData) {
        try {
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
            
            ability.update(rules)
            return true
          }
        } catch (error) {
          console.error('❌ Error loading from userData:', error)
        }
      }
      
      return false
    }
  } catch (error) {
    console.error('❌ Error reloading ability from cookie:', error)
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

// Keep the old function name for backward compatibility but use cookie
export const reloadAbilityFromLocalStorage = reloadAbilityFromCookie
