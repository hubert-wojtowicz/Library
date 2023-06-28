namespace Library.Infrastructure.Database;

public partial class BooksTaken
{
    public long BookId { get; set; }

    public long UserId { get; set; }

    public DateTime DateTaken { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
