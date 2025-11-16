using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.ListItems;

public record ListItemsQuery() : IRequest<IReadOnlyCollection<ItemDto>>;
