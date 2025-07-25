<script setup>
definePage({
  meta: {
    action: 'View',
    subject: 'Dashboard',
  },
})

import product21 from '@images/ecommerce-images/product-21.png'
import product22 from '@images/ecommerce-images/product-22.png'
import product23 from '@images/ecommerce-images/product-23.png'
import product24 from '@images/ecommerce-images/product-24.png'

const orderData = ref()
const route = useRoute('apps-ecommerce-order-details-id')
const { data } = await useApi(`/apps/ecommerce/orders/${ route.params.id }`)
if (data.value)
  orderData.value = data.value
const isConfirmDialogVisible = ref(false)
const isUserInfoEditDialogVisible = ref(false)
const isEditAddressDialogVisible = ref(false)

const headers = [
  {
    title: 'Product',
    key: 'productName',
  },
  {
    title: 'Price',
    key: 'price',
  },
  {
    title: 'Quantity',
    key: 'quantity',
  },
  {
    title: 'Total',
    key: 'total',
  },
]

const resolvePaymentStatus = payment => {
  if (payment === 1)
    return {
      text: 'Paid',
      color: 'success',
    }
  if (payment === 2)
    return {
      text: 'Pending',
      color: 'warning',
    }
  if (payment === 3)
    return {
      text: 'Cancelled',
      color: 'secondary',
    }
  if (payment === 4)
    return {
      text: 'Failed',
      color: 'error',
    }
}

const resolveStatus = status => {
  if (status === 'Delivered')
    return {
      text: 'Delivered',
      color: 'success',
    }
  if (status === 'Out for Delivery')
    return {
      text: 'Out for Delivery',
      color: 'primary',
    }
  if (status === 'Ready to Pickup')
    return {
      text: 'Ready to Pickup',
      color: 'info',
    }
  if (status === 'Dispatched')
    return {
      text: 'Dispatched',
      color: 'warning',
    }
}

const userData = {
  id: null,
  fullName: orderData.value?.customer,
  company: 'Pixinvent',
  role: 'Web developer',
  username: 'T1940',
  country: 'United States',
  contact: '+1 (609) 972-22-22',
  email: orderData.value?.email,
  status: 'Active',
  taxId: 'Tax-8894',
  language: 'English',
  currentPlan: '',
  avatar: '',
  taskDone: null,
  projectDone: null,
}

const currentBillingAddress = {
  fullName: orderData.value?.customer,
  firstName: orderData.value?.customer.split(' ')[0],
  lastName: orderData.value?.customer.split(' ')[1],
  selectedCountry: 'USA',
  addressLine1: '45 Rocker Terrace',
  addressLine2: 'Latheronwheel',
  landmark: 'KW5 8NW, London',
  contact: '+1 (609) 972-22-22',
  country: 'USA',
  city: 'London',
  state: 'London',
  zipCode: 110001,
}

const orderDetail = [
  {
    productName: 'OnePlus 7 Pro',
    productImage: product21,
    subtitle: 'Storage: 128gb',
    price: 799,
    quantity: 1,
    total: 799,
  },
  {
    productName: 'Face Cream',
    productImage: product22,
    subtitle: 'Gender: Women',
    price: 89,
    quantity: 1,
    total: 89,
  },
  {
    productName: 'Wooden Chair',
    productImage: product23,
    subtitle: 'Material: Woodem',
    price: 289,
    quantity: 2,
    total: 578,
  },
  {
    productName: 'Nike Jorden',
    productImage: product24,
    subtitle: 'Size: 8UK',
    price: 299,
    quantity: 2,
    total: 598,
  },
]
</script>

