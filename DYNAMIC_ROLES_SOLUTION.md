# الحل الجديد: الأدوار الديناميكية من قاعدة البيانات

## المشكلة الأصلية

في الفرونت إند، الأدوار محددة بشكل ثابت:
```vue
<VListItem @click="approveUser(item.id, 'Admin')">
<VListItem @click="approveUser(item.id, 'Employee')">
<VListItem @click="approveUser(item.id, 'Customer')">
```

**المشكلة**: إذا تم إضافة أدوار جديدة في المستقبل، ستحتاج إلى تحديث الفرونت إند يدوياً.

## الحل الجديد: الأدوار من قاعدة البيانات

### 1. الأدوار موجودة في جدول ApplicationRole

الأدوار موجودة في قاعدة البيانات في جدول `ApplicationRole`:
```sql
SELECT * FROM AspNetRoles;
-- Admin
-- Employee  
-- Customer
```

### 2. API جديد لجلب الأدوار المتاحة

#### GetAvailableRolesQuery.cs
```csharp
public sealed record GetAvailableRolesQuery : IQuery<List<string>>;
```

#### GetAvailableRolesHandler.cs
```csharp
public class GetAvailableRolesHandler : IQueryHandler<GetAvailableRolesQuery, List<string>>
{
    private readonly IApplicationRoleRepository _roleRepository;

    public GetAvailableRolesHandler(IApplicationRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<EndowmentResponse<List<string>>> Handle(GetAvailableRolesQuery request, CancellationToken cancellationToken)
    {
        // نمط خالد: جلب الأدوار من قاعدة البيانات
        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        var roleNames = roles.Select(r => r.Name).ToList();

        return Response.FilterResponse<List<string>>(roleNames);
    }
}
```

#### UserApprovalController.cs
```csharp
/// <summary>
/// الحصول على الأدوار المتاحة
/// نمط خالد: Query + Handler
/// </summary>
[HttpGet("available-roles")]
public async Task<ActionResult<EndowmentResponse<List<string>>>> GetAvailableRoles()
{
    var query = new GetAvailableRolesQuery();
    var availableRoles = await _mediator.Send(query);
    
    return Ok(new EndowmentResponse<List<string>>(
        data: availableRoles,
        state: ResponseState.Valid,
        message: $"تم جلب {availableRoles.Count} دور متاح بنجاح"
    ));
}
```

### 3. Validator محدث للتحقق من قاعدة البيانات

#### ApproveUserCommandValidator.cs
```csharp
public class ApproveUserCommandValidator : BaseValidator<ApproveUserCommand>
{
    private readonly IApplicationRoleRepository _roleRepository;

    public ApproveUserCommandValidator(IApplicationRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("اسم الدور مطلوب")
            .MustAsync(BeValidRoleAsync)
            .WithMessage("اسم الدور غير صحيح. يرجى اختيار دور صحيح من القائمة.");
    }

    private async Task<bool> BeValidRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return roles.Any(r => string.Equals(r.Name, roleName, StringComparison.OrdinalIgnoreCase));
    }
}
```

## كيفية استخدام الحل الجديد

### 1. في الفرونت إند

#### جلب الأدوار المتاحة
```javascript
// جلب الأدوار المتاحة من الباك إند
const fetchAvailableRoles = async () => {
  try {
    const response = await api('/user-approvals/available-roles', {
      method: 'GET'
    })
    
    if (response.isSuccess) {
      availableRoles.value = response.data || []
    }
  } catch (error) {
    console.error('خطأ في جلب الأدوار المتاحة:', error)
  }
}
```

#### عرض الأدوار ديناميكياً
```vue
<template>
  <!-- عرض الأدوار ديناميكياً -->
  <VMenu activator="parent">
    <VList>
      <VListItem 
        v-for="role in availableRoles" 
        :key="role"
        @click="approveUser(item.id, role)"
      >
        <template #prepend>
          <VIcon :icon="getRoleIcon(role)" />
        </template>
        <VListItemTitle>{{ getRoleDisplayName(role) }}</VListItemTitle>
      </VListItem>
    </VList>
  </VMenu>
</template>

<script setup>
const availableRoles = ref([])

// جلب الأدوار عند تحميل الصفحة
onMounted(async () => {
  await fetchAvailableRoles()
})

// دالة للحصول على أيقونة الدور
const getRoleIcon = (role) => {
  switch (role.toLowerCase()) {
    case 'admin': return 'tabler-crown'
    case 'employee': return 'tabler-user'
    case 'customer': return 'tabler-user-check'
    default: return 'tabler-user'
  }
}

// دالة للحصول على اسم العرض للدور
const getRoleDisplayName = (role) => {
  switch (role.toLowerCase()) {
    case 'admin': return 'مدير'
    case 'employee': return 'موظف'
    case 'customer': return 'عميل'
    default: return role
  }
}
</script>
```

### 2. إضافة دور جديد

#### في قاعدة البيانات
```sql
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES (NEWID(), 'Manager', 'MANAGER', NEWID())
```

#### في الفرونت إند
```javascript
// إضافة أيقونة واسم عرض للدور الجديد
const getRoleIcon = (role) => {
  switch (role.toLowerCase()) {
    case 'admin': return 'tabler-crown'
    case 'employee': return 'tabler-user'
    case 'customer': return 'tabler-user-check'
    case 'manager': return 'tabler-briefcase' // جديد
    default: return 'tabler-user'
  }
}

const getRoleDisplayName = (role) => {
  switch (role.toLowerCase()) {
    case 'admin': return 'مدير'
    case 'employee': return 'موظف'
    case 'customer': return 'عميل'
    case 'manager': return 'مدير قسم' // جديد
    default: return role
  }
}
```

## المزايا

### 1. **ديناميكية كاملة**
- الأدوار تُجلب من قاعدة البيانات
- لا حاجة لتحديث الكود عند إضافة أدوار جديدة

### 2. **أمان محسن**
- التحقق يتم في الباك إند من قاعدة البيانات
- لا يمكن تجاوز التحقق

### 3. **مرونة عالية**
- يمكن إضافة أدوار جديدة من قاعدة البيانات
- يمكن تعديل أسماء الأدوار من قاعدة البيانات

### 4. **تناسق البيانات**
- مصدر واحد للحقيقة (قاعدة البيانات)
- لا توجد تناقضات بين الفرونت إند والباك إند

### 5. **سهولة الصيانة**
- تغيير واحد في قاعدة البيانات يطبق على جميع الأماكن
- لا حاجة لتحديث كود متعدد

## الخلاصة

الحل الجديد يجعل النظام **ديناميكياً بالكامل**:

1. **الأدوار تُجلب من قاعدة البيانات** بدلاً من التحديد الثابت
2. **التحقق يتم من قاعدة البيانات** في الـ Validator
3. **الفرونت إند يعرض الأدوار ديناميكياً** من API
4. **إضافة أدوار جديدة** تتطلب فقط إدخال في قاعدة البيانات

هذا الحل يتبع **نمط خالد** ويضمن **الأمان** و**المرونة** و**سهولة الصيانة**. 🎯
