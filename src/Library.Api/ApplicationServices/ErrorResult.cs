using System.Net;

namespace Library.Api.ApplicationServices;

public class ErrorResult
{
    public string Message { get; }

    public HttpStatusCode StatusCode { get; }

    public ErrorResult(string message, HttpStatusCode statusCode)
    {
        if(string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(nameof(message), $"Passed Null or WhiteSpace {message}");

        Message = message;
        StatusCode = statusCode;
    }
}
