using Library.Infrastructure.Database;

namespace Library.Api.ApplicationServices.Models;

public class BookWithReversedTittleDto : BookDto
{
    public string TitleReversed { get; set; } = null!;

    public static BookWithReversedTittleDto Create(Book book, string reversedTitle) => new()
    {
        TitleReversed = reversedTitle,
        Id = book.Id,
        Title = book.Title,
        Description = book.Description,
        AuthorId = book.AuthorId,
    };
}