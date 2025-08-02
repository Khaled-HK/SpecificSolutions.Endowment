import { createMongoAbility } from '@casl/ability'

export const ability = createMongoAbility()

// دالة لإعادة تحميل الصلاحيات من localStorage
export const reloadAbilityFromLocalStorage = () => {
  try {
    const storedRules = localStorage.getItem('user-ability-rules')
    
    if (storedRules) {
      const userAbilityRules = JSON.parse(storedRules)
      
      if (userAbilityRules && Array.isArray(userAbilityRules)) {
        ability.update(userAbilityRules)
        console.log('✅ Reloaded ability from localStorage:', userAbilityRules)
        return true
      } else {
        console.warn('❌ Invalid ability rules format in localStorage')
        return false
      }
    } else {
      console.warn('❌ No ability rules found in localStorage')
      return false
    }
  } catch (error) {
    console.error('❌ Error reloading ability from localStorage:', error)
    return false
  }
}

// Keep the old function name for backward compatibility but use localStorage
export const reloadAbilityFromCookie = reloadAbilityFromLocalStorage
