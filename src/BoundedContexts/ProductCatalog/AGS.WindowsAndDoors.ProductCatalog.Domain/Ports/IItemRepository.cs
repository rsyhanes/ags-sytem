using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

public interface IItemRepository
{
    Task<Item?> FindByIdAsync(string id);
    Task<Item?> FindByCodeAsync(string code);
    Task<IReadOnlyList<Item>> FindAllActiveAsync();
    Task<IReadOnlyList<Item>> FindByCategoryAsync(string categoryCode);
    Task<bool> ExistsWithCodeAsync(string code);
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(string id);
}
