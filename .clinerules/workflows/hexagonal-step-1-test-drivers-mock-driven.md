# Hexagonal Architecture Step 1: Test Driver Adapters / Mock Driven Adapters

You are implementing Step 1 of the hexagonal architecture development process: **TEST DRIVER ADAPTERS / MOCK DRIVEN ADAPTERS**.

**Goal**: Complete the hexagon (application + domain) with test-driven development, using mock adapters for driven ports.

**Prerequisites**: You have a target spec in `/arc/specs/` that defines the feature to implement.

<detailed_sequence_of_steps>

# Hexagonal Architecture Implementation - Step 1
## Test Driver Adapters / Mock Driven Adapters

## 1. Load Target Specification

1. **Read the target spec** to understand what to implement:
   ```xml
   <read_file>
   <path>arc/specs/[spec-id].yaml</path>
   </read_file>
   ```

2. **Extract key information** from the spec:
   - `objective`: What the feature should accomplish
   - `scope.domain`: Entities, value objects, use cases
   - `scope.ports.driving`: Driver port interfaces (use cases)
   - `scope.ports.driven`: Driven port interfaces (repositories, external services)
   - `scenarios`: BDD test scenarios (Given/When/Then)
   - `packs`: Referenced rule packs

## 2. Resolve Referenced Rule Packs

1. **Load hexagonal architecture rules**:
   ```xml
   <read_file>
   <path>arc/rules/packs/hexagonal.architecture.yaml</path>
   </read_file>
   ```

2. **Load testing rules**:
   ```xml
   <read_file>
   <path>arc/rules/packs/tests.unit.yaml</path>
   </read_file>
   ```

3. **Load AutoFixture testing rules**:
   ```xml
   <read_file>
   <path>arc/rules/packs/testing.unit.autofixture.yaml</path>
   </read_file>
   ```

4. **Load domain modeling rules** (if referenced in spec):
   ```xml
   <read_file>
   <path>arc/rules/packs/domain.modeling.yaml</path>
   </read_file>
   ```

## 3. Resolve Contracts (OpenAPI/JSON Schemas)

1. **Load OpenAPI contract** for driver ports:
   ```xml
   <read_file>
   <path>arc/contracts/openapi/[contract-id].yaml</path>
   </read_file>
   ```

2. **Load JSON schemas** for driven ports:
   ```xml
   <list_files>
   <path>arc/contracts/schemas</path>
   </list_files>
   ```

## 4. Implement Domain Layer

1. **Create domain entities** following the spec scope:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Domain/Entities/[Entity].cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Domain.Entities;

public class [Entity] : IEntity
{
    // Domain entity implementation
    // Business rules and invariants
}</content>
   </write_to_file>
   ```

2. **Create value objects**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Domain/ValueObjects/[ValueObject].cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Domain.ValueObjects;

public record [ValueObject]([parameters])
{
    // Value object with validation
}</content>
   </write_to_file>
   ```

3. **Define driven port interfaces**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Domain/Ports/I[PortName]Port.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Domain.Ports;

public interface I[PortName]Port
{
    // Port interface methods
}</content>
   </write_to_file>
   ```

## 5. Implement Application Layer

1. **Create command/query objects** for use cases:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Application/UseCases/[UseCase]/[UseCase]Command.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Application.UseCases.[UseCase];

public record [UseCase]Command([parameters]) : IRequest<[ResponseType]>;</content>
   </write_to_file>
   ```

2. **Create command handlers**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Application/UseCases/[UseCase]/[UseCase]CommandHandler.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Application.UseCases.[UseCase];

public class [UseCase]CommandHandler : IRequestHandler<[UseCase]Command, [ResponseType]>
{
    private readonly I[DrivenPort]Port _drivenPort;

    public [UseCase]CommandHandler(I[DrivenPort]Port drivenPort)
    {
        _drivenPort = drivenPort;
    }

