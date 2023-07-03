namespace Library.Infrastructure.Database;

public partial class LibrarySearch
{
    public int BookID { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string HoldingUser { get; set; }
}
