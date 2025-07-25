<script setup>
// Define page metadata for permissions
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})

import PersonalDetails from '@/views/wizard-examples/property-listing/PersonalDetails.vue'
import PriceDetails from '@/views/wizard-examples/property-listing/PriceDetails.vue'
import PropertyArea from '@/views/wizard-examples/property-listing/PropertyArea.vue'
import PropertyDetails from '@/views/wizard-examples/property-listing/PropertyDetails.vue'
import PropertyFeatures from '@/views/wizard-examples/property-listing/PropertyFeatures.vue'

const propertyListingSteps = [
  {
    title: 'Personal Details',
    subtitle: 'Your Name/Email',
    icon: 'tabler-users',
  },
  {
    title: 'Property Details',
    subtitle: 'Property Type',
    icon: 'tabler-home',
  },
  {
    title: 'Property Features',
    subtitle: 'Bedrooms/Floor No',
    icon: 'tabler-bookmarks',
  },
  {
    title: 'Property Area',
    subtitle: 'Covered Area',
    icon: 'tabler-map-pin',
  },
  {
    title: 'Price Details',
    subtitle: 'Expected Price',
    icon: 'tabler-currency-dollar',
  },
]

const propertyListingData = ref({
  personalDetails: {
    userType: 'builder',
    firstName: '',
    lastName: '',
    username: '',
    password: '',
    email: '',
    contact: null,
  },
  priceDetails: {
    expectedPrice: null,
    pricePerSqft: null,
    maintenanceCharge: null,
    maintenancePeriod: null,
    bookingAmount: null,
    otherAmount: null,
    priceDisplayType: 'Negotiable',
    priceIncludes: ['Car Parking'],
  },
  propertyFeatures: {
    bedroomCount: '',
    floorNo: '',
    bathroomCount: '',
    isCommonArea: true,
    furnishedStatus: null,
    furnishingDetails: [
      'AC',
      'TV',
      'Fridge',
    ],
    isCommonArea1: 'true',
    isCommonArea2: 'false',
  },
  propertyArea: {
    totalArea: null,
    carpetArea: null,
    plotArea: null,
    availableFrom: null,
    possessionStatus: 'Under Construciton',
    transactionType: 'New Property',
    isOnMainRoad: 'No',
    isGatedColony: 'No',
  },
  propertyDetails: {
    propertyDealType: 'sell',
    propertyType: null,
    zipCode: null,
    country: null,
    state: '',
    city: '',
    landmark: '',
    address: '',
  },
})

const currentStep = ref(0)

const onSubmit = () => {
  console.log('propertyListingData :>> ', propertyListingData.value)
}
</script>

<template>
  <VCard>
    <VRow no-gutters>
      <VCol
        cols="12"
        md="3"
        :class="$vuetify.display.smAndDown ? 'border-b' : 'border-e'"
      >
        <VCardText>
          <AppStepper
            v-model:current-step="currentStep"
            :items="propertyListingSteps"
            direction="vertical"
            icon-size="22"
            class="stepper-icon-step-bg"
          />
        </VCardText>
      </VCol>

      <VCol
        cols="12"
        md="9"
      >
        <VCardText>
          <VWindow
            v-model="currentStep"
            class="disable-tab-transition"
          >
            <VWindowItem>
              <PersonalDetails v-model:form-data="propertyListingData.personalDetails" />
            </VWindowItem>

            <VWindowItem>
              <PropertyDetails v-model:form-data="propertyListingData.propertyDetails" />
            </VWindowItem>

            <VWindowItem>
              <PropertyFeatures v-model:form-data="propertyListingData.propertyFeatures" />
            </VWindowItem>

            <VWindowItem>
              <PropertyArea v-model:form-data="propertyListingData.propertyArea" />
            </VWindowItem>

            <VWindowItem>
              <PriceDetails v-model:form-data="propertyListingData.priceDetails" />
            </VWindowItem>
          </VWindow>

          <div class="d-flex flex-wrap gap-4 justify-space-between mt-6">
            <VBtn
              color="secondary"
              variant="tonal"
              :disabled="currentStep === 0"
              @click="currentStep--"
            >
              <VIcon
                icon="tabler-arrow-left"
                start
                class="flip-in-rtl"
              />
              Previous
            </VBtn>

            <VBtn
              v-if="propertyListingSteps.length - 1 === currentStep"
              color="success"
              @click="onSubmit"
            >
              submit
            </VBtn>

            <VBtn
              v-else
              @click="currentStep++"
            >
              Next

              <VIcon
                icon="tabler-arrow-right"
                end
                class="flip-in-rtl"
              />
            </VBtn>
          </div>
        </VCardText>
      </VCol>
    </VRow>
  </VCard>
</template>
