using FluentAssertions;
using Xunit;
using AGS.WindowsAndDoors.ProductCatalog.Domain.ValueObjects;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Domain.ValueObjects;

public class ItemStateTests
{
    [Fact]
    public void Active_ShouldReturnActiveState()
    {
        // Act
        var state = ItemState.Active;
        
        // Assert
        state.Value.Should().Be("Active");
        state.IsActive.Should().BeTrue();
        state.IsInactive.Should().BeFalse();
        state.IsDraft.Should().BeFalse();
    }

    [Fact]
    public void Inactive_ShouldReturnInactiveState()
    {
        // Act
        var state = ItemState.Inactive;
        
        // Assert
        state.Value.Should().Be("Inactive");
        state.IsActive.Should().BeFalse();
        state.IsInactive.Should().BeTrue();
        state.IsDraft.Should().BeFalse();
    }

    [Fact]
    public void Draft_ShouldReturnDraftState()
    {
        // Act
        var state = ItemState.Draft;
        
        // Assert
        state.Value.Should().Be("Draft");
        state.IsActive.Should().BeFalse();
        state.IsInactive.Should().BeFalse();
        state.IsDraft.Should().BeTrue();
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Inactive")]
    [InlineData("Draft")]
    public void FromString_WithValidValue_ShouldReturnCorrectState(string value)
    {
        // Act
        var state = ItemState.FromString(value);
        
        // Assert
        state.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Invalid")]
    [InlineData("active")]  // case sensitive
    [InlineData(null)]
    public void FromString_WithInvalidValue_ShouldThrowArgumentException(string? invalidValue)
    {
        // Act & Assert
        var act = () => ItemState.FromString(invalidValue!);
        act.Should().Throw<ArgumentException>()
           .WithMessage($"Invalid item state: {invalidValue}*");
    }

    [Fact]
    public void FromString_WithWhitespace_ShouldTrimAndParse()
    {
        // Act
        var state = ItemState.FromString("  Active  ");
        
        // Assert
        state.Should().Be(ItemState.Active);
    }

    [Fact]
    public void States_ShouldBeImmutable()
    {
        // Arrange
        var state1 = ItemState.Active;
        var state2 = ItemState.Active;
        
        // Assert
        state1.Should().Be(state2);
        ReferenceEquals(state1, state2).Should().BeTrue(); // Singleton pattern
    }

    [Fact]
    public void States_ShouldHaveValueSemantics()
    {
        // Arrange
        var active1 = ItemState.Active;
        var active2 = ItemState.Active;
        var inactive = ItemState.Inactive;
        
        // Assert
        (active1 == active2).Should().BeTrue();
        (active1 != inactive).Should().BeTrue();
        active1.Equals(active2).Should().BeTrue();
        active1.Equals(inactive).Should().BeFalse();
    }
}
