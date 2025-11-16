using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;

public class Item : IEntity<string>
{
    public string Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public decimal Price { get; private set; }
    public Color? Color { get; private set; }
    public Measure? Dimensions { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    private Item() { } // EF Constructor

    public Item(string code, string name, string description, Category category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Item code cannot be empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty", nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Item price cannot be negative", nameof(price));

        Id = Guid.NewGuid().ToString();
        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Category = category;
        Price = price;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty", nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Item price cannot be negative", nameof(price));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetColor(Color color)
    {
        Color = color;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetDimensions(Measure dimensions)
    {
        Dimensions = dimensions;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedAt = DateTime.UtcNow;
    }
}
