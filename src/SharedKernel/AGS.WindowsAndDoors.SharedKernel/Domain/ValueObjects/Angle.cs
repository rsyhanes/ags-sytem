namespace AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

public record Angle(decimal Degrees)
{
    public static Angle Right => new(90m);
    public static Angle Straight => new(180m);
    public static Angle Zero => new(0m);

    public static Angle Create(decimal degrees)
    {
        var normalizedDegrees = degrees % 360m;
        if (normalizedDegrees < 0)
            normalizedDegrees += 360m;
        
        return new Angle(normalizedDegrees);
    }

    public decimal Radians => Degrees * (decimal)(Math.PI / 180);
    
    public bool IsRight => Math.Abs(Degrees - 90m) < 0.01m;
    public bool IsStraight => Math.Abs(Degrees - 180m) < 0.01m;
    public bool IsAcute => Degrees > 0m && Degrees < 90m;
    public bool IsObtuse => Degrees > 90m && Degrees < 180m;
}
