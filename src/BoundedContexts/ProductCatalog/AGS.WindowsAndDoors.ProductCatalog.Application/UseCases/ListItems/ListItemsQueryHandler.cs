using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;
using AGS.WindowsAndDoors.ProductCatalog.Application.Mappers;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.ListItems;

/// <summary>
/// Handles listing all catalog items.
/// Follows application service rules: delegate to port, return DTOs.
/// </summary>
public class ListItemsQueryHandler : IRequestHandler<ListItemsQuery, IReadOnlyCollection<ItemDto>>
{
    private readonly IItemRepositoryPort _itemRepository;

    public ListItemsQueryHandler(IItemRepositoryPort itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<IReadOnlyCollection<ItemDto>> Handle(ListItemsQuery request, CancellationToken cancellationToken)
    {
        // Delegate to domain port
        var items = await _itemRepository.FindAllAsync(cancellationToken);

        // Return DTOs
        return ItemMapper.ToDto(items);
    }
}
