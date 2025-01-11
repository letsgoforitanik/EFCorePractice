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
        await db.Genres.AddAsync(genre);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }
}
