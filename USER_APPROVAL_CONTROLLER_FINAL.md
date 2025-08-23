# كونترولر UserApproval - التصحيح النهائي

## ✅ **النمط الصحيح النهائي**

```csharp
/// <summary>
/// Controller لإدارة موافقات المستخدمين - للمسؤولين فقط
/// نمط خالد: CQRS + MediatR
/// </summary>
[Authorize]
[Route("api/user-approvals")]
public class UserApprovalController : ApiController
{
    private readonly IMediator _mediator;
    public UserApprovalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// الحصول على المستخدمين المعلقين (في انتظار الموافقة)
    /// نمط خالد: Query + Handler
    /// </summary>
    [HttpGet("pending")]
    public async Task<EndowmentResponse<List<PendingUserDto>>> GetPendingUsers(
        [FromQuery] int page = 1,
        [FromQuery] int itemsPerPage = 10,
        CancellationToken cancellationToken = default)
        => await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

    /// <summary>
    /// الموافقة على مستخدم ومنحه صلاحيات
    /// نمط خالد: Command + Handler
    /// </summary>
    [HttpPost("approve/{userId}")]
    public async Task<EndowmentResponse> ApproveUser(
        string userId,
        [FromBody] ApproveUserRequest request,
        CancellationToken cancellationToken = default)
        => await _mediator.Send(new ApproveUserCommand(userId, request.RoleName ?? "Employee"), cancellationToken);

    /// <summary>
    /// رفض مستخدم وحذف حسابه
    /// نمط خالد: Command + Handler
    /// </summary>
    [HttpDelete("reject/{userId}")]
    public async Task<EndowmentResponse> RejectUser(string userId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new RejectUserCommand(userId), cancellationToken);

    /// <summary>
    /// الحصول على الأدوار المتاحة
    /// نمط خالد: Query + Handler
    /// </summary>
    [HttpGet("available-roles")]
    public async Task<EndowmentResponse<List<string>>> GetAvailableRoles(CancellationToken cancellationToken = default)
        => await _mediator.Send(new GetAvailableRolesQuery(), cancellationToken);
}
```

## ✅ **المزايا المحققة**

### **1. تناسق مع باقي الكونترولرات**
```csharp
// ✅ نفس النمط في جميع الكونترولرات
public async Task<EndowmentResponse<T>> MethodName(parameters, CancellationToken cancellationToken = default)
    => await _mediator.Send(new QueryOrCommand(parameters), cancellationToken);
```

### **2. بساطة الكود**
```csharp
// ✅ سطر واحد لكل endpoint
=> await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

// ❌ قبل التصحيح (5 أسطر)
var query = new GetPendingUsersQuery(page, itemsPerPage);
var response = await _mediator.Send(query);
return Ok(response);
```

### **3. دعم CancellationToken**
```csharp
// ✅ جميع الـ endpoints تدعم الإلغاء
CancellationToken cancellationToken = default
```

### **4. عدم فقدان البيانات**
```csharp
// ✅ الـ Handler يتحكم في الاستجابة
return Response.FilterResponse<List<PendingUserDto>>(pendingUsers);

// ✅ Controller يعيدها مباشرة
=> await _mediator.Send(query, cancellationToken);
```

## ✅ **مقارنة مع كونترولرات أخرى**

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

### **UserApprovalController.cs** ✅
```csharp
[HttpGet("pending")]
public async Task<EndowmentResponse<List<PendingUserDto>>> GetPendingUsers(
    [FromQuery] int page = 1,
    [FromQuery] int itemsPerPage = 10,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

[HttpPost("approve/{userId}")]
public async Task<EndowmentResponse> ApproveUser(
    string userId,
    [FromBody] ApproveUserRequest request,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new ApproveUserCommand(userId, request.RoleName ?? "Employee"), cancellationToken);
```

## ✅ **نمط خالد النهائي**

### **Queries (استعلامات)**
```csharp
public async Task<EndowmentResponse<T>> MethodName(
    [FromQuery] parameters,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new Query(parameters), cancellationToken);
```

### **Commands (أوامر)**
```csharp
public async Task<EndowmentResponse> MethodName(
    parameters,
    [FromBody] Request request,
    CancellationToken cancellationToken = default)
    => await _mediator.Send(new Command(parameters), cancellationToken);
```

## ✅ **الخلاصة**

### **ما تم تصحيحه:**
1. ✅ **Return Type**: `EndowmentResponse<T>` مباشرة (بدون `ActionResult`)
2. ✅ **Method Body**: `=> await _mediator.Send(query/command, cancellationToken)`
3. ✅ **Parameters**: إضافة `CancellationToken cancellationToken = default`
4. ✅ **تناسق**: جميع الـ endpoints تتبع نفس النمط

### **المزايا المحققة:**
- **بساطة**: سطر واحد لكل endpoint
- **تناسق**: نفس النمط في جميع الكونترولرات
- **عدم فقدان البيانات**: الـ Handler يتحكم في الاستجابة
- **سهولة الصيانة**: تغيير واحد يطبق على جميع الأماكن
- **دعم الإلغاء**: جميع الـ endpoints تدعم `CancellationToken`

### **نمط خالد النهائي:**
```csharp
// Queries
public async Task<EndowmentResponse<T>> MethodName(parameters, CancellationToken cancellationToken = default)
    => await _mediator.Send(new Query(parameters), cancellationToken);

// Commands
public async Task<EndowmentResponse> MethodName(parameters, CancellationToken cancellationToken = default)
    => await _mediator.Send(new Command(parameters), cancellationToken);
```

الآن كونترولر UserApproval يتبع **نمط خالد** الصحيح مثل باقي الكونترولرات! 🎯
