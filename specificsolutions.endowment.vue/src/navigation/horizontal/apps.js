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
        ],
      },
      {
        title: 'Ecommerce',
        icon: { icon: 'tabler-shopping-cart-plus' },
        children: [
          {
            title: 'Dashboard',
            to: 'apps-ecommerce-dashboard',
          },
          {
            title: 'Product',
            children: [
              { title: 'List', to: 'apps-ecommerce-product-list' },
              { title: 'Add', to: 'apps-ecommerce-product-add' },
              { title: 'Category', to: 'apps-ecommerce-product-category-list' },
            ],
          },
          {
            title: 'Order',
            children: [
              { title: 'List', to: 'apps-ecommerce-order-list' },
              { title: 'Details', to: { name: 'apps-ecommerce-order-details-id', params: { id: '9042' } } },
            ],
          },
          {
            title: 'Customer',
            children: [
              { title: 'List', to: 'apps-ecommerce-customer-list' },
              { title: 'Details', to: { name: 'apps-ecommerce-customer-details-id', params: { id: 478426 } } },
            ],
          },
          {
            title: 'Manage Review',
            to: 'apps-ecommerce-manage-review',
          },
          {
            title: 'Referrals',
            to: 'apps-ecommerce-referrals',
          },
          {
            title: 'Settings',
            to: 'apps-ecommerce-settings',
          },
        ],
      },
      {
        title: 'Academy',
        icon: { icon: 'tabler-book' },
        children: [
          { title: 'Dashboard', to: 'apps-academy-dashboard' },
          { title: 'My Course', to: 'apps-academy-my-course' },
          { title: 'Course Details', to: 'apps-academy-course-details' },
        ],
      },
      {
        title: 'Logistics',
        icon: { icon: 'tabler-truck' },
        children: [
          { title: 'Dashboard', to: 'apps-logistics-dashboard' },
          { title: 'Fleet', to: 'apps-logistics-fleet' },
        ],
      },
      {
        title: 'Email',
        icon: { icon: 'tabler-mail' },
        to: 'apps-email',
      },
      {
        title: 'Chat',
        icon: { icon: 'tabler-message-circle' },
        to: 'apps-chat',
      },
      {
        title: 'Calendar',
        to: 'apps-calendar',
        icon: { icon: 'tabler-calendar' },
      },
      {
        title: 'Kanban',
        icon: { icon: 'tabler-layout-kanban' },
        to: 'apps-kanban',
      },
      {
        title: 'Invoice',
        icon: { icon: 'tabler-file-dollar' },
        children: [
          { title: 'List', to: 'apps-invoice-list' },
          { title: 'Preview', to: { name: 'apps-invoice-preview-id', params: { id: '5036' } } },
          { title: 'Edit', to: { name: 'apps-invoice-edit-id', params: { id: '5036' } } },
          { title: 'Add', to: 'apps-invoice-add' },
        ],
      },
      {
        title: 'User',
        icon: { icon: 'tabler-users' },
        children: [
          { title: 'List', to: 'apps-user-list' },
          { title: 'View', to: { name: 'apps-user-view-id', params: { id: 21 } } },
        ],
      },
      {
        title: 'Roles & Permissions',
        icon: { icon: 'tabler-settings' },
        children: [
          { title: 'Roles', to: 'apps-roles' },
          { title: 'Permissions', to: 'apps-permissions' },
        ],
      },
    ],
  },
]

