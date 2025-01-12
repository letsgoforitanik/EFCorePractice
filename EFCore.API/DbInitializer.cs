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
            db.Genres.AddRange(
                new Genre { Name = "Drama" },
                new Genre { Name = "Action" },
                new Genre { Name = "Sci-Fi" }
            );

            db.SaveChanges();
        }

        if (!db.Movies.Any())
        {
            var genres = db.Genres.ToList();

            db.Movies.AddRange(
                new CinemaMovie
                {
                    Title = "Fight Club",
                    GenreId = genres[0].Id,
                    ReleaseDate = new DateTime(1999, 9, 10),
                    Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other",
                    AgeRating = AgeRating.Adolescent,
                    Director = new Person { FirstName = "David", LastName = "Fincher" },
                    Actors =
                    [
                        new() { FirstName = "Brad", LastName = "Pitt" },
                        new() { FirstName = "Edward", LastName = "Norton" },
                        new() { FirstName = "Helena", LastName = "Bonham Carter" }
                    ],
                    IsSoftDeleted = false,
                    GrossRevenue = 10
                },
                new CinemaMovie
                {
                    Title = "The Shawshank Redemption",
                    GenreId = genres[0].Id,
                    ReleaseDate = new DateTime(1994, 9, 14),
                    Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through",
                    AgeRating = AgeRating.Adult,
                    Director = new Person { FirstName = "Frank", LastName = "Darabont" },
                    Actors =
                    [
                        new() { FirstName = "Morgan", LastName = "Freeman" },
                        new() { FirstName = "Tim", LastName = "Robbins" }
                    ],
                    IsSoftDeleted = true,
                    GrossRevenue = 20,
                },
                new CinemaMovie
                {
                    Title = "The Dark Knight",
                    GenreId = genres[1].Id,
                    ReleaseDate = new DateTime(2008, 7, 18),
                    Synopsis = "When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end",
                    AgeRating = AgeRating.HighSchool,
                    Director = new Person { FirstName = "Christopher", LastName = "Nolan" },
                    Actors =
                    [
                        new() { FirstName = "Christian", LastName = "Bale" },
                        new() { FirstName = "Heath", LastName = "Ledger" }
                    ],
                    IsSoftDeleted = false,
                    GrossRevenue = 30,
                },
                new CinemaMovie
                {
                    Title = "Inception",
                    GenreId = genres[2].Id,
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Synopsis = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO",
                    AgeRating = AgeRating.All,
                    Director = new Person { FirstName = "Christopher", LastName = "Nolan" },
                    Actors =
                    [
                        new() { FirstName = "Leonardo", LastName = "DiCaprio" },
                        new() { FirstName = "Cillian", LastName = "Murphy" }
                    ],
                    IsSoftDeleted = false,
                    GrossRevenue = 40,
                },
                new TelevisionMovie
                {
                    Title = "Behind the Candelabra",
                    GenreId = genres[0].Id,
                    ReleaseDate = new DateTime(2013, 5, 26),
                    Synopsis = "A chronicle of the tempestuous six-year romance between megastar singer Liberace and his young lover Scott Thorson",
                    AgeRating = AgeRating.Adolescent,
                    Director = new Person { FirstName = "Steven", LastName = "Soderbergh" },
                    Actors =
                    [
                        new() { FirstName = "Michael", LastName = "Douglas" },
                        new() { FirstName = "Matt", LastName = "Demon" }
                    ],
                    IsSoftDeleted = false,
                    ChannelFirstAiredOn = "HBO"
                }
            );

            db.SaveChanges();
        }

    }
}