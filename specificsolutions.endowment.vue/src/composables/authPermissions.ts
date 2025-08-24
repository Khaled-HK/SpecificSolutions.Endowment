// الصلاحيات الأساسية المطلوبة - محدثة وشاملة
// 
// تنسيق الصلاحيات من الباك إند:
// - Account_View, User_Add, Building_Edit, etc. (مع underscore)
// - AccountView, UserAdd, BuildingEdit, etc. (بدون underscore)
// - manage, read, write (تنسيق CASL)
//
// تحويل الصلاحيات:
// - _View -> View
// - _Add -> create  
// - _Edit -> update
// - _Delete -> delete
export const BASE_PERMISSIONS = [
  // صلاحيات أساسية للوحة القيادة والمصادقة
  { action: 'View', subject: 'Dashboard' },
  { action: 'read', subject: 'Auth' },
  { action: 'write', subject: 'Auth' },
  
  // صلاحيات للصفحات العامة
  { action: 'View', subject: 'Login' },
  { action: 'View', subject: 'Register' },
  { action: 'View', subject: 'ForgotPassword' },
  { action: 'View', subject: 'ResendEmailConfirmation' },
  { action: 'View', subject: 'ConfirmEmail' },
  
  // صلاحيات أساسية لإدارة الأوقاف
  { action: 'View', subject: 'Building' },
  { action: 'View', subject: 'Mosque' },
  { action: 'View', subject: 'City' },
  { action: 'View', subject: 'Region' },
  { action: 'View', subject: 'Office' },
  { action: 'View', subject: 'Product' },
  { action: 'View', subject: 'Account' },
  { action: 'View', subject: 'User' },
  { action: 'View', subject: 'Request' },
  { action: 'View', subject: 'Decision' },
  
  // صلاحيات القراءة الأساسية
  { action: 'read', subject: 'Building' },
  { action: 'read', subject: 'Mosque' },
  { action: 'read', subject: 'City' },
  { action: 'read', subject: 'Region' },
  { action: 'read', subject: 'Office' },
  { action: 'read', subject: 'Product' },
  { action: 'read', subject: 'Account' },
  { action: 'read', subject: 'User' },
  { action: 'read', subject: 'Request' },
  { action: 'read', subject: 'Decision' }
]

export const SUBJECTS = [
  'AccountDetail', 'ConstructionRequest', 'MaintenanceRequest', 'ChangeOfPathRequest',
  'DemolitionRequest', 'NameChangeRequest', 'NeedsRequest', 'ExpenditureChangeRequest',
  'Account', 'User', 'Role', 'Decision', 'Request',
  'Office', 'Endowment', 'City', 'Region', 'Building', 'Mosque'
] as const

// دوال مساعدة للصلاحيات - محدثة لتعمل مع الباك إند
export const mapPermissionToAction = (permission: string | Record<string, any>): string | null => {
  if (typeof permission === 'string') {
    // تنسيق الباك إند: Account_View, User_Add, etc. (مع underscore)
    if (permission.endsWith('_View')) return 'View'
    if (permission.endsWith('_Add')) return 'create'
    if (permission.endsWith('_Edit')) return 'update'
    if (permission.endsWith('_Delete')) return 'delete'
    
    // تنسيق الباك إند: AccountView, UserAdd, etc. (بدون underscore)
    if (permission.includes('View')) return 'View'
    if (permission.includes('Add')) return 'create'
    if (permission.includes('Edit')) return 'update'
    if (permission.includes('Delete')) return 'delete'
    
    // تنسيق CASL: manage, read, write
    if (permission.includes('manage')) return 'manage'
    if (permission.includes('read')) return 'read'
    if (permission.includes('write')) return 'write'
  }
  
  if (typeof permission === 'object' && 'action' in permission) {
    return String(permission.action)
  }
  
  return null
}

export const mapPermissionToSubject = (permission: string | Record<string, any>): string | null => {
  if (typeof permission === 'string') {
    // تنسيق الباك إند: Account_View, User_Add, etc. (مع underscore)
    for (const subject of SUBJECTS) {
      if (permission.startsWith(`${subject}_`)) {
        return subject
      }
    }
    
    // تنسيق الباك إند: AccountView, UserAdd, etc. (بدون underscore)
    for (const subject of SUBJECTS) {
      if (permission.includes(subject)) {
        return subject
      }
    }
    
    // صلاحيات خاصة
    if (permission.includes('all')) return 'all'
    if (permission.includes('Auth')) return 'Auth'
    if (permission.includes('Dashboard')) return 'Dashboard'
  }
  
  if (typeof permission === 'object' && 'subject' in permission) {
    return String(permission.subject)
  }
  
  return null
} 