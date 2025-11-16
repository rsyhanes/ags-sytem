using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.ValueObjects;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Domain.Entities;

public class ItemTests
{
    [Theory, AutoData]
    public void Create_WithValidData_ShouldCreateItem(string code, string name, string description, decimal price)
    {
        // Arrange
        var category = Category.Window;
        
        // Act
        var item = Item.Create(code, name, description, category, price);
        
        // Assert
        item.Should().NotBeNull();
        item.Code.Should().Be(code.Trim().ToUpper());
        item.Name.Should().Be(name.Trim());
        item.Description.Should().Be(description.Trim());
        item.Category.Should().Be(category);
        item.Price.Should().Be(price);
        item.State.Should().Be(ItemState.Draft);
        item.IsActive.Should().BeFalse(); // Draft items are not active
        item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        item.ModifiedAt.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidCode_ShouldThrowArgumentException(string? invalidCode)
    {
        // Arrange
        var category = Category.Window;
        
        // Act & Assert
        var act = () => Item.Create(invalidCode!, "name", "description", category, 100m);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Item code cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
    {
        // Arrange
        var category = Category.Window;
        
        // Act & Assert
        var act = () => Item.Create("CODE123", invalidName!, "description", category, 100m);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Item name cannot be empty*");
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowArgumentException()
    {
        // Arrange
        var category = Category.Window;
        
        // Act & Assert
        var act = () => Item.Create("CODE123", "name", "description", category, -100m);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Item price cannot be negative*");
    }

    [Fact]
    public void WithUpdatedDetails_ShouldReturnNewInstanceWithUpdatedValues()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem();
        var newName = "Updated Name";
        var newDescription = "Updated Description";
        var newPrice = originalItem.Price + 100;

        // Act
        var updatedItem = originalItem.WithUpdatedDetails(newName, newDescription, newPrice);

        // Assert
        updatedItem.Should().NotBeSameAs(originalItem); // Immutability check
        updatedItem.Id.Should().Be(originalItem.Id);
        updatedItem.Code.Should().Be(originalItem.Code);
        updatedItem.Name.Should().Be(newName);
        updatedItem.Description.Should().Be(newDescription);
        updatedItem.Price.Should().Be(newPrice);
        updatedItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        updatedItem.CreatedAt.Should().Be(originalItem.CreatedAt);
    }

    [Fact]
    public void WithColor_ShouldReturnNewInstanceWithColor()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem();
        var color = Color.Create("Bronze", "#CD853F");

        // Act
        var updatedItem = originalItem.WithColor(color);

        // Assert
        updatedItem.Should().NotBeSameAs(originalItem); // Immutability check
        updatedItem.Color.Should().Be(color);
        updatedItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void WithDimensions_ShouldReturnNewInstanceWithDimensions()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem();
        var dimensions = Measure.Inches(24.5m);

        // Act
        var updatedItem = originalItem.WithDimensions(dimensions);

        // Assert
        updatedItem.Should().NotBeSameAs(originalItem); // Immutability check
        updatedItem.Dimensions.Should().Be(dimensions);
        updatedItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AsActivated_ShouldReturnNewInstanceWithActiveState()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem();

        // Act
        var activatedItem = originalItem.AsActivated();

        // Assert
        activatedItem.Should().NotBeSameAs(originalItem); // Immutability check
        activatedItem.State.Should().Be(ItemState.Active);
        activatedItem.IsActive.Should().BeTrue();
        activatedItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AsDeactivated_ShouldReturnNewInstanceWithInactiveState()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem().AsActivated();

        // Act
        var deactivatedItem = originalItem.AsDeactivated();

        // Assert
        deactivatedItem.Should().NotBeSameAs(originalItem); // Immutability check
        deactivatedItem.State.Should().Be(ItemState.Inactive);
        deactivatedItem.IsActive.Should().BeFalse();
        deactivatedItem.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Item_ShouldBeImmutable()
    {
        // Arrange
        var originalItem = ItemTestDataBuilder.ValidItem();
        var originalName = originalItem.Name;

        // Act - All domain methods should return new instances
        var updated1 = originalItem.WithUpdatedDetails("New Name", "New Desc", 200m);
        var updated2 = originalItem.AsActivated();
        var updated3 = originalItem.WithColor(Color.Create("Blue", "#0000FF"));

        // Assert - Original should be unchanged
        originalItem.Name.Should().Be(originalName);
        updated1.Should().NotBeSameAs(originalItem);
        updated2.Should().NotBeSameAs(originalItem);
        updated3.Should().NotBeSameAs(originalItem);
    }
}
