using EFCore.API.Data;
using EFCore.API.Models;

namespace EFCore.API;

public static class DbInitializer
{
    public static void Initialize(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        Seed(db);

    }

    private static void Seed(MoviesDbContext db)
    {
        if (!db.Genres.Any())
        {
            db.Genres.Add(new Genre { Name = "Drama" });
            db.SaveChanges();
        }

        if (!db.Movies.Any())
        {
            var genre = db.Genres.FirstOrDefault()!;

            db.Movies.Add(new Movie
            {
                ReleaseDate = new DateTime(1999, 9, 10),
                Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other",
                Title = "Fight Club",
                GenreId = genre.Id,
            });

            db.SaveChanges();
        }

    }
}