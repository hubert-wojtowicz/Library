using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Database.Search;

public class BooksSearchFilter
{
    public BooksSearchFilter? Left { get; set; }

    [RegularExpression("^(AND|OR|EQUAL|CONTAINS)$", ErrorMessage = "Operator must be out of values: 'AND', 'OR', 'EQUAL' or 'CONTAINS'.")]
    public string? Operator { get; set; }

    public BooksSearchFilter? Right { get; set; }

    public Condition? Condition { get; set; }
}