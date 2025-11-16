var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Items endpoints (ProductCatalog)
app.MapPost("/api/items", () => Results.Created("/api/items", new { message = "Item created (placeholder)" }));
app.MapPut("/api/items/{code}", (string code) => Results.Ok(new { message = $"Item {code} updated (placeholder)" }));
app.MapGet("/api/items", () => Results.Ok(new[] { new { code = "placeholder", name = "Sample Item" } }));
app.MapGet("/api/items/{code}", (string code) => Results.Ok(new { code, name = "Sample Item", message = "Placeholder response" }));

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
