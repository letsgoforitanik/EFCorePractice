namespace EFCore.API.Models;

public class TelevisionMovie : Movie
{
    public string ChannelFirstAiredOn { get; set; } = default!;
}
