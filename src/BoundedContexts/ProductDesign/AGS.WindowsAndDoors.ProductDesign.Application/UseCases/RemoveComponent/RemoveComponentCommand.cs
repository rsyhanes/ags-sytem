using MediatR;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.RemoveComponent;

public record RemoveComponentCommand(string ComponentId) : IRequest;
