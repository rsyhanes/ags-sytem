using MediatR;
using System.Net;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.AddComponent;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.UpdateComponent;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.RemoveComponent;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.ListComponents;
using AGS.WindowsAndDoors.ProductDesign.Application.UseCases.GetComponent;
using AGS.WindowsAndDoors.SharedKernel.Domain.ValueObjects;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.WebAPI;

public record CreateComponentRequest(
    string ItemCode,
    string Name,
    int Quantity,
    string? Description = null,
    string? LengthFormula = null,
    decimal? FixedLengthValue = null,
    string? FixedLengthUnit = null,
    bool IsRequired = true,
    int SortOrder = 0
);

public record UpdateComponentRequest(
    string? Name = null,
    int? Quantity = null,
    string? Description = null,
    string? LengthFormula = null,
    decimal? FixedLengthValue = null,
    string? FixedLengthUnit = null,
    bool? IsRequired = null,
    int? SortOrder = null
);

public record ComponentResponse(
    string Id,
    string SystemCode,
    string ItemCode,
    string Name,
    string Description,
    int Quantity,
    ComponentDimensionsResponse? Dimensions,
    bool IsRequired,
    int SortOrder,
    DateTime CreatedAt
);

public record ComponentDimensionsResponse(
    string? LengthFormula,
    MeasureResponse? FixedLength
);

public record MeasureResponse(decimal Value, string Unit);

public static class ComponentsEndpoints
{
    public static void MapComponentsEndpoints(this IEndpointRouteBuilder app)
    {
        var components = app.MapGroup("/api").WithTags("Components");

        // POST /api/systems/{code}/components
        components.MapPost("/systems/{code}/components", AddComponent)
            .WithName("AddComponent")
            .WithOpenApi();

        // GET /api/systems/{code}/components
        components.MapGet("/systems/{code}/components", GetComponents)
            .WithName("GetComponents")
            .WithOpenApi();

        // GET /api/components/{id}
        components.MapGet("/components/{id}", GetComponent)
            .WithName("GetComponent")
            .WithOpenApi();

        // PUT /api/components/{id}
        components.MapPut("/components/{id}", UpdateComponent)
            .WithName("UpdateComponent")
            .WithOpenApi();

        // DELETE /api/components/{id}
        components.MapDelete("/components/{id}", DeleteComponent)
            .WithName("DeleteComponent")
            .WithOpenApi();
    }

    private static async Task<IResult> AddComponent(string code, CreateComponentRequest request, IMediator mediator)
    {
        try
        {
            var command = new AddComponentCommand(
                SystemCode: code,
                ItemCode: request.ItemCode,
                Name: request.Name,
                Quantity: request.Quantity,
                Description: request.Description,
                LengthFormula: request.LengthFormula,
                FixedLengthValue: request.FixedLengthValue,
                FixedLengthUnit: request.FixedLengthUnit,
                IsRequired: request.IsRequired,
                SortOrder: request.SortOrder
            );

            var componentId = await mediator.Send(command);
            return Results.Created($"/api/components/{componentId}", new { id = componentId });
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "Validation Error"
            );
        }
        catch (BusinessRuleViolationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.Conflict,
                title: "Business Rule Violation",
                type: ex.RuleName
            );
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> GetComponents(string code, IMediator mediator)
    {
        try
        {
            var query = new ListComponentsQuery(code);
            var components = await mediator.Send(query);

            var responses = components.Select(MapToResponse).ToList();
            return Results.Ok(responses);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> GetComponent(string id, IMediator mediator)
    {
        try
        {
            var query = new GetComponentQuery(id);
            var component = await mediator.Send(query);

            if (component is null)
            {
                return Results.NotFound($"Component with id '{id}' not found");
            }

            var response = MapToResponse(component);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> UpdateComponent(string id, UpdateComponentRequest request, IMediator mediator)
    {
        try
        {
            var command = new UpdateComponentCommand(
                ComponentId: id,
                Name: request.Name,
                Quantity: request.Quantity,
                Description: request.Description,
                LengthFormula: request.LengthFormula,
                FixedLengthValue: request.FixedLengthValue,
                FixedLengthUnit: request.FixedLengthUnit,
                IsRequired: request.IsRequired,
                SortOrder: request.SortOrder
            );

            var componentId = await mediator.Send(command);
            return Results.Ok(new { id = componentId });
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "Validation Error"
            );
        }
        catch (BusinessRuleViolationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.Conflict,
                title: "Business Rule Violation",
                type: ex.RuleName
            );
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> DeleteComponent(string id, IMediator mediator)
    {
        try
        {
            var command = new RemoveComponentCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: "Validation Error"
            );
        }
        catch (BusinessRuleViolationException ex)
        {
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.Conflict,
                title: "Business Rule Violation",
                type: ex.RuleName
            );
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static ComponentResponse MapToResponse(ComponentDto dto)
    {
        return new ComponentResponse(
            Id: dto.Id,
            SystemCode: dto.SystemCode,
            ItemCode: dto.ItemCode,
            Name: dto.Name,
            Description: dto.Description,
            Quantity: dto.Quantity,
            Dimensions: MapDimensions(dto.Dimensions),
            IsRequired: dto.IsRequired,
            SortOrder: dto.SortOrder,
            CreatedAt: dto.CreatedAt
        );
    }

    private static ComponentDimensionsResponse MapDimensions(ComponentDimensionsDto dimensions)
    {
        MeasureResponse? fixedLength = null;
        if (dimensions.FixedLength is not null)
        {
            fixedLength = new MeasureResponse(dimensions.FixedLength.Value, dimensions.FixedLength.Unit);
        }

        return new ComponentDimensionsResponse(
            LengthFormula: dimensions.LengthFormula,
            FixedLength: fixedLength
        );
    }
}
