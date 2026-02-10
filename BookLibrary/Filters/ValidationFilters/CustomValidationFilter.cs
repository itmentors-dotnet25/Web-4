using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BookLibrary.Filters.ValidationFilters;

public class CustomValidationFilter(IServiceProvider serviceProvider) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = ProcessModelBindingErrors(context);
            
            // Вызываем валидатор FluentValidation для получения ошибок обязательных полей
            var validationErrors = RunFluentValidation(context);
            
            // Объединяем ошибки
            foreach (var error in validationErrors)
            {
                if (errors.ContainsKey(error.Key))
                {
                    errors[error.Key] = errors[error.Key]
                        .Concat(error.Value)
                        .Distinct()
                        .ToArray();
                }
                else
                {
                    errors[error.Key] = error.Value;
                }
            }

            var problemDetails = new
            {
                message = "Указанные данные были неверными.",
                errors
            };

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }

    /// <summary>
    /// Обрабатывает ошибки привязки модели
    /// </summary>
    private Dictionary<string, string[]> ProcessModelBindingErrors(ActionExecutingContext context)
    {
        var errors = new Dictionary<string, string[]>();
        
        foreach (var kvp in context.ModelState)
        {
            if (kvp.Value?.Errors.Count > 0)
            {
                string fieldName = NormalizeFieldName(kvp.Key, context);
                
                // Пропускаем ошибки на уровне всего параметра
                if (string.IsNullOrEmpty(fieldName) || fieldName == "model")
                {
                    continue;
                }
                
                errors[fieldName] = kvp.Value.Errors
                    .Select(e => FormatErrorMessage(fieldName, e))
                    .ToArray();
            }
        }

        return errors;
    }

    /// <summary>
    /// Запускает валидатор FluentValidation и возвращает ошибки
    /// </summary>
    private Dictionary<string, string[]> RunFluentValidation(ActionExecutingContext context)
    {
        var errors = new Dictionary<string, string[]>();
        
        // Получаем параметры метода контроллера
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        
        if (controllerActionDescriptor != null)
        {
            // Проходим по всем параметрам метода
            for (int i = 0; i < controllerActionDescriptor.Parameters.Count; i++)
            {
                var parameter = controllerActionDescriptor.Parameters[i];
                var parameterName = parameter.Name;
                var parameterType = parameter.ParameterType;
                
                // Получаем значение параметра из ActionArguments
                var parameterValue = context.ActionArguments.ContainsKey(parameterName) 
                    ? context.ActionArguments[parameterName] 
                    : null;
                
                // Пропускаем примитивные типы и токены отмены
                if (IsPrimitiveType(parameterType) || 
                    parameterType == typeof(CancellationToken) ||
                    parameterType == typeof(object))
                {
                    continue;
                }
                
                // Получаем валидатор для типа параметра
                var validatorType = typeof(IValidator<>).MakeGenericType(parameterType);
                var validator = serviceProvider.GetService(validatorType) as IValidator;
                
                if (validator != null)
                {
                    // Создаем пустой объект для валидации, если текущий null
                    var validationTarget = parameterValue ?? Activator.CreateInstance(parameterType);
                    
                    if (validationTarget != null)
                    {
                        // Выполняем валидацию
                        var validationResult = validator.Validate(new ValidationContext<object>(validationTarget));
                        
                        // Добавляем ошибки в словарь
                        foreach (var failure in validationResult.Errors)
                        {
                            var fieldName = failure.PropertyName;
                            
                            if (!errors.ContainsKey(fieldName))
                            {
                                errors[fieldName] = new[] { failure.ErrorMessage };
                            }
                            else
                            {
                                errors[fieldName] = errors[fieldName]
                                    .Append(failure.ErrorMessage)
                                    .ToArray();
                            }
                        }
                    }
                }
            }
        }
        
        return errors;
    }

    private bool IsPrimitiveType(Type type)
    {
        return type.IsPrimitive || 
               type == typeof(string) || 
               type == typeof(decimal) || 
               type == typeof(DateTime) ||
               type == typeof(Guid) ||
               type.IsEnum;
    }

    /// <summary>
    /// Нормализует имя поля, убирая префикс параметра метода
    /// </summary>
    private string NormalizeFieldName(string key, ActionExecutingContext context)
    {
        if (string.IsNullOrEmpty(key) || key == "$")
        {
            return "model";
        }

        // Получаем имена параметров метода контроллера
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var parameterNames = controllerActionDescriptor?.Parameters
            .Select(p => p.Name)
            .ToList() ?? new List<string>();

        // Если ключ совпадает с именем параметра - это ошибка на уровне всего объекта
        if (parameterNames.Contains(key, StringComparer.OrdinalIgnoreCase))
        {
            return "model";
        }

        // Если ключ начинается с имени параметра + точка, убираем префикс
        foreach (var paramName in parameterNames)
        {
            if (key.StartsWith($"{paramName}.", StringComparison.OrdinalIgnoreCase))
            {
                return key.Substring(paramName.Length + 1);
            }
        }

        // Если ключ содержит точку, берем последнюю часть (вложенное свойство)
        if (key.Contains('.'))
        {
            return key.Split('.').Last();
        }

        // Иначе возвращаем как есть
        return key;
    }

    private string FormatErrorMessage(string fieldName, ModelError error)
    {
        if (!string.IsNullOrEmpty(error.ErrorMessage))
        {
            var message = error.ErrorMessage;
            
            // Убираем технические детали из сообщения об ошибке конвертации
            if (message.Contains("Path:") && message.Contains("LineNumber:"))
            {
                var cleanMessage = message.Split("Path:")[0].Trim();
                
                if (cleanMessage.Contains("could not be converted", StringComparison.OrdinalIgnoreCase))
                {
                    return $"Неверный формат данных для поля '{fieldName}'.";
                }
                
                return cleanMessage;
            }
            
            // Убираем упоминания типа данных
            if (message.Contains("System.Int32", StringComparison.OrdinalIgnoreCase))
            {
                return message.Replace("System.Int32", "число");
            }
            
            if (message.Contains("System.DateTime", StringComparison.OrdinalIgnoreCase))
            {
                return message.Replace("System.DateTime", "дата");
            }
            
            return message;
        }
        
        if (error.Exception != null)
        {
            return GetDefaultErrorMessage(fieldName, error.Exception);
        }
        
        return $"Некорректное значение поля '{fieldName}'.";
    }

    private string GetDefaultErrorMessage(string fieldName, Exception exception)
    {
        var message = exception.Message.ToLower();
        
        if (message.Contains("convert") || message.Contains("parse") || message.Contains("invalid"))
        {
            return $"Неверный формат данных для поля '{fieldName}'.";
        }
        
        if (message.Contains("required") || message.Contains("null") || message.Contains("missing"))
        {
            return $"Поле '{fieldName}' обязательно для заполнения.";
        }
        
        return $"Не удалось привязать значение к полю '{fieldName}'.";
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
