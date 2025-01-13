namespace EFCore.API.Models;

// Many-to-Many Relationship
// Actor + Movie

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
    public Person Director { get; set; } = default!;
    public ExternalInformation ExternalInformation { get; set; } = default!;

    // Navigation Properties for Actor
    public ICollection<ActorMovie>? Actors { get; set; }

}