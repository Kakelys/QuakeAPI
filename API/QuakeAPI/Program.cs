
using QuakeAPI.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositoryService(builder.Configuration);

var app = builder.Build();

app.Run();
