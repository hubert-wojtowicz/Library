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

    public async Task<OperationResult<List<Book>, ErrorResult>> SearchBooks(BooksSearchFilter filter)
    {
        try
        {
            var sqlFactory = new BooksSearchSqlFactory(filter);
            var sql = sqlFactory.CreateSql();
            var bookIds = await _libraryDbContext.Library.FromSqlRaw(sql).Select(b => b.Book).ToListAsync();

            return OperationResult<List<Book>, ErrorResult>.Succeed(bookIds);

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed searching Library with with message '{e.Message}' for filter '{JsonConvert.SerializeObject(filter)}'.");
            return OperationResult<List<Book>, ErrorResult>.Fail(
                new ErrorResult($"Failed to search Library '{e.Message}'", HttpStatusCode.BadRequest));
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