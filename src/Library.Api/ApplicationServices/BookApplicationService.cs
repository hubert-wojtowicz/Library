using Library.Domain;
using Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Library.Api.ApplicationServices.Models;
using Library.Infrastructure.Database.Search;
using Newtonsoft.Json;

namespace Library.Api.ApplicationServices;

public class BookApplicationService : IBookApplicationService
{
    private readonly ILogger<BookApplicationService> _logger;
    private readonly LibraryDbContext _libraryDbContext;
    private readonly ITitleReverser _titleReverser;

    public BookApplicationService(
        ILogger<BookApplicationService> logger,
        LibraryDbContext libraryDbContext,
        ITitleReverser titleReverser)
    {
        _logger = logger;
        _libraryDbContext = libraryDbContext;
        _titleReverser = titleReverser;
    }

    public async Task<OperationResult<List<Book>, ErrorResult>> Search(BooksSearchFilter filter)
    {
        /*
        I decided to translate filter directly to SQL. I attempted translating to Expression to be able access foreign keys, but I couldn't translate Expression to SQL with EF, so I decided to go this way.

        I noticed SQL injection problem in BooksSearchFilter, and I decided to avoid it using REGEX validation. SQL injection prevention isn't generally done through regex alone. It involves escaping input, using parameterized queries, and various other techniques. However I decided to stop here due to limited time for this project.

        This Solution does not allow query by Author or by User holding book (requirements 1a and 1c)
        On the other hand it allows to search by book title or description and combine condition with binary tree where operators AND, OR, EQUAL, CONTAINS can be applied.

        For this kind search operation I think dedicated SQL VIEW can be beneficial to recalculate query table and optimize query with thoughtful indexes. I next iteration of this project I would try to add view to satisfy 1a and 1c.
         */

        try
        {
            var sqlFactory = new BooksSearchSqlFactory(filter);
            var sql = sqlFactory.CreateSql();
            var books = await _libraryDbContext.Books.FromSqlRaw(sql).ToListAsync();

            return OperationResult<List<Book>, ErrorResult>.Succeed(books);

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed searching books with with message '{e.Message}' for filter '{JsonConvert.SerializeObject(filter)}'.");
            return OperationResult<List<Book>, ErrorResult>.Fail(
                new ErrorResult($"Failed to search books '{e.Message}'", HttpStatusCode.BadRequest));
        }
    }

    public async Task<OperationResult<string, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators)
    {
        var book = await _libraryDbContext.Books.FirstOrDefaultAsync(book => book.Id == bookId);
        if (book == null)
            return OperationResult<string, ErrorResult>.Fail(
                new ErrorResult($"{nameof(bookId)} = '{bookId}' has not been found.", HttpStatusCode.NotFound));

        var result = preserveSeparators
            ? _titleReverser.ReverseTitle(book.Title)
            : _titleReverser.ReverseTitleLoosingSeparators(book.Title);

        return OperationResult<string, ErrorResult>.Succeed(result);
    }

    public async Task<OperationResult<List<UserReportModel>, ErrorResult>> CalculateUserBorrowingActivityReport()
    {
        var report = await _libraryDbContext.BooksTakens
            .GroupBy(b => b.UserId)
            .Select(g => new UserReportModel
            {
                UserDetails = g.Select(b => b.User).FirstOrDefault(),
                TotalBooks = g.Count(),
                TotalDays = g.Sum(b => EF.Functions.DateDiffDay(b.DateTaken, DateTime.UtcNow))
            })
            .ToListAsync();

        return OperationResult<List<UserReportModel>, ErrorResult>.Succeed(report);
    }
}