    public async Task<[ResponseType]> Handle([UseCase]Command request, CancellationToken ct)
    {
        // Use case implementation
        // Orchestrate domain logic
        // Call driven ports
    }
}</content>
   </write_to_file>
   ```

3. **Create DTOs and mappers**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Application/DTOs/[DtoName]Dto.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Application.DTOs;

public record [DtoName]Dto([parameters]);</content>
   </write_to_file>
   ```

## 6. Create Test Project Structure

1. **Set up test project** with proper references:
   ```xml
   <execute_command>
   <command>dotnet new xunit -n AGS.WindowsAndDoors.[Context].Tests -o src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

2. **Add project references**:
   ```xml
   <execute_command>
   <command>dotnet add src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/AGS.WindowsAndDoors.[Context].Tests.csproj reference src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Domain/AGS.WindowsAndDoors.[Context].Domain.csproj src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Application/AGS.WindowsAndDoors.[Context].Application.csproj src/SharedKernel/AGS.WindowsAndDoors.SharedKernel.Testing/AGS.WindowsAndDoors.SharedKernel.Testing.csproj</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

## 7. Implement Test Driver Adapters

1. **Create domain entity tests**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/Domain/Entities/[Entity]Tests.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Tests.Domain.Entities;

public class [Entity]Tests
{
    [Fact]
    public void [TestScenario]_Should_[ExpectedBehavior]()
    {
        // Arrange
        var fixture = FixtureHelper.WithDefaultCustomizations();

        // Act

        // Assert
    }
}</content>
   </write_to_file>
   ```

2. **Create use case handler tests** mapping to spec scenarios:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/Application/UseCases/[UseCase]CommandHandlerTests.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Tests.Application.UseCases;

public class [UseCase]CommandHandlerTests
{
    private readonly Fixture _fixture = FixtureHelper.WithDefaultCustomizations();

    [Fact]
    public async Task [ScenarioName]_Should_[ExpectedBehavior]()
    {
        // Arrange
        var mockDrivenPort = _fixture.Create<I[DrivenPort]Port>();
        var handler = new [UseCase]CommandHandler(mockDrivenPort);

        // Act
        var result = await handler.Handle([command], CancellationToken.None);

        // Assert
        // Verify behavior and mock interactions
    }
}</content>
   </write_to_file>
   ```

## 8. Create Mock Driven Adapters

1. **Create mock repository implementations**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/TestUtilities/Mocks/Mock[RepositoryName].cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Tests.TestUtilities.Mocks;

public class Mock[RepositoryName] : I[RepositoryName]Port
{
    private readonly List<[Entity]> _data = new();

    public Task<[Entity]> SaveAsync([Entity] entity, CancellationToken ct = default)
    {
        _data.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<[Entity]?> FindByCodeAsync(string code, CancellationToken ct = default)
    {
        return Task.FromResult(_data.FirstOrDefault(x => x.Code == code));
    }

    // Other mock implementations...
}</content>
   </write_to_file>
   ```

2. **Create test data builders**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/TestUtilities/Builders/[Entity]TestDataBuilder.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Tests.TestUtilities.Builders;

public class [Entity]TestDataBuilder
{
    private string _code = "TEST001";
    private string _name = "Test Entity";

    public [Entity]TestDataBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public [Entity]TestDataBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public [Entity] Build()
    {
        return [Entity].Create(_code, _name, /* other params */);
    }
}</content>
   </write_to_file>
   ```

## 9. Configure Composition Root for Testing

1. **Create test startup configuration**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/TestStartup.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Tests;

public class TestStartup
{
    public static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        // Register application services
        services.Add[Context]Application();

        // Register mock driven adapters
        services.AddScoped<I[RepositoryName]Port, Mock[RepositoryName]>();

        return services.BuildServiceProvider();
    }
}</content>
   </write_to_file>
   ```

## 10. Validate Against Spec Scenarios

1. **Run tests to ensure they pass**:
   ```xml
   <execute_command>
   <command>dotnet test src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/AGS.WindowsAndDoors.[Context].Tests.csproj</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

2. **Verify test coverage of spec scenarios**:
   - Each `scenario` in the spec should have corresponding tests
   - Test names should follow BDD pattern: `[ScenarioName]_Should_[ExpectedBehavior]`

