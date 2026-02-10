using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Exceptions.Base;
using BookLibrary.Exceptions.Contracts;
using BookLibrary.Filters.ValidationFilters;
using BookLibrary.Models;
using BookLibrary.Repositories.AuthorRepositories;
using BookLibrary.Repositories.BookRepositories;
using BookLibrary.Services;
using BookLibrary.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class ServiceRegistrationSteps
{
    [BootstrapStep(50, "Register API services")]
    public static void RegisterApiServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationResponseFilter>();
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            // Продолжаем десериализацию после ошибок
            options.SerializerSettings.Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            };
        });
        
        builder.Services.AddEndpointsApiExplorer();
    }
    
    [BootstrapStep(51, "Configure validation behavior")]
    public static void ConfigureValidation(WebApplicationBuilder builder)
    {
        // Регистрация валидатора для модели Book
        builder.Services.AddScoped<IValidator<Book>, BookValidator>();
        
        // Кастомизация ответа валидации для ВСЕХ контроллеров
        // может понадобиться в будущем, если:
        // - Добавим валидацию на уровне параметров действия ([FromQuery], [FromRoute])
        // - Используем другие фильтры, которые добавляют ошибки в ModelState
        // - Вернем автоматическую валидацию для некоторых контроллеров
        // - Добавим кастомные атрибуты валидации, работающие через ModelState
        // - Используем [ApiController] с его встроенными фичами
        // - сейчас этот функционал не используется
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                
                return ValidationResponseFormat.FromModelStateErrors(errors);
            };
        });
    }

    [BootstrapStep(52, "Register CORS policy")]
    public static void RegisterCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5139")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    [BootstrapStep(53, "Register application services")]
    public static void RegisterApplicationServices(WebApplicationBuilder builder)
    {
        // Services
        builder.Services.AddScoped<IBookService, BookService>();

        // Repositories
        builder.Services.AddScoped<IBookRepository, MemoryBookRepository>();
        builder.Services.AddScoped<IAuthorReadRepository, MemoryAuthorReadRepository>();
        
        // Регистрация обработчика исключений
        builder.Services.AddSingleton<IExceptionMetadataProvider, AttributeExceptionMetadataProvider>();
        builder.Services.AddScoped<IExceptionHandlingFacade, ExceptionHandler>();

        // Другие зависимости
        // builder.Services.AddScoped<IEmailService, EmailService>();
        // builder.Services.AddScoped<ICacheService, RedisCacheService>();
    }
}
