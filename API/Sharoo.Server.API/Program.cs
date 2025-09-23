using Sharoo.Server.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddArchitectures();
builder.Services.AddModules();

var app = builder.Build();

app.UseArchitectures();

app.Run();