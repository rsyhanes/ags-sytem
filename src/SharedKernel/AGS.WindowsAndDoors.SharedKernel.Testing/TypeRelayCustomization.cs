using AutoFixture;
using AutoFixture.Kernel;

namespace AGS.WindowsAndDoors.SharedKernel.Testing;

/// <summary>
/// TypeRelayCustomization specifically meant to mock infrastructure.
/// E.g:
/// public class HappyItemRepositoryCustomization : TypeRelayCustomization<IItemRepositoryPort, HappyItemRepository>;
/// </summary>
/// <typeparam name="TBase"></typeparam>
/// <typeparam name="TDerived"></typeparam>
public class TypeRelayCustomization<TBase, TDerived> : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(
            // TypeRelay is basically a mapper
            // it maps request for typeof(TBase) ("from")
            // to a request for typeof(TDerived) ("to")
            new TypeRelay(
                typeof(TBase),
                typeof(TDerived)));
    }
}