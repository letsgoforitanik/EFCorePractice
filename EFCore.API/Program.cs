using System.Text.Json.Serialization;
using EFCore.API;
using EFCore.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllers();

mvcBuilder.AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<MoviesDbContext>(opt => opt.UseSqlServer(builder.Configuration["DbConnectionString"]));

var app = builder.Build();

DbInitializer.Initialize(app);

app.MapControllers();
app.Run();
