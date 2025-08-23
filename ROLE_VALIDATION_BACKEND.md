# التحقق من الأدوار في الباك إند - نمط خالد

## الموقع الصحيح للتحقق من الأدوار

التحقق من الأدوار موجود في **الباك إند** وليس في الفرونت إند، وهذا هو النمط الصحيح حسب **نمط خالد**.

## الملفات المسؤولة عن التحقق من الأدوار

### 1. ApproveUserCommandValidator.cs
```csharp
// SpecificSolutions.Endowment.Application/Validators/UserApprovals/ApproveUserCommandValidator.cs

public class ApproveUserCommandValidator : AbstractValidator<ApproveUserCommand>
{
    public ApproveUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("معرف المستخدم مطلوب")
            .Must(BeValidGuid)
            .WithMessage("معرف المستخدم غير صحيح");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("اسم الدور مطلوب")
            .Must(BeValidRole)
            .WithMessage("اسم الدور غير صحيح. الأدوار المسموحة: Admin, Employee, Customer");
    }

    private static bool BeValidRole(string roleName)
    {
        var validRoles = new[] { "Admin", "Employee", "Customer" };
        return validRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
    }
}
```

### 2. ValidationContainer.cs
```csharp
// SpecificSolutions.Endowment.Application/Validators/ValidationContainer.cs

// User Approval Validators - نمط خالد
services.AddTransient<IValidator<ApproveUserCommand>, ApproveUserCommandValidator>();
services.AddTransient<IValidator<RejectUserCommand>, RejectUserCommandValidator>();
```

### 3. ValidationPipelineBehavior.cs
```csharp
// SpecificSolutions.Endowment.Application/Abstractions/Behaviors/ValidationPipelineBehavior.cs

// تنفيذ الـ validation
var context = new ValidationContext<TRequest>(request);
var validationResults = await Task.WhenAll(
    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

var failures = validationResults
    .Where(r => r.Errors.Any())
    .SelectMany(r => r.Errors)
    .ToList();

if (failures.Any())
{
    var validationException = new ValidationException(failures);
    throw validationException;
}
```

### 4. GlobalExceptionHandler.cs
```csharp
// SpecificSolutions.Endowment.Application/Abstractions/Exceptions/GlobalExceptionHandler.cs

private async Task HandleValidationException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
{
    var exception = (ValidationException)ex;
    
    // تحويل Dictionary<string, string[]> إلى Errors
    var errors = exception.Errors
        .SelectMany(kvp => kvp.Value.Select(errorMessage => new Error(kvp.Key, errorMessage)))
        .ToArray();

    var response = new EndowmentResponse(state: ResponseState.BadRequest, "Validation failed", errors);
    
    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    await httpContext.Response.WriteAsJsonAsync<EndowmentResponse>(response, cancellationToken);
}
```

## الأدوار المسموحة

الأدوار المسموحة محددة في الباك إند:

1. **Admin** - مدير النظام
2. **Employee** - موظف
3. **Customer** - عميل

## كيفية عمل التحقق

### 1. عند إرسال طلب الموافقة على مستخدم
```http
POST /api/user-approvals/approve/{userId}
Content-Type: application/json

{
  "roleName": "Employee"
}
```

### 2. ValidationPipelineBehavior يتحقق من البيانات
- يتحقق من أن `userId` صحيح (GUID)
- يتحقق من أن `roleName` غير فارغ
- يتحقق من أن `roleName` من الأدوار المسموحة

### 3. في حالة وجود أخطاء
```json
{
  "errors": [
    {
      "propertyName": "RoleName",
      "errorMessage": "اسم الدور غير صحيح. الأدوار المسموحة: Admin, Employee, Customer"
    }
  ],
  "message": "Validation failed",
  "isSuccess": false,
  "state": 400
}
```

### 4. في حالة النجاح
```json
{
  "message": "تم قبول المستخدم ومنحه الصلاحيات بنجاح",
  "isSuccess": true,
  "state": 1
}
```

## الاختبارات

تم إنشاء اختبارات شاملة للتحقق من الأدوار:

```csharp
// SpecificSolutions.Endowment.Test/UserApprovals/ApproveUserCommandValidatorTests.cs

[Test]
public void Validate_WithValidAdminRole_ShouldPass()
{
    var command = new ApproveUserCommand(
        userId: Guid.NewGuid().ToString(),
        roleName: "Admin"
    );
    
    var result = _validator.TestValidate(command);
    result.ShouldNotHaveAnyValidationErrors();
}

[Test]
public void Validate_WithInvalidRole_ShouldFail()
{
    var command = new ApproveUserCommand(
        userId: Guid.NewGuid().ToString(),
        roleName: "InvalidRole"
    );
    
    var result = _validator.TestValidate(command);
    result.ShouldHaveValidationErrorFor(x => x.RoleName);
}
```

## الفرونت إند

في الفرونت إند، يتم عرض قائمة منسدلة بالأدوار المسموحة، لكن التحقق النهائي يتم في الباك إند:

```vue
<!-- specificsolutions.endowment.vue/src/pages/apps/user-approvals.vue -->

<VListItem @click="approveUser(item.id, 'Admin')">
  <VListItemTitle>{{ t('userApprovals.approveAsAdmin') }}</VListItemTitle>
</VListItem>

<VListItem @click="approveUser(item.id, 'Employee')">
  <VListItemTitle>{{ t('userApprovals.approveAsEmployee') }}</VListItemTitle>
</VListItem>

<VListItem @click="approveUser(item.id, 'Customer')">
  <VListItemTitle>{{ t('userApprovals.approveAsCustomer') }}</VListItemTitle>
</VListItem>
```

## المزايا

1. **الأمان**: التحقق في الباك إند يضمن عدم إمكانية تجاوز التحقق
2. **المرونة**: يمكن تغيير الأدوار المسموحة من مكان واحد
3. **التناسق**: جميع الطلبات تمر بنفس التحقق
4. **الاختبار**: يمكن اختبار التحقق بشكل منفصل
5. **التوثيق**: الأخطاء واضحة ومحددة

## الخلاصة

التحقق من الأدوار موجود في الباك إند في الـ Validator، وهذا هو النمط الصحيح حسب **نمط خالد**. الفرونت إند يعرض فقط واجهة مستخدم سهلة الاستخدام، لكن التحقق النهائي والأمان يتم في الباك إند.
