namespace EFCore.API.Models;

// Genre has one to many relationship with Movie
// One Genre can be associated with many Movies
// However, one movie can only be associated 
// with one Genre
public class Genre : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public string Description { get; set; } = "Nothing special";

    // Optimistic Locking
    public byte[] ConcurrencyToken { get; set; } = [];


    // Navigation Property
    public ICollection<Movie>? Movies { get; set; }

}
