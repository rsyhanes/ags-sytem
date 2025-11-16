using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;
using AGS.WindowsAndDoors.ProductCatalog.Application.Mappers;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.UpdateItem;

/// <summary>
/// Handles updating existing catalog items.
/// Follows application service rules: delegate to domain, return DTOs, catch domain errors.
/// </summary>
public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
{
    private readonly IItemRepositoryPort _itemRepository;

    public UpdateItemCommandHandler(IItemRepositoryPort itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find existing item
            var existingItem = await _itemRepository.FindByCodeAsync(request.Code, cancellationToken);
            if (existingItem == null)
            {
            throw new BusinessRuleViolationException("item.not.found", $"Item with code '{request.Code}' not found");
            }

            // Update item details through domain method
            var updatedItem = existingItem.WithUpdatedDetails(
                request.Name,
                request.Description,
                request.Price
            );

            // Apply optional color update
            if (!string.IsNullOrWhiteSpace(request.ColorName) && !string.IsNullOrWhiteSpace(request.ColorHex))
            {
                var color = Color.Create(request.ColorName, request.ColorHex);
                updatedItem = updatedItem.WithColor(color);
            }

            // Apply optional dimensions update
            if (request.DimensionValue.HasValue && !string.IsNullOrWhiteSpace(request.DimensionUnit))
            {
                var dimensions = Measure.Create(request.DimensionValue.Value, request.DimensionUnit);
                updatedItem = updatedItem.WithDimensions(dimensions);
            }

            // Delegate to domain port for persistence
            var savedItem = await _itemRepository.SaveAsync(updatedItem, cancellationToken);

            // Return DTO
            return ItemMapper.ToDto(savedItem);
        }
        catch (ArgumentException ex)
        {
            // Translate domain validation errors
            throw new BusinessRuleViolationException("domain.validation.error", ex.Message, ex);
        }
    }
}
