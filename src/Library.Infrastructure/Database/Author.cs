using Library.Infrastructure.Database.Search;

namespace Library.Infrastructure.Database;

public partial class Author
{
    public long Id { get; set; }

    [CanFilterWith]
    public string FirstName { get; set; } = null!;

    [CanFilterWith]
    public string? MiddleName { get; set; }

    [CanFilterWith]
    public string LastName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
