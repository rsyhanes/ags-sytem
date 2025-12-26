using AutoFixture;
using NSubstitute;
using Xunit;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.AddComponent;
using ProductDesignEntities = AGS.WindowsAndDoors.ProductDesign.Domain.Entities;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using ProductDesignValueObjects = AGS.WindowsAndDoors.ProductDesign.Domain.ValueObjects;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.SharedKernel.Testing;

namespace AGS.WindowsAndDoors.ProductDesign.Tests.Application.UseCases;

public class AddComponentCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().WithDefaultCustomizations();

    [Fact]
    public async Task AddComponent_HappyPath_Should_SaveComponentAndReturnId()
    {
        // Arrange
        var systemCode = "WF00";
        var itemCode = "2103";
        var command = new AddComponentCommand(
            SystemCode: systemCode,
            ItemCode: itemCode,
            Name: "Frame Vertical",
            Quantity: 2,
            LengthFormula: "frame.Height");

        var mockSystemRepository = _fixture.Create<ISystemRepositoryPort>();
        var mockComponentRepository = _fixture.Create<ISystemComponentRepositoryPort>();
        var mockItemRepository = _fixture.Create<IItemRepositoryPort>();

        // Setup mocks
        mockSystemRepository.GetByCodeAsync(systemCode, Arg.Any<CancellationToken>())
            .Returns(new ProductDesignEntities.System(systemCode, "Window Frame", "Window frame system", ProductDesignValueObjects.Category.Window));

        mockItemRepository.ExistsByCodeAsync(itemCode, Arg.Any<CancellationToken>())
            .Returns(true);

        mockComponentRepository.ExistsBySystemAndItemAsync(systemCode, itemCode, Arg.Any<CancellationToken>())
            .Returns(false);

        mockComponentRepository.SaveAsync(Arg.Any<ProductDesignEntities.SystemComponent>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<ProductDesignEntities.SystemComponent>());

        var handler = new AddComponentCommandHandler(
            mockSystemRepository,
            mockComponentRepository,
            mockItemRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        await mockSystemRepository.Received(1).GetByCodeAsync(systemCode, Arg.Any<CancellationToken>());
        await mockItemRepository.Received(1).ExistsByCodeAsync(itemCode, Arg.Any<CancellationToken>());
        await mockComponentRepository.Received(1).ExistsBySystemAndItemAsync(systemCode, itemCode, Arg.Any<CancellationToken>());
        await mockComponentRepository.Received(1).SaveAsync(Arg.Is<ProductDesignEntities.SystemComponent>(c =>
            c.SystemCode == systemCode &&
            c.ItemCode == itemCode &&
            c.Name == "Frame Vertical" &&
            c.Quantity == 2 &&
            c.Dimensions.LengthFormula == "frame.Height"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddComponent_SystemNotFound_Should_ThrowException()
    {
        // Arrange
        var command = new AddComponentCommand(
            SystemCode: "INVALID",
            ItemCode: "2103",
            Name: "Test Component",
            Quantity: 1);

        var mockSystemRepository = _fixture.Create<ISystemRepositoryPort>();
        var mockComponentRepository = _fixture.Create<ISystemComponentRepositoryPort>();
        var mockItemRepository = _fixture.Create<IItemRepositoryPort>();

        mockSystemRepository.GetByCodeAsync("INVALID", Arg.Any<CancellationToken>())
            .Returns((ProductDesignEntities.System?)null);

        var handler = new AddComponentCommandHandler(
            mockSystemRepository,
            mockComponentRepository,
            mockItemRepository);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("System with code 'INVALID' not found", exception.Message);
    }

    [Fact]
    public async Task AddComponent_ItemNotFound_Should_ThrowException()
    {
        // Arrange
        var command = new AddComponentCommand(
            SystemCode: "WF00",
            ItemCode: "INVALID",
            Name: "Test Component",
            Quantity: 1);

        var mockSystemRepository = _fixture.Create<ISystemRepositoryPort>();
        var mockComponentRepository = _fixture.Create<ISystemComponentRepositoryPort>();
        var mockItemRepository = _fixture.Create<IItemRepositoryPort>();

        mockSystemRepository.GetByCodeAsync("WF00", Arg.Any<CancellationToken>())
            .Returns(new ProductDesignEntities.System("WF00", "Window Frame", "Window frame system", ProductDesignValueObjects.Category.Window));

        mockItemRepository.ExistsByCodeAsync("INVALID", Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new AddComponentCommandHandler(
            mockSystemRepository,
            mockComponentRepository,
            mockItemRepository);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("Item with code 'INVALID' not found", exception.Message);
    }

    [Fact]
    public async Task AddComponent_DuplicateItem_Should_ThrowException()
    {
        // Arrange
        var systemCode = "WF00";
        var itemCode = "2103";
        var command = new AddComponentCommand(
            SystemCode: systemCode,
            ItemCode: itemCode,
            Name: "Frame Vertical",
            Quantity: 2);

        var mockSystemRepository = _fixture.Create<ISystemRepositoryPort>();
        var mockComponentRepository = _fixture.Create<ISystemComponentRepositoryPort>();
        var mockItemRepository = _fixture.Create<IItemRepositoryPort>();

        mockSystemRepository.GetByCodeAsync(systemCode, Arg.Any<CancellationToken>())
            .Returns(new ProductDesignEntities.System(systemCode, "Window Frame", "Window frame system", ProductDesignValueObjects.Category.Window));

        mockItemRepository.ExistsByCodeAsync(itemCode, Arg.Any<CancellationToken>())
            .Returns(true);

        mockComponentRepository.ExistsBySystemAndItemAsync(systemCode, itemCode, Arg.Any<CancellationToken>())
            .Returns(true); // Duplicate exists

        var handler = new AddComponentCommandHandler(
            mockSystemRepository,
            mockComponentRepository,
            mockItemRepository);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Contains("already exists in system", exception.Message);
    }
}
