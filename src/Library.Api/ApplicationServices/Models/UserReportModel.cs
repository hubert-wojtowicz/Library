using Library.Infrastructure.Database;

namespace Library.Api.ApplicationServices.Models
{
    public class UserReportModel
    {
        public User UserDetails { get; set; }
        public int TotalBooks { get; set; }
        public int TotalDays { get; set; }
    }
}
