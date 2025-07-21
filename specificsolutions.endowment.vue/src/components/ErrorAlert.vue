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
})

const emit = defineEmits(['update:modelValue'])

const closeAlert = () => {
  emit('update:modelValue', false)
}

// Auto-hide functionality
if (props.timeout > 0) {
  setTimeout(() => {
    closeAlert()
  }, props.timeout)
}
</script>

<template>
  <VAlert
    :model-value="modelValue"
    :type="type"
    variant="tonal"
    :closable="closable"
    class="mb-4"
    @update:model-value="closeAlert"
  >
    {{ message }}
  </VAlert>
</template> 