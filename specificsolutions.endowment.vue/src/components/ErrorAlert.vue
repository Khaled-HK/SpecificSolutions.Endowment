<script setup>
const props = defineProps({
  modelValue: {
    type: Boolean,
    required: true,
  },
  message: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    default: 'error',
    validator: (value) => ['error', 'warning', 'info', 'success'].includes(value),
  },
  closable: {
    type: Boolean,
    default: true,
  },
  timeout: {
    type: Number,
    default: 0, // 0 means no auto-hide
  },
  clickToDismiss: {
    type: Boolean,
    default: true, // إضافة خاصية الاختفاء عند الضغط
  },
  backdrop: {
    type: Boolean,
    default: false, // إضافة خيار الخلفية الشفافة
  },
})

const emit = defineEmits(['update:modelValue'])

const closeAlert = () => {
  emit('update:modelValue', false)
}

const dismissAlert = () => {
  if (props.clickToDismiss) {
    closeAlert()
  }
}

// معالجة الضغط على الخلفية
const handleBackdropClick = (event) => {
  // التأكد من أن الضغط كان على الخلفية وليس على التنبيه نفسه
  if (event.target === event.currentTarget) {
    dismissAlert()
  }
}

// معالجة الضغط على التنبيه نفسه
const handleAlertClick = (event) => {
  // منع انتشار الحدث إلى الخلفية
  event.stopPropagation()
  dismissAlert()
}

// Auto-hide functionality
if (props.timeout > 0) {
  setTimeout(() => {
    closeAlert()
  }, props.timeout)
}

// الحصول على الأيقونة المناسبة حسب نوع التنبيه
const getIcon = (type) => {
  switch (type) {
    case 'success':
      return 'tabler-check-circle'
    case 'error':
      return 'tabler-alert-circle'
    case 'warning':
      return 'tabler-alert-triangle'
    case 'info':
      return 'tabler-info-circle'
    default:
      return 'tabler-info-circle'
  }
}

// الحصول على الألوان المناسبة حسب نوع التنبيه
const getAlertStyles = (type) => {
  const baseStyles = {
    position: 'relative',
    zIndex: 9999,
    maxWidth: '600px',
    width: '100%',
    cursor: props.clickToDismiss ? 'pointer' : 'default',
    transition: 'all 0.3s ease',
  }

  switch (type) {
    case 'success':
      return {
        ...baseStyles,
        borderRadius: '16px',
        boxShadow: '0 2px 4px rgba(76, 175, 80, 0.2)',
        border: '1px solid #4caf50',
        backgroundColor: '#e8f5e8',
        padding: '12px 16px',
      }
    case 'error':
      return {
        ...baseStyles,
        borderRadius: '8px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.15)',
        border: '1px solid #f44336',
        backgroundColor: '#ffebee',
        padding: '16px',
      }
    case 'warning':
      return {
        ...baseStyles,
        borderRadius: '8px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.15)',
        border: '1px solid #ff9800',
        backgroundColor: '#fff8e1',
        padding: '16px',
      }
    case 'info':
      return {
        ...baseStyles,
        borderRadius: '8px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.15)',
        border: '1px solid #2196f3',
        backgroundColor: '#e3f2fd',
        padding: '16px',
      }
    default:
      return baseStyles
  }
}

// الحصول على لون النص حسب نوع التنبيه
const getTextColor = (type) => {
  switch (type) {
    case 'success':
      return '#2e7d32'
    case 'error':
      return '#c62828'
    case 'warning':
      return '#ef6c00'
    case 'info':
      return '#1565c0'
    default:
      return 'inherit'
  }
}
</script>

<template>
  <!-- إذا كان backdrop مفعل -->
  <div
    v-if="backdrop && modelValue"
    class="error-alert-backdrop"
    @click="handleBackdropClick"
    :style="{
      position: 'fixed',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      backgroundColor: 'rgba(0, 0, 0, 0.3)',
      zIndex: 9999,
      display: 'flex',
      alignItems: 'flex-start',
      justifyContent: 'center',
      paddingTop: '20px',
    }"
  >
    <div
      class="error-alert-container"
      :style="{
        maxWidth: '90vw',
        width: '600px',
        zIndex: 10000,
      }"
    >
      <VAlert
        :model-value="modelValue"
        :type="type"
        variant="tonal"
        :closable="closable"
        @click="handleAlertClick"
        @update:model-value="closeAlert"
        :style="getAlertStyles(type)"
      >
        <div class="d-flex align-center">
          <VIcon
            :icon="getIcon(type)"
            :color="type"
            class="me-2"
            size="20"
          />
          <span 
            class="font-weight-medium" 
            :style="{ color: getTextColor(type) }"
          >
            {{ message }}
          </span>
        </div>
      </VAlert>
    </div>
  </div>

  <!-- إذا لم يكن backdrop مفعل (السلوك العادي) -->
  <VAlert
    v-else
    :model-value="modelValue"
    :type="type"
    variant="tonal"
    :closable="closable"
    class="mb-4"
    @update:model-value="closeAlert"
    @click="dismissAlert"
    :style="getAlertStyles(type)"
  >
    <div class="d-flex align-center">
      <VIcon
        :icon="getIcon(type)"
        :color="type"
        class="me-2"
        size="20"
      />
      <span 
        class="font-weight-medium" 
        :style="{ color: getTextColor(type) }"
      >
        {{ message }}
      </span>
    </div>
  </VAlert>
</template>

<style scoped>
.error-alert-backdrop {
  pointer-events: auto;
  backdrop-filter: blur(2px);
}

.error-alert-container {
  pointer-events: auto;
}

/* تحسين مظهر التنبيه عند الضغط */
.v-alert {
  user-select: none;
}

.v-alert:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
}

.v-alert:active {
  transform: translateY(0);
  transition: transform 0.1s ease;
}
</style> 