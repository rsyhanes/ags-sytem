using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;
using AGS.WindowsAndDoors.ProductCatalog.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;

public record Item : IEntity<string>
{
    public string Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public Category Category { get; init; }
    public decimal Price { get; init; }
    public Color? Color { get; init; }
    public Measure? Dimensions { get; init; }
    public ItemState State { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }

    private Item() 
    { 
        // For serialization - compiler requires initialization
        Id = string.Empty;
        Code = string.Empty;
        Name = string.Empty;
        Description = string.Empty;
        Category = Category.Window;
        State = ItemState.Draft;
    }

    private Item(string id, string code, string name, string description, Category category, 
        decimal price, ItemState state, DateTime createdAt, Color? color = null, 
        Measure? dimensions = null, DateTime? modifiedAt = null)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Color = color;
        Dimensions = dimensions;
        State = state;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
    }

    public static Item Create(string code, string name, string description, Category category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Item code cannot be empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty", nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Item price cannot be negative", nameof(price));

        return new Item(
            id: Guid.NewGuid().ToString(),
            code: code.Trim().ToUpper(),
            name: name.Trim(),
            description: description?.Trim() ?? string.Empty,
            category: category,
            price: price,
            state: ItemState.Draft,
            createdAt: DateTime.UtcNow
        );
    }

    public Item WithUpdatedDetails(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name cannot be empty", nameof(name));
        
        if (price < 0)
            throw new ArgumentException("Item price cannot be negative", nameof(price));

        return this with
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public Item WithColor(Color color)
    {
        return this with
        {
            Color = color,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public Item WithDimensions(Measure dimensions)
    {
        return this with
        {
            Dimensions = dimensions,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public Item AsActivated()
    {
        return this with
        {
            State = ItemState.Active,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public Item AsDeactivated()
    {
        return this with
        {
            State = ItemState.Inactive,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public Item WithState(ItemState state)
    {
        return this with
        {
            State = state,
            ModifiedAt = DateTime.UtcNow
        };
    }

    // Convenience properties for backward compatibility
    public bool IsActive => State.IsActive;
}
