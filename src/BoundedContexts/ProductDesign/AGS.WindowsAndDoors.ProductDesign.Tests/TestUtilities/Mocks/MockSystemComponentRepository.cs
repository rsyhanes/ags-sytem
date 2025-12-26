using AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductDesign.Tests.TestUtilities.Mocks;

public class MockSystemComponentRepository : ISystemComponentRepositoryPort
{
    private readonly List<ProductDesignEntities.SystemComponent> _components = new();

    public Task<ProductDesignEntities.SystemComponent?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return Task.FromResult(_components.FirstOrDefault(c => c.Id == id));
    }

    public Task<IReadOnlyCollection<ProductDesignEntities.SystemComponent>> GetBySystemCodeAsync(string systemCode, CancellationToken ct = default)
    {
        var components = _components.Where(c => c.SystemCode.Equals(systemCode, StringComparison.OrdinalIgnoreCase)).ToList();
        return Task.FromResult<IReadOnlyCollection<ProductDesignEntities.SystemComponent>>(components);
    }

    public Task<ProductDesignEntities.SystemComponent> SaveAsync(ProductDesignEntities.SystemComponent component, CancellationToken ct = default)
    {
        var existing = _components.FirstOrDefault(c => c.Id == component.Id);
        if (existing != null)
        {
            _components.Remove(existing);
        }
        _components.Add(component);
        return Task.FromResult(component);
    }

    public Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var component = _components.FirstOrDefault(c => c.Id == id);
        if (component != null)
        {
            _components.Remove(component);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsBySystemAndItemAsync(string systemCode, string itemCode, CancellationToken ct = default)
    {
        var exists = _components.Any(c =>
            c.SystemCode.Equals(systemCode, StringComparison.OrdinalIgnoreCase) &&
            c.ItemCode.Equals(itemCode, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }

    public void AddComponent(ProductDesignEntities.SystemComponent component)
    {
        _components.Add(component);
    }

    public void Clear()
    {
        _components.Clear();
    }
}
