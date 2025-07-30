export default [
  { heading: 'Others' },
  {
    title: 'Access Control',
    icon: { icon: 'tabler-command' },
    to: 'access-control',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Nav Levels',
    icon: { icon: 'tabler-menu-2' },
    children: [
      {
        title: 'Level 2.1',
        to: null,
        action: 'View',
        subject: 'otherview',
      },
      {
        title: 'Level 2.2',
        children: [
          {
            title: 'Level 3.1',
            to: null,
            action: 'View',
            subject: 'otherview',
          },
          {
            title: 'Level 3.2',
            to: null,
            action: 'View',
            subject: 'otherview',
          },
        ],
      },
    ],
  },
  {
    title: 'Disabled Menu',
    to: null,
    icon: { icon: 'tabler-eye-off' },
    disable: true,
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Raise Support',
    href: 'https://pixinvent.ticksy.com/',
    icon: { icon: 'tabler-headphones' },
    target: '_blank',
    action: 'View',
    subject: 'otherview',
  },
  {
    title: 'Documentation',
    href: 'https://demos.pixinvent.com/vuexy-vuejs-admin-template/documentation/',
    icon: { icon: 'tabler-file-text' },
    target: '_blank',
    action: 'View',
    subject: 'otherview',
  },
]
