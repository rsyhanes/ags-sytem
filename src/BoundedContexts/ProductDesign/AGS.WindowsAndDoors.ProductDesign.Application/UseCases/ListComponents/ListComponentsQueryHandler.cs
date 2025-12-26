using MediatR;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.ListComponents;

public class ListComponentsQueryHandler : IRequestHandler<ListComponentsQuery, IReadOnlyCollection<ComponentDto>>
{
    private readonly ISystemComponentRepositoryPort _componentRepository;

    public ListComponentsQueryHandler(ISystemComponentRepositoryPort componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<IReadOnlyCollection<ComponentDto>> Handle(ListComponentsQuery request, CancellationToken ct)
    {
        // Get components for the system
        var components = await _componentRepository.GetBySystemCodeAsync(request.SystemCode, ct);

        // Sort by SortOrder as per spec requirement
        var sortedComponents = components.OrderBy(c => c.SortOrder).ToList();

        // Map to DTOs
        var dtos = sortedComponents.Select(MapToDto).ToList();

        return dtos;
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
