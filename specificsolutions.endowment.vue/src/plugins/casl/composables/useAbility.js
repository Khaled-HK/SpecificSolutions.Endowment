import { useAbility as useCaslAbility } from '@casl/vue'
import { reloadAbilityFromCookie } from '../ability'

export const useAbility = () => {
  const ability = useCaslAbility()
  
  // إضافة دالة إعادة تحميل الصلاحيات
  ability.reloadFromCookie = reloadAbilityFromCookie
  
  return ability
}
