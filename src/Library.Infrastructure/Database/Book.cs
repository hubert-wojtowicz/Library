using Library.Infrastructure.Database.Search;

namespace Library.Infrastructure.Database;

public partial class Book
{
    public long Id { get; set; }

    [CanFilterWith]
    public string Title { get; set; } = null!;

    [CanFilterWith]
    public string? Description { get; set; }

    public long AuthorId { get; set; }

    [CanFilterWith]
    public virtual Author Author { get; set; } = null!;
}
