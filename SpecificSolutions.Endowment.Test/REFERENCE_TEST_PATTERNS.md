# Reference Test Patterns Guide

This document serves as a comprehensive reference for writing tests in the SpecificSolutions.Endowment project, using `CreateMosqueHandlerTests` as the primary example.

## Table of Contents

1. [Test Structure Overview](#test-structure-overview)
2. [Base Test Class](#base-test-class)
3. [Handler Test Patterns](#handler-test-patterns)
4. [Faker Patterns](#faker-patterns)
5. [Assertion Patterns](#assertion-patterns)
6. [Test Categories](#test-categories)
7. [Best Practices](#best-practices)

## Test Structure Overview

The test project follows a clean architecture pattern with the following structure:

```
SpecificSolutions.Endowment.Test/
├── BaseTest.cs                    # Base test class with common setup
├── Fakers/                        # Test data generators
│   └── Mosques/
│       └── CreateMosqueCommandFaker.cs
├── Asserts/                       # Custom assertion helpers
│   └── Mosques/
│       └── MosqueAssert.cs
├── Mosques/                       # Handler tests
│   └── CreateMosqueHandlerTests.cs
└── Helper/                        # Test utilities
```

## Base Test Class

The `BaseTest` class provides common functionality for all tests:

```csharp
public abstract class BaseTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected HandlerHelper _handlerHelper;
    protected const string user_id = "a2d890d8-01d1-494b-9f62-6336b937e6fc";
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly Mock<ICurrentUser> _currentUserMock = new();
    
    // Common setup and utilities
}
```

### Key Features:
- **WebApplicationFactory**: Provides in-memory test server
- **HandlerHelper**: Simplifies handler execution
- **CurrentUser Mock**: Simulates authenticated user context
- **Query Method**: Executes database queries in test scope

## Handler Test Patterns

### 1. Basic Success Test

```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccessResponse()
{
    // Arrange
    var createMosqueCommandFaker = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    var result = await _handlerHelper.Handle(createMosqueCommandFaker, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal("تم الإضافة بنجاح", result.Message);
}
```

### 2. Entity Creation Test

```csharp
[Fact]
public async Task Handle_ValidCommand_CreatesBuildingAndMosque()
{
    // Arrange
    var createMosqueCommandFaker = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    await _handlerHelper.Handle(createMosqueCommandFaker, CancellationToken.None);

    // Assert
    var building = await Query(b => b.Buildings.FirstOrDefaultAsync());
    var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());

    Assert.NotNull(building);
    Assert.NotNull(mosque);
    Assert.Equal(building.Id, mosque.BuildingId);
}
```

### 3. Property Validation Test

```csharp
[Fact]
public async Task Handle_ValidCommand_SetsCorrectUserId()
{
    // Arrange
    var createMosqueCommandFaker = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .Generate();

    // Act
    await _handlerHelper.Handle(createMosqueCommandFaker, CancellationToken.None);

    // Assert
    var building = await Query(b => b.Buildings.FirstOrDefaultAsync());
    
    Assert.NotNull(building);
    Assert.Equal(user_id, building.UserId);
}
```

### 4. Error Handling Test

```csharp
[Theory]
[InlineData("")]
[InlineData(null)]
public async Task Handle_InvalidUserId_ReturnsFailureResponse(string invalidUserId)
{
    // Arrange
    var createMosqueCommandFaker = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, invalidUserId)
        .Generate();

    // Act
    var result = await _handlerHelper.Handle(createMosqueCommandFaker, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains("User context is unavailable", result.Message);
}
```

## Faker Patterns

### 1. Basic Faker Structure

```csharp
public sealed class CreateMosqueCommandFaker : ParameterlessObjectFaker<CreateMosqueCommand>
{
    public CreateMosqueCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Company.CompanyName());
        RuleFor(x => x.RegionId, f => f.Random.Guid().ToString());
        RuleFor(x => x.OfficeId, f => f.Random.Guid().ToString());
        RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
    }
}
```

### 2. Custom Data Generation

```csharp
[Fact]
public async Task Handle_ValidCommand_WithAllOptionalFields()
{
    // Arrange
    var createMosqueCommandFaker = new CreateMosqueCommandFaker()
        .RuleFor(x => x.UserId, user_id)
        .RuleFor(x => x.Definition, "مسجد كبير في وسط المدينة")
        .RuleFor(x => x.Classification, "مسجد جامع")
        .RuleFor(x => x.Unit, "وحدة إدارية")
        .Generate();

    // Act & Assert
    // ...
}
```

## Assertion Patterns

### 1. Comprehensive Assertion Class

```csharp
public static class MosqueAssert
{
    public static void AssertEquality(Mosque mosque, CreateMosqueCommand command)
    {
        Assert.NotNull(mosque);
        Assert.NotNull(command);
        Assert.NotNull(mosque.Building);
        
        // Assert Mosque specific properties
        Assert.Equal(command.MosqueDefinition, mosque.MosqueDefinition);
        Assert.Equal(command.MosqueClassification, mosque.MosqueClassification);
        
        // Assert Building properties
        Assert.Equal(command.Name, mosque.Building.Name);
        Assert.Equal(command.RegionId, mosque.Building.RegionId);
        // ... more assertions
    }
}
```

### 2. Focused Assertion Methods

```csharp
public static void AssertMosqueProperties(Mosque mosque, CreateMosqueCommand command)
{
    Assert.NotNull(mosque);
    Assert.Equal(command.MosqueDefinition, mosque.MosqueDefinition);
    Assert.Equal(command.MosqueClassification, mosque.MosqueClassification);
    Assert.NotEqual(Guid.Empty, mosque.Id);
}

public static void AssertBuildingProperties(Mosque mosque, CreateMosqueCommand command)
{
    Assert.NotNull(mosque.Building);
    Assert.Equal(command.Name, mosque.Building.Name);
    Assert.Equal(command.RegionId, mosque.Building.RegionId);
    Assert.NotEqual(Guid.Empty, mosque.Building.Id);
}
```

## Test Categories

### 1. Success Tests
- Valid command processing
- Entity creation
- Property mapping
- Response validation

### 2. Validation Tests
- Required field validation
- Business rule validation
- Error message verification

### 3. Integration Tests
- Database persistence
- Transaction completion
- Relationship integrity

### 4. Edge Case Tests
- Boundary values
- Null/empty inputs
- Invalid data scenarios

## Best Practices

### 1. Test Naming Convention
```
Handle_[Scenario]_[ExpectedResult]
```
Examples:
- `Handle_ValidCommand_ReturnsSuccessResponse`
- `Handle_InvalidUserId_ReturnsFailureResponse`
- `Handle_ValidCommand_CreatesBuildingAndMosque`

### 2. Arrange-Act-Assert Pattern
```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccessResponse()
{
    // Arrange - Set up test data and dependencies
    var command = new CreateMosqueCommandFaker().Generate();

    // Act - Execute the method under test
    var result = await _handlerHelper.Handle(command, CancellationToken.None);

    // Assert - Verify the results
    Assert.True(result.IsSuccess);
}
```

### 3. Database Query Pattern
```csharp
// Use the Query method for database operations
var mosque = await Query(m => m.Mosques
    .Include(m => m.Building)
    .FirstOrDefaultAsync());
```

### 4. Faker Usage
```csharp
// Always set required fields
var command = new CreateMosqueCommandFaker()
    .RuleFor(x => x.UserId, user_id)
    .Generate();

// Override specific fields for test scenarios
var command = new CreateMosqueCommandFaker()
    .RuleFor(x => x.UserId, user_id)
    .RuleFor(x => x.Name, "Test Mosque")
    .Generate();
```

### 5. Assertion Organization
```csharp
// Use custom assertion classes for complex validations
MosqueAssert.AssertEquality(mosque, command);

// Use focused assertion methods for specific validations
MosqueAssert.AssertMosqueProperties(mosque, command);
MosqueAssert.AssertBuildingProperties(mosque, command);
```

## Common Test Scenarios

### 1. Create Handler Tests
- Valid command success
- Entity creation verification
- Property mapping validation
- Error handling scenarios

### 2. Update Handler Tests
- Valid update success
- Entity not found scenarios
- Property update verification
- Audit trail validation

### 3. Delete Handler Tests
- Valid deletion success
- Entity not found scenarios
- Cascade deletion verification
- Soft delete validation

### 4. Query Handler Tests
- Valid query success
- Filtering scenarios
- Pagination validation
- Empty result handling

## Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~CreateMosqueHandlerTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~Handle_ValidCommand_ReturnsSuccessResponse"
```

## Troubleshooting

### Common Issues:
1. **Database Context**: Ensure proper test database setup
2. **User Context**: Verify current user mock configuration
3. **Dependencies**: Check service registration in test container
4. **Async/Await**: Ensure proper async test method signatures

### Debug Tips:
1. Use `Query` method to inspect database state
2. Check response messages for error details
3. Verify faker data generation
4. Validate entity relationships

This reference guide should help maintain consistency across all test implementations in the project.
