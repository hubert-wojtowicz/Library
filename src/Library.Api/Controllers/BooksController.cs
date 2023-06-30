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

    // TODO
    [HttpGet("invertwords")]
    public async Task<string> InvertWords([FromQuery] string ids)
    {
        return await Task.FromResult("");
    }

    // TODO
    [HttpGet("report")]
    public async Task<string> Report([FromQuery] string ids)
    {
        return await Task.FromResult("");
    }
}

