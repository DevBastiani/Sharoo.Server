using Sharoo.Server.API.Extensions;
using Sharoo.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddArchitectures();
builder.AddModules();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseArchitectures();

app.Services.SeedDatabase();

app.Run();