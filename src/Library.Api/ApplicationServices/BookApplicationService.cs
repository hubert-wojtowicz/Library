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

    public async Task<OperationResult<List<BookDto>, ErrorResult>> SearchBooks(BooksSearchFilter filter)
    {
        try
        {
            var books = await _libraryDbContext.Library.WhereBooks(filter).Select(b => new BookDto()
            {
                AuthorId = b.Book.AuthorId,
                Description = b.Book.Description,
                Id = b.Book.Id,
                Title = b.Book.Title
            }).ToListAsync();

            return OperationResult<List<BookDto>, ErrorResult>.Succeed(books);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed searching Library with with message '{e.Message}' for filter '{JsonConvert.SerializeObject(filter)}'.");
            return OperationResult<List<BookDto>, ErrorResult>.Fail(
                new ErrorResult($"Failed to search Library '{e.Message}'", HttpStatusCode.BadRequest));
        }
    }

    public async Task<OperationResult<BookWithReversedTittleDto, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators)
    {
        var book = await _libraryDbContext.Books.FirstOrDefaultAsync(book => book.Id == bookId);
        if (book == null)
            return OperationResult<BookWithReversedTittleDto, ErrorResult>.Fail(
                new ErrorResult($"{nameof(bookId)} = '{bookId}' has not been found.", HttpStatusCode.NotFound));

        var reversedTitle = preserveSeparators
            ? _titleReverser.ReverseTitle(book.Title)
            : _titleReverser.ReverseTitleLoosingSeparators(book.Title);

        return OperationResult<BookWithReversedTittleDto, ErrorResult>.Succeed(BookWithReversedTittleDto.Create(book, reversedTitle));
    }

    public async Task<OperationResult<List<UserReportDto>, ErrorResult>> CalculateUserBorrowingActivityReport()
    {
        var report = await _libraryDbContext.BooksTakens
            .GroupBy(b => b.UserId)
            .Select(g => new UserReportDto
            {
                UserDetails = g.Select(b => b.User).FirstOrDefault(),
                TotalBooks = g.Count(),
                TotalDays = g.Sum(b => EF.Functions.DateDiffDay(b.DateTaken, DateTime.UtcNow))
            })
            .ToListAsync();


        /*
            FROM (
                SELECT COUNT(*) AS [c]
                    ,COALESCE(SUM(DATEDIFF(day, [b].[DateTaken], GETUTCDATE())), 0) AS [c0]
                    ,[b].[UserID]
                FROM [dbo].[BooksTaken] AS [b]
                GROUP BY [b].[UserID]
                ) AS [t]
            LEFT JOIN (
                SELECT [t1].[ID]
                    ,[t1].[Email]
                    ,[t1].[FirstName]
                    ,[t1].[LastName]
                    ,[t1].[UserID]
                FROM (
                    SELECT [u].[ID]
                        ,[u].[Email]
                        ,[u].[FirstName]
                        ,[u].[LastName]
                        ,[b0].[UserID]
                        ,ROW_NUMBER() OVER (
                            PARTITION BY [b0].[UserID] ORDER BY (
                                    SELECT 1
                                    )
                            ) AS [row]
                    FROM [dbo].[BooksTaken] AS [b0]
                    INNER JOIN [dbo].[User] AS [u] ON [b0].[UserID] = [u].[ID]
                    ) AS [t1]
                WHERE [t1].[row] <= 1
                ) AS [t0] ON [t].[UserID] = [t0].[UserID]
         */

        return OperationResult<List<UserReportDto>, ErrorResult>.Succeed(report);
    }
}