using BookLibrary.Exceptions.Contracts;

namespace BookLibrary.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var exceptionHandler = context.RequestServices.GetService<IExceptionHandlingFacade>();
            
            if (exceptionHandler != null)
            {
                await exceptionHandler.HandleExceptionAsync(context, ex);
            }
            else
            {
                // Фолбэк
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Ошибка сервера.",
                    error = new
                    {
                        code = 500,
                        message = "Произошла непредвиденная ошибка"
                    }
                });
            }
        }
    }
}
