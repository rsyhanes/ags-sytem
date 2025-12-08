using AGS.WindowsAndDoors.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR for handling use cases
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AGS.WindowsAndDoors.ProductCatalog.Application.UseCases.CreateItem.CreateItemCommand).Assembly);
});

// Register infrastructure services
// For Step 2 (Real Drivers + Mock Driven), use mock adapters for isolated testing
builder.Services.AddScoped<AGS.WindowsAndDoors.ProductCatalog.Domain.Ports.IItemRepositoryPort, AGS.WindowsAndDoors.ProductCatalog.Infrastructure.Mocks.MockItemRepositoryAdapter>();
// In Step 3, this would be replaced with real adapter registrations

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthorization(); // Will be enabled in Step 3 when implementing authentication

// ProductCatalog Items endpoints
app.MapItemsEndpoints();

// Systems endpoints (ProductDesign)
app.MapPost("/api/systems", () => Results.Created("/api/systems", new { message = "System created (placeholder)" }));
app.MapPut("/api/systems/{code}", (string code) => Results.Ok(new { message = $"System {code} updated (placeholder)" }));
app.MapGet("/api/systems", () => Results.Ok(new[] { new { code = "placeholder", category = "Window" } }));
app.MapGet("/api/systems/{code}", (string code) => Results.Ok(new { code, category = "Window", message = "Placeholder response" }));

// System Components endpoints (ProductDesign)
app.MapPost("/api/systems/{code}/components", (string code) => Results.Created($"/api/systems/{code}/components", new { message = $"Component configured for system {code} (placeholder)" }));
app.MapPut("/api/components/{id}", (string id) => Results.Ok(new { message = $"Component {id} updated (placeholder)" }));
app.MapGet("/api/systems/{code}/components", (string code) => Results.Ok(new[] { new { id = "placeholder", name = "Sample Component" } }));
app.MapDelete("/api/components/{id}", (string id) => Results.NoContent());
app.MapPost("/api/components/{id}/test", (string id) => Results.Ok(new { result = 0, message = $"Component {id} test calculation (placeholder)" }));

// Orders endpoints (OrderProcessing)
app.MapPost("/api/orders", () => Results.Created("/api/orders", new { id = "placeholder-order-id", message = "Order submitted (placeholder)" }));
app.MapGet("/api/orders/{id}", (string id) => Results.Ok(new { id, status = "New", message = "Placeholder response" }));
app.MapGet("/api/orders", () => Results.Ok(new[] { new { id = "placeholder", status = "New" } }));
app.MapPut("/api/orders/{id}/status", (string id) => Results.Ok(new { message = $"Order {id} status updated (placeholder)" }));

// Calculation endpoints
app.MapPost("/api/calculate-bom", () => Results.Ok(new { bomLines = new[] { new { itemCode = "placeholder", quantity = 1 } }, message = "BOM calculated (placeholder)" }));
app.MapPost("/api/validate-frame", () => Results.Ok(new { valid = true, message = "Frame validated (placeholder)" }));

app.Run();
