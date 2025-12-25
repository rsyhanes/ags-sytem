namespace AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;

public record SizeConstraints
{
    public decimal MinHeight { get; init; }
    public decimal MaxHeight { get; init; }
    public decimal MinWidth { get; init; }
    public decimal MaxWidth { get; init; }

    private SizeConstraints() { }

    public static SizeConstraints Create(decimal minHeight, decimal maxHeight, decimal minWidth, decimal maxWidth)
    {
        if (minHeight < 0)
            throw new ArgumentException("Minimum height cannot be negative", nameof(minHeight));
        if (maxHeight <= minHeight)
            throw new ArgumentException("Maximum height must be greater than minimum height", nameof(maxHeight));
        if (minWidth < 0)
            throw new ArgumentException("Minimum width cannot be negative", nameof(minWidth));
        if (maxWidth <= minWidth)
            throw new ArgumentException("Maximum width must be greater than minimum width", nameof(maxWidth));

        return new SizeConstraints
        {
            MinHeight = minHeight,
            MaxHeight = maxHeight,
            MinWidth = minWidth,
            MaxWidth = maxWidth
        };
    }
}
