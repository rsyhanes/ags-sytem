namespace AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}
