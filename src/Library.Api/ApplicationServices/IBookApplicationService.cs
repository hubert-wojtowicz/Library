using Library.Api.ApplicationServices.Models;
using Library.Infrastructure.Database;
using Library.Infrastructure.Database.Search;

namespace Library.Api.ApplicationServices;

public interface IBookApplicationService
{
    Task<OperationResult<List<Book>, ErrorResult>> SearchBooks(BooksSearchFilter  filter);

    Task<OperationResult<BookWithReversedTittleDto, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators);

    Task<OperationResult<List<UserReportModel>, ErrorResult>> CalculateUserBorrowingActivityReport();
}