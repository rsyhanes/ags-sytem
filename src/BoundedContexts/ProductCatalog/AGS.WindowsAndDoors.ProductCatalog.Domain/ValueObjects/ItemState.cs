namespace AGS.WindowsAndDoors.ProductCatalog.Domain.ValueObjects;

public abstract record ItemState
{
    private static readonly ItemState _active = new ActiveState();
    private static readonly ItemState _inactive = new InactiveState();
    private static readonly ItemState _draft = new DraftState();

    public static ItemState Active => _active;
    public static ItemState Inactive => _inactive;
    public static ItemState Draft => _draft;

    public abstract string Value { get; }

    private sealed record ActiveState : ItemState
    {
        public override string Value => "Active";
    }

    private sealed record InactiveState : ItemState
    {
        public override string Value => "Inactive";
    }

    private sealed record DraftState : ItemState
    {
        public override string Value => "Draft";
    }

    public static ItemState FromString(string value)
    {
        return value?.Trim() switch
        {
            "Active" => Active,
            "Inactive" => Inactive,
            "Draft" => Draft,
            _ => throw new ArgumentException($"Invalid item state: {value}", nameof(value))
        };
    }

    public bool IsActive => this is ActiveState;
    public bool IsInactive => this is InactiveState;
    public bool IsDraft => this is DraftState;
}
