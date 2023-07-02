namespace Library.Api.ApplicationServices;

public class OperationResult<TResult, TFailure>
{
    private OperationResult(TResult result, TFailure failure, bool isSuccessful)
    {
        Result = result;
        Failure = failure;
        IsSuccessful = isSuccessful;
    }

    public TResult Result { get; }

    public TFailure Failure { get; }

    public bool IsSuccessful { get; }

    public static OperationResult<TResult, TFailure> Succeed(TResult result)
    {
        return new OperationResult<TResult, TFailure>(result, default, true);
    }

    public static OperationResult<TResult, TFailure> Fail(TFailure failure)
    {
        return new OperationResult<TResult, TFailure>(default, failure, false);
    }
}