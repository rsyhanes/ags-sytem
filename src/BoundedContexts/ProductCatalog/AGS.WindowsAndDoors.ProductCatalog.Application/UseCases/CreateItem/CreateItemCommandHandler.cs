using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;
using AGS.WindowsAndDoors.ProductCatalog.Application.Mappers;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem;

/// <summary>
/// Handles the creation of new catalog items.
/// Follows application service rules: delegate to domain, return DTOs, catch domain errors.
/// </summary>
public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IItemRepositoryPort _itemRepository;

    public CreateItemCommandHandler(IItemRepositoryPort itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check for duplicate code
            var existingItem = await _itemRepository.FindByCodeAsync(request.Code, cancellationToken);
            if (existingItem != null)
            {
            throw new BusinessRuleViolationException("duplicate.item.code", $"Item with code '{request.Code}' already exists");
            }

            // Create domain objects from request
            var category = Category.Create(request.CategoryName, request.CategoryCode);
            
            // Create the item through the domain
            var item = Item.Create(
                request.Code,
                request.Name,
                request.Description,
                category,
                request.Price
            );

            // Apply optional properties
            if (!string.IsNullOrWhiteSpace(request.ColorName) && !string.IsNullOrWhiteSpace(request.ColorHex))
            {
                var color = Color.Create(request.ColorName, request.ColorHex);
                item = item.WithColor(color);
            }

            if (request.DimensionValue.HasValue && !string.IsNullOrWhiteSpace(request.DimensionUnit))
            {
                var dimensions = Measure.Create(request.DimensionValue.Value, request.DimensionUnit);
                item = item.WithDimensions(dimensions);
            }

            // Delegate to domain port for persistence
            var savedItem = await _itemRepository.SaveAsync(item, cancellationToken);

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
