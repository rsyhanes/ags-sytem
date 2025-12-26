using MediatR;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.ListComponents;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.GetComponent;

public class GetComponentQueryHandler : IRequestHandler<GetComponentQuery, ComponentDto?>
{
    private readonly ISystemComponentRepositoryPort _componentRepository;

    public GetComponentQueryHandler(ISystemComponentRepositoryPort componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<ComponentDto?> Handle(GetComponentQuery request, CancellationToken ct)
    {
        // Get component by ID
        var component = await _componentRepository.GetByIdAsync(request.ComponentId, ct);

        if (component is null)
        {
            return null;
        }

        // Map to DTO using the same logic as ListComponents
        return MapToDto(component);
    }

    private static ComponentDto MapToDto(Domain.Entities.SystemComponent component)
    {
        return new ComponentDto(
            Id: component.Id,
            SystemCode: component.SystemCode,
            ItemCode: component.ItemCode,
            Name: component.Name,
            Description: component.Description,
            Quantity: component.Quantity,
            Dimensions: MapDimensions(component.Dimensions),
            IsRequired: component.IsRequired,
            SortOrder: component.SortOrder,
            CreatedAt: component.CreatedAt
        );
    }

    private static ComponentDimensionsDto MapDimensions(Domain.ValueObjects.ComponentDimensions dimensions)
    {
        MeasureDto? fixedLength = null;
        if (dimensions.HasFixedLength && dimensions.FixedLength is not null)
        {
            fixedLength = new MeasureDto(dimensions.FixedLength.Value, dimensions.FixedLength.Unit);
        }

        return new ComponentDimensionsDto(
            LengthFormula: dimensions.LengthFormula,
            FixedLength: fixedLength
        );
    }
}