<template>
  <div>
    <div class="d-flex justify-space-between align-center flex-wrap gap-y-4 mb-6">
      <div>
        <div class="d-flex gap-2 align-center mb-2 flex-wrap">
          <h5 class="text-h5">
            Order #{{ route.params.id }}
          </h5>
          <div class="d-flex gap-x-2">
            <VChip
              v-if="orderData?.payment"
              variant="tonal"
              :color="resolvePaymentStatus(orderData.payment)?.color"
              label
              size="small"
            >
              {{ resolvePaymentStatus(orderData.payment)?.text }}
            </VChip>
            <VChip
              v-if="orderData?.status"
              v-bind="resolveStatus(orderData?.status)"
              label
              size="small"
            />
          </div>
        </div>
        <div class="text-body-1">
          Aug 17, 2020, 5:48 (ET)
        </div>
      </div>

      <VBtn
        variant="tonal"
        color="error"
        @click="isConfirmDialogVisible = !isConfirmDialogVisible"
      >
        Delete Order
      </VBtn>
    </div>

    <VRow>
      <VCol
        cols="12"
        md="8"
      >
        <!-- 👉 Order Details -->
        <VCard class="mb-6">
          <VCardItem>
            <template #title>
              <h5 class="text-h5">
                Order Details
              </h5>
            </template>
            <template #append>
              <div class="text-base font-weight-medium text-primary cursor-pointer">
                Edit
              </div>
            </template>
          </VCardItem>

          <VDivider />
          <VDataTable
            :headers="headers"
            :items="orderDetail"
            item-value="productName"
            show-select
            class="text-no-wrap"
          >
            <template #item.productName="{ item }">
              <div class="d-flex gap-x-3 align-center">
                <VAvatar
                  size="34"
                  :image="item.productImage"
                  :rounded="0"
                />

                <div class="d-flex flex-column align-start">
                  <h6 class="text-h6">
                    {{ item.productName }}
                  </h6>

                  <span class="text-body-2">
                    {{ item.subtitle }}
                  </span>
                </div>
              </div>
            </template>

            <template #item.price="{ item }">
              <div class="text-body-1">
                ${{ item.price }}
              </div>
            </template>

            <template #item.total="{ item }">
              <div class="text-body-1">
                ${{ item.total }}
              </div>
            </template>

            <template #item.quantity="{ item }">
              <div class="text-body-1">
                {{ item.quantity }}
              </div>
            </template>

            <template #bottom />
          </VDataTable>
          <VDivider />

          <VCardText>
            <div class="d-flex align-end flex-column">
              <table class="text-high-emphasis">
                <tbody>
                  <tr>
                    <td width="200px">
                      Subtotal:
                    </td>
                    <td class="font-weight-medium">
                      $2,093
                    </td>
                  </tr>
                  <tr>
                    <td>Shipping Total: </td>
                    <td class="font-weight-medium">
                      $2
                    </td>
                  </tr>
                  <tr>
                    <td>Tax: </td>
                    <td class="font-weight-medium">
                      $28
                    </td>
                  </tr>
                  <tr>
                    <td class="text-high-emphasis font-weight-medium">
                      Total:
                    </td>
                    <td class="font-weight-medium">
                      $2,113
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </VCardText>
        </VCard>

        <!-- 👉 Shipping Activity -->
        <VCard title="Shipping Activity">
          <VCardText>
            <VTimeline
              truncate-line="both"
              line-inset="9"
              align="start"
              side="end"
              line-color="primary"
              density="compact"
            >
              <VTimelineItem
                dot-color="primary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <div class="app-timeline-title">
                    Order was placed (Order ID: #32543)
                  </div>
                  <div class="app-timeline-meta">
                    Tuesday 10:20 AM
                  </div>
                </div>
                <p class="app-timeline-text mb-0 mt-3">
                  Your order has been placed successfully
                </p>
              </VTimelineItem>

              <VTimelineItem
                dot-color="primary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <span class="app-timeline-title">Pick-up</span>
                  <span class="app-timeline-meta">Wednesday 11:29 AM</span>
                </div>
                <p class="app-timeline-text mb-0 mt-3">
                  Pick-up scheduled with courier
                </p>
              </VTimelineItem>

              <VTimelineItem
                dot-color="primary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <span class="app-timeline-title">Dispatched</span>
                  <span class="app-timeline-meta">Thursday 8:15 AM</span>
                </div>
                <p class="app-timeline-text mb-0 mt-3">
                  Item has been picked up by courier.
                </p>
              </VTimelineItem>

              <VTimelineItem
                dot-color="primary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <span class="app-timeline-title">Package arrived</span>
                  <span class="app-timeline-meta">Saturday 15:20 AM</span>
                </div>
                <p class="app-timeline-text mb-0 mt-3">
                  Package arrived at an Amazon facility, NY
                </p>
              </VTimelineItem>

              <VTimelineItem
                dot-color="primary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <span class="app-timeline-title">Dispatched for delivery</span>
                  <span class="app-timeline-meta">Today 14:12 PM</span>
                </div>
                <p class="app-timeline-text mb-0 mt-3">
                  Package has left an Amazon facility , NY
                </p>
              </VTimelineItem>

              <VTimelineItem
                dot-color="secondary"
                size="x-small"
              >
                <div class="d-flex justify-space-between align-center">
                  <span class="app-timeline-title">Delivery</span>
                </div>
                <p class="app-timeline-text mb-4 mt-3">
                  Package will be delivered by tomorrow
                </p>
              </VTimelineItem>
            </VTimeline>
          </VCardText>
        </VCard>
      </VCol>

      <VCol
        cols="12"
        md="4"
      >
        <!-- 👉 Customer Details  -->
        <VCard class="mb-6">
          <VCardText class="d-flex flex-column gap-y-6">
            <h5 class="text-h5">
              Customer details
            </h5>

            <div class="d-flex align-center">
              <VAvatar
                v-if="orderData"
                :variant="!orderData?.avatar.length ? 'tonal' : undefined"
                :rounded="1"
                class="me-3"
              >
                <VImg
                  v-if="orderData?.avatar"
                  :src="orderData?.avatar"
                />

                <span
                  v-else
                  class="font-weight-medium"
                >{{ avatarText(orderData?.customer) }}</span>
              </VAvatar>
              <div>
                <h6 class="text-h6">
                  {{ orderData?.customer }}
                </h6>
                <div class="text-body-1">
                  Customer ID: #{{ orderData?.order }}
                </div>
              </div>
            </div>

            <div class="d-flex gap-x-3 align-center">
              <VAvatar
                variant="tonal"
                color="success"
              >
                <VIcon icon="tabler-shopping-cart" />
              </VAvatar>
              <h6 class="text-h6">
                12 Orders
              </h6>
            </div>

            <div class="d-flex flex-column gap-y-1">
              <div class="d-flex justify-space-between align-center">
                <h6 class="text-h6">
                  Contact Info
                </h6>
                <div
                  class="text-base text-primary cursor-pointer font-weight-medium"
                  @click="isUserInfoEditDialogVisible = !isUserInfoEditDialogVisible"
                >
                  Edit
                </div>
              </div>
              <span>Email: {{ orderData?.email }}</span>
              <span>Mobile: +1 (609) 972-22-22</span>
            </div>
          </VCardText>
        </VCard>

        <!-- 👉 Shipping Address -->
        <VCard class="mb-6">
          <VCardItem>
            <VCardTitle>Shipping Address</VCardTitle>
            <template #append>
              <div class="d-flex align-center justify-space-between">
                <div
                  class="text-base font-weight-medium text-primary cursor-pointer"
                  @click="isEditAddressDialogVisible = !isEditAddressDialogVisible"
                >
                  Edit
                </div>
              </div>
            </template>
          </VCardItem>

          <VCardText>
            <div class="text-body-1">
              45 Rocker Terrace <br> Latheronwheel <br> KW5 8NW, London <br> UK
            </div>
          </VCardText>
        </VCard>

        <!-- 👉 Billing Address -->
        <VCard>
          <VCardText>
            <div class="d-flex align-center justify-space-between mb-2">
              <h5 class="text-h5">
                Billing Address
              </h5>
              <div
                class="text-base font-weight-medium text-primary cursor-pointer"
                @click="isEditAddressDialogVisible = !isEditAddressDialogVisible"
              >
                Edit
              </div>
            </div>
            <div>
              45 Rocker Terrace <br> Latheronwheel <br> KW5 8NW, London <br> UK
            </div>

            <div class="mt-6">
              <h5 class="text-h5 mb-1">
                Mastercard
              </h5>
              <div class="text-body-1">
                Card Number: ******{{ orderData?.methodNumber }}
              </div>
            </div>
          </VCardText>
        </VCard>
      </VCol>
    </VRow>

    <ConfirmDialog
      v-model:is-dialog-visible="isConfirmDialogVisible"
      confirmation-question="Are you sure to cancel your Order?"
      cancel-msg="Order cancelled!!"
      cancel-title="Cancelled"
      confirm-msg="Your order cancelled successfully."
      confirm-title="Cancelled!"
    />

    <UserInfoEditDialog
      v-model:is-dialog-visible="isUserInfoEditDialogVisible"
      :user-data="userData"
    />

    <AddEditAddressDialog
      v-model:is-dialog-visible="isEditAddressDialogVisible"
      :billing-address="currentBillingAddress"
    />
  </div>
</template>
