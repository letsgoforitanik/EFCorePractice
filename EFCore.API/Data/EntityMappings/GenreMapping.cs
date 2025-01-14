using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings;

// Optimistic Locking assumes that conflicts rarely occur
// When data is fetched it fetches the ConcurrencyToken
// When date is about to be updated, the current 
// concurrency token is compared with the fetched token
// and the update only takes place if both versions are 
// same.

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Add unique key Genre.Name
        builder.HasAlternateKey(genre => genre.Name);

        builder.Property(genre => genre.Name)
            .HasDefaultValue("NA");

        builder.Property(genre => genre.CreatedAt)
            .HasDefaultValueSql("getdate()");

        builder.Property(genre => genre.UpdatedAt)
            .HasDefaultValueSql("getdate()");

        // Shadow Property
        // This doesn't exist on model but will
        // exist in database table 'Genre'
        // Used to implement soft delete
        builder.Property<bool>("IsDeleted")
            .HasDefaultValue(false);

        // Query filter that uses Shadow Property
        // 'IsDeleted' to show only the records
        // that haven't been soft deleted
        builder
            .HasQueryFilter(genre => EF.Property<bool>(genre, "IsDeleted") == false);

        // Optimistic Locking
        // Using RowVersion
        builder.Property(movie => movie.ConcurrencyToken)
            .IsRowVersion();

    }
}
