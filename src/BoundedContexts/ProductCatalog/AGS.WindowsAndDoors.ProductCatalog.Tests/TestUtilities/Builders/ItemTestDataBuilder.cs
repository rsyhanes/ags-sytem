using AutoFixture;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.ValueObjects;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;

/// <summary>
/// Test data builder for Item entities.
/// Follows tests.setup rule: Use AutoFixture for test setup.
/// </summary>
public class ItemTestDataBuilder
{
    private readonly Fixture _fixture;

    public ItemTestDataBuilder()
    {
        _fixture = new Fixture();

        // Configure AutoFixture for domain objects
        _fixture.Register(() => ItemState.Active);
        _fixture.Register(() => Color.Create("Bronze", "#CD853F"));
        _fixture.Register(() => Measure.Inches(10.5m));
    }

    public static ItemTestDataBuilder New() => new();

    public Item Build()
    {
        var code = _fixture.Create<string>().Substring(0, Math.Min(8, _fixture.Create<string>().Length)).ToUpper();
        var name = _fixture.Create<string>();
        var description = _fixture.Create<string>();
        var price = Math.Abs(_fixture.Create<decimal>()) % 1000; // Keep price reasonable

        return Item.Create(code, name, description, price);
    }

    public Item WithCode(string code)
    {
        var item = Build();
        return Item.Create(code, item.Name, item.Description, item.Price);
    }

    public Item WithName(string name)
    {
        var item = Build();
        return Item.Create(item.Code, name, item.Description, item.Price);
    }

    public Item WithPrice(decimal price)
    {
        var item = Build();
        return Item.Create(item.Code, item.Name, item.Description, price);
    }

    public Item WithColor(Color color)
    {
        var item = Build();
        return item.WithColor(color);
    }

    public Item WithDimensions(Measure dimensions)
    {
        var item = Build();
        return item.WithDimensions(dimensions);
    }

    public Item Activated()
    {
        return Build().AsActivated();
    }

    public Item Deactivated()
    {
        return Build().AsDeactivated();
    }

    public static Item ValidItem() => New().Build();

    public static Item ValidItemWithCode(string code) => New().WithCode(code);
}
