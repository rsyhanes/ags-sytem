using MediatR;
using AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.UpdateComponent;

public class UpdateComponentCommandHandler : IRequestHandler<UpdateComponentCommand, string>
{
    private readonly ISystemComponentRepositoryPort _componentRepository;

    public UpdateComponentCommandHandler(ISystemComponentRepositoryPort componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<string> Handle(UpdateComponentCommand request, CancellationToken ct)
    {
        // Get existing component
        var existingComponent = await _componentRepository.GetByIdAsync(request.ComponentId, ct);
        if (existingComponent is null)
        {
            throw new InvalidOperationException($"Component with id '{request.ComponentId}' not found");
        }

        // Validate quantity if provided
        if (request.Quantity.HasValue && request.Quantity.Value <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero");
        }

        // Create updated dimensions
        var updatedDimensions = CreateUpdatedDimensions(existingComponent.Dimensions, request);

        // Create updated component (immutable - create new instance with existing ID)
        var updatedComponent = ProductDesignEntities.SystemComponent.Create(
            systemCode: existingComponent.SystemCode,
            name: request.Name ?? existingComponent.Name,
            itemCode: existingComponent.ItemCode,
            quantity: request.Quantity ?? existingComponent.Quantity,
            dimensions: updatedDimensions,
            isRequired: request.IsRequired ?? existingComponent.IsRequired,
            sortOrder: request.SortOrder ?? existingComponent.SortOrder,
            description: request.Description ?? existingComponent.Description,
            existingId: existingComponent.Id
        );

        // Save updated component
        var savedComponent = await _componentRepository.SaveAsync(updatedComponent, ct);

        return savedComponent.Id;
    }

    private static ComponentDimensions CreateUpdatedDimensions(ComponentDimensions existing, UpdateComponentCommand request)
    {
        // If no dimension updates requested, keep existing
        if (string.IsNullOrEmpty(request.LengthFormula) &&
            !request.FixedLengthValue.HasValue &&
            string.IsNullOrEmpty(request.FixedLengthUnit))
        {
            return existing;
        }

        // Create new dimensions based on request
        if (!string.IsNullOrEmpty(request.LengthFormula))
        {
            if (request.FixedLengthValue.HasValue || !string.IsNullOrEmpty(request.FixedLengthUnit))
            {
                throw new InvalidOperationException("Cannot specify both length formula and fixed length");
            }
            return new ComponentDimensions(request.LengthFormula, null);
        }

        if (request.FixedLengthValue.HasValue && !string.IsNullOrEmpty(request.FixedLengthUnit))
        {
            var measure = new Measure(request.FixedLengthValue.Value, request.FixedLengthUnit);
            return new ComponentDimensions(null, measure);
        }

        // If only partial fixed length provided, keep existing or throw error
        if (request.FixedLengthValue.HasValue || !string.IsNullOrEmpty(request.FixedLengthUnit))
        {
            throw new InvalidOperationException("Both fixed length value and unit must be provided together");
        }

        return existing;
    }
}
