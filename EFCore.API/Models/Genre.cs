namespace EFCore.API.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    // Navigation Property
    public ICollection<Movie> Movies { get; set; } = default!;
}
