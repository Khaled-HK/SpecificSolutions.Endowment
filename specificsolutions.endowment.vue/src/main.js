import { createApp } from 'vue'
import App from '@/App.vue'
import { registerPlugins } from '@core/utils/plugins'

// Styles
import '@core/scss/template/index.scss'
import '@styles/styles.scss'

// Create vue app
const app = createApp(App)

// Global error handler
app.config.errorHandler = (err, vm, info) => {
  console.error('Vue Error:', err)
  console.error('Component:', vm)
  console.error('Info:', info)
}

// Global warn handler
app.config.warnHandler = (msg, vm, trace) => {
  // Only log in development mode
  if (import.meta.env.DEV) {
    console.warn('Vue Warning:', msg)
  }
}

// Register plugins
registerPlugins(app)

// Mount vue app
app.mount('#app')
