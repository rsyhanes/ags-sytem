# Hexagonal Architecture Step 3: Test Driver Adapters / Real Driven Adapters

You are implementing Step 3 of the hexagonal architecture development process: **TEST DRIVER ADAPTERS / REAL DRIVEN ADAPTERS**.

**Goal**: Implement real driven adapters (databases, external services) while using test driver adapters for isolated testing.

**Prerequisites**: Steps 1 and 2 are complete - hexagon is implemented and real driver adapters exist.

<detailed_sequence_of_steps>

# Hexagonal Architecture Implementation - Step 3
## Test Driver Adapters / Real Driven Adapters

## 1. Load Target Specification and Contracts

1. **Re-read the target spec** to understand driven port requirements:
   ```xml
   <read_file>
   <path>arc/specs/[spec-id].yaml</path>
   </read_file>
   ```

2. **Load JSON schemas** for driven port contracts:
   ```xml
   <list_files>
   <path>arc/contracts/schemas</path>
   </list_files>
   ```

3. **Review infrastructure rule packs**:
   ```xml
   <read_file>
   <path>arc/rules/packs/infrastructure.persistence.yaml</path>
   </read_file>
   ```

## 2. Create Infrastructure Project Structure

1. **Set up infrastructure project** with proper references:
   ```xml
   <execute_command>
   <command>dotnet new classlib -n AGS.WindowsAndDoors.[Context].Infrastructure -o src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

2. **Add project references**:
   ```xml
   <execute_command>
   <command>dotnet add src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/AGS.WindowsAndDoors.[Context].Infrastructure.csproj reference src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Domain/AGS.WindowsAndDoors.[Context].Domain.csproj src/SharedKernel/AGS.WindowsAndDoors.SharedKernel/AGS.WindowsAndDoors.SharedKernel.csproj</command>
   <requires_approval>false</requires_approval>
   </execute_command>
   ```

## 3. Implement Real Driven Adapters

### Database Repository Adapter:
1. **Create DbContext**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/[Context]DbContext.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure;

public class [Context]DbContext : DbContext
{
    public [Context]DbContext(DbContextOptions<[Context]DbContext> options) : base(options) { }

    public DbSet<[Entity]> [Entities] { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof([Context]DbContext).Assembly);
    }
}
