namespace EFCore.API.Models;

public class Movie
{
    public int Identifier { get; set; }
    public string Title { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }


    // Navigation Properties
    public Genre Genre { get; set; } = default!;
    public int MainGenreId { get; set; }
}