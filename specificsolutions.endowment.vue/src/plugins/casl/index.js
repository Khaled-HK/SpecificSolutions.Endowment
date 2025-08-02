import { createMongoAbility } from '@casl/ability'
import { abilitiesPlugin } from '@casl/vue'
import { reloadAbilityFromCookie } from './ability'
import Cookies from 'js-cookie'

export default function (app) {
  // قراءة الصلاحيات من الكوكيز
  let userAbilityRules = []
  try {
    const storedRules = Cookies.get('user-ability-rules')
    if (storedRules) {
      userAbilityRules = JSON.parse(storedRules)
    }
  } catch (error) {
    console.error('❌ Error reading from cookie in index.js:', error)
    userAbilityRules = []
  }
  
  const initialAbility = createMongoAbility(userAbilityRules ?? [])

  app.use(abilitiesPlugin, initialAbility, {
    useGlobalProperties: true,
  })

  // إعادة تحميل الصلاحيات عند تحميل التطبيق
  if (typeof window !== 'undefined') {
    // تأخير قليل للتأكد من تحميل الكوكيز
    setTimeout(() => {
      reloadAbilityFromCookie()
    }, 100)
  }
}
