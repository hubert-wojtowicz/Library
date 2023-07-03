using Library.Infrastructure.Database;

namespace Library.Api.ApplicationServices.Models;

public class BookDto
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public long AuthorId { get; set; }

    public static BookDto Create(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description,
        AuthorId = book.AuthorId,
    };
}