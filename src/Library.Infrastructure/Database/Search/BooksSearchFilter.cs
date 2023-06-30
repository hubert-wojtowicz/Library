namespace Library.Infrastructure.Database.Search;

public class BooksSearchFilter
{
    public BooksSearchFilter? Left { get; set; }

    public string Operator { get; set; }

    public BooksSearchFilter? Right { get; set; }

    public Condition? Condition { get; set; }
}