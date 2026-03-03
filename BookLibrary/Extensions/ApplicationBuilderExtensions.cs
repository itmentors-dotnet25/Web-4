using BookLibrary.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BookLibrary.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Добавляет глобальную обработку исключений
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        /// <summary>
        /// Добавляет обработку несуществующих маршрутов
        /// </summary>
        public static IApplicationBuilder UseNotFoundHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NotFoundHandlingMiddleware>();
        }
    }
}