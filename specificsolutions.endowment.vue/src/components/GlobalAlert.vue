<script setup lang="ts">
import { useAlert } from '@/composables/useAlert'

const { alertState, dismissAlert } = useAlert()

// الحصول على الأيقونة المناسبة حسب نوع التنبيه
const getIcon = (type: string) => {
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
const getAlertStyles = (type: string) => {
  const baseStyles = {
    position: 'relative',
    zIndex: 9999,
    maxWidth: '600px',
    width: '100%',
    cursor: 'pointer',
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
const getTextColor = (type: string) => {
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

// معالجة الضغط على الخلفية
const handleBackdropClick = (event: Event) => {
  // التأكد من أن الضغط كان على الخلفية وليس على التنبيه نفسه
  if (event.target === event.currentTarget) {
    dismissAlert()
  }
}

// معالجة الضغط على التنبيه نفسه
const handleAlertClick = (event: Event) => {
  // منع انتشار الحدث إلى الخلفية
  event.stopPropagation()
  dismissAlert()
}
</script>

<template>
  <Teleport to="body">
    <Transition name="alert-fade">
      <div
        v-if="alertState.show"
        class="global-alert-backdrop"
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
          class="global-alert-container"
          :style="{
            maxWidth: '90vw',
            width: '600px',
            zIndex: 10000,
          }"
        >
          <VAlert
            :model-value="alertState.show"
            :type="alertState.type"
            variant="tonal"
            :closable="alertState.closable"
            @click="handleAlertClick"
            @update:model-value="dismissAlert"
            :style="getAlertStyles(alertState.type)"
          >
            <div class="d-flex align-center">
              <VIcon
                :icon="getIcon(alertState.type)"
                :color="alertState.type"
                class="me-2"
                size="20"
              />
              <span 
                class="font-weight-medium" 
                :style="{ color: getTextColor(alertState.type) }"
              >
                {{ alertState.message }}
              </span>
            </div>
          </VAlert>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.global-alert-backdrop {
  pointer-events: auto;
  backdrop-filter: blur(2px);
}

.global-alert-container {
  pointer-events: auto;
}

.alert-fade-enter-active,
.alert-fade-leave-active {
  transition: all 0.3s ease;
}

.alert-fade-enter-from {
  opacity: 0;
}

.alert-fade-leave-to {
  opacity: 0;
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