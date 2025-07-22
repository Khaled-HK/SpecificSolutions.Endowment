const emailRouteComponent = () => import('@/pages/apps/email/index.vue')

// 👉 Redirects
export const redirects = [
  // ℹ️ We are redirecting to different pages based on role.
  // NOTE: Role is just for UI purposes. ACL is based on abilities.
  {
    path: '/',
    name: 'index',
    redirect: to => {
      // التحقق من وجود بيانات المستخدم بطريقة آمنة
      try {
        const userData = useCookie('userData')
        const userRole = userData.value?.role
        
        // إذا كان المستخدم مسجل دخول، توجيهه حسب دوره
        if (userRole === 'admin')
          return { name: 'dashboard' }
        if (userRole === 'client')
          return { name: 'dashboard' }
        
        // إذا لم يكن مسجل دخول، توجيهه لصفحة تسجيل الدخول
        return { name: 'login', query: to.query }
      } catch (error) {
        console.log('Error accessing user data, redirecting to login:', error)
        // في حالة حدوث خطأ، توجيه المستخدم لصفحة تسجيل الدخول
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
  // مسار مباشر لصفحة تسجيل الدخول
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/login.vue'),
    meta: {
      layout: 'blank',
      unauthenticatedOnly: true,
      public: false,
    },
  },
  // Email filter
  {
    path: '/apps/email/filter/:filter',
    name: 'apps-email-filter',
    component: emailRouteComponent,
    meta: {
      navActiveLink: 'apps-email',
      layoutWrapperClasses: 'layout-content-height-fixed',
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
    },
  },
  {
    path: '/dashboards/logistics',
    name: 'dashboards-logistics',
    component: () => import('@/pages/apps/logistics/dashboard.vue'),
  },
  {
    path: '/dashboards/academy',
    name: 'dashboards-academy',
    component: () => import('@/pages/apps/academy/dashboard.vue'),
  },
  {
    path: '/apps/ecommerce/dashboard',
    name: 'apps-ecommerce-dashboard',
    component: () => import('@/pages/dashboards/ecommerce.vue'),
  },
]
