namespace EFCore.API.Models;

// Genre has one to many relationship with Movie
// One Genre can be associated with many Movies
// However, one movie can only be associated 
// with one Genre
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    // Navigation Property
    public ICollection<Movie> Movies { get; set; } = default!;
}
