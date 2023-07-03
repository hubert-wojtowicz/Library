using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Database.Search;
using Library.Api.ApplicationServices;

namespace Library.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookApplicationService _bookApplicationService;

    public BooksController(IBookApplicationService bookApplicationService)
    {
        _bookApplicationService = bookApplicationService;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] BooksSearchFilter filter)
    {
        var operationResult = await _bookApplicationService.Search(filter);
        return ToActionResult(operationResult);
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
        return operationResult.IsSuccessful
            ? Ok(operationResult.Result)
            : StatusCode((int)operationResult.Failure.StatusCode, operationResult.Failure.Message);
    }
}
