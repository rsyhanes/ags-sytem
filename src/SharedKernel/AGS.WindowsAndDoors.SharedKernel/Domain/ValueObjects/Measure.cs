namespace AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

public record Measure(decimal Value, string Unit)
{
    public static Measure Inches(decimal value) => new(value, "in");
    public static Measure Millimeters(decimal value) => new(value, "mm");
    public static Measure Feet(decimal value) => new(value, "ft");

    public static Measure Create(decimal value, string unit)
    {
        if (value < 0)
            throw new ArgumentException("Measure value cannot be negative", nameof(value));
        
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty", nameof(unit));

        return new Measure(value, unit.Trim().ToLower());
    }

    public Measure ConvertTo(string targetUnit)
    {
        return targetUnit.ToLower() switch
        {
            "in" when Unit == "mm" => new(Value / 25.4m, "in"),
            "mm" when Unit == "in" => new(Value * 25.4m, "mm"),
            "ft" when Unit == "in" => new(Value / 12m, "ft"),
            "in" when Unit == "ft" => new(Value * 12m, "in"),
            _ when Unit == targetUnit.ToLower() => this,
            _ => throw new NotSupportedException($"Conversion from {Unit} to {targetUnit} is not supported")
        };
    }
}
