namespace AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

public record Color(string Name, string HexValue)
{
    public static Color White => new("White", "#FFFFFF");
    public static Color Black => new("Black", "#000000");
    public static Color Brown => new("Brown", "#8B4513");
    public static Color Clear => new("Clear", "#TRANSPARENT");

    public static Color Create(string name, string hexValue)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Color name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(hexValue))
            throw new ArgumentException("Hex value cannot be empty", nameof(hexValue));

        if (!hexValue.StartsWith('#') || hexValue.Length != 7)
            throw new ArgumentException("Hex value must be in format #RRGGBB", nameof(hexValue));

        return new Color(name.Trim(), hexValue.ToUpper());
    }
}
