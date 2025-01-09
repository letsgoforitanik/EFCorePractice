using EFCore.API.Data;
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
        var movies = await db.Movies.ToListAsync();
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
        var movie = await db.Movies.SingleOrDefaultAsync(movie => movie.Id == id);

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

    [HttpPost]
    public async Task<IActionResult> CreateMovie(Movie movie)
    {
        // First Way
        //db.Movies.Attach(movie); // Adds to change tracker
        //await db.SaveChangesAsync();

        // Second Way
        // db.Entry(movie).State = EntityState.Added;
        // await db.SaveChangesAsync();

        // Third Way
        await db.Movies.AddAsync(movie);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
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
}