# دليل الاختبارات الشامل - SpecificSolutions.Endowment

هذا الدليل يغطي جميع أنواع الاختبارات الموجودة في المشروع مع أمثلة عملية لكل نوع.

## 📋 جدول المحتويات

1. [أنواع الاختبارات](#أنواع-الاختبارات)
2. [اختبارات الإنشاء (Create Tests)](#اختبارات-الإنشاء-create-tests)
3. [اختبارات الاستعلام (Query Tests)](#اختبارات-الاستعلام-query-tests)
4. [اختبارات التحقق (Validator Tests)](#اختبارات-التحقق-validator-tests)
5. [اختبارات الموافقة (Approval Tests)](#اختبارات-الموافقة-approval-tests)
6. [أنماط Faker](#أنماط-faker)
7. [أنماط Assertion](#أنماط-assertion)
8. [أفضل الممارسات](#أفضل-الممارسات)

## 🎯 أنواع الاختبارات

### 1. اختبارات الإنشاء (Create Handler Tests)
- اختبار إنشاء الكيانات الجديدة
- مثال: `CreateMosqueHandlerTests`, `CreateAccountHandlerTests`

### 2. اختبارات الاستعلام (Query Handler Tests)
- اختبار استرجاع البيانات مع الفلترة والترقيم
- مثال: `GetPendingUsersHandlerTests`

### 3. اختبارات التحقق (Validator Tests)
- اختبار قواعد التحقق من صحة البيانات
- مثال: `ApproveUserCommandValidatorTests`

### 4. اختبارات الموافقة (Approval Tests)
- اختبار عمليات الموافقة والرفض
- مثال: `GetPendingUsersCountHandlerTests`

## 🏗️ اختبارات الإنشاء (Create Tests)

### النمط الأساسي

```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccessResponse()
{
    // Arrange
    var command = new CreateEntityCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    var result = await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal("تم الإضافة بنجاح", result.Message);
}
```

### اختبار إنشاء الكيان

```csharp
[Fact]
public async Task Handle_ValidCommand_CreatesEntity()
{
    // Arrange
    var command = new CreateEntityCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert
    var entity = await Query(e => e.Entities.FirstOrDefaultAsync());
    Assert.NotNull(entity);
    EntityAssert.AssertEquality(entity, command);
}
```

### اختبار العلاقات بين الكيانات

```csharp
[Fact]
public async Task Handle_ValidCommand_CreatesRelatedEntities()
{
    // Arrange
    var command = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert
    var building = await Query(b => b.Buildings.FirstOrDefaultAsync());
    var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());

    Assert.NotNull(building);
    Assert.NotNull(mosque);
    Assert.Equal(building.Id, mosque.BuildingId);
}
```

## 🔍 اختبارات الاستعلام (Query Tests)

### النمط الأساسي للاستعلامات

```csharp
[Test]
public async Task Handle_WithPagination_ShouldReturnCorrectPage()
{
    // Arrange
    await CreateTestData(10); // إنشاء بيانات اختبار

    var query = new GetEntitiesQuery(pageNumber: 2, pageSize: 3);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data.Count, Is.EqualTo(3));
}
```

### اختبار النتائج الفارغة

```csharp
[Test]
public async Task Handle_WithEmptyResult_ShouldReturnEmptyList()
{
    // Arrange
    var query = new GetEntitiesQuery(pageNumber: 1, pageSize: 10);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data, Is.Empty);
}
```

### اختبار فلترة البيانات

```csharp
[Test]
public async Task Handle_ShouldFilterDataCorrectly()
{
    // Arrange
    await CreatePendingUsers(3);
    await CreateApprovedUsers(2);

    var query = new GetPendingUsersQuery(pageNumber: 1, pageSize: 10);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data.Count, Is.EqualTo(3)); // المستخدمين المعلقين فقط
}
```

### اختبار البيانات المُرجعة

```csharp
[Test]
public async Task Handle_ShouldReturnCorrectData()
{
    // Arrange
    var email = "test.user@gmail.com";
    var firstName = "Test";
    var lastName = "User";

    await CreateTestUser(firstName, lastName, email);

    var query = new GetUsersQuery(pageNumber: 1, pageSize: 10);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data.Count, Is.EqualTo(1));

    var user = result.Data.First();
    Assert.That(user.Email, Is.EqualTo(email));
    Assert.That(user.FirstName, Is.EqualTo(firstName));
    Assert.That(user.LastName, Is.EqualTo(lastName));
}
```

## ✅ اختبارات التحقق (Validator Tests)

### النمط الأساسي للتحقق

```csharp
[Test]
public void Validate_WithValidData_ShouldPass()
{
    // Arrange
    var command = new CreateEntityCommand(
        name: "Test Entity",
        userId: Guid.NewGuid().ToString()
    );

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
}
```

### اختبار الحقول المطلوبة

```csharp
[Test]
public void Validate_WithEmptyRequiredField_ShouldFail()
{
    // Arrange
    var command = new CreateEntityCommand(
        name: "",
        userId: Guid.NewGuid().ToString()
    );

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Name);
    result.ShouldNotHaveValidationErrorFor(x => x.UserId);
}
```

### اختبار تنسيق البيانات

```csharp
[Test]
public void Validate_WithInvalidFormat_ShouldFail()
{
    // Arrange
    var command = new ApproveUserCommand(
        userId: "invalid-guid",
        roleName: "Employee"
    );

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.UserId);
    result.ShouldNotHaveValidationErrorFor(x => x.RoleName);
}
```

### اختبار القيم الفارغة

```csharp
[Test]
public void Validate_WithNullValue_ShouldFail()
{
    // Arrange
    var command = new CreateEntityCommand(
        name: null!,
        userId: Guid.NewGuid().ToString()
    );

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Name);
}
```

## 🔐 اختبارات الموافقة (Approval Tests)

### اختبار عمليات الموافقة

```csharp
[Test]
public async Task Handle_ValidApproval_ShouldSucceed()
{
    // Arrange
    var userId = Guid.NewGuid().ToString();
    var roleName = "Employee";
    
    await CreatePendingUser(userId);

    var command = new ApproveUserCommand(userId, roleName);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    
    var user = await GetUserById(userId);
    Assert.That(user.IsApproved, Is.True);
    Assert.That(user.RoleName, Is.EqualTo(roleName));
}
```

### اختبار العد والإحصائيات

```csharp
[Test]
public async Task Handle_ShouldReturnCorrectCount()
{
    // Arrange
    await CreatePendingUsers(5);
    await CreateApprovedUsers(3);

    var query = new GetPendingUsersCountQuery();

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data, Is.EqualTo(5));
}
```

## 🎲 أنماط Faker

### Faker أساسي

```csharp
public sealed class CreateEntityCommandFaker : ParameterlessObjectFaker<CreateEntityCommand>
{
    public CreateEntityCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Company.CompanyName());
        RuleFor(x => x.Description, f => f.Lorem.Sentence());
        RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
    }
}
```

### Faker مع بيانات مخصصة

```csharp
public sealed class CreateAccountCommandFaker : ParameterlessObjectFaker<CreateAccountCommand>
{
    public CreateAccountCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.MotherName, f => f.Person.FullName);
        RuleFor(x => x.BirthDate, f => f.Date.Past(30));
        RuleFor(x => x.AccountNumber, f => f.Finance.Account());
        RuleFor(x => x.Balance, f => f.Finance.Amount(0, 100000));
        RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
    }
}
```

### استخدام Faker في الاختبارات

```csharp
// استخدام Faker أساسي
var command = new CreateEntityCommandFaker().Generate();

// تخصيص حقول معينة
var command = new CreateEntityCommandFaker()
    .RuleFor(x => x.Name, "Test Entity")
    .RuleFor(x => x.Description, "Custom Description")
    .Generate();

// تعيين UserId المطلوب
var command = new CreateEntityCommandFaker()
    .RuleFor(x => x.UserId, user_id)
    .Generate();
```

## ✅ أنماط Assertion

### Assertion شامل

```csharp
public static void AssertEquality(Entity entity, CreateEntityCommand command)
{
    Assert.NotNull(entity);
    Assert.NotNull(command);
    
    Assert.Equal(command.Name, entity.Name);
    Assert.Equal(command.Description, entity.Description);
    Assert.Equal(command.UserId, entity.UserId);
    Assert.NotEqual(Guid.Empty, entity.Id);
}
```

### Assertion للكيانات المركبة

```csharp
public static void AssertEquality(Mosque mosque, CreateMosqueCommand command)
{
    Assert.NotNull(mosque);
    Assert.NotNull(mosque.Building);
    
    // Assert Mosque properties
    Assert.Equal(command.MosqueDefinition, mosque.MosqueDefinition);
    Assert.Equal(command.MosqueClassification, mosque.MosqueClassification);
    
    // Assert Building properties
    Assert.Equal(command.Name, mosque.Building.Name);
    Assert.Equal(command.RegionId, mosque.Building.RegionId);
    Assert.Equal(command.UserId, mosque.Building.UserId);
}
```

### Assertion للعلاقات

```csharp
public static void AssertRelationship(Mosque mosque)
{
    Assert.NotNull(mosque);
    Assert.NotNull(mosque.Building);
    Assert.Equal(mosque.BuildingId, mosque.Building.Id);
}
```

## 📚 أفضل الممارسات

### 1. تسمية الاختبارات

```
Handle_[Scenario]_[ExpectedResult]
```

أمثلة:
- `Handle_ValidCommand_ReturnsSuccessResponse`
- `Handle_WithPagination_ShouldReturnCorrectPage`
- `Validate_WithEmptyRequiredField_ShouldFail`

### 2. نمط Arrange-Act-Assert

```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccessResponse()
{
    // Arrange - إعداد البيانات والتبعيات
    var command = new CreateEntityCommandFaker().Generate();

    // Act - تنفيذ العملية المختبرة
    var result = await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert - التحقق من النتائج
    Assert.True(result.IsSuccess);
}
```

### 3. استخدام Query Method

```csharp
// استخدام Query method للعمليات على قاعدة البيانات
var entity = await Query(e => e.Entities
    .Include(e => e.RelatedEntity)
    .FirstOrDefaultAsync());
```

### 4. إعداد البيانات المساعدة

```csharp
#region Helper Methods

private async Task CreateTestData(int count)
{
    for (int i = 1; i <= count; i++)
    {
        await CreateTestEntity($"Entity{i}");
    }
}

private async Task CreateTestEntity(string name)
{
    var entity = Entity.Create(name, user_id);
    Context.Entities.Add(entity);
    await Context.SaveChangesAsync();
}

#endregion
```

### 5. اختبار الحالات الحدية

```csharp
[Theory]
[InlineData("")]
[InlineData(null)]
[InlineData("invalid-guid")]
public async Task Handle_InvalidInput_ReturnsFailureResponse(string invalidInput)
{
    // Arrange
    var command = new CreateEntityCommandFaker()
        .RuleFor(x => x.UserId, invalidInput)
        .Generate();

    // Act
    var result = await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
}
```

## 🏃‍♂️ تشغيل الاختبارات

```bash
# تشغيل جميع الاختبارات
dotnet test

# تشغيل فئة اختبار محددة
dotnet test --filter "FullyQualifiedName~CreateMosqueHandlerTests"

# تشغيل طريقة اختبار محددة
dotnet test --filter "FullyQualifiedName~Handle_ValidCommand_ReturnsSuccessResponse"

# تشغيل اختبارات نوع محدد
dotnet test --filter "TestCategory=Create"
dotnet test --filter "TestCategory=Query"
dotnet test --filter "TestCategory=Validator"
```

## 🔧 استكشاف الأخطاء

### المشاكل الشائعة:

1. **مشاكل قاعدة البيانات**: تأكد من إعداد قاعدة بيانات الاختبار
2. **مشاكل المستخدم**: تحقق من تكوين mock المستخدم الحالي
3. **مشاكل التبعيات**: تحقق من تسجيل الخدمات في حاوية الاختبار
4. **مشاكل Async/Await**: تأكد من توقيعات الطرق الصحيحة

### نصائح التصحيح:

1. استخدم `Query` method لفحص حالة قاعدة البيانات
2. تحقق من رسائل الاستجابة للحصول على تفاصيل الأخطاء
3. تحقق من توليد بيانات Faker
4. تحقق من صحة العلاقات بين الكيانات

## 📝 ملاحظات مهمة

### نمط خالد للاستعلامات:
- تنفيذ الفلترة والتحويل إلى DTO على مستوى قاعدة البيانات
- استخدام `GetByFilterAsync` في Repository
- دعم الترقيم (Paging)
- استدعاء Handler مباشرة للـ Repository

### نمط خالد للأوامر:
- استخدام `Response.SuccessResponse` للنجاح
- استخدام `Response.FailureResponse` للفشل
- التحقق من صحة البيانات قبل المعالجة

هذا الدليل الشامل يجب أن يساعد في الحفاظ على اتساق جميع تنفيذات الاختبار في المشروع.
