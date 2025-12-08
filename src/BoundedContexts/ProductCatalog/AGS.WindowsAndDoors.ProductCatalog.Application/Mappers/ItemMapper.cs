using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.Mappers;

/// <summary>
/// Mapper for converting between Item domain entities and DTOs.
/// </summary>
public static class ItemMapper
{
    public static ItemDto ToDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Color = item.Color != null ? new ColorDto
            {
                Name = item.Color.Name,
                Hex = item.Color.HexValue
            } : null,
            Dimensions = item.Dimensions != null ? new MeasureDto
            {
                Value = item.Dimensions.Value,
                Unit = item.Dimensions.Unit
            } : null,
            State = item.State.Value,
            CreatedAt = item.CreatedAt,
            ModifiedAt = item.ModifiedAt
        };
    }

    public static IReadOnlyCollection<ItemDto> ToDto(IEnumerable<Item> items)
    {
        return items.Select(ToDto).ToList().AsReadOnly();
    }
}
