using System.ComponentModel.DataAnnotations;

namespace API.Entities.ViewModels.Author;

public class AuthorCreate
{
    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(250)]
    public string? Biography { get; set; }
}
