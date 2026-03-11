using BookLibrary.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookLibrary.Filters;

public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        logger.LogError(context.Exception, "Unhandled exception");

        var response = ApiResponse<object>.ErrorResponse("Произошла внутренняя ошибка сервера");

        context.Result = new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}