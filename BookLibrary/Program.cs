using BookLibrary.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

// Выполняем шаги для билдера — РЕГИСТРАЦИЯ СЕРВИСОВ
Bootstrap.ExecuteBuilderSteps(builder);

var app = builder.Build();

// Выполняем шаги для приложения — КОНФИГУРАЦИЯ ПАЙПЛАЙНА
Bootstrap.ExecuteAppSteps(app);

app.Run();
// Критически важно: публичный частичный класс нужен для интеграционных тестов
public abstract partial class Program { }
