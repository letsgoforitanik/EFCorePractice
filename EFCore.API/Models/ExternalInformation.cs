namespace EFCore.API.Models;

// Usually One-to-One Relationship exists between strong and weak
// entities. One-to-One Relationship can be represented either 
// by complex properties, owned types, or full-blown one-to-one
// relationship


public class ExternalInformation
{
    public string ImdbUrl { get; set; } = default!;
    public string RottenTomatoesUrl { get; set; } = default!;
    public string TmdbUrl { get; set; } = default!;

    // Navigation Property 
    // For full blown One-to-One Relationship
    public Movie? Movie { get; set; }
    public int MovieId { get; set; }
}
