using Library.Infrastructure.Database.Search;

namespace Library.Infrastructure.Database;

public partial class LibrarySearchView
{
    public long BookID { get; set; }
    [CanFilterWith]
    public Book Book { get; set; }
    [CanFilterWith]
    public string Author { get; set; }
    [CanFilterWith]
    public string Title { get; set; }
    [CanFilterWith]
    public string Description { get; set; }
    [CanFilterWith]
    public string HoldingUser { get; set; }
}
