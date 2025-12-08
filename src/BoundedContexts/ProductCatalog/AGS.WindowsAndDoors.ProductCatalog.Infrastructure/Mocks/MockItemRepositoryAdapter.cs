using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

namespace AGS.WindowsAndDoors.ProductCatalog.Infrastructure.Mocks;

public class MockItemRepositoryAdapter : IItemRepositoryPort
{
    private readonly List<Item> _items = new();

    public async Task<Item> SaveAsync(Item item, CancellationToken cancellationToken = default)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
        _items.Add(item);
        return await Task.FromResult(item);
    }

    public async Task<Item?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_items.FirstOrDefault(item => item.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<IReadOnlyCollection<Item>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_items.AsReadOnly() as IReadOnlyCollection<Item>);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_items.Any(item => item.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));
    }
}
