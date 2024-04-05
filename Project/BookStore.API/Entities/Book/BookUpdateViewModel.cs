using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Entities.Book;

public class BookUpdateViewModel : BaseViewModel
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(1800, int.MaxValue)]
    public int Year { get; set; }

    [Required]
    public string Isbn { get; set; } = null!;

    [Required]
    [StringLength(250, MinimumLength = 10)]
    public string Summary { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
}
