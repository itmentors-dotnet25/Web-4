using BookLibrary.Filters;
using BookLibrary.Services;
using BookLibrary.Validators;
using BookLibrary.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddControllers(options => { options.Filters.AddService<GlobalExceptionFilter>(); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Библиотека книг API",
        Version = "v1",
        Description = "REST API для управления электронной библиотекой книг"
    });
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateRequestValidator>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Библиотека книг API");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseRouting();
app.UseNotFoundMiddleware();

app.UseAuthorization();
app.MapControllers();

app.Run();

public abstract partial class Program
{
}