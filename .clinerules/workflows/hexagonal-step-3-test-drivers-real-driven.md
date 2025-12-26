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
   <path>arc/specs/[spec-id].spec.yaml</path>
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

## 3. Determine Persistence Technology

1. **Check rule packs for persistence technology preferences**:
   - Review `arc/rules/packs/infrastructure.persistence.yaml` for technology constraints
   - Check `arc/rules/packs/` for any project-specific persistence technology rules

2. **If technology not specified in rules, ask and codify**:
   - Common options: Entity Framework, Dapper, ADO.NET, Document DB, etc.
   - Consider factors: performance, complexity, team experience, project requirements
   - Document decision in appropriate rule pack

3. **Note**: This workflow assumes Entity Framework unless otherwise specified in rules. If different technology preferred, adapt the implementation steps accordingly.

## 4. Create Adapter-Specific Models

1. **Create persistence models** optimized for the chosen persistence technology:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/Models/[Entity]PersistenceModel.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure.Models;

// Persistence model - optimized for database storage
// May differ from domain model structure
public class [Entity]PersistenceModel
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    // Additional fields as needed for persistence
    // May include denormalized data, audit fields, etc.
}</content>
   </write_to_file>
   ```

## 5. Implement Mapping Logic

1. **Create mapper classes** for domain ↔ adapter model conversion:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/Mappers/[Entity]Mapper.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure.Mappers;

public static class [Entity]Mapper
{
    public static [Entity]PersistenceModel ToPersistenceModel([Entity] domainEntity)
    {
        return new [Entity]PersistenceModel
        {
            Id = domainEntity.Id,
            Code = domainEntity.Code,
            Name = domainEntity.Name,
            // Map other properties as needed
        };
    }

    public static [Entity] ToDomainEntity([Entity]PersistenceModel persistenceModel)
    {
        return [Entity].Create(
            persistenceModel.Code,
            persistenceModel.Name
            // Map other properties as needed
        );
    }
}</content>
   </write_to_file>
   ```

## 6. Implement Real Driven Adapters

### Entity Framework Repository Adapter (if EF chosen):
1. **Create DbContext**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/[Context]DbContext.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure;

public class [Context]DbContext : DbContext
{
    public [Context]DbContext(DbContextOptions<[Context]DbContext> options) : base(options) { }

    public DbSet<[Entity]PersistenceModel> [Entities] { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof([Context]DbContext).Assembly);
    }
}</content>
   </write_to_file>
   ```

2. **Create entity configurations**:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/Configurations/[Entity]Configuration.cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure.Configurations;

public class [Entity]Configuration : IEntityTypeConfiguration<[Entity]PersistenceModel>
{
    public void Configure(EntityTypeBuilder<[Entity]PersistenceModel> builder)
    {
        builder.ToTable("[entity_table]");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(e => e.Code).IsUnique();
    }
}</content>
   </write_to_file>
   ```

3. **Implement repository adapters** using persistence models and mappers:
   ```xml
   <write_to_file>
   <path>src/BoundedContexts/[Context]/AGS.WindowsAndDoors.[Context].Infrastructure/Repositories/[RepositoryName].cs</path>
   <content>namespace AGS.WindowsAndDoors.[Context].Infrastructure.Repositories;

public class [RepositoryName] : I[RepositoryName]Port
{
    private readonly [Context]DbContext _context;

    public [RepositoryName]([Context]DbContext context)
    {
        _context = context;
    }

    public async Task<[Entity]> SaveAsync([Entity] entity, CancellationToken ct = default)
    {
        var persistenceModel = [Entity]Mapper.ToPersistenceModel(entity);
        _context.[Entities].Update(persistenceModel);
        await _context.SaveChangesAsync(ct);
        return [Entity]Mapper.ToDomainEntity(persistenceModel);
    }

    public async Task<[Entity]?> FindByCodeAsync(string code, CancellationToken ct = default)
    {
        var persistenceModel = await _context.[Entities]
            .FirstOrDefaultAsync(e => e.Code == code, ct);
        return persistenceModel != null ? [Entity]Mapper.ToDomainEntity(persistenceModel) : null;
    }

    public async Task<IReadOnlyCollection<[Entity]>> FindAllAsync(CancellationToken ct = default)
    {
        var persistenceModels = await _context.[Entities].ToListAsync(ct);
        return persistenceModels.Select([Entity]Mapper.ToDomainEntity).ToList();
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.[Entities].AnyAsync(e => e.Code == code, ct);
    }
}</content>
   </write_to_file>
   ```
