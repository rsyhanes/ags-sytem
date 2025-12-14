using FluentAssertions;
using NSubstitute;
using Xunit;
using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.UpdateItem;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.DomainEvents;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Mocks;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Application.UseCases;

public class UpdateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdateItemHappyPath_ShouldUpdateAndPersistItem()
    {
        // Arrange - Covers spec scenario: update-item-happy-path
        var existingItem = ItemTestDataBuilder.ValidItemWithCode("2103");
        var repository = MockItemRepository.WithExistingItem(existingItem)
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new UpdateItemCommandHandler(repository, publisher);
        var command = new UpdateItemCommand(
            Code: "2103",
            Name: "Updated Frame",
            Description: "Updated description",
            Price: 30.00m,
            DimensionValue: 12.0m,
            DimensionUnit: "in"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("2103");
        result.Name.Should().Be("Updated Frame");
        result.Description.Should().Be("Updated description");
        result.Price.Should().Be(30.00m);
        result.Dimensions.Should().NotBeNull();
        result.Dimensions!.Value.Should().Be(12.0m);
        result.Dimensions.Unit.Should().Be("in");

        // Verify repository interactions
        await repository.Received(1).FindByCodeAsync("2103", Arg.Any<CancellationToken>());
        await repository.Received(1).SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UpdateItemNotFound_ShouldThrowBusinessRuleViolationException()
    {
        // Arrange - Covers spec scenario: update-item-not-found
        var repository = MockItemRepository.WithNoItems();
        var publisher = Substitute.For<IPublisher>();

        var handler = new UpdateItemCommandHandler(repository, publisher);
        var command = new UpdateItemCommand(
            Code: "NONEXISTENT",
            Name: "Updated Frame",
            Description: "Updated description",
            Price: 30.00m
        );

        // Act & Assert
        var act = () => handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<BusinessRuleViolationException>()
                 .WithMessage("Item with code 'NONEXISTENT' not found");

        // Verify no save operation attempted
        await repository.DidNotReceive().SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var publisher = Substitute.For<IPublisher>();
        var act = () => new UpdateItemCommandHandler(null!, publisher);
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*itemRepository*");
    }

    [Fact]
    public async Task Handle_FollowsApplicationServiceRules_ShouldDelegateAndReturnDto()
    {
        // Arrange - Verifies app.delegate-only and app.dto-output rules
        var existingItem = ItemTestDataBuilder.ValidItemWithCode("TEST");
        var repository = MockItemRepository.WithExistingItem(existingItem)
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new UpdateItemCommandHandler(repository, publisher);
        var command = new UpdateItemCommand(
            Code: "TEST",
            Name: "Updated Test Item",
            Description: "Updated Description",
            Price: 150m
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert - Verifies application layer rules compliance
        result.Should().NotBeNull(); // Returns DTO
        result.GetType().Name.Should().Be("ItemDto"); // Not domain object

        // Verifies delegation to domain port
        await repository.Received(1).FindByCodeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await repository.Received(1).SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }
}
