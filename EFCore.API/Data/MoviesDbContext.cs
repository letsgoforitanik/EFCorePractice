using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .ToTable("Pictures")
            .HasKey(movie => movie.Identifier);

        modelBuilder.Entity<Movie>().Property(movie => movie.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();

        modelBuilder.Entity<Movie>().Property(movie => movie.ReleaseDate)
            .HasColumnType("date");

        modelBuilder.Entity<Movie>().Property(movie => movie.Synopsis)
            .HasColumnName("Plot")
            .HasColumnType("varchar(max)");

        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Movie> Movies { get; init; }
}