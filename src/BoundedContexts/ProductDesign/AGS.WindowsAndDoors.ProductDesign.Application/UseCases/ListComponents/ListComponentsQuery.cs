using MediatR;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.ListComponents;

public record ListComponentsQuery(string SystemCode) : IRequest<IReadOnlyCollection<ComponentDto>>;

public record ComponentDto(
    string Id,
    string SystemCode,
    string ItemCode,
    string Name,
    string Description,
    int Quantity,
    ComponentDimensionsDto Dimensions,
    bool IsRequired,
    int SortOrder,
    DateTime CreatedAt
);

public record ComponentDimensionsDto(
    string? LengthFormula,
    MeasureDto? FixedLength
);

public record MeasureDto(decimal Value, string Unit);
