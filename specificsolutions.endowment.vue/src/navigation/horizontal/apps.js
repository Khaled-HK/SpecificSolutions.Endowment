export default [
  {
    title: 'Apps',
    icon: { icon: 'tabler-layout-grid-add' },
    children: [
      {
        title: 'Endowment Management',
        icon: { icon: 'tabler-building-mosque' },
        children: [
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
        ],
      },
      {
        title: 'Ecommerce',
        icon: { icon: 'tabler-shopping-cart-plus' },
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
              { title: 'List', to: 'apps-ecommerce-product-list', action: 'View', subject: 'otherview' },
              { title: 'Add', to: 'apps-ecommerce-product-add', action: 'View', subject: 'otherview' },
              { title: 'Category', to: 'apps-ecommerce-product-category-list', action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Order',
            children: [
              { title: 'List', to: 'apps-ecommerce-order-list', action: 'View', subject: 'otherview' },
              { title: 'Details', to: { name: 'apps-ecommerce-order-details-id', params: { id: '9042' } }, action: 'View', subject: 'otherview' },
            ],
          },
          {
            title: 'Customer',
            children: [
              { title: 'List', to: 'apps-ecommerce-customer-list', action: 'View', subject: 'otherview' },
              { title: 'Details', to: { name: 'apps-ecommerce-customer-details-id', params: { id: 478426 } }, action: 'View', subject: 'otherview' },
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
        icon: { icon: 'tabler-book' },
        children: [
          { title: 'Dashboard', to: 'apps-academy-dashboard', action: 'View', subject: 'otherview' },
          { title: 'My Course', to: 'apps-academy-my-course', action: 'View', subject: 'otherview' },
          { title: 'Course Details', to: 'apps-academy-course-details', action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'Logistics',
        icon: { icon: 'tabler-truck' },
        children: [
          { title: 'Dashboard', to: 'apps-logistics-dashboard', action: 'View', subject: 'otherview' },
          { title: 'Fleet', to: 'apps-logistics-fleet', action: 'View', subject: 'otherview' },
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
        icon: { icon: 'tabler-message-circle' },
        to: 'apps-chat',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Calendar',
        to: 'apps-calendar',
        icon: { icon: 'tabler-calendar' },
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
        icon: { icon: 'tabler-file-dollar' },
        children: [
          { title: 'List', to: 'apps-invoice-list', action: 'View', subject: 'otherview' },
          { title: 'Preview', to: { name: 'apps-invoice-preview-id', params: { id: '5036' } }, action: 'View', subject: 'otherview' },
          { title: 'Edit', to: { name: 'apps-invoice-edit-id', params: { id: '5036' } }, action: 'View', subject: 'otherview' },
          { title: 'Add', to: 'apps-invoice-add', action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'User',
        icon: { icon: 'tabler-users' },
        children: [
          { title: 'List', to: 'apps-user-list', action: 'View', subject: 'otherview' },
          { title: 'View', to: { name: 'apps-user-view-id', params: { id: 21 } }, action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'Roles & Permissions',
        icon: { icon: 'tabler-settings' },
        children: [
          { title: 'Roles', to: 'apps-roles', action: 'View', subject: 'otherview' },
          { title: 'Permissions', to: 'apps-permissions', action: 'View', subject: 'otherview' },
        ],
      },
    ],
  },
]

