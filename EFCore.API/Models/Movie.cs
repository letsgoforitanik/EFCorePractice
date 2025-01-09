using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.API.Models;

[Table("Pictures")]
public class Movie
{
    [Key]
    public int Identifier { get; set; }

    [MaxLength(128)]
    [Column(TypeName = "varchar")]
    public string Title { get; set; } = default!;

    [Column(TypeName = "date")]
    public DateTime ReleaseDate { get; set; }

    [Column("Plot", TypeName = "varchar(max)")]
    public string? Synopsis { get; set; }
}