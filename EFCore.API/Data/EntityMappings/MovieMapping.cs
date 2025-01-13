using EFCore.API.Data.ValueConverters;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Update;

namespace EFCore.API.Data.EntityMappings;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {

        // TPC - Table Per Concrete Type
        builder
            .UseTpcMappingStrategy()
            .HasQueryFilter(movie => movie.IsSoftDeleted == false);

        builder.Property(movie => movie.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(movie => movie.ReleaseDate)
            .HasColumnType("char(8)")
            .HasConversion(AppValueConverters.GetDateTimeToChar8Converter());

        builder.Property(movie => movie.AgeRating)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.OwnsMany(movie => movie.Actors)
            .ToTable("MovieActors");
    }
}
