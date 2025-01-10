using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
{
    public required DbSet<Movie> Movies { get; init; }
    public required DbSet<Genre> Genres { get; init; }
}