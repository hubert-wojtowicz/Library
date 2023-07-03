using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Database.Search;

public class Condition
{
    [Required(ErrorMessage = "PropertyName is required.")]
    [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "PropertyName must contain only alphanumeric characters and underscores.")]
    public string PropertyName { get; set; }

    [Required(ErrorMessage = "PropertyName is required.")]
    [RegularExpression(@"^[a-zA-Z0-9_ .-]+$", ErrorMessage = "PropertyName must contain only alphanumeric characters, spaces, underscores, periods, and hyphens.")]
    public string Value { get; set; }
}