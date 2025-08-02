import { HttpResponse, http } from 'msw'

// Handlers for auth
export const handlerAuth = [
  http.post(('/api/auth/login'), async ({ request }) => {
    const { email, password } = await request.json()
    
    // Simple validation for demo purposes
    if (email === 'admin@demo.com' && password === 'admin') {
      const response = {
        userAbilityRules: [
          { action: 'manage', subject: 'all' },
          { action: 'View', subject: 'Dashboard' },
          { action: 'read', subject: 'Auth' },
          { action: 'write', subject: 'Auth' },
          { action: 'delete', subject: 'Auth' }
        ],
        accessToken: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MX0.fhc3wykrAnRpcKApKhXiahxaOe8PSHatad31NuIZ0Zg',
        userData: {
          id: 1,
          fullName: 'Admin User',
          username: 'admin',
          avatar: '/images/avatars/avatar-1.png',
          email: 'admin@demo.com',
          role: 'admin'
        }
      }
      
      return HttpResponse.json(response, { status: 201 })
    }
    
    if (email === 'client@demo.com' && password === 'client') {
      const response = {
        userAbilityRules: [
          { action: 'read', subject: 'AclDemo' },
          { action: 'View', subject: 'Dashboard' },
          { action: 'read', subject: 'Auth' },
          { action: 'write', subject: 'Auth' },
          { action: 'delete', subject: 'Auth' }
        ],
        accessToken: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Mn0.cat2xMrZLn0FwicdGtZNzL7ifDTAKWB0k1RurSWjdnw',
        userData: {
          id: 2,
          fullName: 'Client User',
          username: 'client',
          avatar: '/images/avatars/avatar-2.png',
          email: 'client@demo.com',
          role: 'client'
        }
      }
      
      return HttpResponse.json(response, { status: 201 })
    }
    
    return HttpResponse.json({ 
      errors: { email: ['Invalid email or password'] } 
    }, { status: 400 })
  }),
]
