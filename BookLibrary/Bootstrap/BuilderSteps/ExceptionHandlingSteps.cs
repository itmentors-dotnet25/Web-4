using BookLibrary.Exceptions.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace BookLibrary.Bootstrap.BuilderSteps;

public static class ExceptionHandlingSteps
{
    [BootstrapStep(75, "Use global exception handler middleware")]
    public static void UseGlobalExceptionHandler(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
        }
    }

    [BootstrapStep(76, "Map error endpoint")]
    public static void MapErrorEndpoint(WebApplication app)
    {
        app.Map("/error", HandleGlobalException);
    }

    private static async Task HandleGlobalException(HttpContext context)
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        
        if (exceptionHandlerFeature?.Error is Exception ex)
        {
            // Получаем сервисы из DI-контейнера
            var metadataProvider = context.RequestServices.GetService<IExceptionMetadataProvider>();
            var exceptionHandler = context.RequestServices.GetService<IExceptionHandlingFacade>();
            var logger = context.RequestServices.GetService<ILogger<Program>>();

            if (metadataProvider != null && exceptionHandler != null)
            {
                // Используем наш кастомный обработчик
                await exceptionHandler.HandleExceptionAsync(context, ex);
                return;
            }
        }

        // Фолбэк: стандартный ответ об ошибке
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
