using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Data;

public static class CompiledQueries
{
    private static readonly Func<MoviesDbContext, AgeRating, IEnumerable<Movie>> getMoviesByAgeRatingQuery
        = EF.CompileQuery((MoviesDbContext db, AgeRating ageRating) => db.Movies.Where(movie => movie.AgeRating <= ageRating));

    public static Func<MoviesDbContext, AgeRating, IEnumerable<Movie>> FetchGetMoviesByAgeRatingQuery()
    {
        return getMoviesByAgeRatingQuery;
    }
}
