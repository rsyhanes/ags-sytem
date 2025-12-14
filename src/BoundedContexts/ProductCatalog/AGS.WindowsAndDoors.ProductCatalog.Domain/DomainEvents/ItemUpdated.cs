using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;

namespace AGS.WindowsAndDoors.ProductCatalog.Domain.DomainEvents;

/// <summary>
/// Domain event raised when an existing catalog item is updated.
/// </summary>
public record ItemUpdated : IDomainEvent
{
    /// <summary>
    /// The code of the updated item.
    /// </summary>
    public string ItemCode { get; init; }

    /// <summary>
    /// The identifier of the updated item.
    /// </summary>
    public string ItemId { get; init; }

    /// <summary>
    /// Date and time when the event occurred.
    /// </summary>
    public DateTime OccurredOn { get; init; }

    /// <summary>
    /// Unique identifier for this event instance.
    /// </summary>
    public Guid EventId { get; init; }

    public ItemUpdated(string itemCode, string itemId)
    {
        ItemCode = itemCode ?? throw new ArgumentNullException(nameof(itemCode));
        ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}
