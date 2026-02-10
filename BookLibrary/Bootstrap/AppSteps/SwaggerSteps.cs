namespace BookLibrary.Bootstrap.AppSteps;

public class SwaggerSteps
{
    [BootstrapStep(65, "Configure Swagger documentation")]
    public static void ConfigureSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Library API v1");
            c.RoutePrefix = "swagger"; // путь для отображения Swagger 
            
            // Скрыть раздел Schemas
            c.DefaultModelsExpandDepth(-1);  // Скрывает раздел "Schemas"
            c.DisplayRequestDuration();      // Показывать время выполнения
        });
    }
}
