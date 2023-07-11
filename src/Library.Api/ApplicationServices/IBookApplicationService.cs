using Library.Api.ApplicationServices.Models;
using Library.Infrastructure.Database.Search;

namespace Library.Api.ApplicationServices;

public interface IBookApplicationService
{
    Task<OperationResult<List<BookDto>, ErrorResult>> SearchBooks(BooksSearchFilter filter);

    Task<OperationResult<BookWithReversedTittleDto, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators);

    Task<OperationResult<List<UserReportDto>, ErrorResult>> CalculateUserBorrowingActivityReport();
}