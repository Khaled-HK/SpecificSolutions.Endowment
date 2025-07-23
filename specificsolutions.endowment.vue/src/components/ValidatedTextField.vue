<template>
  <div>
    <VTextField
      v-model="modelValue"
      v-bind="$attrs"
      :error="hasError"
      :error-messages="errorMessages"
      :color="hasError ? 'error' : undefined"
      @blur="handleBlur"
      @input="handleInput"
      @update:model-value="handleModelValueUpdate"
    />
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue'

interface Props {
  modelValue?: any
  fieldName: string
  validationState?: {
    errors: Record<string, string[]>
    touched: Record<string, boolean>
  }
  validateOnBlur?: boolean
  validateOnInput?: boolean
  customValidation?: (value: any) => string | null
}

const props = withDefaults(defineProps<Props>(), {
  validateOnBlur: true,
  validateOnInput: false,
})

const emit = defineEmits<{
  'update:modelValue': [value: any]
  'validation-error': [fieldName: string, error: string]
  'validation-success': [fieldName: string]
}>()

// التحقق من وجود خطأ
const hasError = computed(() => {
  if (!props.validationState) return false
  return props.validationState.errors[props.fieldName] && 
         props.validationState.errors[props.fieldName].length > 0 &&
         props.validationState.touched[props.fieldName]
})

// الحصول على رسائل الخطأ
const errorMessages = computed(() => {
  if (!props.validationState || !hasError.value) return []
  return props.validationState.errors[props.fieldName] || []
})

// معالجة حدث blur
const handleBlur = () => {
  if (props.validateOnBlur && props.customValidation) {
    const error = props.customValidation(props.modelValue)
    if (error) {
      emit('validation-error', props.fieldName, error)
    } else {
      emit('validation-success', props.fieldName)
    }
  }
}

// معالجة حدث input
const handleInput = () => {
  if (props.validateOnInput && props.customValidation) {
    const error = props.customValidation(props.modelValue)
    if (error) {
      emit('validation-error', props.fieldName, error)
    } else {
      emit('validation-success', props.fieldName)
    }
  }
}

// معالجة تحديث القيمة
const handleModelValueUpdate = (value: any) => {
  emit('update:modelValue', value)
}
</script>

<style scoped>
/* تخصيص مظهر الحقل عند وجود خطأ */
:deep(.v-field--error) {
  border-color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-field__outline) {
  color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-field--error .v-label) {
  color: rgb(var(--v-theme-error)) !important;
}

/* تخصيص مظهر رسائل الخطأ */
:deep(.v-messages__message) {
  color: rgb(var(--v-theme-error)) !important;
  font-size: 0.75rem;
  margin-top: 4px;
}
</style> 