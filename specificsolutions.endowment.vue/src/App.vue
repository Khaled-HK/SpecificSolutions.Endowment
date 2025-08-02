image.png<script setup>
import { useTheme } from 'vuetify'
import ScrollToTop from '@core/components/ScrollToTop.vue'
import GlobalAlert from '@/components/GlobalAlert.vue'
import initCore from '@core/initCore'
import {
  initConfigStore,
  useConfigStore,
} from '@core/stores/config'
import { hexToRgb } from '@core/utils/colorConverter'
import { reloadAbilityFromCookie } from '@/plugins/casl/ability'
import Cookies from 'js-cookie'

const { global } = useTheme()

// ℹ️ Sync current theme with initial loader theme
initCore()
initConfigStore()

const configStore = useConfigStore()

  // إعادة تحميل الصلاحيات عند تحميل التطبيق
  onMounted(() => {
    // تأخير قليل للتأكد من تحميل الكوكيز
    setTimeout(() => {
      reloadAbilityFromCookie()
    }, 200)
  })

  // إعادة تحميل الصلاحيات عند تحديث الصفحة
  onBeforeMount(() => {
    if (typeof window !== 'undefined') {
      window.addEventListener('beforeunload', () => {
        // حفظ الصلاحيات قبل إغلاق الصفحة
        const userAbilityRules = Cookies.get('user-ability-rules')
        if (userAbilityRules) {
          console.log('💾 Saving ability rules before page unload')
        }
      })
    }
  })
</script>

<template>
  <VLocaleProvider :rtl="configStore.isAppRTL">
    <!-- ℹ️ This is required to set the background color of active nav link based on currently active global theme's primary -->
    <VApp :style="`--v-global-theme-primary: ${hexToRgb(global.current.value.colors.primary)}`">
      <RouterView />
      <ScrollToTop />
      <GlobalAlert />
    </VApp>
  </VLocaleProvider>
</template>
