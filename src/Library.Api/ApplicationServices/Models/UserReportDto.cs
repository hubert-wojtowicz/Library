using Library.Infrastructure.Database;

namespace Library.Api.ApplicationServices.Models;

public class UserReportDto
{
    public User UserDetails { get; set; }
    public int TotalBooks { get; set; }
    public int TotalDays { get; set; }
    public IEnumerable<long> UserId { get; set; }
}
