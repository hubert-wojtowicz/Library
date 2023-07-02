namespace Library.Api.ApplicationServices;

public interface IBookApplicationService
{
    Task<OperationResult<string, ErrorResult>> ReverseBookTitle(long bookId, bool preserveSeparators);
}