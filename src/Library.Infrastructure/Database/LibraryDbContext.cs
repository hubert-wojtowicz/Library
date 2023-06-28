using Library.Domain;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Database;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }
}