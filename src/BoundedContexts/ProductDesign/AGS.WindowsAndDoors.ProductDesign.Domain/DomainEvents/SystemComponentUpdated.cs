using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.DomainEvents;

public record SystemComponentUpdated(
    string SystemCode,
    string ComponentId,
    string ItemCode,
    int OldQuantity,
    int NewQuantity) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}
