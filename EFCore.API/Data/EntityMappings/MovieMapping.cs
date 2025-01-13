using EFCore.API.Data.ValueConverters;
using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {

        builder.HasKey(movie => new { movie.Title, movie.ReleaseDate });

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

        builder
            .HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies)
            .HasPrincipalKey(genre => genre.Id)
            .HasForeignKey(movie => movie.GenreId);

        builder
            .ComplexProperty(movie => movie.Director);

        builder
            .OwnsMany(movie => movie.Actors)
            .ToTable("MovieActors");

        // One-to-One modelling as owned types
        builder
            .OwnsOne(movie => movie.ExternalInformation);

    }
}
