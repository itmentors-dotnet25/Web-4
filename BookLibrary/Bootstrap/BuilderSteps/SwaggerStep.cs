using System.Diagnostics;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class SwaggerStep
{
    [BootstrapStep(50.1, "Register Swagger service")]
    [Obsolete("Obsolete")]
    public static void RegisterApiServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Book Library API",
                Version = "v1",
                Description = "API documentation for Book Library application"
            });
    
            // Автоматическая группировка по пространству имён контроллеров
            options.TagActionsBy(api =>
                api.ActionDescriptor.DisplayName switch
                {
                    { } name when name.Contains("Controllers.Books") => "Книги",
                    { } name when name.Contains("Controllers.Authors") => "Авторы",
                    _ => "Общее"
                }
            );
    
            // Сортировка операций
            options.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
        });
    }
}
