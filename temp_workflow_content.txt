# Hexagonal Architecture Step 2: Real Driver Adapters / Mock Driven Adapters

You are implementing Step 2 of the hexagonal architecture development process: **REAL DRIVER ADAPTERS / MOCK DRIVEN ADAPTERS**.

**Goal**: Add real driver adapters (Web API, Web UI) while keeping mock driven adapters for isolated testing.

**Prerequisites**: Step 1 is complete - hexagon (domain + application) is implemented and tested.

<detailed_sequence_of_steps>

# Hexagonal Architecture Implementation - Step 2
## Real Driver Adapters / Mock Driven Adapters

## 1. Load Target Specification and Contracts

1. **Re-read the target spec** to understand driver port requirements:
   ```xml
   <read_file>
   <path>arc/specs/[spec-id].yaml</path>
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
