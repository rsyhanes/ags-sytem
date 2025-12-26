using Microsoft.Extensions.DependencyInjection;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;
using AGS.WindowsAndDoors.ProductCatalog.Domain.Ports;
using AGS.WindowsAndDoors.ProductDesign.Tests.TestUtilities.Mocks;

namespace AGS.WindowsAndDoors.ProductDesign.Tests;

public static class TestStartup
{
    public static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        // Register mock driven adapters
        services.AddScoped<ISystemRepositoryPort, MockSystemRepository>();
        services.AddScoped<ISystemComponentRepositoryPort, MockSystemComponentRepository>();
        services.AddScoped<IItemRepositoryPort, MockItemRepository>();

        // Register application services
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TestStartup).Assembly));

        return services.BuildServiceProvider();
    }
}
