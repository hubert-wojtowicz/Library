using System.Net;

namespace Library.Api.ApplicationServices;

public class ErrorResult
{
    public string Message { get; }

    public HttpStatusCode StatusCode { get; }

    public ErrorResult(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
