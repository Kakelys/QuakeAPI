using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuakeDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuakeDb"));
});

var app = builder.Build();

app.Run();
