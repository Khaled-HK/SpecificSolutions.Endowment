import { createMongoAbility } from '@casl/ability'

export const ability = createMongoAbility()

// دالة لإعادة تحميل الصلاحيات من الكوكيز
export const reloadAbilityFromCookie = () => {
  try {
    const userAbilityRules = useCookie('user-ability-rules', {
      default: () => [],
      maxAge: 60 * 60 * 24 * 7, // 7 days
      path: '/',
      secure: true,
      sameSite: 'strict'
    }).value

    if (userAbilityRules && Array.isArray(userAbilityRules)) {
      ability.update(userAbilityRules)
      console.log('✅ Reloaded ability from cookie:', userAbilityRules)
      return true
    } else {
      console.warn('❌ No valid ability rules found in cookie')
      return false
    }
  } catch (error) {
    console.error('❌ Error reloading ability from cookie:', error)
    return false
  }
}