</detailed_sequence_of_steps>

<example_implementation>

# Example: ProductCatalog Item Management

## Step 1: Load Target Spec

```xml
<read_file>
<path>arc/specs/items.manage-catalog-item.v1.yaml</path>
</read_file>
```

## Step 2: Implement Domain Layer

```xml
<write_to_file>
<path>src/BoundedContexts/ProductCatalog/AGS.WindowsAndDoors.ProductCatalog.Domain/Entities/Item.cs</path>
<content>namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;

public class Item : IEntity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public Measure Measure { get; private set; }

    private Item(string code, string name, Measure measure)
    {
        Code = code;
        Name = name;
        Measure = measure;
    }

    public static Item Create(string code, string name, Measure measure)
    {
        // Business rules validation
        if (string.IsNullOrWhiteSpace(code)) throw new BusinessRuleViolationException("Code is required");
        if (string.IsNullOrWhiteSpace(name)) throw new BusinessRuleViolationException("Name is required");

        return new Item(code, name, measure);
    }
}</content>
</write_to_file>
```

## Step 3: Define Driven Port

```xml
<write_to_file>
<path>src/BoundedContexts/ProductCatalog/AGS.WindowsAndDoors.ProductCatalog.Domain/Ports/IItemRepositoryPort.cs</path>
<content>namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

public interface IItemRepositoryPort
{
    Task<Item> SaveAsync(Item item, CancellationToken ct = default);
    Task<Item?> FindByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyCollection<Item>> FindAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
}</content>
</write_to_file>
```

## Step 4: Implement Application Use Case

```xml
<write_to_file>
<path>src/BoundedContexts/ProductCatalog/AGS.WindowsAndDoors.ProductCatalog.Application/UseCases/CreateItem/CreateItemCommand.cs</path>
<content>namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem;

public record CreateItemCommand(
    string Code,
    string Name,
    string Description,
    string CategoryName,
    string CategoryCode,
    decimal Price
) : IRequest<ItemDto>;</content>
</write_to_file>
```

## Step 5: Create Test for Use Case

```xml
<write_to_file>
<path>src/BoundedContexts/ProductCatalog/AGS.WindowsAndDoors.ProductCatalog.Tests/Application/UseCases/CreateItemCommandHandlerTests.cs</path>
<content>namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Application.UseCases;

public class CreateItemCommandHandlerTests
{
    private readonly Fixture _fixture = FixtureHelper.WithDefaultCustomizations();

    [Fact]
    public async Task CreateItem_HappyPath_Should_SaveItemAndReturnDto()
    {
        // Arrange
        var mockRepository = _fixture.Create<IItemRepositoryPort>();
        var handler = new CreateItemCommandHandler(mockRepository);
        var command = new CreateItemCommand("TEST001", "Test Item", "Description", "Category", "CAT001", 99.99m);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("TEST001");
        result.Name.Should().Be("Test Item");
    }
}</content>
</write_to_file>
```

</example_implementation>

<validation_checklist>

# Validation Checklist - Step 1 Complete

Before proceeding to Step 2, verify:

- [ ] **Spec Compliance**: All scenarios in the target spec have corresponding tests
- [ ] **Domain Purity**: Domain layer contains no infrastructure dependencies
- [ ] **Port Definitions**: All driven ports are defined as interfaces in domain layer
- [ ] **Test Coverage**: Tests use BDD naming and cover happy path + error scenarios
- [ ] **Mock Isolation**: Application layer is fully testable with mock driven adapters
- [ ] **Rule Compliance**: Implementation follows referenced rule packs
- [ ] **Contract Adherence**: Port interfaces conform to JSON schema contracts

</validation_checklist>

<next_steps>

# Ready for Step 2

Once Step 1 is complete:
- The hexagon (domain + application) is fully implemented and tested
- All driver ports are tested with mock driven adapters
- Ask for confirmation to proceed to **Step 2: Real Driver Adapters / Mock Driven Adapters**

If confirmed, invoke the next workflow:
```
/hexagonal-step-2-real-drivers-mock-driven.md
```

</next_steps>
