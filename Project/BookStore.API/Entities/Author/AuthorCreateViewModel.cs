using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Entities.Author;

public class AuthorCreateViewModel
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(250)]
    public string Biography { get; set; } = string.Empty;
}
