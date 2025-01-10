namespace EFCore.API.Dto;

public class MovieInsertDto
{
    public required string Title { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public required int GenreId { get; set; }
}
