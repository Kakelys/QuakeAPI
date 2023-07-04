using QuakeAPI.Data.Repository;
using QuakeAPI.Middlewares;
using QuakeAPI.Extensions;
using QuakeAPI.Services;
using QuakeAPI.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

//logging
builder.Logging.AddConsole();

//repository
builder.Services.AddRepositoryService(builder.Configuration);

//services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();

//swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QuakeAPI", Version = "v1" });
});

//auth
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ExceptionLoggerMiddleware>();

var staticFilesProvider = new FileExtensionContentTypeProvider();
staticFilesProvider.Mappings[".loc"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = staticFilesProvider
});

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
