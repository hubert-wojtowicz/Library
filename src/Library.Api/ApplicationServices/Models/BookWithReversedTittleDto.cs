using Library.Infrastructure.Database;

namespace Library.Api.ApplicationServices.Models;

public class BookWithReversedTittleDto
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string TitleReversed { get; set; } = null!;

    public string? Description { get; set; }

    public long AuthorId { get; set; }

    public static BookWithReversedTittleDto Create(Book book, string reversedTitle)
    {
        return new BookWithReversedTittleDto
        {
            Id = book.Id,
            Title = book.Title,
            TitleReversed = reversedTitle,
            Description = book.Description,
            AuthorId = book.AuthorId,
        };
    }
}