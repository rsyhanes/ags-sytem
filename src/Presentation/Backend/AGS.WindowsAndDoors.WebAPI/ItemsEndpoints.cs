using MediatR;
using System.Net;
using AGS.WindowsAndDoors.ProductCatalog.Application.DTOs;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.UpdateItem;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.GetItem;
using AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.ListItems;
using AGS.WindowsAndDoors.SharedKernel.Domain.Exceptions;

namespace AGS.WindowsAndDoors.WebAPI;

public record CreateItemRequest(string Code, string Name, ItemMeasure Measure);
public record UpdateItemRequest(string? Name, ItemMeasure? Measure);
public record ItemResponse(string Code, string Name, ItemMeasure Measure);

public enum ItemMeasure
{
    Units,
    Inches
}

public static class ItemsEndpoints
{
    public static void MapItemsEndpoints(this IEndpointRouteBuilder app)
    {
        var items = app.MapGroup("/api/items").WithTags("Items");

        items.MapGet("/", ListItems)
            .WithName("ListItems")
            .WithOpenApi();

        items.MapGet("/{code}", GetItem)
            .WithName("GetItem")
            .WithOpenApi();

        items.MapPost("/", CreateItem)
            .WithName("CreateItem")
            .WithOpenApi();

        items.MapPut("/{code}", UpdateItem)
            .WithName("UpdateItem")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateItem(CreateItemRequest request, IMediator mediator)
    {
        try
        {
            var command = new CreateItemCommand(
                Code: request.Code,
                Name: request.Name,
                Description: "Description", // Default for OpenAPI simplifed schema
                Price: 0.0m, // Default price
                ColorName: null,
                ColorHex: null,
                DimensionValue: null,
                DimensionUnit: request.Measure.ToString()
            );

            var result = await mediator.Send(command);
            var response = ToItemResponse(result);
            return Results.Created($"/api/items/{response.Code}", response);
        }
        catch (BusinessRuleViolationException ex)
        {
            // Handle domain business rule violations (e.g., duplicate codes)
            return Results.Problem(
                ex.Message,
                statusCode: (int)HttpStatusCode.Conflict,
                title: "Business Rule Violation",
                type: ex.RuleName
            );
        }
        catch (Exception ex)
        {
            // Handle all other exceptions as internal server errors
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> UpdateItem(string code, UpdateItemRequest request, IMediator mediator)
    {
        try
        {
            var command = new UpdateItemCommand(
                Code: code,
                Name: request.Name ?? string.Empty, // Provide default if not specified
                Description: "Description", // Default
                Price: 0.0m, // Default price
                ColorName: null,
                ColorHex: null,
                DimensionValue: null,
                DimensionUnit: request.Measure?.ToString() ?? "Units"
            );

            var result = await mediator.Send(command);
            if (result == null)
            {
                return Results.NotFound($"Item with code '{code}' not found");
            }

            var response = ToItemResponse(result);
            return Results.Ok(response);
        }
        catch (BusinessRuleViolationException ex)
        {
            // Handle domain business rule violations
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

    private static async Task<IResult> GetItem(string code, IMediator mediator)
    {
        try
        {
            var query = new GetItemQuery(code);
            var result = await mediator.Send(query);

            if (result == null)
            {
                return Results.NotFound($"Item with code '{code}' not found");
            }

            var response = ToItemResponse(result);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task<IResult> ListItems(IMediator mediator)
    {
        try
        {
            var query = new ListItemsQuery();
            var results = await mediator.Send(query);

            var responses = results.Select(ToItemResponse).ToList();
            return Results.Ok(responses);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    private static ItemResponse ToItemResponse(ItemDto dto)
    {
        return new ItemResponse(
            dto.Code,
            dto.Name,
            dto.Dimensions?.Unit == "Inches" ? ItemMeasure.Inches : ItemMeasure.Units
        );
    }

}
