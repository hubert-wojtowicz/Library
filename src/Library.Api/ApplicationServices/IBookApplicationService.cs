using Library.Api.ApplicationServices.Models;

namespace Library.Api.ApplicationServices;

public interface IBookApplicationService
{
    Task<OperationResult<string, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators);

    Task<OperationResult<List<UserReportModel>, ErrorResult>> CalculateUserBorrowingActivityReport();
}