export default [
  {
    title: 'Dashboards',
    icon: { icon: 'tabler-smart-home' },
    children: [
      {
        title: 'Analytics',
        to: 'dashboards-analytics',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'CRM',
        to: 'dashboards-crm',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Ecommerce',
        to: 'dashboards-ecommerce',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Academy',
        to: 'dashboards-academy',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Logistics',
        to: 'dashboards-logistics',
        action: 'View',
        subject: 'otherview',
      },
    ],
    badgeContent: '5',
    badgeClass: 'bg-error',
  },
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
]
