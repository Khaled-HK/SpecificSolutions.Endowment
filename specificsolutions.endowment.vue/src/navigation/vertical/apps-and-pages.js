export default [
  { heading: 'Apps & Pages' },
  {
    title: 'endowmentManagement',
    icon: { icon: 'tabler-building-mosque' },
    children: [
      {
        title: 'Buildings',
        to: 'apps-buildings',
        icon: { icon: 'tabler-building' },
        action: 'View',
        subject: 'Building',
      },
      {
        title: 'Mosques',
        to: 'apps-mosques',
        icon: { icon: 'tabler-building-mosque' },
        action: 'View',
        subject: 'Mosque',
      },
      {
        title: 'Cities',
        to: 'apps-cities',
        icon: { icon: 'tabler-map-pin' },
        action: 'View',
        subject: 'City',
      },
      {
        title: 'Regions',
        to: 'apps-regions',
        icon: { icon: 'tabler-map' },
        action: 'View',
        subject: 'Region',
      },
      {
        title: 'Offices',
        to: 'apps-offices',
        icon: { icon: 'tabler-building-community' },
        action: 'View',
        subject: 'Office',
      },
      {
        title: 'Products',
        to: 'apps-products',
        icon: { icon: 'tabler-package' },
        action: 'View',
        subject: 'Product',
      },
      {
        title: 'Decisions',
        to: 'apps-decisions',
        icon: { icon: 'tabler-file-text' },
        action: 'View',
        subject: 'Decision',
      },
      {
        title: 'Accounts',
        to: 'apps-accounts',
        icon: { icon: 'tabler-users' },
        action: 'View',
        subject: 'Account',
      },
      {
        title: 'Account Details',
        to: 'apps-account-details',
        icon: { icon: 'tabler-user-detail' },
        action: 'View',
        subject: 'AccountDetail',
      },
      {
        title: 'Requests',
        to: 'apps-requests',
        icon: { icon: 'tabler-file-text' },
        action: 'View',
        subject: 'Request',
      },
      {
        title: 'Construction Requests',
        to: 'apps-construction-requests',
        icon: { icon: 'tabler-building' },
        action: 'View',
        subject: 'ConstructionRequest',
      },
      {
        title: 'Maintenance Requests',
        to: 'apps-maintenance-requests',
        icon: { icon: 'tabler-tools' },
        action: 'View',
        subject: 'MaintenanceRequest',
      },
      {
        title: 'Change Requests',
        to: 'apps-change-requests',
        icon: { icon: 'tabler-edit' },
        action: 'View',
        subject: 'ChangeRequest',
      },
    ],
  },
  {
    title: 'Ecommerce',
    icon: { icon: 'tabler-shopping-cart' },
    children: [
      {
        title: 'Dashboard',
        to: 'apps-ecommerce-dashboard',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Product',
        children: [
          { 
            title: 'List', 
            to: 'apps-ecommerce-product-list',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Add', 
            to: 'apps-ecommerce-product-add',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Category', 
            to: 'apps-ecommerce-product-category-list',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Order',
        children: [
          { 
            title: 'List', 
            to: 'apps-ecommerce-order-list',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Details', 
            to: { name: 'apps-ecommerce-order-details-id', params: { id: '9042' } },
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Customer',
        children: [
          { 
            title: 'List', 
            to: 'apps-ecommerce-customer-list',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Details', 
            to: { name: 'apps-ecommerce-customer-details-id', params: { id: 478426 } },
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Manage Review',
        to: 'apps-ecommerce-manage-review',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Referrals',
        to: 'apps-ecommerce-referrals',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Settings',
        to: 'apps-ecommerce-settings',
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Academy',
    icon: { icon: 'tabler-school' },
    children: [
      { 
        title: 'Dashboard', 
        to: 'apps-academy-dashboard',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'My Course', 
        to: 'apps-academy-my-course',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Course Details', 
        to: 'apps-academy-course-details',
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Logistics',
    icon: { icon: 'tabler-truck' },
    children: [
      { 
        title: 'Dashboard', 
        to: 'apps-logistics-dashboard',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Fleet', 
        to: 'apps-logistics-fleet',
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Email',
    icon: { icon: 'tabler-mail' },
    to: 'apps-email',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Chat',
    icon: { icon: 'tabler-message-circle-2' },
    to: 'apps-chat',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Calendar',
    icon: { icon: 'tabler-calendar' },
    to: 'apps-calendar',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Kanban',
    icon: { icon: 'tabler-layout-kanban' },
    to: 'apps-kanban',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Invoice',
    icon: { icon: 'tabler-file-invoice' },
    children: [
      { 
        title: 'List', 
        to: 'apps-invoice-list',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Preview', 
        to: { name: 'apps-invoice-preview-id', params: { id: '5036' } },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Edit', 
        to: { name: 'apps-invoice-edit-id', params: { id: '5036' } },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Add', 
        to: 'apps-invoice-add',
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'User',
    icon: { icon: 'tabler-user' },
    children: [
      { 
        title: 'List', 
        to: 'apps-user-list',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'View', 
        to: { name: 'apps-user-view-id', params: { id: 21 } },
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Roles & Permissions',
    icon: { icon: 'tabler-lock' },
    children: [
      { 
        title: 'Roles', 
        to: 'apps-roles',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Permissions', 
        to: 'apps-permissions',
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Pages',
    icon: { icon: 'tabler-file' },
    children: [
      { 
        title: 'User Profile', 
        to: { name: 'pages-user-profile-tab', params: { tab: 'profile' } },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Account Settings', 
        to: { name: 'pages-account-settings-tab', params: { tab: 'account' } },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Pricing', 
        to: 'pages-pricing',
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'FAQ', 
        to: 'pages-faq',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Miscellaneous',
        children: [
          { 
            title: 'Coming Soon', 
            to: 'pages-misc-coming-soon', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Under Maintenance', 
            to: 'pages-misc-under-maintenance', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Page Not Found - 404', 
            to: { path: '/pages/misc/not-found' }, 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Not Authorized - 401', 
            to: { path: '/pages/misc/not-authorized' }, 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
    ],
  },
  {
    title: 'Authentication',
    icon: { icon: 'tabler-shield-lock' },
    children: [
      {
        title: 'Login',
        children: [
          { 
            title: 'Login v1', 
            to: 'pages-authentication-login-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Login v2', 
            to: 'pages-authentication-login-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Register',
        children: [
          { 
            title: 'Register v1', 
            to: 'pages-authentication-register-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Register v2', 
            to: 'pages-authentication-register-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Register Multi-Steps', 
            to: 'pages-authentication-register-multi-steps', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Verify Email',
        children: [
          { 
            title: 'Verify Email v1', 
            to: 'pages-authentication-verify-email-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Verify Email v2', 
            to: 'pages-authentication-verify-email-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Forgot Password',
        children: [
          { 
            title: 'Forgot Password v1', 
            to: 'pages-authentication-forgot-password-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Forgot Password v2', 
            to: 'pages-authentication-forgot-password-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Reset Password',
        children: [
          { 
            title: 'Reset Password v1', 
            to: 'pages-authentication-reset-password-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Reset Password v2', 
            to: 'pages-authentication-reset-password-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
      {
        title: 'Two Steps',
        children: [
          { 
            title: 'Two Steps v1', 
            to: 'pages-authentication-two-steps-v1', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
          { 
            title: 'Two Steps v2', 
            to: 'pages-authentication-two-steps-v2', 
            target: '_blank',
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
    ],
  },
  {
    title: 'Wizard Examples',
    icon: { icon: 'tabler-dots' },
    children: [
      { 
        title: 'Checkout', 
        to: { name: 'wizard-examples-checkout' },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Property Listing', 
        to: { name: 'wizard-examples-property-listing' },
        action: 'View',
        subject: 'otherview',
      },
      { 
        title: 'Create Deal', 
        to: { name: 'wizard-examples-create-deal' },
        action: 'View',
        subject: 'otherview',
      },
    ],
  },
  {
    title: 'Dialog Examples',
    icon: { icon: 'tabler-square' },
    to: 'pages-dialog-examples',
    action: 'View',
    subject: 'otherview',
  },
]
