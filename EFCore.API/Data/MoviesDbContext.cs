using System.Reflection;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // TPT - Table Per Type Mapping Strategy
        /*
        modelBuilder.Entity<CinemaMovie>();
        modelBuilder.Entity<TelevisionMovie>();
        */

        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Movie> Movies { get; init; }
    public required DbSet<Genre> Genres { get; init; }

}