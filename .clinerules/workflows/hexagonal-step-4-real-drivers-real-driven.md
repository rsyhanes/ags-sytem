# Hexagonal Architecture Step 4: Real Driver Adapters / Real Driven Adapters

You are implementing Step 4 of the hexagonal architecture development process: **REAL DRIVER ADAPTERS / REAL DRIVEN ADAPTERS**.

**Goal**: Connect real driver and driven adapters for full end-to-end system testing and production deployment.

**Prerequisites**: Steps 1-3 are complete - all adapters are implemented and tested in isolation.

<detailed_sequence_of_steps>

# Hexagonal Architecture Implementation - Step 4
## Real Driver Adapters / Real Driven Adapters

## 1. Load All Specifications and Contracts

1. **Final review of target spec**:
   ```xml
   <read_file>
   <path>arc/specs/[spec-id].spec.yaml</path>
   </read_file>
   ```

2. **Review all contracts** (OpenAPI + JSON schemas):
   ```xml
   <list_files>
   <path>arc/contracts</path>
   </list_files>
   ```

## 2. Configure Production Composition Root

1. **Update Program.cs** for production configuration:
   ```xml
   <read_file>
   <path>src/Presentation/Backend/AGS.WindowsAndDoors.WebAPI/Program.cs</path>
   </read_file>
   ```

   ```xml
   <replace_in_file>
   <path>src/Presentation/Backend/AGS.WindowsAndDoors.WebAPI/Program.cs</path>
   <diff>------- SEARCH
// Register application services
builder.Services.Add[Context]Application();

// Register real driven adapters
builder.Services.AddScoped<I[RepositoryName]Port, [RepositoryName]>();
builder.Services.AddDbContext<[Context]DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("[Context]Db")));
=======
// Register application services
builder.Services.Add[Context]Application();

// Register infrastructure adapters
builder.Services.Add[Context]Infrastructure(builder.Configuration);
+++++++ REPLACE</diff>
   </replace_in_file>
   ```

2. **Create infrastructure extension method**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/DependencyInjection.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection Add[Context]Infrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<[Context]DbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("[Context]Db")));

        // Repositories
        services.AddScoped<I[RepositoryName]Port, [RepositoryName]>();

        // External services
        // services.AddHttpClient<IExternalService, ExternalServiceAdapter>();

        return services;
    }
}
