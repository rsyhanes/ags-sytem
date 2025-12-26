using MediatR;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.ListComponents;

namespace AGS.WindowsAndDoors.ProductDesign.Application.UseCases.GetComponent;

public record GetComponentQuery(string ComponentId) : IRequest<ComponentDto?>;
