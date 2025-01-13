namespace EFCore.API.Models;

// EF Core Inheritance
// TPH - Table Per Hierarchy
// TPC - Table Per Concrete Type
// TPT - Table Per Type 

public abstract class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
    public ICollection<Person> Actors { get; set; } = default!;
    public bool IsSoftDeleted { get; set; }

}