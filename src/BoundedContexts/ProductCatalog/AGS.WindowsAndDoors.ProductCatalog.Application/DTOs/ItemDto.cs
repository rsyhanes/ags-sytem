namespace AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

/// <summary>
/// Data Transfer Object for Item entity.
/// Used for external API responses and cross-boundary communication.
/// </summary>
public record ItemDto
{
    public required string Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required CategoryDto Category { get; init; }
    public required decimal Price { get; init; }
    public ColorDto? Color { get; init; }
    public MeasureDto? Dimensions { get; init; }
    public required string State { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
    
    // Convenience properties
    public bool IsActive => State == "Active";
}

public record CategoryDto
{
    public required string Name { get; init; }
    public required string Code { get; init; }
}

public record ColorDto
{
    public required string Name { get; init; }
    public required string Hex { get; init; }
}

public record MeasureDto
{
    public required decimal Value { get; init; }
    public required string Unit { get; init; }
}
