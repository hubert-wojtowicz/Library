using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Database.Search;
using Library.Api.ApplicationServices;
using Library.Infrastructure.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

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

    [HttpGet("searchSchema")]
    public IActionResult Search()
    {
        JSchemaGenerator generator = new JSchemaGenerator();
        JSchema schema = generator.Generate(typeof(LibrarySearchView));
        return Ok(JsonConvert.SerializeObject(schema));
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] BooksSearchFilter filter)
    {
        var operationResult = await _bookApplicationService.SearchBooks(filter);
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
            : StatusCode((int)operationResult.Failure.StatusCode, new { errorMessage = operationResult.Failure.Message });
    }
}
