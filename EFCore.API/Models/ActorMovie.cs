namespace EFCore.API.Models;

public class ActorMovie
{
    // Navigation Properties for Actor
    public int ActorId { get; set; }
    public Actor? Actor { get; set; }

    // Navigation Properties for Movie
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }

    public string? Role { get; set; }
}
