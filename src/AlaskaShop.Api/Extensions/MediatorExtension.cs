using AlaskaShop.Shareable.Response;
using MediatR;
using Npgsql;
using OperationResult;
using System.Net;

namespace AlaskaShop.Api.Extensions;

public static class MediatorExtension
{
    public static async Task<IResult> SendCommand<T>(this IMediator mediator, IRequest<Result<T>> request)
        => await mediator.Send(request) switch
        {
            (true, var result, _) => Results.Ok(result),
            var (_, _, error) => HandleError(error)
        };

    private static IResult HandleError(Exception? error)
        => error switch
        {
            NpgsqlException ex when ex.InnerException is TimeoutException e => new ErrorResult<ErrorResponse>((int)HttpStatusCode.RequestTimeout, new ErrorResponse(e.Message)),
            NpgsqlException e => new ErrorResult<ErrorResponse>((int)HttpStatusCode.ServiceUnavailable, new ErrorResponse(e.Message)),
            HttpRequestException e => new ErrorResult<ErrorResponse>((int)HttpStatusCode.ServiceUnavailable, new ErrorResponse(e.Message)),
            TimeoutException e => new ErrorResult<ErrorResponse>((int)HttpStatusCode.RequestTimeout, new ErrorResponse(e.Message)),
            ApplicationException e => new ErrorResult<ErrorResponse>((int)HttpStatusCode.BadRequest, new ErrorResponse(e.Message)),
            _ => Results.StatusCode(500)
        };

    private readonly record struct ErrorResult<T>(int StatusCode, T? Value) : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCode;
            return Value is null
                ? Task.CompletedTask
                : httpContext.Response.WriteAsJsonAsync(Value, Value.GetType(), options: null, contentType: "application/json");
        }
    }
}
