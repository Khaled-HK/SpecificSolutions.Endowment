export const routes = [
  { path: '/', redirect: '/dashboard' },
  {
    path: '/',
    component: () => import('@/layouts/default.vue'),
    children: [
      {
        path: 'dashboard',
        component: () => import('@/pages/dashboard.vue'),
      },
      {
        path: 'account-settings',
        component: () => import('@/pages/account-settings.vue'),
      },
      {
        path: 'typography',
        component: () => import('@/pages/typography.vue'),
      },
      {
        path: 'icons',
        component: () => import('@/pages/icons.vue'),
      },
      {
        path: 'cards',
        component: () => import('@/pages/cards.vue'),
      },
      {
        path: 'tables',
        component: () => import('@/pages/tables.vue'),
      },
      {
        path: 'form-layouts',
        component: () => import('@/pages/form-layouts.vue'),
      },
    ],
  },
  {
    path: '/',
    component: () => import('@/layouts/blank.vue'),
    children: [
      {
        path: 'login',
        component: () => import('@/pages/login.vue'),
      },
      {
        path: 'register',
        component: () => import('@/pages/register.vue'),
      },
      {
        path: 'confirm-email',
        component: () => import('@/pages/confirm-email.vue'),
      },
      {
        path: 'resend-email-confirmation',
        component: () => import('@/pages/resend-email-confirmation.vue'),
      },
      {
        path: 'resend-verification-code',
        component: () => import('@/pages/resend-verification-code.vue'),
      },
      {
        path: 'email-not-confirmed',
        component: () => import('@/pages/email-not-confirmed.vue'),
      },
      {
        path: 'forgot-password',
        component: () => import('@/pages/forgot-password.vue'),
      },
      {
        path: 'reset-password',
        component: () => import('@/pages/reset-password.vue'),
      },
      {
        path: 'change-password',
        component: () => import('@/pages/change-password.vue'),
      },
      {
        path: '/:pathMatch(.*)*',
        component: () => import('@/pages/[...error].vue'),
      },
    ],
  },
]
