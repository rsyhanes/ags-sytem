using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;
using AGS.WindowsAndDoors.ProductCatalog.Application.Mappers;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.GetItem;

/// <summary>
/// Handles retrieving a single catalog item by code.
/// Follows application service rules: delegate to port, return DTOs.
/// </summary>
public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto?>
{
    private readonly IItemRepositoryPort _itemRepository;

    public GetItemQueryHandler(IItemRepositoryPort itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<ItemDto?> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        // Delegate to domain port
        var item = await _itemRepository.FindByCodeAsync(request.Code, cancellationToken);

        // Return DTO or null
        return item != null ? ItemMapper.ToDto(item) : null;
    }
}
