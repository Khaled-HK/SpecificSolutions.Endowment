# النمط الصحيح للاستجابات في Controller - نمط خالد

## ✅ **النمط الصحيح من كونترولر المساجد**

```csharp
// ✅ النمط الصحيح - من MosqueController.cs
[HttpPost]
public async Task<EndowmentResponse> Create(CreateMosqueCommand command, CancellationToken cancellationToken)
    => await _mediator.Send(command, cancellationToken);

[HttpGet("{id}")]
public async Task<EndowmentResponse> GetMosqueById(Guid id, CancellationToken cancellationToken)
    => await _mediator.Send(new GetMosqueQuery(id), cancellationToken);

[HttpGet("filter")]
public async Task<EndowmentResponse<PagedList<MosqueDTO>>> Filter([FromQuery] FilterMosqueQuery query, CancellationToken cancellationToken)
    => await _mediator.Send(query, cancellationToken);
```

## ❌ **النمط الخاطئ (قبل التصحيح)**

```csharp
// ❌ خطأ في التصميم
[HttpGet("pending")]
public async Task<ActionResult<EndowmentResponse<List<PendingUserDto>>>> GetPendingUsers(
    [FromQuery] int page = 1,
    [FromQuery] int itemsPerPage = 10)
{
    var query = new GetPendingUsersQuery(page, itemsPerPage);
    var response = await _mediator.Send(query);
    
    return Ok(response); // ❌ لا حاجة لـ Ok()
}
```

## ✅ **النمط الصحيح (بعد التصحيح)**

```csharp
// ✅ التصميم الصحيح - نمط خالد
[HttpGet("pending")]
public async Task<EndowmentResponse<List<PendingUserDto>>> GetPendingUsers(
    [FromQuery] int page = 1,
    [FromQuery] int itemsPerPage = 10,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);
```

## ✅ **نمط خالد للاستجابات**

### **1. Queries (استعلامات)**
```csharp
// ✅ نمط خالد للـ Queries
[HttpGet("pending")]
public async Task<EndowmentResponse<List<PendingUserDto>>> GetPendingUsers(
    [FromQuery] int page = 1,
    [FromQuery] int itemsPerPage = 10,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

[HttpGet("available-roles")]
public async Task<EndowmentResponse<List<string>>> GetAvailableRoles(CancellationToken cancellationToken = default)
    => await _mediator.Send(new GetAvailableRolesQuery(), cancellationToken);
```

### **2. Commands (أوامر)**
```csharp
// ✅ نمط خالد للـ Commands
[HttpPost("approve/{userId}")]
public async Task<EndowmentResponse> ApproveUser(
    string userId,
    [FromBody] ApproveUserRequest request,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new ApproveUserCommand(userId, request.RoleName ?? "Employee"), cancellationToken);

[HttpDelete("reject/{userId}")]
public async Task<EndowmentResponse> RejectUser(string userId, CancellationToken cancellationToken = default)
    => await _mediator.Send(new RejectUserCommand(userId), cancellationToken);
```

## ✅ **المزايا**

### **1. بساطة الكود**
```csharp
// ✅ بسيط ومباشر
=> await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

// ❌ معقد وغير ضروري
var query = new GetPendingUsersQuery(page, itemsPerPage);
var response = await _mediator.Send(query);
return Ok(response);
```

### **2. تناسق في التصميم**
```csharp
// جميع الـ endpoints تتبع نفس النمط
public async Task<EndowmentResponse<T>> MethodName(parameters, CancellationToken cancellationToken = default)
    => await _mediator.Send(new QueryOrCommand(parameters), cancellationToken);
```

### **3. عدم فقدان البيانات**
```csharp
// الـ Handler يتحكم في الرسالة والحالة
return Response.FilterResponse<List<PendingUserDto>>(pendingUsers);
// Controller يعيدها مباشرة
=> await _mediator.Send(query, cancellationToken);
```

### **4. سهولة الصيانة**
```csharp
// تغيير واحد في الـ Handler يطبق على جميع الأماكن
// لا حاجة لتحديث Controller عند تغيير الرسالة
```

## ✅ **مقارنة النمطين**

| النمط | المزايا | العيوب |
|-------|---------|--------|
| **❌ ActionResult + Ok()** | - مرونة في HTTP status codes | - تكرار الكود<br>- تعقيد غير ضروري<br>- عدم التناسق |
| **✅ EndowmentResponse مباشرة** | - بساطة<br>- تناسق<br>- عدم فقدان البيانات<br>- سهولة الصيانة | - أقل مرونة في HTTP status codes |

## ✅ **أمثلة من كونترولرات أخرى**

### **MosqueController.cs**
```csharp
[HttpPost]
public async Task<EndowmentResponse> Create(CreateMosqueCommand command, CancellationToken cancellationToken)
    => await _mediator.Send(command, cancellationToken);

[HttpGet("{id}")]
public async Task<EndowmentResponse> GetMosqueById(Guid id, CancellationToken cancellationToken)
    => await _mediator.Send(new GetMosqueQuery(id), cancellationToken);
```

### **BuildingDetailController.cs**
```csharp
[HttpPost]
public async Task<EndowmentResponse> Create(CreateBuildingDetailCommand command, CancellationToken cancellationToken)
    => await _mediator.Send(command, cancellationToken);

[HttpGet("filter")]
public async Task<EndowmentResponse<PagedList<BuildingDetailDTO>>> Filter([FromQuery] FilterBuildingDetailQuery query, CancellationToken cancellationToken)
    => await _mediator.Send(query, cancellationToken);
```

## ✅ **الخلاصة**

### **النمط الصحيح:**
1. **Return Type**: `EndowmentResponse<T>` مباشرة (بدون `ActionResult`)
2. **Method Body**: `=> await _mediator.Send(query/command, cancellationToken)`
3. **Parameters**: إضافة `CancellationToken cancellationToken = default`
4. **تناسق**: جميع الـ endpoints تتبع نفس النمط

### **نمط خالد:**
- **Queries**: `public async Task<EndowmentResponse<T>> MethodName(parameters, CancellationToken cancellationToken = default) => await _mediator.Send(new Query(parameters), cancellationToken);`
- **Commands**: `public async Task<EndowmentResponse> MethodName(parameters, CancellationToken cancellationToken = default) => await _mediator.Send(new Command(parameters), cancellationToken);`

هذا النمط يضمن **البساطة** و**التناسق** و**سهولة الصيانة**! 🎯
