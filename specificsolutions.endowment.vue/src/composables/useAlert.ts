import { reactive } from 'vue'

export interface AlertOptions {
  message: string
  type?: 'success' | 'error' | 'warning' | 'info'
  timeout?: number
  closable?: boolean
  clickToDismiss?: boolean
}

export interface AlertState {
  show: boolean
  message: string
  type: 'success' | 'error' | 'warning' | 'info'
  timeout: number
  closable: boolean
  clickToDismiss: boolean
}

// Singleton state (shared across the whole app)
const alertState = reactive<AlertState>({
  show: false,
  message: '',
  type: 'info',
  timeout: 0,
  closable: true,
  clickToDismiss: true,
})

let timeoutId: NodeJS.Timeout | null = null

export const useAlert = () => {

  const showAlert = (options: AlertOptions) => {
    // إلغاء التنبيه السابق إذا كان موجوداً
    if (timeoutId) {
      clearTimeout(timeoutId)
      timeoutId = null
    }

    // تحديث حالة التنبيه
    alertState.show = true
    alertState.message = options.message
    alertState.type = options.type || 'info'
    alertState.timeout = options.timeout || 0
    alertState.closable = options.closable !== false
    alertState.clickToDismiss = options.clickToDismiss !== false

    // إعداد الإخفاء التلقائي إذا كان محدداً
    if (alertState.timeout > 0) {
      timeoutId = setTimeout(() => {
        hideAlert()
      }, alertState.timeout)
    }
  }

  const hideAlert = () => {
    alertState.show = false
    if (timeoutId) {
      clearTimeout(timeoutId)
      timeoutId = null
    }
  }

  const dismissAlert = () => {
    if (alertState.clickToDismiss) {
      hideAlert()
    }
  }

  // دوال مساعدة للأنواع المختلفة
  const showSuccess = (message: string, options?: Partial<AlertOptions>) => {
    // مدة افتراضية 4000ms إن لم تُحدَّد
    showAlert({ message, type: 'success', timeout: 4000, ...options })
  }

  const showError = (message: string, options?: Partial<AlertOptions>) => {
    showAlert({ message, type: 'error', timeout: 5000, ...options })
  }

  const showWarning = (message: string, options?: Partial<AlertOptions>) => {
    showAlert({ message, type: 'warning', timeout: 5000, ...options })
  }

  const showInfo = (message: string, options?: Partial<AlertOptions>) => {
    showAlert({ message, type: 'info', timeout: 4000, ...options })
  }

  return {
    alertState,
    showAlert,
    hideAlert,
    dismissAlert,
    showSuccess,
    showError,
    showWarning,
    showInfo,
  }
} 