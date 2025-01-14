using EFCore.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.API.Controllers;

[Route("api/actors")]
[ApiController]
public class ActorsController(MoviesDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActors()
    {
        var query = db.Actors.GroupJoin(
                        db.ActorsMovies.Include(am => am.Movie),
                        actor => actor.Id,
                        am => am.ActorId,
                        (actor, movies) => new { Actor = actor, Movies = movies }
                    )
                    .SelectMany(
                        grp => grp.Movies.DefaultIfEmpty(),
                        (grp, movie) => new { grp.Actor, Movie = movie }
                    );

        var result = await query.ToListAsync();
        return Ok(result);

    }
}
