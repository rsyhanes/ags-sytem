using FluentAssertions;
using NSubstitute;
using Xunit;
using MediatR;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.DomainEvents;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Mocks;
using AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Builders;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.Application.UseCases;

public class CreateItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreateItemHappyPath_ShouldCreateAndPersistItem()
    {
        // Arrange - Covers spec scenario: create-item-happy-path  
        var repository = MockItemRepository.WithNoItems()
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: "2103",
            Name: "Frame",
            Description: "Window frame component",
            Price: 25.50m,
            DimensionValue: 10.5m,
            DimensionUnit: "in"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("2103");
        result.Name.Should().Be("Frame");
        result.Description.Should().Be("Window frame component");
        result.Price.Should().Be(25.50m);
        result.State.Should().Be("Draft");
        result.IsActive.Should().BeFalse();
        result.Dimensions.Should().NotBeNull();
        result.Dimensions!.Value.Should().Be(10.5m);
        result.Dimensions.Unit.Should().Be("in");

        // Verify repository interactions
        await repository.Received(1).FindByCodeAsync("2103", Arg.Any<CancellationToken>());
        await repository.Received(1).SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CreateItemDuplicateCode_ShouldThrowBusinessRuleViolationException()
    {
        // Arrange - Covers spec scenario: create-item-duplicate-code
        var existingItem = ItemTestDataBuilder.ValidItemWithCode("2103");
        var repository = MockItemRepository.WithExistingItem(existingItem);
        var publisher = Substitute.For<IPublisher>();

        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: "2103",
            Name: "Frame",
            Description: "Window frame component",
            Price: 25.50m
        );

        // Act & Assert
        var act = () => handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<BusinessRuleViolationException>()
                 .WithMessage("Item with code '2103' already exists");

        // Verify no save operation attempted
        await repository.DidNotReceive().SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("", "Frame", "Description", 100.0)]
    [InlineData("   ", "Frame", "Description", 100.0)]
    [InlineData("CODE", "", "Description", 100.0)]
    [InlineData("CODE", "   ", "Description", 100.0)]
    [InlineData("CODE", "Frame", "Description", -10.0)]
    public async Task Handle_CreateItemInvalidData_ShouldThrowBusinessRuleViolationException(
        string code, string name, string description, decimal price)
    {
        // Arrange - Covers spec scenario: create-item-invalid-code
        var repository = MockItemRepository.WithNoItems();
        var publisher = Substitute.For<IPublisher>();
        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: code,
            Name: name,
            Description: description,
            Price: price
        );

        // Act & Assert
        var act = () => handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<BusinessRuleViolationException>();

        // Verify no save operation attempted
        await repository.DidNotReceive().SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CreateItemWithColor_ShouldIncludeColorInResult()
    {
        // Arrange
        var repository = MockItemRepository.WithNoItems()
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: "2103",
            Name: "Frame",
            Description: "Window frame component",
            Price: 25.50m,
            ColorName: "Bronze",
            ColorHex: "#CD853F"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Color.Should().NotBeNull();
        result.Color!.Name.Should().Be("Bronze");
        result.Color.Hex.Should().Be("#CD853F");
    }

    [Fact]
    public async Task Handle_CreateItemWithoutOptionalFields_ShouldCreateBasicItem()
    {
        // Arrange
        var repository = MockItemRepository.WithNoItems()
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: "2103",
            Name: "Frame",
            Description: "Window frame component",
            Price: 25.50m
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Color.Should().BeNull();
        result.Dimensions.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var publisher = Substitute.For<IPublisher>();
        var act = () => new CreateItemCommandHandler(null!, publisher);
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*itemRepository*");
    }

    [Fact]
    public async Task Handle_FollowsApplicationServiceRules_ShouldDelegateAndReturnDto()
    {
        // Arrange - Verifies app.delegate-only and app.dto-output rules
        var repository = MockItemRepository.WithNoItems()
                                         .ThatSavesSuccessfully();
        var publisher = Substitute.For<IPublisher>();

        var handler = new CreateItemCommandHandler(repository, publisher);
        var command = new CreateItemCommand(
            Code: "TEST",
            Name: "Test Item",
            Description: "Test Description",
            Price: 100m
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
