import { ref, reactive, computed } from 'vue'
import { propertyNameMapping } from './fieldMapping'

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
  const setErrorsFromResponse = (response: any, context: 'add' | 'edit' = 'add') => {
    if (response?.errors && Array.isArray(response.errors)) {
      response.errors.forEach((error: ValidationError) => {
        // إذا كان propertyName فارغاً، فهذا خطأ عام
        if (!error.propertyName || error.propertyName.trim() === '') {
          addError('general', error.errorMessage)
          return
        }
        
        // تحويل اسم الحقل من الباك اند إلى الفرونت إند
        const baseFrontendFieldName = propertyNameMapping[error.propertyName] || error.propertyName.toLowerCase()
        
        // إضافة prefix للحقول حسب السياق (add/edit)
        const frontendFieldName = context === 'edit' ? `edit${baseFrontendFieldName.charAt(0).toUpperCase()}${baseFrontendFieldName.slice(1)}` : baseFrontendFieldName
        
        // إضافة الخطأ
        addError(frontendFieldName, error.errorMessage)
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
    const hasError = hasFieldError(fieldName)
    const isTouched = validationState.touched[fieldName]
    return hasError && isTouched
  }

  // دوال التحقق المساعدة
  const validateRequired = (value: any, fieldName: string, message = 'هذا الحقل مطلوب') => {
    if (!value || (typeof value === 'string' && value.trim() === '')) {
      addError(fieldName, message)
      return false
    }
    removeError(fieldName)
    return true
  }

  const validateEmail = (email: string, fieldName: string, message = 'البريد الإلكتروني غير صحيح') => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (email && !emailRegex.test(email)) {
      addError(fieldName, message)
      return false
    }
    removeError(fieldName)
    return true
  }

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