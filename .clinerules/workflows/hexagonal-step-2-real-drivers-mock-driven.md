# Hexagonal Architecture Step 2: Real Driver Adapters / Mock Driven Adapters

You are implementing Step 2 of the hexagonal architecture development process: **REAL DRIVER ADAPTERS / MOCK DRIVEN ADAPTERS**.

**Goal**: Add real driver adapters (Web API, Web UI) while keeping mock driven adapters for isolated testing.

**Prerequisites**: Step 1 is complete - hexagon (domain + application) is implemented and tested.

---

# Hexagonal Architecture Implementation - Step 2
## Real Driver Adapters / Mock Driven Adapters

## 1. Load Target Specification and Contracts

1. **Re-read the target spec** to understand driver port requirements:
   ```xml
   <read_file>
   <path>arc/specs/[spec-id].spec.yaml</path>
   </read_file>
   ```

2. **Load OpenAPI contract** for HTTP API specification:
   ```xml
   <read_file>
   <path>arc/contracts/openapi/[contract-id].yaml</path>
   </read_file>
   ```

3. **Review existing rule packs** for adapter patterns:
   ```xml
   <read_file>
   <path>arc/rules/packs/hexagonal.architecture.yaml</path>
   </read_file>
   ```

## 2. Create Web API Driver Adapter Structure

1. **Set up Minimal API endpoints** in the WebAPI project:
   ```xml
   <write_to_file>
   <path>src/Presentation/Backend/AGS.WindowsAndDoors.WebAPI/Endpoints/[Feature]Endpoints.cs</path>
   <content>namespace AGS.WindowsAndDoors.WebAPI.Endpoints;

public static class [Feature]Endpoints
{
    public static void Map[Feature]Endpoints(this IEndpointRouteBuilder app)
    {
        // Map endpoints conforming to OpenAPI contract
        app.MapPost("/api/[resource]", [HandlerMethod])
            .WithName("[EndpointName]")
            .WithOpenApi();
    }
}

2. **Implement endpoint handlers** that delegate to application use cases:
   Add the following method to the `[Feature]Endpoints.cs` file:

   ```csharp
   private static async Task<IResult> [HandlerMethod](IMediator mediator, [RequestType] request)
   {
       var command = new [CommandType](/* map request properties */);
       var result = await mediator.Send(command);
       return Results.Created($"/api/[resource]/{result.Id}", result);
   }
   ```

## 3. Create Mock Driven Adapters

1. **Identify driven ports** in the bounded context domain ports directory:
   ```xml
   <list_files>
   <path>src/BoundedContexts/[BoundedContext]/AGS.WindowsAndDoors.[BoundedContext].Domain/Ports</path>
   </list_files>
   ```

2. **Create Mocks folder** in the infrastructure project:
   ```xml
   <execute_command>
   <command>mkdir src/BoundedContexts/[BoundedContext]/AGS.WindowsAndDoors.[BoundedContext].Infrastructure/Mocks</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

3. **Create mock adapter implementations** for each driven port:
   Create new file: `src/BoundedContexts/[BoundedContext]/AGS.WindowsAndDoors.[BoundedContext].Infrastructure/Mocks/Mock[PortName]Adapter.cs`

   ```csharp
   using AGS.WindowsAndDoors.[BoundedContext].Domain.Ports;

   namespace AGS.WindowsAndDoors.[BoundedContext].Infrastructure.Mocks;

   public class Mock[PortName]Adapter : I[PortName]Port
   {
       private readonly List<[EntityType]> _items = new();

       public async Task<[EntityType]> [MethodName]Async([MethodParameters])
       {
           // Mock implementation - return configured test data or defaults
           return await Task.FromResult([MockReturnValue]);
       }

       // Additional mock methods for testing scenarios
   }

   // Example concrete implementation for ProductCatalog:
   public class MockItemRepositoryAdapter : IItemRepositoryPort
   {
       private readonly List<Item> _items = new();

       public async Task<Item> SaveAsync(Item item, CancellationToken ct = default)
       {
           _items.Add(item);
           return await Task.FromResult(item);
       }

       public async Task<Item?> FindByCodeAsync(string code, CancellationToken ct = default)
       {
           return await Task.FromResult(_items.FirstOrDefault(i => i.Code == code));
       }

       public async Task<IReadOnlyCollection<Item>> FindAllAsync(CancellationToken ct = default)
       {
           return await Task.FromResult((IReadOnlyCollection<Item>)_items.AsReadOnly());
       }

       public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
       {
           return await Task.FromResult(_items.Any(i => i.Code == code));
       }
   }
   ```

4. **Configure composition root** to optionally use mock adapters for step 2 development:

   In `src/Presentation/Backend/AGS.WindowsAndDoors.WebAPI/Program.cs`, add this configuration block:

   ```csharp
   // Configure infrastructure adapters based on environment
   // In Step 2 (Real Drivers + Mock Driven), use mock adapters for isolated testing
   if (builder.Environment.IsEnvironment("Testing") ||
       builder.Configuration.GetValue<bool>("UseMockAdapters"))
   {
       builder.Services.AddScoped<IItemRepositoryPort, MockItemRepositoryAdapter>();
       // Register other mock adapters for this bounded context
   }
   else
   {
       builder.Services.AddScoped<IItemRepositoryPort, SqlItemRepositoryAdapter>();
       // Register real adapters for production/development
   }
   ```

   This allows switching between mock and real adapters via:
   - Environment variable: `ASPNETCORE_ENVIRONMENT=Testing`
   - Configuration setting: `UseMockAdapters=true`

## 4. Validate Contract Compliance

1. **Verify OpenAPI contract alignment**:
   - Ensure endpoint paths match `interactions.inbound.api` from spec
   - Validate request/response schemas conform to OpenAPI specifications
   - Check that all required CRUD operations are implemented

2. **Validate internal contract usage**:
   ```xml
   <list_files>
   <path>arc/contracts/internal/commands</path>
   </list_files>
   ```

   Ensure command/query objects in application layer use internal contract schemas.

## 5. Verify Application Layer Integration

1. **Confirm command handlers are implemented**:
   - CQRS pattern followed (commands for writes, queries for reads)
   - Dependency injection properly configured for ports
   - Error handling with `BusinessRuleViolationException`

2. **Test use case orchestration**:
   ```xml
   <execute_command>
   <command>dotnet test src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Tests/AGS.WindowsAndDoors.[Context].Tests.csproj --filter "Application"</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

## 6. Register and Test Endpoints

1. **Register the new endpoints** in `Program.cs`:
   ```csharp
   app.MapControllers();
   app.Map[Feature]Endpoints();
   ```

2. **Test the Web API endpoints** with mock-driven adapters:
   ```xml
   <execute_command>
   <command>cd src/Presentation/Backend/AGS.WindowsAndDoors.WebAPI && dotnet run</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

3. **Validate full CRUD coverage**:
   - POST for create operations
   - GET for read operations (single + collection)
   - PUT/PATCH for update operations
   - DELETE for removal operations (if applicable)

## 7. Verify Step 2 Complete

1. **Test API endpoints** using tools like Postman or curl to ensure they work with mock data
2. **Run existing unit tests** to ensure the new driver adapters don't break existing functionality
3. **Validate architecture compliance** by checking that business logic remains in the domain layer
4. **Confirm mock adapters** can be easily switched with real implementations in the composition root
5. **Verify contract compliance** - endpoints align with OpenAPI specs and internal contracts
6. **Confirm application layer integration** - CQRS pattern, dependency injection, error handling
7. **Document the Step 2 completion** and prepare for Step 3 (real driven adapters)
