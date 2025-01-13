namespace EFCore.API.Models;

public class Actor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    // Navigation Property
    public ICollection<ActorMovie>? Movies { get; set; }

}
