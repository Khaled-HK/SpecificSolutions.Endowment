// ملف تصحيح للقائمة الجانبية
// هذا الملف يحتوي على عناصر بسيطة للتأكد من أن القائمة الجانبية تعمل

export default [
  { heading: 'Debug Navigation' },
  {
    title: 'Test Item 1',
    icon: { icon: 'tabler-home' },
    to: '/dashboard',
  },
  {
    title: 'Test Item 2',
    icon: { icon: 'tabler-user' },
    to: '/pages/user-profile',
  },
  {
    title: 'Test Group',
    icon: { icon: 'tabler-settings' },
    children: [
      {
        title: 'Sub Item 1',
        to: '/pages/account-settings',
      },
      {
        title: 'Sub Item 2',
        to: '/pages/pricing',
      },
    ],
  },
] 