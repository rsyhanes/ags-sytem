using MediatR;
using AGS.WindowsAndDoors.ProductDesign.Domain.Ports;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.RemoveComponent;

public class RemoveComponentCommandHandler : IRequestHandler<RemoveComponentCommand>
{
    private readonly ISystemComponentRepositoryPort _componentRepository;

    public RemoveComponentCommandHandler(ISystemComponentRepositoryPort componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task Handle(RemoveComponentCommand request, CancellationToken ct)
    {
        // Get existing component
        var existingComponent = await _componentRepository.GetByIdAsync(request.ComponentId, ct);
        if (existingComponent is null)
        {
            throw new InvalidOperationException($"Component with id '{request.ComponentId}' not found");
        }

        // Check if component is required - cannot remove required components
        if (existingComponent.IsRequired)
        {
            throw new InvalidOperationException($"Cannot remove required component '{existingComponent.Name}' from system '{existingComponent.SystemCode}'");
        }

        // Delete the component
        await _componentRepository.DeleteAsync(request.ComponentId, ct);
    }
}
