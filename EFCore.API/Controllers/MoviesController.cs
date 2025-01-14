using EFCore.API.Data;
using EFCore.API.Dto;
using EFCore.API.Models;
using EFCore.API.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController(MoviesDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        // Get all movies

        // By using 'AsNoTracking' we are instructing Changetracker to not track
        // the returned objects. Since we are just returning the result and 
        // not modifiying them, tracking them makes no sense and only adds performance
        // penalty.

        // Eager Loading of Genre

        /*
        var movies = await db.Movies.Include(movie => movie.Genre).AsNoTracking().ToListAsync();
        return Ok(movies);
        */

        // Explicit Loading of Genre + Actors
        // More roundtrips than Eager Loading 
        var movies = await db.Movies.ToListAsync();

        foreach (var movie in movies)
        {
            // Explicitly loading Genre
            await db.Entry(movie).Reference(movie => movie.Genre).LoadAsync();

            // Explicitly loading Actors
            await db.Entry(movie).Collection(movie => movie.Actors!).LoadAsync();

        }

        return Ok(movies);

    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovie(int id)
    {
        // First Way
        // First time serves from db, next time, serves from memory
        // So, there is always the chance of getting stale data
        // var movie = await db.Movies.FindAsync(id);

        // Second Way
        // Always serves fresh data
        // Generates same SQL as above
        // var movie = await db.Movies.FirstOrDefaultAsync(movie => movie.Id == id);

        // Third Way
        // This one ensures that only one data exists with the given criteria
        // If more than one matches are found, it throws exception
        // Thus the generated SQL is different, it fetches TOP(2) instead of TOP(1)


        // Eager Loading of Genre
        var movie = await db.Movies
            .Include(movie => movie.Genre)
            .SingleOrDefaultAsync(movie => movie.Id == id);

        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetMoviesByQuery([FromQuery] MovieQueryParameters parameters)
    {
        IQueryable<Movie> query = db.Movies;

        // Fluent Syntax
        if (parameters.Year.HasValue) query = query.Where(movie => movie.ReleaseDate.Year == parameters.Year);

        /*
         // Query Syntax
         if (parameters.Year.HasValue)
            query = from movie in query where movie.ReleaseDate.Year == parameters.Year select movie;
        */

        var filteredMovies = await query.ToListAsync();
        return Ok(filteredMovies);
    }

    [HttpGet("by-age/{ageRating}")]
    public async Task<IActionResult> GetMoviesByAgeRating(AgeRating ageRating)
    {
        var compiledQuery = CompiledQueries.FetchGetMoviesByAgeRatingQuery();
        var movies = await Task.Run(() => compiledQuery.Invoke(db, ageRating).ToList());
        return Ok(movies);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(MovieInsertDto dto)
    {

        // One-to-many relationship insert
        // What if genreId is not present in Movie 
        // Approach 1

        /*
        var genre = await db.Genres.FindAsync(dto.GenreId);

        if (genre is null) return NotFound("Genre not found");

        var newMovie = new Movie
        {
            ReleaseDate = dto.ReleaseDate,
            Genre = genre,
            Synopsis = dto.Synopsis,
            Title = dto.Title
        };

        await db.Movies.AddAsync(newMovie);

        var movieEntityState = db.Entry(newMovie).State; // EntityState.Added (has to be added)
        var genreEntityState = db.Entry(newMovie.Genre).State; // EntityState.Unchanged (nothing needs to be done) 

        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        
        */

        // What if GenreId isn't present in Movie
        // Approach 2
        /*
        var genre = new Genre { Id = dto.GenreId };
        db.Entry(genre).State = EntityState.Unchanged;

        var newMovie = new Movie
        {
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            Synopsis = dto.Synopsis,
            Genre = genre
        };

        await db.Movies.AddAsync(newMovie);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        */

        // What if GenreId is present in Movie
        var newMovie = new Movie
        {
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            Synopsis = dto.Synopsis,
            GenreId = dto.GenreId
        };

        await db.Movies.AddAsync(newMovie);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        // First Way =============
        // 'existingMovie' here is a proxy object. When changes are
        // made to this object, the corresponding EntityState automatically
        // changes from 'Unchanged' to 'Modified'. Thus, we don't need to call
        // any other method. Only SaveChanges() is enough.

        /*
        var existingMovie = await db.Movies.FindAsync(id);
        var entryState = db.Entry(movie).State; // Unchanged
        if (existingMovie is null) return NotFound();

        existingMovie.Synopsis = movie.Synopsis;
        existingMovie.Title = movie.Title;
        existingMovie.ReleaseDate = movie.ReleaseDate;

        entryState = db.Entry(existingMovie).State; // Modified
        await db.SaveChangesAsync();
        return Ok(existingMovie);
        */

        // Second Way ===========================
        /*
        var movieExists = db.Movies.Any(mv => mv.Id == id);
        if (!movieExists) return NotFound();

        movie.Id = id;

        db.Entry(movie).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return Ok(movie);
        */

        // Third Way =============
        // No change tracker involved
        // Good for partial update
        /*
        await db.Movies
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(propSetter => propSetter
                .SetProperty(m => m.Title, movie.Title)
                .SetProperty(m => m.ReleaseDate, movie.ReleaseDate)
                .SetProperty(m => m.Synopsis, movie.Synopsis));

        return Ok();
        */

        // Fourth Way ========================
        var movieExists = await db.Movies.AnyAsync(m => m.Id == id);
        if (!movieExists) return NotFound();

        movie.Id = id;

        db.Movies.Update(movie);
        await db.SaveChangesAsync();
        return Ok(movie);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> RemoveMovie(int id)
    {
        // First Way =======
        /*
        var existingMovie = await db.Movies.FindAsync(id);
        if (existingMovie is null) return NotFound();
        db.Entry(existingMovie).State = EntityState.Deleted;
        await db.SaveChangesAsync();
        return NoContent();
        */

        // Second Way =======
        /*
        var existingMovie = await db.Movies.FindAsync(id);
        if (existingMovie is null) return NotFound();
        db.Movies.Remove(existingMovie);
        await db.SaveChangesAsync();
        return NoContent();
        */

        // Third Way ====
        /*
        var movieExists = await db.Movies.AnyAsync(movie => movie.Id == id);
        if (!movieExists) return NotFound();
        var movieToDelete = new Movie { Id = id };
        db.Entry(movieToDelete).State = EntityState.Deleted;
        await db.SaveChangesAsync();
        return NoContent();
        */

        // Fourth Way ====
        /*
        var movieExists = await db.Movies.AnyAsync(movie => movie.Id == id);
        if (!movieExists) return NotFound();
        var movieToDelete = new Movie { Id = id };
        db.Movies.Remove(movieToDelete);
        await db.SaveChangesAsync();
        return NoContent();
        */

        // Fifth Way ====
        /*
        var movieExists = await db.Movies.AnyAsync(movie => movie.Id == id);
        if (!movieExists) return NotFound();
        var movieToDelete = new Movie { Id = id };
        db.Remove(movieToDelete);
        await db.SaveChangesAsync();
        return NoContent();
        */

        // Sixth Way ====
        // Without change tracker
        var movieExists = await db.Movies.AnyAsync(movie => movie.Id == id);
        if (!movieExists) return NotFound();
        await db.Movies.Where(movie => movie.Id == id).ExecuteDeleteAsync();
        return NoContent();
    }

    [HttpGet("with-actors")]
    public async Task<IActionResult> GetMoviesWithActors()
    {

        var query = db.Database.SqlQuery<Result>(@$"SELECT 
                        m.Id as MovieId, 
                        m.Title as MovieTitle, 
                        m.ReleaseDate as MovieReleaseDate, 
                        a.Id as ActorId, 
                        a.FirstName as ActorFirstName, 
                        a.LastName as ActorLastName, 
                        am.Role as ActorRole 
                        FROM [dbo].[Movies] AS m
                        JOIN [dbo].[ActorsMovies] AS am ON m.Id = am.MovieId
                        JOIN [dbo].[Actors] AS a ON a.Id = am.ActorId"
                    );

        var result = await query.ToListAsync();
        return Ok(result);

    }

    [HttpGet("with-actors/brad")]
    public async Task<IActionResult> GetMoviesWithActorBradPitt()
    {

        var query = db.Database.SqlQuery<Result>(@$"SELECT 
                        m.Id as MovieId, 
                        m.Title as MovieTitle, 
                        m.ReleaseDate as MovieReleaseDate, 
                        a.Id as ActorId, 
                        a.FirstName as ActorFirstName, 
                        a.LastName as ActorLastName, 
                        am.Role as ActorRole 
                        FROM [dbo].[Movies] AS m
                        JOIN [dbo].[ActorsMovies] AS am ON m.Id = am.MovieId
                        JOIN [dbo].[Actors] AS a ON a.Id = am.ActorId"
                    );

        var result = await query.Where(q => q.ActorFirstName == "Brad" && q.ActorLastName == "Pitt").ToListAsync();
        return Ok(result);


    }

    [HttpGet("by-proc")]
    public async Task<IActionResult> GetAllMoviesByProcedure()
    {
        var movies = await db.Movies.FromSql($"EXEC [dbo].[GetAllMovies]").ToListAsync();
        return Ok(movies);
    }

    [HttpGet("titles")]
    public IActionResult GetMovieTitles()
    {
        var titles = db.MovieTitles.ToList();
        return Ok(titles);
    }


}

class Result
{
    public int MovieId { get; set; }
    public string? MovieTitle { get; set; }
    public string? MovieReleaseDate { get; set; }
    public int ActorId { get; set; }
    public string? ActorFirstName { get; set; }
    public string? ActorLastName { get; set; }
    public string? ActorRole { get; set; }
}