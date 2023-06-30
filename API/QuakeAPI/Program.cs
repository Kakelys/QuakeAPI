
using QuakeAPI.Data.Repository;
using QuakeAPI.Middlewares;
using QuakeAPI.Extensions;
using QuakeAPI.Services;
using QuakeAPI.Services.Interfaces;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositoryService(builder.Configuration);

builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ISessionService, SessionsService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QuakeAPI", Version = "v1" });
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint => {
    endpoint.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI( options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "";
});

app.Run();
