using BookLibrary.Middleware;

namespace BookLibrary.Bootstrap.AppSteps;

public class MiddlewareSteps
{
    [BootstrapStep(70, "Configure middleware pipeline")]
    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseStaticFiles();
        
        // Логирование запросов
        app.UseMiddleware<RequestLoggingMiddleware>();
        
        // Обработка несуществующих маршрутов
        app.UseMiddleware<RouteNotFoundMiddleware>();
        
        // Обработка валидации
        // app.UseMiddleware<ValidationMiddleware>();
        
        // Обработка исключений
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
