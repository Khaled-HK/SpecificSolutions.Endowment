export default [
  {
    title: 'User Interface',
    icon: { icon: 'tabler-color-swatch' },
    children: [
      {
        title: 'Icons',
        icon: { icon: 'tabler-brand-tabler' },
        to: 'pages-icons',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Typography',
        icon: { icon: 'tabler-square-letter-t' },
        to: 'pages-typography',
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Cards',
        icon: { icon: 'tabler-id' },
        children: [
          { title: 'Basic', to: 'pages-cards-card-basic', action: 'View', subject: 'otherview' },
          { title: 'Advance', to: 'pages-cards-card-advance', action: 'View', subject: 'otherview' },
          { title: 'Statistics', to: 'pages-cards-card-statistics', action: 'View', subject: 'otherview' },
          { title: 'Widgets', to: 'pages-cards-card-widgets', action: 'View', subject: 'otherview' },
          { title: 'Actions', to: 'pages-cards-card-actions', action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'Components',
        icon: { icon: 'tabler-toggle-left' },
        children: [
          { title: 'Alert', to: 'components-alert', action: 'View', subject: 'otherview' },
          { title: 'Avatar', to: 'components-avatar', action: 'View', subject: 'otherview' },
          { title: 'Badge', to: 'components-badge', action: 'View', subject: 'otherview' },
          { title: 'Button', to: 'components-button', action: 'View', subject: 'otherview' },
          { title: 'Chip', to: 'components-chip', action: 'View', subject: 'otherview' },
          { title: 'Dialog', to: 'components-dialog', action: 'View', subject: 'otherview' },
          { title: 'Expansion Panel', to: 'components-expansion-panel', action: 'View', subject: 'otherview' },
          { title: 'List', to: 'components-list', action: 'View', subject: 'otherview' },
          { title: 'Menu', to: 'components-menu', action: 'View', subject: 'otherview' },
          { title: 'Pagination', to: 'components-pagination', action: 'View', subject: 'otherview' },
          { title: 'Progress Circular', to: 'components-progress-circular', action: 'View', subject: 'otherview' },
          { title: 'Progress Linear', to: 'components-progress-linear', action: 'View', subject: 'otherview' },
          { title: 'Snackbar', to: 'components-snackbar', action: 'View', subject: 'otherview' },
          { title: 'Tabs', to: 'components-tabs', action: 'View', subject: 'otherview' },
          { title: 'Timeline', to: 'components-timeline', action: 'View', subject: 'otherview' },
          { title: 'Tooltip', to: 'components-tooltip', action: 'View', subject: 'otherview' },
        ],
      },
      {
        title: 'Extensions',
        icon: { icon: 'tabler-box' },
        children: [
          { title: 'Tour', to: 'extensions-tour', action: 'View', subject: 'otherview' },
          { title: 'Swiper', to: 'extensions-swiper', action: 'View', subject: 'otherview' },
        ],
      },
    ],
  },
]
