namespace Library.Infrastructure.Database;

public partial class Book
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public long AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;
}
