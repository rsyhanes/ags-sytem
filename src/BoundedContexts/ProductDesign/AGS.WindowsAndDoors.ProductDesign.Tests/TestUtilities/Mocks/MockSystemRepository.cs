using AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductDesign.Tests.TestUtilities.Mocks;

public class MockSystemRepository : ISystemRepositoryPort
{
    private readonly Dictionary<string, ProductDesignEntities.System> _systems = new();

    public Task<ProductDesignEntities.System?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return Task.FromResult(_systems.GetValueOrDefault(code.ToUpper()));
    }

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
    {
        return Task.FromResult(_systems.ContainsKey(code.ToUpper()));
    }

    public void AddSystem(ProductDesignEntities.System system)
    {
        _systems[system.Code.ToUpper()] = system;
    }

    public void Clear()
    {
        _systems.Clear();
    }
}
