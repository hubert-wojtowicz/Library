using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Database;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksTaken> BooksTakens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<LibrarySearchView> Library { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<LibrarySearchView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("LibrarySearchView", "dbo");
            });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author", "dbo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book", "dbo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Author");
        });

        modelBuilder.Entity<BooksTaken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BooksTaken", "dbo");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.DateTaken).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BooksTaken_Book");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_BooksTaken_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "dbo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
