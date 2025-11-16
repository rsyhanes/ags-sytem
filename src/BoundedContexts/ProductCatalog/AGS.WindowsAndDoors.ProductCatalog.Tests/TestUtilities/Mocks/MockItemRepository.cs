using AGS.WindowsAndDoors.ProductCatalog.Domain.Entities;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using NSubstitute;

namespace AGS.WindowsAndDoors.ProductCatalog.Tests.TestUtilities.Mocks;

/// <summary>
/// Mock implementation of IItemRepositoryPort for testing.
/// Follows tests.mocks rule: Use NSubstitute as mocking library.
/// </summary>
public static class MockItemRepository
{
    public static IItemRepositoryPort Create()
    {
        return Substitute.For<IItemRepositoryPort>();
    }

    public static IItemRepositoryPort WithExistingItem(Item item)
    {
        var mock = Create();
        mock.FindByCodeAsync(item.Code, Arg.Any<CancellationToken>())
            .Returns(item);
        mock.ExistsByCodeAsync(item.Code, Arg.Any<CancellationToken>())
            .Returns(true);
        return mock;
    }

    public static IItemRepositoryPort WithNoItems()
    {
        var mock = Create();
        mock.FindByCodeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Item?)null);
        mock.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);
        mock.FindAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Item>().AsReadOnly());
        return mock;
    }

    public static IItemRepositoryPort WithItems(params Item[] items)
    {
        var mock = Create();
        
        foreach (var item in items)
        {
            mock.FindByCodeAsync(item.Code, Arg.Any<CancellationToken>())
                .Returns(item);
            mock.ExistsByCodeAsync(item.Code, Arg.Any<CancellationToken>())
                .Returns(true);
        }

        mock.FindAllAsync(Arg.Any<CancellationToken>())
            .Returns(items.ToList().AsReadOnly());

        return mock;
    }

    public static IItemRepositoryPort ThatSavesSuccessfully(this IItemRepositoryPort mock)
    {
        mock.SaveAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.ArgAt<Item>(0));
        return mock;
    }
}
