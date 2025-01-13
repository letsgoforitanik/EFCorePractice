namespace EFCore.API.Models;

public class ActorMovie
{
    public int ActorId { get; set; }
    public int MovieId { get; set; }
    public string? Role { get; set; }
}
