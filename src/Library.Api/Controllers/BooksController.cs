using Library.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
namespace Library.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly LibraryDbContext _dbContext;

    public BooksController(ILogger<BooksController> logger, LibraryDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // TODO
    [HttpGet("/api/search")]
    public async Task<string> Search([FromQuery] int next)
    {
        return await Task.FromResult("");
    }


    // TODO
    [HttpGet("/api/invertwords")]
    public async Task<string> InvertWords([FromQuery] string ids)
    {
        return await Task.FromResult("");
    }

    // TODO
    [HttpGet("/api/report")]
    public async Task<string> Report([FromQuery] string ids)
    {
        return await Task.FromResult("");
    }
}
