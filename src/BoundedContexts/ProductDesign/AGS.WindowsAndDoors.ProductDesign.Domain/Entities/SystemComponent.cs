using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;
using AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

public record SystemComponent : IEntity<string>
{
    public string Id { get; init; }
    public string SystemCode { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ItemCode { get; init; }
    public int Quantity { get; init; }
    public ComponentDimensions Dimensions { get; init; }
    public bool IsRequired { get; init; }
    public int SortOrder { get; init; }
    public DateTime CreatedAt { get; init; }

    private SystemComponent() { } // EF Constructor

    public static SystemComponent Create(
        string systemCode,
        string name,
        string itemCode,
        int quantity,
        ComponentDimensions dimensions,
        bool isRequired = true,
        int sortOrder = 0,
        string? description = null,
        string? existingId = null)
    {
        if (string.IsNullOrWhiteSpace(systemCode))
            throw new ArgumentException("System code cannot be empty", nameof(systemCode));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("Item code cannot be empty", nameof(itemCode));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        return new SystemComponent
        {
            Id = existingId ?? Guid.NewGuid().ToString(),
            SystemCode = systemCode.Trim().ToUpper(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            ItemCode = itemCode.Trim().ToUpper(),
            Quantity = quantity,
            Dimensions = dimensions ?? new ComponentDimensions(),
            IsRequired = isRequired,
            SortOrder = sortOrder,
            CreatedAt = DateTime.UtcNow
        };
    }
}
