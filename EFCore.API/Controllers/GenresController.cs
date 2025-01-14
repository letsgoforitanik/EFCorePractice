using EFCore.API.Data;
using EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore.API.Controllers;


[ApiController]
[Route("api/genres")]
public class GenresController(MoviesDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await db.Genres.ToListAsync();
        return Ok(genres);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGenre(int id)
    {
        var genre = await db.Genres.FindAsync(id);
        return genre is not null ? Ok(genre) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenre(Genre genre)
    {
        /*
        await db.Genres.AddAsync(genre);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        */

        /*
        await db.Database.ExecuteSqlRawAsync("INSERT INTO [dbo].[Genres] (Name) VALUES ({0})", genre.Name);
        return Created();
        */

        await db.Database.ExecuteSqlAsync($"INSERT INTO [dbo].[Genres] (Name) VALUES ({genre.Name})");
        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGenre(int id, Genre genre)
    {
        var existingGenre = await db.Genres.FindAsync(id);

        if (existingGenre is null) return NotFound();

        existingGenre.Name = genre.Name;
        await db.SaveChangesAsync();
        return Ok(existingGenre);
    }

    [HttpPut("batch")]
    public async Task<IActionResult> BatchUpdateGenres(List<Genre> genres)
    {
        var existingGenres = await db.Genres.Where(genre => genres.Contains(genre)).ToListAsync();

        foreach (var genre in existingGenres)
        {
            genre.Name = genres.FirstOrDefault(g => g.Id == genre.Id)?.Name ?? genre.Name;
        }

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("by-query")]
    public async Task<IActionResult> GetGenresByQuery()
    {
        int minGenreId = 2;
        var genres = await db.Genres.FromSql($"SELECT * FROM [dbo].[Genres] WHERE Id >= {minGenreId}").ToListAsync();
        return Ok(genres);
    }

    [HttpGet("by-query-raw")]
    public async Task<IActionResult> GetGenresByQueryRaw()
    {
        int minGenreId = 2;
        var genres = await db.Genres.FromSqlRaw("SELECT * FROM [dbo].[Genres] WHERE Id >= @p0", minGenreId).ToListAsync();
        return Ok(genres);
    }

    [HttpGet("names")]
    public async Task<IActionResult> GetAllGenreNames()
    {
        var genreNames = await db.GenreNames.ToListAsync();
        return Ok(genreNames);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var genre = await db.Genres.FindAsync(id);

        if (genre is null) return NotFound();

        db.Genres.Remove(genre);
        await db.SaveChangesAsync();
        return NoContent();
    }

}
