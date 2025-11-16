using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

public class SystemComponent : IEntity<string>
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ItemCode { get; private set; }
    public Category Category { get; private set; }
    public int Quantity { get; private set; }
    public string? LengthFormula { get; private set; }
    public Measure? FixedLength { get; private set; }
    public bool IsRequired { get; private set; }
    public int SortOrder { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    private SystemComponent() { } // EF Constructor

    public SystemComponent(string name, string description, string itemCode, Category category, 
        int quantity = 1, bool isRequired = true, int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("Item code cannot be empty", nameof(itemCode));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Id = Guid.NewGuid().ToString();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        ItemCode = itemCode.Trim().ToUpper();
        Category = category;
        Quantity = quantity;
        IsRequired = isRequired;
        SortOrder = sortOrder;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Component name cannot be empty", nameof(name));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Quantity = quantity;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetLengthFormula(string formula)
    {
        LengthFormula = formula?.Trim();
        FixedLength = null; // Clear fixed length when formula is set
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetFixedLength(Measure length)
    {
        FixedLength = length;
        LengthFormula = null; // Clear formula when fixed length is set
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetRequired(bool isRequired)
    {
        IsRequired = isRequired;
        ModifiedAt = DateTime.UtcNow;
    }
}
