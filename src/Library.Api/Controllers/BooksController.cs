using Library.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Database.Search;
using Microsoft.EntityFrameworkCore;
using Library.Api.ApplicationServices;

namespace Library.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookApplicationService _bookApplicationService;
    private readonly LibraryDbContext _dbContext;

    public BooksController(
        IBookApplicationService bookApplicationService,
        LibraryDbContext dbContext)
    {
        _bookApplicationService = bookApplicationService;
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
    public async Task<IActionResult> InvertWords(long bookId, [FromQuery] bool preserveSeparators = true)
    {
        var operationResult = await _bookApplicationService.ReverseBookTitle(bookId, preserveSeparators);
        return ToActionResult(operationResult);
    }

    [HttpGet("report")]
    public async Task<IActionResult> Report()
    {
        var operationResult = await _bookApplicationService.CalculateUserBorrowingActivityReport();
        return ToActionResult(operationResult);
    }

    private IActionResult ToActionResult<TResult>(OperationResult<TResult, ErrorResult> operationResult)
    {
        return operationResult.IsSuccessful ?
            Ok(operationResult.Result) :
            StatusCode((int)operationResult.Failure.StatusCode, operationResult.Failure.Message);
    }
}
