namespace AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

public record ItemDto(
    string Id,
    string Code,
    string Name,
    string Description,
    string CategoryName,
    string CategoryCode,
    decimal Price,
    string? ColorName,
    string? ColorHex,
    decimal? DimensionValue,
    string? DimensionUnit,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ModifiedAt
);
