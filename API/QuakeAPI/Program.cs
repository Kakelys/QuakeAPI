using QuakeAPI.Data.Repository;
using QuakeAPI.Middlewares;
using QuakeAPI.Extensions;
using QuakeAPI.Services;
using QuakeAPI.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using QuakeAPI.Options;
using QuakeAPI.Extensions.Mapper;
using QuakeAPI.Mongo;

var builder = WebApplication.CreateBuilder(args);

//logging
builder.Logging.AddConsole();
builder.Logging.AddMongoLogger();
 
//options
builder.Services.AddAppOptions(builder.Configuration);

//auto mapper
builder.Services.ConfigureAutoMapper();

//telegram bot
builder.Services.ConfigureTelegramBot(builder.Configuration);

//repository
builder.Services.AddRepositoryService(builder.Configuration);

//mongodb
builder.Services.AddSingleton(builder.Configuration);

//services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAnalyticService, AnalyticService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

//account timed service
builder.Services.AddHostedService<AccountTimedHostedService>();
builder.Services.AddScoped<IAccountTimedService, AccountTimedService>();

//swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QuakeAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Description = @"Example: 'Bearer eyASDsddw....'",
         Name = "Authorization",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.ApiKey,
         Scheme = "Bearer"
       });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
      });
});

//validators
builder.Services.AddValidatorService();

//auth
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//check folders for locations and posters
var LOCATION_PATH = builder.Environment.WebRootPath + @"\Locations\";
var POSTER_PATH = builder.Environment.WebRootPath + @"\Posters\";

if(!Directory.Exists(LOCATION_PATH))
    Directory.CreateDirectory(LOCATION_PATH);

if(!Directory.Exists(POSTER_PATH))
    Directory.CreateDirectory(POSTER_PATH);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ExceptionLoggerMiddleware>();

//static files
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
