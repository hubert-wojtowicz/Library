using Library.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Database.Search;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly LibraryDbContext _dbContext;

    public BooksController(ILogger<BooksController> logger, LibraryDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // TODO: I know there is possible sql injection (can be fixed with regex validations) and it does not fulfill requirements a) and c) but that was the best I could figure out so far to combine conditions. I attempted also building query with Expressions and Reflection, but I couldnt finish due to exception thrown from database provider.
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] BooksSearchFilter filter)
    {
        var sqlFactory = new BooksSearchSqlFactory(filter);
        var sql = sqlFactory.CreateSql();
        var books = _dbContext.Books.FromSqlRaw(sql);

        return Ok(books);
    }

    [HttpGet("invertwords/{bookId}")]
    public async Task<IActionResult> InvertWords(long bookId)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == bookId);
        if (book == null)
            return NotFound();

        var reversed = string.Join(' ', book.Title.Split(',', ' ', ';', '.', '-').Reverse());
        return Ok(reversed);
    }

    [HttpGet("report")]
    public async Task<IActionResult> Report()
    {
        var report = await _dbContext.BooksTakens
            .GroupBy(b => b.UserId)
            .Select(g => new
            {
                UserDetails = g.Select(b => b.User).FirstOrDefault(),
                TotalBooks = g.Count(),
                TotalDays = g.Sum(b => EF.Functions.DateDiffDay(b.DateTaken, DateTime.UtcNow))
            })
            .ToListAsync();

        return Ok(report);
    }
}

