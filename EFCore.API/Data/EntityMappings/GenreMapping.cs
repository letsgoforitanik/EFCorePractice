using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings;

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

    }
}
