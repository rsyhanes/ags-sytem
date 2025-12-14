using FluentAssertions;
using NSubstitute;
using Xunit;
using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.GetItem;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Mocks;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Application.UseCases;

public class GetItemQueryHandlerTests
{
    [Fact]
    public async Task Handle_GetItemHappyPath_ShouldReturnItem()
    {
        // Arrange
        var existingItem = ItemTestDataBuilder.ValidItemWithCode("2103");
        var repository = MockItemRepository.WithExistingItem(existingItem);

        var handler = new GetItemQueryHandler(repository);
        var query = new GetItemQuery("2103");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Code.Should().Be("2103");
        result.Name.Should().Be(existingItem.Name);

        // Verify repository interactions
        await repository.Received(1).FindByCodeAsync("2103", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_GetItemNotFound_ShouldReturnNull()
    {
        // Arrange
        var repository = MockItemRepository.WithNoItems();

        var handler = new GetItemQueryHandler(repository);
        var query = new GetItemQuery("NONEXISTENT");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        // Verify repository interactions
        await repository.Received(1).FindByCodeAsync("NONEXISTENT", Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new GetItemQueryHandler(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*itemRepository*");
    }
}
