import { createMongoAbility } from '@casl/ability'
import { abilitiesPlugin } from '@casl/vue'
import { reloadAbilityFromLocalStorage } from './ability'

export default function (app) {
  // قراءة الصلاحيات من localStorage
  let userAbilityRules = []
  try {
    const storedRules = localStorage.getItem('user-ability-rules')
    if (storedRules) {
      userAbilityRules = JSON.parse(storedRules)
    }
  } catch (error) {
    console.error('❌ Error reading from localStorage in index.js:', error)
    userAbilityRules = []
  }
  
  const initialAbility = createMongoAbility(userAbilityRules ?? [])

  app.use(abilitiesPlugin, initialAbility, {
    useGlobalProperties: true,
  })

  // إعادة تحميل الصلاحيات عند تحميل التطبيق
  if (typeof window !== 'undefined') {
    // تأخير قليل للتأكد من تحميل localStorage
    setTimeout(() => {
      reloadAbilityFromLocalStorage()
    }, 100)
  }
}
