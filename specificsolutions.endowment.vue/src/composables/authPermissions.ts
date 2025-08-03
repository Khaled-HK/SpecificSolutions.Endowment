// الصلاحيات الأساسية المطلوبة
export const BASE_PERMISSIONS = [
  { action: 'read', subject: 'Auth' },
  { action: 'write', subject: 'Auth' },
  { action: 'delete', subject: 'Auth' },
  { action: 'View', subject: 'Dashboard' },
  { action: 'read', subject: 'Dashboard' },
  { action: 'write', subject: 'Dashboard' }
]

export const SUBJECTS = [
  'AccountDetail', 'ConstructionRequest', 'MaintenanceRequest', 'ChangeRequest',
  'DemolitionRequest', 'NameChangeRequest', 'NeedsRequest', 'ExpenditureChangeRequest',
  'ChangeOfPathRequest', 'Account', 'User', 'Role', 'Decision', 'Request',
  'Office', 'Endowment', 'City', 'Region', 'Building', 'Mosque'
] as const

// دوال مساعدة للصلاحيات
export const mapPermissionToAction = (permission: string | Record<string, any>): string | null => {
  if (typeof permission === 'string') {
    if (permission.endsWith('_View')) return 'View'
    if (permission.endsWith('_Add')) return 'Add'
    if (permission.endsWith('_Edit')) return 'Edit'
    if (permission.endsWith('_Delete')) return 'Delete'
  }
  
  if (typeof permission === 'object' && 'action' in permission) {
    return String(permission.action)
  }
  
  return null
}

export const mapPermissionToSubject = (permission: string | Record<string, any>): string | null => {
  if (typeof permission === 'string') {
    for (const subject of SUBJECTS) {
      if (permission.startsWith(`${subject}_`)) {
        return subject
      }
    }
  }
  
  if (typeof permission === 'object' && 'subject' in permission) {
    return String(permission.subject)
  }
  
  return null
} 