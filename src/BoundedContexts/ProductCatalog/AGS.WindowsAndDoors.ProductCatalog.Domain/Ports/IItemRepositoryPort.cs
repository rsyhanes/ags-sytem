using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

/// <summary>
/// Driven port for Item persistence operations.
/// To be implemented by infrastructure adapters.
/// </summary>
public interface IItemRepositoryPort
{
    /// <summary>
    /// Save an item to the persistence store.
    /// </summary>
    /// <param name="item">The item to save</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The saved item</returns>
    Task<Item> SaveAsync(Item item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find an item by its unique code.
    /// </summary>
    /// <param name="code">The item code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The item if found, null otherwise</returns>
    Task<Item?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find all items in the catalog.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of all items</returns>
    Task<IReadOnlyCollection<Item>> FindAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if an item with the given code already exists.
    /// </summary>
    /// <param name="code">The item code to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
