var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var database = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("ags-windowsdoors-db");

// Add the Web API service
var webApi = builder
    .AddProject<Projects.AGS_WindowsAndDoors_WebAPI>("ags-webapi")
    .WithReference(database)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
