using System.Reflection;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private void SetBaseEntityTimestamps()
    {
        var baseEntityEntries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in baseEntityEntries)
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetBaseEntityTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetBaseEntityTimestamps();
        return base.SaveChanges();
    }

    public required DbSet<Movie> Movies { get; init; }
    public required DbSet<Genre> Genres { get; init; }
    public required DbSet<Actor> Actors { get; init; }
}