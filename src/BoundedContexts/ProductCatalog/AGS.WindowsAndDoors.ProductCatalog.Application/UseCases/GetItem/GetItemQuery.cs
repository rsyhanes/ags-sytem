using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.GetItem;

public record GetItemQuery(string Code) : IRequest<ItemDto?>;
