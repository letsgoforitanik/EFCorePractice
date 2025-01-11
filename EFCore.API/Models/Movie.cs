namespace EFCore.API.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }

    // Enums are mapped to int in Database by EFCore
    // by default. However, we can change this behavior
    public AgeRating AgeRating { get; set; }

    // Navigation Properties
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = default!;

}