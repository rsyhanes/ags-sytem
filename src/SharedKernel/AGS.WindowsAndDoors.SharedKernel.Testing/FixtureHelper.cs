using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace AGS.WindowsAndDoors.SharedKernel.Testing;

public static class FixtureHelper
{
    public static IFixture WithDefaultCustomizations(this IFixture fixture) => fixture.Customize(
        new CompositeCustomization(new AutoNSubstituteCustomization())
    );
    
    public static IFixture WithCustomizations(this IFixture fixture, params ICustomization[] customizations) => fixture.Customize(
        new CompositeCustomization([..customizations, new AutoNSubstituteCustomization()])    
    );
}
