using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

// With compiled queries
// EF doesn't need to parse the LINQ query
// every time. Instead it just parses the
// query only the first time

public static class CompiledQueries
{
    private static readonly Func<MoviesDbContext, AgeRating, IEnumerable<Movie>> getMoviesByAgeRatingQuery
        = EF.CompileQuery((MoviesDbContext db, AgeRating ageRating) => db.Movies.Where(movie => movie.AgeRating <= ageRating));

    public static Func<MoviesDbContext, AgeRating, IEnumerable<Movie>> FetchGetMoviesByAgeRatingQuery()
    {
        return getMoviesByAgeRatingQuery;
    }
}
