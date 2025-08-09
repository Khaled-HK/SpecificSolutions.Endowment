import type { Ref } from 'vue'
import { useAlert, type AlertOptions } from '@/composables/useAlert'

// App-wide alerts helper following the shared UI pattern:
// - On success: hide any local dialog alert then show a global toast with auto-timeout
// - On error/warning/info: show a global toast; local validation alerts (if any) remain visible
export const useAppAlerts = (localShowAlertRef?: Ref<boolean | undefined>) => {
  const { showSuccess, showError, showWarning, showInfo } = useAlert()

  const hideLocal = () => {
    if (localShowAlertRef) localShowAlertRef.value = false
  }

  const success = (message: string, options?: Partial<AlertOptions>) => {
    hideLocal()
    showSuccess(message, options)
  }

  const error = (message: string, options?: Partial<AlertOptions>) => {
    showError(message, options)
  }

  const warning = (message: string, options?: Partial<AlertOptions>) => {
    showWarning(message, options)
  }

  const info = (message: string, options?: Partial<AlertOptions>) => {
    showInfo(message, options)
  }

  return { success, error, warning, info }
}


