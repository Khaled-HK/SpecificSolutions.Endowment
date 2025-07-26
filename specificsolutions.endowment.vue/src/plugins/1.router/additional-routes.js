const emailRouteComponent = () => import('@/pages/apps/email/index.vue')

// 👉 Redirects
export const redirects = [
  // ℹ️ We are redirecting to different pages based on role.
  // NOTE: Role is just for UI purposes. ACL is based on abilities.
  {
    path: '/',
    name: 'index',
    redirect: to => {
      try {
        // Check if user is logged in
        const userData = useCookie('userData')
        const accessToken = useCookie('accessToken')
        
        const isLoggedIn = !!(userData.value && accessToken.value)
        
        if (isLoggedIn) {
          // User is logged in, redirect to dashboard
          return { name: 'dashboard' }
        } else {
          // User is not logged in, redirect to login
          return { name: 'login', query: to.query }
        }
      } catch (error) {
        console.error('Redirect error:', error)
        // Fallback to login page
        return { name: 'login', query: to.query }
      }
    },
  },
  {
    path: '/pages/user-profile',
    name: 'pages-user-profile',
    redirect: () => ({ name: 'pages-user-profile-tab', params: { tab: 'profile' } }),
  },
  {
    path: '/pages/account-settings',
    name: 'pages-account-settings',
    redirect: () => ({ name: 'pages-account-settings-tab', params: { tab: 'account' } }),
  },
]
export const routes = [
  // Email filter
  {
    path: '/apps/email/filter/:filter',
    name: 'apps-email-filter',
    component: emailRouteComponent,
    meta: {
      navActiveLink: 'apps-email',
      layoutWrapperClasses: 'layout-content-height-fixed',
      action: 'View',
      subject: 'Dashboard',
    },
  },

  // Email label
  {
    path: '/apps/email/label/:label',
    name: 'apps-email-label',
    component: emailRouteComponent,
    meta: {
      // contentClass: 'email-application',
      navActiveLink: 'apps-email',
      layoutWrapperClasses: 'layout-content-height-fixed',
      action: 'View',
      subject: 'Dashboard',
    },
  },
  {
    path: '/dashboards/logistics',
    name: 'dashboards-logistics',
    component: () => import('@/pages/apps/logistics/dashboard.vue'),
    meta: {
      action: 'View',
      subject: 'Dashboard',
    },
  },
  {
    path: '/dashboards/academy',
    name: 'dashboards-academy',
    component: () => import('@/pages/apps/academy/dashboard.vue'),
    meta: {
      action: 'View',
      subject: 'Dashboard',
    },
  },
  {
    path: '/apps/ecommerce/dashboard',
    name: 'apps-ecommerce-dashboard',
    component: () => import('@/pages/dashboards/ecommerce.vue'),
    meta: {
      action: 'View',
      subject: 'Dashboard',
    },
  },
]
