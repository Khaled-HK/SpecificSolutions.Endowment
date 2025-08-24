# 🔐 استخدام الصلاحيات

## 📋 **كيفية عمل النظام:**

### **Backend - إرسال الصلاحيات مع تسجيل الدخول:**
```csharp
// في Authenticator.cs - LoginAsync
var permissions = await _permissionService.GetUserPermissionsAsync(user);

return new UserLogin
{
    Id = user.Id,
    Name = user.Name,
    Token = token,
    RefreshToken = refreshToken,
    Permissions = permissions  // الصلاحيات تُرسل مع الاستجابة
};
```

### **Frontend - حفظ واستخدام الصلاحيات:**
```javascript
// عند تسجيل الدخول
const loginResponse = await login(userData);
localStorage.setItem('permissions', JSON.stringify(loginResponse.permissions));

// التحقق من الصلاحيات
function hasPermission(permission) {
    const permissions = JSON.parse(localStorage.getItem('permissions') || '[]');
    return permissions.includes(permission);
}
```

## 🎯 **أمثلة الاستخدام:**

### **في Vue Component:**
```vue
<template>
  <div>
    <button v-if="hasPermission('AdminAccess')" @click="openAdminPanel">
      لوحة الإدارة
    </button>
    
    <div v-if="hasPermission('UserView')">
      <h3>قائمة المستخدمين</h3>
    </div>
  </div>
</template>

<script setup>
const hasPermission = (permission) => {
  const permissions = JSON.parse(localStorage.getItem('permissions') || '[]')
  return permissions.includes(permission)
}
</script>
```

### **في Router Guard:**
```javascript
router.beforeEach((to, from, next) => {
  const permissions = JSON.parse(localStorage.getItem('permissions') || '[]')
  
  if (to.meta.requiresPermission) {
    if (permissions.includes(to.meta.requiresPermission)) {
      next()
    } else {
      next('/unauthorized')
    }
  } else {
    next()
  }
})
```

## ✅ **المزايا:**
- **بسيط** - لا تعقيد
- **سريع** - الصلاحيات محفوظة محلياً
- **آمن** - التحقق من Backend
- **سهل الاستخدام** - دالة واحدة للتحقق
