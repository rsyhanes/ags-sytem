using MediatR;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.UpdateComponent;

public record UpdateComponentCommand(
    string ComponentId,
    string? Name = null,
    int? Quantity = null,
    string? Description = null,
    string? LengthFormula = null,
    decimal? FixedLengthValue = null,
    string? FixedLengthUnit = null,
    bool? IsRequired = null,
    int? SortOrder = null
) : IRequest<string>; // Returns component ID
