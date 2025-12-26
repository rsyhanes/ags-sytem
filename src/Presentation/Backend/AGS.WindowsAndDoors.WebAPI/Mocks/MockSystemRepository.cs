using AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

namespace AGS.WindowsAndDoors.WebAPI.Mocks;

public class MockSystemRepository : ISystemRepositoryPort
{
    private readonly Dictionary<string, ProductDesignEntities.System> _systems = new();

    public Task<ProductDesignEntities.System?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        // For demo purposes, create a default system if it doesn't exist
        if (!_systems.ContainsKey(code.ToUpper()))
        {
            var system = new ProductDesignEntities.System(code, $"{code} System", $"Description for {code}",
                AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects.Category.Window);
            _systems[code.ToUpper()] = system;
        }

        return Task.FromResult(_systems.GetValueOrDefault(code.ToUpper()));
    }

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
    {
        return Task.FromResult(_systems.ContainsKey(code.ToUpper()) || code.ToUpper() == "WF00"); // WF00 always exists for demo
    }
}
