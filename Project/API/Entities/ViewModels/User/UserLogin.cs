using System.ComponentModel.DataAnnotations;

namespace API.Entities.ViewModels.User;

public class UserLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
