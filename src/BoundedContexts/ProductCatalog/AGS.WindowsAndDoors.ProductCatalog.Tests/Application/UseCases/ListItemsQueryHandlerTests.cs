using FluentAssertions;
using NSubstitute;
using Xunit;
using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.ListItems;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Mocks;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Application.UseCases;

public class ListItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ListItemsHappyPath_ShouldReturnAllItems()
    {
        // Arrange - Covers spec scenario: list-items-happy-path
        var items = new[]
        {
            ItemTestDataBuilder.ValidItemWithCode("2103"),
            ItemTestDataBuilder.ValidItemWithCode("0040"),
            ItemTestDataBuilder.ValidItemWithCode("0001")
        };
        var repository = MockItemRepository.WithItems(items);

        var handler = new ListItemsQueryHandler(repository);
        var query = new ListItemsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Select(r => r.Code).Should().Contain(new[] { "2103", "0040", "0001" });

        // Verify repository interactions
        await repository.Received(1).FindAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ListItemsNoItems_ShouldReturnEmptyCollection()
    {
        // Arrange
        var repository = MockItemRepository.WithNoItems();

        var handler = new ListItemsQueryHandler(repository);
        var query = new ListItemsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        // Verify repository interactions
        await repository.Received(1).FindAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new ListItemsQueryHandler(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*itemRepository*");
    }
}
