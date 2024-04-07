using System.ComponentModel.DataAnnotations;

namespace API.Entities.ViewModels.User;

public class UserRegister : UserLogin
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }
}
