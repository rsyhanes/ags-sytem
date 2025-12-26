using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.DomainEvents;

public record SystemComponentRemoved(
    string SystemCode,
    string ComponentId,
    string ItemCode,
    int Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EventId { get; init; } = Guid.NewGuid();
}
