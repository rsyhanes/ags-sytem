using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

namespace AGS.WindowsAndDoors.WebAPI.Mocks;

public class MockItemRepository : IItemRepositoryPort
{
    private readonly Dictionary<string, Item> _items = new();

    public MockItemRepository()
    {
        // Add some default items for demo
        var item = Item.Create("2103", "Frame", "Window frame component", 25.50m).AsActivated();
        _items["2103"] = item;
    }

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
        return Task.FromResult(_items.ContainsKey(code.ToUpper()) || code.ToUpper() == "2103"); // 2103 always exists for demo
    }
}
