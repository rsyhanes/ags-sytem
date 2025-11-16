namespace AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

public record Category(string Name, string Code)
{
    public static Category Window => new("Window", "WIN");
    public static Category Door => new("Door", "DOOR");
    public static Category Frame => new("Frame", "FRAME");
    public static Category Hardware => new("Hardware", "HW");
    public static Category Glass => new("Glass", "GLASS");

    public static Category Create(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Category code cannot be empty", nameof(code));

        return new Category(name.Trim(), code.Trim().ToUpper());
    }
}
