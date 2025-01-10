using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder
            .ToTable("Pictures")
            .HasKey(movie => movie.Identifier);

        builder.Property(movie => movie.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(movie => movie.ReleaseDate)
            .HasColumnType("date");

        builder.Property(movie => movie.Synopsis)
            .HasColumnName("Plot")
            .HasColumnType("varchar(max)");

        builder
            .HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies)
            .HasPrincipalKey(genre => genre.Id)
            .HasForeignKey(movie => movie.MainGenreId);

        // Seeding - data that should be present always - needs migration
        builder.HasData(new Movie
        {
            Identifier = 1,
            ReleaseDate = new DateTime(1999, 9, 10),
            Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other",
            Title = "Fight Club",
            MainGenreId = 1
        });

    }
}
