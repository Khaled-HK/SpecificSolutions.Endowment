<template>
  <div class="alert-test-container pa-4">
    <VCard>
      <VCardTitle class="text-h5 mb-4">
        اختبار نظام التنبيهات
      </VCardTitle>
      
      <VCardText>
        <div class="d-flex flex-wrap gap-4">
          <!-- اختبار التنبيهات التي تختفي عند الضغط -->
          <VBtn
            color="success"
            @click="testClickableSuccess"
          >
            نجاح - يختفي عند الضغط (4 ثواني)
          </VBtn>
          
          <VBtn
            color="error"
            @click="testClickableError"
          >
            خطأ - يختفي عند الضغط (لا يختفي تلقائياً)
          </VBtn>
          
          <VBtn
            color="warning"
            @click="testClickableWarning"
          >
            تحذير - يختفي عند الضغط (5 ثواني)
          </VBtn>
          
          <VBtn
            color="info"
            @click="testClickableInfo"
          >
            معلومات - يختفي عند الضغط (3 ثواني)
          </VBtn>
        </div>
        
        <VDivider class="my-4" />
        
        <div class="d-flex flex-wrap gap-4">
          <!-- اختبار التنبيهات التي لا تختفي عند الضغط -->
          <VBtn
            color="success"
            variant="outlined"
            @click="testNonClickableSuccess"
          >
            نجاح - لا يختفي عند الضغط
          </VBtn>
          
          <VBtn
            color="error"
            variant="outlined"
            @click="testNonClickableError"
          >
            خطأ - لا يختفي عند الضغط
          </VBtn>
        </div>
        
        <VDivider class="my-4" />
        
        <div class="d-flex flex-wrap gap-4">
          <!-- اختبار التنبيهات بدون timeout -->
          <VBtn
            color="warning"
            variant="tonal"
            @click="testPermanentWarning"
          >
            تحذير دائم - يختفي عند الضغط فقط
          </VBtn>
          
          <VBtn
            color="info"
            variant="tonal"
            @click="testPermanentInfo"
          >
            معلومات دائمة - يختفي عند الضغط فقط
          </VBtn>
        </div>

        <VDivider class="my-4" />
        
        <div class="d-flex flex-wrap gap-4">
          <!-- اختبار التنبيهات مع خلفية شفافة -->
          <VBtn
            color="success"
            variant="elevated"
            @click="testBackdropSuccess"
          >
            نجاح مع خلفية شفافة
          </VBtn>
          
          <VBtn
            color="error"
            variant="elevated"
            @click="testBackdropError"
          >
            خطأ مع خلفية شفافة
          </VBtn>
          
          <VBtn
            color="warning"
            variant="elevated"
            @click="testBackdropWarning"
          >
            تحذير مع خلفية شفافة
          </VBtn>
          
          <VBtn
            color="info"
            variant="elevated"
            @click="testBackdropInfo"
          >
            معلومات مع خلفية شفافة
          </VBtn>
        </div>
      </VCardText>
    </VCard>
  </div>
</template>

<script setup lang="ts">
import { useAlert } from '@/composables/useAlert'

const { showSuccess, showError, showWarning, showInfo } = useAlert()

// اختبار التنبيهات التي تختفي عند الضغط
const testClickableSuccess = () => {
  showSuccess('✅ تم حفظ البيانات بنجاح! هذا التنبيه يختفي عند الضغط عليه أو بعد 4 ثواني', {
    timeout: 4000,
    clickToDismiss: true
  })
}

const testClickableError = () => {
  showError('❌ حدث خطأ في الاتصال بالخادم. هذا التنبيه يختفي عند الضغط عليه فقط', {
    timeout: 0,
    clickToDismiss: true
  })
}

const testClickableWarning = () => {
  showWarning('⚠️ يرجى التحقق من البيانات المدخلة. هذا التنبيه يختفي عند الضغط عليه أو بعد 5 ثواني', {
    timeout: 5000,
    clickToDismiss: true
  })
}

const testClickableInfo = () => {
  showInfo('ℹ️ تم تحديث النظام بنجاح. هذا التنبيه يختفي عند الضغط عليه أو بعد 3 ثواني', {
    timeout: 3000,
    clickToDismiss: true
  })
}

// اختبار التنبيهات التي لا تختفي عند الضغط
const testNonClickableSuccess = () => {
  showSuccess('✅ تم حفظ البيانات بنجاح! هذا التنبيه لا يختفي عند الضغط عليه', {
    timeout: 4000,
    clickToDismiss: false
  })
}

const testNonClickableError = () => {
  showError('❌ حدث خطأ في الاتصال بالخادم. هذا التنبيه لا يختفي عند الضغط عليه', {
    timeout: 0,
    clickToDismiss: false
  })
}

// اختبار التنبيهات الدائمة
const testPermanentWarning = () => {
  showWarning('⚠️ تحذير دائم - يختفي عند الضغط عليه فقط', {
    timeout: 0,
    clickToDismiss: true
  })
}

const testPermanentInfo = () => {
  showInfo('ℹ️ معلومات دائمة - يختفي عند الضغط عليه فقط', {
    timeout: 0,
    clickToDismiss: true
  })
}

// اختبار التنبيهات مع خلفية شفافة
const testBackdropSuccess = () => {
  showSuccess('✅ نجاح مع خلفية شفافة! يمكنك الضغط خارج التنبيه لإخفائه', {
    timeout: 0,
    clickToDismiss: true
  })
}

const testBackdropError = () => {
  showError('❌ خطأ مع خلفية شفافة! يمكنك الضغط خارج التنبيه لإخفائه', {
    timeout: 0,
    clickToDismiss: true
  })
}

const testBackdropWarning = () => {
  showWarning('⚠️ تحذير مع خلفية شفافة! يمكنك الضغط خارج التنبيه لإخفائه', {
    timeout: 0,
    clickToDismiss: true
  })
}

const testBackdropInfo = () => {
  showInfo('ℹ️ معلومات مع خلفية شفافة! يمكنك الضغط خارج التنبيه لإخفائه', {
    timeout: 0,
    clickToDismiss: true
  })
}
</script>

<style scoped>
.alert-test-container {
  max-width: 800px;
  margin: 0 auto;
}
</style> 