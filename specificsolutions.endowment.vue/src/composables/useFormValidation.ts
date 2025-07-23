import { ref, reactive, computed } from 'vue'

export interface ValidationError {
  propertyName: string
  errorMessage: string
}

export interface FormValidationState {
  errors: Record<string, string[]>
  touched: Record<string, boolean>
  dirty: Record<string, boolean>
}

export function useFormValidation() {
  const validationState = reactive<FormValidationState>({
    errors: {},
    touched: {},
    dirty: {},
  })

  // إضافة خطأ لحقل معين
  const addError = (fieldName: string, errorMessage: string) => {
    if (!validationState.errors[fieldName]) {
      validationState.errors[fieldName] = []
    }
    if (!validationState.errors[fieldName].includes(errorMessage)) {
      validationState.errors[fieldName].push(errorMessage)
    }
  }

  // إزالة خطأ من حقل معين
  const removeError = (fieldName: string, errorMessage?: string) => {
    if (!validationState.errors[fieldName]) return

    if (errorMessage) {
      validationState.errors[fieldName] = validationState.errors[fieldName].filter(
        error => error !== errorMessage
      )
    } else {
      delete validationState.errors[fieldName]
    }
  }

  // تعيين أخطاء من استجابة الباك إند
  const setErrorsFromResponse = (response: any) => {
    if (response?.errors && Array.isArray(response.errors)) {
      response.errors.forEach((error: ValidationError) => {
        addError(error.propertyName, error.errorMessage)
      })
    }
  }

  // مسح جميع الأخطاء
  const clearErrors = () => {
    validationState.errors = {}
  }

  // التحقق من وجود أخطاء
  const hasErrors = computed(() => {
    return Object.keys(validationState.errors).length > 0
  })

  // التحقق من وجود خطأ في حقل معين
  const hasFieldError = (fieldName: string) => {
    return validationState.errors[fieldName] && validationState.errors[fieldName].length > 0
  }

  // الحصول على أخطاء حقل معين
  const getFieldErrors = (fieldName: string) => {
    return validationState.errors[fieldName] || []
  }

  // الحصول على أول خطأ لحقل معين
  const getFirstFieldError = (fieldName: string) => {
    const errors = getFieldErrors(fieldName)
    return errors.length > 0 ? errors[0] : ''
  }

  // تعيين حقل كملموس
  const setFieldTouched = (fieldName: string, touched = true) => {
    validationState.touched[fieldName] = touched
  }

  // تعيين حقل كمتغير
  const setFieldDirty = (fieldName: string, dirty = true) => {
    validationState.dirty[fieldName] = dirty
  }

  // التحقق من إمكانية عرض خطأ لحقل معين
  const shouldShowFieldError = (fieldName: string) => {
    return hasFieldError(fieldName) && validationState.touched[fieldName]
  }

  // دالة مساعدة للتحقق من الحقول المطلوبة
  const validateRequired = (value: any, fieldName: string, message = 'هذا الحقل مطلوب') => {
    if (!value || (typeof value === 'string' && value.trim() === '')) {
      addError(fieldName, message)
      return false
    }
    removeError(fieldName)
    return true
  }

  // دالة مساعدة للتحقق من البريد الإلكتروني
  const validateEmail = (email: string, fieldName: string, message = 'البريد الإلكتروني غير صحيح') => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (email && !emailRegex.test(email)) {
      addError(fieldName, message)
      return false
    }
    removeError(fieldName)
    return true
  }

  // دالة مساعدة للتحقق من الطول
  const validateLength = (value: string, fieldName: string, min: number, max: number, message?: string) => {
    if (!value) return true
    
    if (value.length < min || value.length > max) {
      const defaultMessage = `يجب أن يكون الطول بين ${min} و ${max} حرف`
      addError(fieldName, message || defaultMessage)
      return false
    }
    removeError(fieldName)
    return true
  }

  return {
    validationState,
    addError,
    removeError,
    setErrorsFromResponse,
    clearErrors,
    hasErrors,
    hasFieldError,
    getFieldErrors,
    getFirstFieldError,
    setFieldTouched,
    setFieldDirty,
    shouldShowFieldError,
    validateRequired,
    validateEmail,
    validateLength,
  }
} 