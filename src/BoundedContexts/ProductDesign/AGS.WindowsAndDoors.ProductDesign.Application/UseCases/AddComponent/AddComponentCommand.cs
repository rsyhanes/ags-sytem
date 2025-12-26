using MediatR;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.AddComponent;

public record AddComponentCommand(
    string SystemCode,
    string ItemCode,
    string Name,
    int Quantity,
    string? Description = null,
    string? LengthFormula = null,
    decimal? FixedLengthValue = null,
    string? FixedLengthUnit = null,
    bool IsRequired = true,
    int SortOrder = 0
) : IRequest<string>; // Returns component ID
