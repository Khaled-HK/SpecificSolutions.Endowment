import { createMongoAbility } from '@casl/ability'
import { abilitiesPlugin } from '@casl/vue'
import { reloadAbilityFromCookie } from './ability'

export default function (app) {
  const userAbilityRules = useCookie('user-ability-rules', {
    default: () => [],
    maxAge: 60 * 60 * 24 * 7, // 7 days
    path: '/',
    secure: true,
    sameSite: 'strict'
  })
  const initialAbility = createMongoAbility(userAbilityRules.value ?? [])

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
