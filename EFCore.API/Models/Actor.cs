namespace EFCore.API.Models;

public class Actor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    // Navigation Properties for Movie
    public ICollection<Movie>? Movies { get; set; }

    // Nav
    public ICollection<ActorMovie>? Roles { get; set; }
}
