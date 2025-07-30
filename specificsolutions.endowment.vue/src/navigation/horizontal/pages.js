export default [
  {
    title: 'Pages',
    icon: { icon: 'tabler-file' },
    children: [
      {
        title: 'User Profile',
        icon: { icon: 'tabler-user-circle' },
        to: { name: 'pages-user-profile-tab', params: { tab: 'profile' } },
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Account Settings',
        icon: { icon: 'tabler-settings' },
        to: { name: 'pages-account-settings-tab', params: { tab: 'account' } },
        action: 'View',
        subject: 'otherview',
      },
      { title: 'FAQ', icon: { icon: 'tabler-help' }, to: 'pages-faq', action: 'View', subject: 'otherview' },
      { title: 'Pricing', icon: { icon: 'tabler-diamond' }, to: 'pages-pricing', action: 'View', subject: 'otherview' },
      {
        title: 'Misc',
        icon: { icon: 'tabler-cube' },
        children: [
          { title: 'Coming Soon', to: 'pages-misc-coming-soon', action: 'View', subject: 'otherview' },
          { title: 'Under Maintenance', to: 'pages-misc-under-maintenance', target: '_blank', action: 'View', subject: 'otherview' },
          { title: 'Page Not Found - 404', to: { path: '/pages/misc/not-found' }, target: '_blank', action: 'View', subject: 'otherview' },
          { title: 'Not Authorized - 401', to: { path: '/pages/misc/not-authorized' }, target: '_blank', action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'Authentication',
        icon: { icon: 'tabler-lock' },
        children: [
          {
            title: 'Login',
            children: [
              { title: 'Login v1', to: 'pages-authentication-login-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Login v2', to: 'pages-authentication-login-v2', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Register',
            children: [
              { title: 'Register v1', to: 'pages-authentication-register-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Register v2', to: 'pages-authentication-register-v2', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Register Multi-Steps', to: 'pages-authentication-register-multi-steps', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Verify Email',
            children: [
              { title: 'Verify Email v1', to: 'pages-authentication-verify-email-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Verify Email v2', to: 'pages-authentication-verify-email-v2', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Forgot Password',
            children: [
              { title: 'Forgot Password v1', to: 'pages-authentication-forgot-password-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Forgot Password v2', to: 'pages-authentication-forgot-password-v2', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Reset Password',
            children: [
              { title: 'Reset Password v1', to: 'pages-authentication-reset-password-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Reset Password v2', to: 'pages-authentication-reset-password-v2', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Two Steps',
            children: [
              { title: 'Two Steps v1', to: 'pages-authentication-two-steps-v1', target: '_blank', action: 'View', subject: 'otherview' },
              { title: 'Two Steps v2', to: 'pages-authentication-two-steps-v2', target: '_blank', action: 'View', subject: 'otherview' },
            ],
          },
        ],
      },
      {
        title: 'Wizard Pages',
        icon: { icon: 'tabler-forms' },
        children: [
          { title: 'Checkout', to: { name: 'wizard-examples-checkout' }, action: 'View', subject: 'otherview' },
          { title: 'Property Listing', to: { name: 'wizard-examples-property-listing' }, action: 'View', subject: 'otherview' },
          { title: 'Create Deal', to: { name: 'wizard-examples-create-deal' }, action: 'View', subject: 'otherview' },
        ],
      },
      { title: 'Dialog Examples', icon: { icon: 'tabler-square' }, to: 'pages-dialog-examples', action: 'View', subject: 'otherview' },
      {
        title: 'Front Pages',
        icon: { icon: 'tabler-files' },
        children: [
          {
            title: 'Landing',
            to: 'front-pages-landing-page',
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          {
            title: 'Pricing',
            to: 'front-pages-pricing',
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          {
            title: 'Payment',
            to: 'front-pages-payment',
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          {
            title: 'Checkout',
            to: 'front-pages-checkout',
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          {
            title: 'Help Center',
            to: 'front-pages-help-center',
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
    ],
  },
]
