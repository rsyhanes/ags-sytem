using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;

public record ComponentDimensions
{
    public string? LengthFormula { get; init; }
    public Measure? FixedLength { get; init; }

    public ComponentDimensions()
    {
        LengthFormula = null;
        FixedLength = null;
    }

    public ComponentDimensions(string? lengthFormula, Measure? fixedLength)
    {
        if (!string.IsNullOrEmpty(lengthFormula) && fixedLength is not null)
        {
            throw new ArgumentException("Cannot specify both length formula and fixed length");
        }

        LengthFormula = lengthFormula;
        FixedLength = fixedLength;
    }

    public bool HasFormula => !string.IsNullOrEmpty(LengthFormula);
    public bool HasFixedLength => FixedLength is not null;
}
