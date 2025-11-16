using MassTransit.Mediator;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem;

public record CreateItemCommand(
    string Code,
    string Name,
    string Description,
    string CategoryName,
    string CategoryCode,
    decimal Price,
    string? ColorName = null,
    string? ColorHex = null,
    decimal? DimensionValue = null,
    string? DimensionUnit = null
) : Request<ItemDto>;
