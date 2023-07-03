using Library.Api.ApplicationServices.Models;
using Library.Infrastructure.Database;
using Library.Infrastructure.Database.Search;

namespace Library.Api.ApplicationServices;

public interface IBookApplicationService
{
    Task<OperationResult<List<LibrarySearchView>, ErrorResult>> Search(BooksSearchFilter  filter);

    Task<OperationResult<string, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators);

    Task<OperationResult<List<UserReportModel>, ErrorResult>> CalculateUserBorrowingActivityReport();
}