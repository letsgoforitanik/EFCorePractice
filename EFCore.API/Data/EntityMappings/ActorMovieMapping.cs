using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.API.Data.EntityMappings;

public class ActorMovieMapping : IEntityTypeConfiguration<ActorMovie>
{
    public void Configure(EntityTypeBuilder<ActorMovie> builder)
    {
        builder.HasKey(item => new { item.ActorId, item.MovieId });

        builder
            .HasOne(item => item.Actor)
            .WithMany(actor => actor.Movies)
            .HasPrincipalKey(actor => actor.Id)
            .HasForeignKey(item => item.ActorId);

        builder
            .HasOne(item => item.Movie)
            .WithMany(movie => movie.Actors)
            .HasPrincipalKey(movie => movie.Id)
            .HasForeignKey(movie => movie.MovieId);

    }
}
