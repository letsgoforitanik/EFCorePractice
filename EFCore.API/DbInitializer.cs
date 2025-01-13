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
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Romance" }
            );

            db.SaveChanges();
        }

        if (!db.Movies.Any())
        {
            var genres = db.Genres.ToList();

            db.Movies.AddRange(
                new Movie
                {
                    Title = "Fight Club",
                    GenreId = genres[0].Id,
                    ReleaseDate = new DateTime(1999, 9, 10),
                    Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other",
                    AgeRating = AgeRating.Adolescent,
                    Director = new() { FirstName = "David", LastName = "Fincher" },
                    Actors =
                    [
                        new()
                        {
                            Actor = new() { FirstName = "Brad", LastName = "Pitt" },
                            Role = "Tyler Durden"
                        },
                        new()
                        {
                            Actor = new() { FirstName = "Edward", LastName = "Norton" },
                            Role = "The Narrator"
                        },
                        new()
                        {
                            Actor = new() { FirstName = "Helena", LastName = "Bonham Carter" },
                            Role = "Marla Singer"
                        }
                    ],
                    ExternalInformation = new()
                    {
                        ImdbUrl = "https://www.imdb.com/title/tt0137523/",
                        RottenTomatoesUrl = "https://www.rottentomatoes.com/m/fight_club",
                        TmdbUrl = "https://www.themoviedb.org/movie/550-fight-club"
                    }
                },
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    GenreId = genres[0].Id,
                    ReleaseDate = new DateTime(1994, 9, 14),
                    Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through",
                    AgeRating = AgeRating.Adult,
                    Director = new() { FirstName = "Frank", LastName = "Darabont" },
                    Actors =
                    [
                        new()
                        {
                            Actor = new() { FirstName = "Morgan", LastName = "Freeman" },
                            Role = "Ellis Boyd Redding"
                        },
                        new()
                        {
                            Actor = new() { FirstName = "Tim", LastName = "Robbins" },
                            Role = "Andy Dufresne"
                        }
                    ],
                    ExternalInformation = new()
                    {
                        ImdbUrl = "https://www.imdb.com/title/tt0111161/",
                        RottenTomatoesUrl = "https://www.rottentomatoes.com/m/shawshank_redemption",
                        TmdbUrl = "https://www.themoviedb.org/movie/278-the-shawshank-redemption"
                    }
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    GenreId = genres[1].Id,
                    ReleaseDate = new DateTime(2008, 7, 18),
                    Synopsis = "When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end",
                    AgeRating = AgeRating.HighSchool,
                    Director = new() { FirstName = "Christopher", LastName = "Nolan" },
                    Actors =
                    [
                        new()
                        {
                            Actor = new() { FirstName = "Christian", LastName = "Bale" },
                            Role = "Batman"
                        },
                        new()
                        {
                            Actor = new() { FirstName = "Heath", LastName = "Ledger" },
                            Role = "Joker"
                        }
                    ],
                    ExternalInformation = new()
                    {
                        ImdbUrl = "https://www.imdb.com/title/tt0468569/",
                        RottenTomatoesUrl = "https://www.rottentomatoes.com/m/the_dark_knight",
                        TmdbUrl = "https://www.themoviedb.org/movie/155-the-dark-knight"
                    }
                },
                new Movie
                {
                    Title = "Inception",
                    GenreId = genres[2].Id,
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Synopsis = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO",
                    AgeRating = AgeRating.All,
                    Director = new() { FirstName = "Christopher", LastName = "Nolan" },
                    Actors =
                    [
                        new()
                        {
                            Actor = new() { FirstName = "Leonardo", LastName = "DiCaprio" },
                            Role = "Cobb"
                        },
                        new()
                        {
                            Actor = new() { FirstName = "Cillian", LastName = "Murphy" },
                            Role = "Fischer"
                        }
                    ],
                    ExternalInformation = new()
                    {
                        ImdbUrl = "https://www.imdb.com/title/tt1375666/",
                        RottenTomatoesUrl = "https://www.rottentomatoes.com/m/inception",
                        TmdbUrl = "https://www.themoviedb.org/movie/27205-inception"
                    }
                }
            );

            db.SaveChanges();
        }

    }
}