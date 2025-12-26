using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

namespace AGS.WindowsAndDoors.ProductDesign.Tests.TestUtilities.Mocks;

public class MockItemRepository : IItemRepositoryPort
{
    private readonly Dictionary<string, Item> _items = new();

    public Task<Item> SaveAsync(Item item, CancellationToken cancellationToken = default)
    {
        _items[item.Code.ToUpper()] = item;
        return Task.FromResult(item);
    }

    public Task<Item?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_items.GetValueOrDefault(code.ToUpper()));
    }

    public Task<IReadOnlyCollection<Item>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Item>>(_items.Values.ToList());
    }

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_items.ContainsKey(code.ToUpper()));
    }

    public void AddItem(Item item)
    {
        _items[item.Code.ToUpper()] = item;
    }

    public void Clear()
    {
        _items.Clear();
    }
}
