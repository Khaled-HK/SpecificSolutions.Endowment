<script setup>
import Footer from '@/views/front-pages/front-page-footer.vue'
import Navbar from '@/views/front-pages/front-page-navbar.vue'
import PricingPlanDialog from '@/components/dialogs/PricingPlanDialog.vue'
import paypalDark from '@images/icons/payments/img/paypal-dark.png'
import paypalLight from '@images/icons/payments/img/paypal-light.png'
import visaDark from '@images/icons/payments/img/visa-dark.png'
import visaLight from '@images/icons/payments/img/visa-light.png'
import { useConfigStore } from '@core/stores/config'

const visa = useGenerateImageVariant(visaLight, visaDark)
const paypal = useGenerateImageVariant(paypalLight, paypalDark)
const store = useConfigStore()

store.skin = 'default'
definePage({
  meta: {
    layout: 'blank',
    public: true,
  },
})

const radioContent = [
  {
    title: 'Credit Card',
    value: 'credit card',
    images: visa.value,
  },
  {
    title: 'PayPal',
    value: 'paypal',
    images: paypal.value,
  },
]

const selectedRadio = ref('credit card')
const selectedCountry = ref('USA')
const isPricingPlanDialogVisible = ref(false)
</script>

<template>
  <!-- eslint-disable vue/attribute-hyphenation -->

  <div class="payment-page">
    <!-- 👉 Navbar -->
    <Navbar />

    <!-- 👉 Payment card  -->
    <VContainer>
      <div class="d-flex justify-center align-center payment-card">
        <VCard width="100%">
          <VRow>
            <VCol
              cols="12"
              md="7"
              :class="$vuetify.display.mdAndUp ? 'border-e' : 'border-b'"
            >
              <VCardText class="pa-8 pe-5">
                <!-- Checkout header -->
                <div>
                  <h4 class="text-h4 mb-2">
                    Checkout
                  </h4>
                  <div class="text-body-1">
                    All plans include 40+ advanced tools and features to boost your product. Choose the best plan to fit your needs.
                  </div>
                </div>

                <CustomRadios
                  v-model:selected-radio="selectedRadio"
                  :radio-content="radioContent"
                  :grid-column="{ cols: '12', sm: '6' }"
                  class="my-8"
                >
                  <template #default="{ item }">
                    <div class="d-flex align-center gap-x-4 ms-3">
                      <img
                        :src="item.images"
                        height="34"
                      >
                      <h6 class="text-h6">
                        {{ item.title }}
                      </h6>
                    </div>
                  </template>
                </CustomRadios>

                <!-- billing Details -->
                <div class="mb-8">
                  <h4 class="text-h4 mb-6">
                    Billing Details
                  </h4>
                  <VForm>
                    <VRow>
                      <VCol
                        cols="12"
                        md="6"
                      >
                        <AppTextField
                          label="Email Address"
                          type="email"
                          placeholder="johndoe@email.com"
                        />
                      </VCol>
                      <VCol
                        cols="12"
                        md="6"
                      >
                        <AppTextField
                          label="Password"
                          type="password"
                          placeholder="············"
                          autocomplete="on"
                        />
                      </VCol>
                      <VCol
                        cols="12"
                        md="6"
                      >
                        <AppSelect
                          v-model="selectedCountry"
                          label="Billing Country"
                          :items="['USA', 'Canada', 'UK', 'AUS']"
                        />
                      </VCol>
                      <VCol
                        cols="12"
                        md="6"
                      >
                        <AppTextField
                          label="Billing Zip/Postal Code"
                          type="number"
                          placeholder="129211"
                        />
                      </VCol>
                    </VRow>
                  </VForm>
                </div>

                <!-- Credit card info -->
                <div
                  class="mb-8"
                  :class="selectedRadio === 'paypal' ? 'd-none' : 'd-block'"
                >
                  <h4 class="text-h4 mb-6">
                    Credit Card Info
                  </h4>
                  <VRow>
                    <VCol cols="12">
                      <AppTextField
                        label="Card Number"
                        placeholder="8787 2345 3458"
                        type="number"
                      />
                    </VCol>

                    <VCol
                      cols="12"
                      md="4"
                    >
                      <AppTextField
                        label="Card Holder"
                        placeholder="John Doe"
                      />
                    </VCol>

                    <VCol
                      cols="12"
                      md="4"
                    >
                      <AppTextField
                        label="Exp. date"
                        placeholder="05/2020"
                      />
                    </VCol>

                    <VCol
                      cols="12"
                      md="4"
                    >
                      <AppTextField
                        label="CVV"
                        type="number"
                        placeholder="784"
                      />
                    </VCol>
                  </VRow>
                </div>
              </VCardText>
            </VCol>

            <VCol
              cols="12"
              md="5"
            >
              <VCardText class="pa-8 ps-5">
                <!-- order summary -->
                <div class="mb-8">
                  <h4 class="text-h4 mb-2">
                    Order Summary
                  </h4>
                  <div class="text-body-1">
                    It can help you manage and service orders before, during, and after fulfillment.
                  </div>
                </div>

                <VCard
                  flat
                  color="rgba(var(--v-theme-on-surface), var(--v-hover-opacity))"
                >
                  <VCardText>
                    <div class="text-body-1">
                      A simple start for everyone
                    </div>
                    <h1 class="text-h1 my-4">
                      $59.99<span class="text-body-1 font-weight-medium">/month</span>
                    </h1>
                    <VBtn
                      variant="tonal"
                      block
                      @click="isPricingPlanDialogVisible = !isPricingPlanDialogVisible"
                    >
                      Change Plan
                    </VBtn>
                  </VCardText>
                </VCard>

                <div class="my-5">
                  <div class="d-flex justify-space-between mb-2">
                    <span>Subscription</span>
                    <h6 class="text-h6">
                      $85.99
                    </h6>
                  </div>
                  <div class="d-flex justify-space-between">
                    <span>Tax</span>
                    <h6 class="text-h6">
                      $4.99
                    </h6>
                  </div>
                  <VDivider class="my-4" />
                  <div class="d-flex justify-space-between">
                    <span>Total</span>
                    <h6 class="text-h6">
                      $90.98
                    </h6>
                  </div>
                </div>

                <VBtn
                  block
                  color="success"
                  class="mb-8"
                >
                  <template #append>
                    <VIcon
                      icon="tabler-arrow-right"
                      class="flip-in-rtl"
                    />
                  </template>
                  Proceed With Payment
                </VBtn>

                <div class="text-body-1">
                  By continuing, you accept to our Terms of Services and Privacy Policy. Please note that payments are non-refundable.
                </div>
              </VCardText>
            </VCol>
          </VRow>
        </VCard>
      </div>
    </VContainer>

    <!-- 👉 Footer -->
    <Footer />

    <PricingPlanDialog v-model:is-dialog-visible="isPricingPlanDialogVisible" />
  </div>
</template>

<style lang="scss" scoped>
.footer {
  position: static !important;
  inline-size: 100%;
  inset-block-end: 0;
}

.payment-card {
  margin-block: 10.5rem 5.25rem;
}

.payment-page {
  @media (min-width: 600px) and (max-width: 960px) {
    .v-container {
      padding-inline: 2rem !important;
    }
  }
}
</style>

<style lang="scss">
.payment-card {
  .custom-radio {
    .v-radio {
      margin-block-start: 0 !important;
    }
  }
}
</style>
