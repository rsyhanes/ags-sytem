using AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.Ports;

public interface ISystemComponentRepositoryPort
{
    Task<Entities.SystemComponent?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyCollection<Entities.SystemComponent>> GetBySystemCodeAsync(string systemCode, CancellationToken ct = default);
    Task<Entities.SystemComponent> SaveAsync(Entities.SystemComponent component, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
    Task<bool> ExistsBySystemAndItemAsync(string systemCode, string itemCode, CancellationToken ct = default);
}
