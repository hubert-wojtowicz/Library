using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Database.Search;

public class Condition
{
    [RegularExpression("^^[a-zA-Z0-9._-]+$", ErrorMessage = "PropertyName must contain only alphanumeric characters and underscores.")]
    public string PropertyName { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9\s._-]+$", ErrorMessage = "PropertyName must contain only alphanumeric characters, spaces, underscores, periods, and hyphens.")]
    public string Value { get; set; }
}