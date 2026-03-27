using BookLibrary.Contracts;
using BookLibrary.Middleware;
using BookLibrary.Repositories;
using BookLibrary.Services;
using BookLibrary.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

builder.Services.AddSingleton<IBookRepository, MemoryBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers()
    .AddControllersAsServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseMiddleware<NotFoundHandlerMiddleware>();

app.Run();

public abstract partial class Program
{
}