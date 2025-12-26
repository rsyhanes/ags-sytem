using MediatR;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.AddComponent;

public class AddComponentCommandHandler : IRequestHandler<AddComponentCommand, string>
{
    private readonly ISystemRepositoryPort _systemRepository;
    private readonly ISystemComponentRepositoryPort _componentRepository;
    private readonly IItemRepositoryPort _itemRepository;

    public AddComponentCommandHandler(
        ISystemRepositoryPort systemRepository,
        ISystemComponentRepositoryPort componentRepository,
        IItemRepositoryPort itemRepository)
    {
        _systemRepository = systemRepository;
        _componentRepository = componentRepository;
        _itemRepository = itemRepository;
    }

    public async Task<string> Handle(AddComponentCommand request, CancellationToken ct)
    {
        // Validate system exists
        var system = await _systemRepository.GetByCodeAsync(request.SystemCode, ct);
        if (system is null)
        {
            throw new InvalidOperationException($"System with code '{request.SystemCode}' not found");
        }

        // Validate item exists
        var itemExists = await _itemRepository.ExistsByCodeAsync(request.ItemCode, ct);
        if (!itemExists)
        {
            throw new InvalidOperationException($"Item with code '{request.ItemCode}' not found");
        }

        // Check for duplicate component
        var exists = await _componentRepository.ExistsBySystemAndItemAsync(request.SystemCode, request.ItemCode, ct);
        if (exists)
        {
            throw new InvalidOperationException($"Component with item '{request.ItemCode}' already exists in system '{request.SystemCode}'");
        }

        // Create dimensions
        var dimensions = CreateDimensions(request);

        // Create component
        var component = ProductDesignEntities.SystemComponent.Create(
            systemCode: request.SystemCode,
            name: request.Name,
            itemCode: request.ItemCode,
            quantity: request.Quantity,
            dimensions: dimensions,
            isRequired: request.IsRequired,
            sortOrder: request.SortOrder,
            description: request.Description);

        // Save component
        var savedComponent = await _componentRepository.SaveAsync(component, ct);

        return savedComponent.Id;
    }

    private static ComponentDimensions CreateDimensions(AddComponentCommand request)
    {
        if (!string.IsNullOrEmpty(request.LengthFormula))
        {
            return new ComponentDimensions(request.LengthFormula, null);
        }

        if (request.FixedLengthValue.HasValue && !string.IsNullOrEmpty(request.FixedLengthUnit))
        {
            var measure = new Measure(request.FixedLengthValue.Value, request.FixedLengthUnit);
            return new ComponentDimensions(null, measure);
        }

        return new ComponentDimensions();
    }
}
