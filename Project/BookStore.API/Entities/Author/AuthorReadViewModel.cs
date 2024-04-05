namespace BookStore.API.Entities.Author;

public class AuthorReadViewModel : BaseViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
}
