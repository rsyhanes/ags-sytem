using AGS.WindowsAndDoors.SharedKernel.Domain.Interfaces;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductDesign.Domain.Entities;

public class System : IAggregateRoot<string>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<SystemComponent> _components = [];

    public string Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<SystemComponent> Components => _components.AsReadOnly();

    private System() { } // EF Constructor

    public System(string code, string name, string description, Category category)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("System code cannot be empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("System name cannot be empty", nameof(name));

        Id = Guid.NewGuid().ToString();
        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Category = category;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("System name cannot be empty", nameof(name));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        ModifiedAt = DateTime.UtcNow;
    }

    public void AddComponent(SystemComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));

        _components.Add(component);
        ModifiedAt = DateTime.UtcNow;
    }

    public void RemoveComponent(string componentId)
    {
        var component = _components.FirstOrDefault(c => c.Id == componentId);
        if (component != null)
        {
            _components.Remove(component);
            ModifiedAt = DateTime.UtcNow;
        }
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

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
