using EFCore.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MoviesDbContext>(opt => opt.UseSqlServer(builder.Configuration["DbConnectionString"]));

var app = builder.Build();
app.MapControllers();
app.Run();
