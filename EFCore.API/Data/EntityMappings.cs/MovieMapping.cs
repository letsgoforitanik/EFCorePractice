using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings.cs;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {

        builder.Property(movie => movie.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();

        // Changed column datatype from datetime to char(23)
        // However we will not run into any problem when 
        // running the application, because EF Core ships with
        // built-in value converter and for this use case
        // EF Core has a DateTimeToStringConverter, which is 
        // used in this case.
        // Read : https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations
        builder.Property(movie => movie.ReleaseDate)
            .HasColumnType("char(23)");

        /*
        // Foreign Key
        builder
            .HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies)
            .HasPrincipalKey(genre => genre.Id)
            .HasForeignKey(movie => movie.GenreId);
        */


    }
}
