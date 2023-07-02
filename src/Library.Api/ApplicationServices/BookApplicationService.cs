using Library.Domain;
using Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Library.Api.ApplicationServices;

public class BookApplicationService : IBookApplicationService
{
    private readonly LibraryDbContext _libraryDbContext;
    private readonly ITitleReverser _titleReverser;

    public BookApplicationService(LibraryDbContext libraryDbContext,
        ITitleReverser titleReverser)
    {
        _libraryDbContext = libraryDbContext;
        _titleReverser = titleReverser;
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
}