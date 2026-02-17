using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookLibrary.Filters.ValidationFilters;

public class ValidationResponseFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        
        if (controllerActionDescriptor != null)
        {
            foreach (var parameter in controllerActionDescriptor.Parameters)
            {
                var parameterName = parameter.Name;
                var parameterType = parameter.ParameterType;
                
                if (IsPrimitiveType(parameterType) || parameterType == typeof(CancellationToken))
                    continue;
                
                var parameterValue = context.ActionArguments.TryGetValue(parameterName, out var value) 
                    ? value 
                    : null;
                
                var validatorType = typeof(IValidator<>).MakeGenericType(parameterType);
                var validator = serviceProvider.GetService(validatorType) as IValidator;
                
                if (validator != null && parameterValue != null)
                {
                    // ПРОВЕРЯЕМ, ЕСТЬ ЛИ ОШИБКИ ПРИВЯЗКИ ДЛЯ СВОЙСТВ ЭТОГО ТИПА
                    var propertyNames = parameterType.GetProperties()
                        .Select(p => p.Name.ToLowerInvariant())
                        .ToHashSet();
                    
                    bool hasBindingErrors = context.ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .Select(kvp => 
                        {
                            string key = kvp.Key.ToLowerInvariant();
                            // Убираем префиксы
                            if (key.StartsWith("$."))
                                key = key.Substring(2);
                            if (key.StartsWith(parameterName.ToLowerInvariant() + "."))
                                key = key.Substring(parameterName.Length + 1);
                            if (key.StartsWith("[frombody]"))
                                key = key.Replace("[frombody]", "").TrimStart(' ', '.');
                            return key;
                        })
                        .Any(key => propertyNames.Contains(key) || key.Contains("."));
                    
                    // ВЫЗЫВАЕМ ВАЛИДАТОР ТОЛЬКО ЕСЛИ НЕТ ОШИБОК ПРИВЯЗКИ
                    if (hasBindingErrors) continue;
                    
                    // АСИНХРОННЫЙ ВЫЗОВ!
                    var validationResult = await validator.ValidateAsync(new ValidationContext<object>(parameterValue));
                        
                    foreach (var failure in validationResult.Errors)
                    {
                        var fieldName = ToCamelCase(failure.PropertyName);
                            
                        var modelStateEntry = context.ModelState.GetValueOrDefault(fieldName);
                        var errorExists = modelStateEntry?.Errors.Any(e => e.ErrorMessage == failure.ErrorMessage) ?? false;
                            
                        if (!errorExists)
                        {
                            context.ModelState.AddModelError(fieldName, failure.ErrorMessage);
                        }
                    }
                }
            }
        }
        
        // Формируем ответ
        if (!context.ModelState.IsValid)
        {
            var errors = new Dictionary<string, List<string>>();
            
            foreach (var kvp in context.ModelState.Where(m => m.Value?.Errors.Count > 0))
            {
                var fieldName = NormalizeFieldName(kvp.Key, context);
                
                if (string.IsNullOrEmpty(fieldName))
                    continue;
                
                var errorMessages = new List<string>();
                
                foreach (var error in kvp.Value!.Errors)
                {
                    var formatted = FormatErrorMessage(fieldName, error);
                    errorMessages.Add(formatted);
                }
                
                errors[fieldName] = errorMessages.Distinct().ToList();
            }

            var response = ValidationResponseFormat.FromModelStateErrors(errors);
            context.Result = response;
            return; // Не продолжаем выполнение
        }
        
        // Продолжаем выполнение экшена
        await next();
    }

    private string ToCamelCase(string? input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            return input ?? string.Empty;
        
        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }

    private string NormalizeFieldName(string? key, ActionExecutingContext context)
    {
        if (string.IsNullOrEmpty(key) || key == "$")
            return string.Empty;

        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var parameterNames = controllerActionDescriptor?.Parameters
            .Select(p => p.Name)
            .ToList() ?? new List<string>();

        if (parameterNames.Contains(key, StringComparer.OrdinalIgnoreCase))
            return string.Empty;

        if (key.StartsWith("$.", StringComparison.OrdinalIgnoreCase))
            return key.Substring(2);

        foreach (var paramName in parameterNames)
        {
            var simplePrefix = $"{paramName}.";
            if (key.StartsWith(simplePrefix, StringComparison.OrdinalIgnoreCase))
                return key.Substring(simplePrefix.Length);
            
            var bodyPrefix = $"[FromBody] {paramName}.";
            if (key.StartsWith(bodyPrefix, StringComparison.OrdinalIgnoreCase))
                return key.Substring(bodyPrefix.Length);
            
            var bodyPrefixAlt = $"[FromBody]{paramName}.";
            if (key.StartsWith(bodyPrefixAlt, StringComparison.OrdinalIgnoreCase))
                return key.Substring(bodyPrefixAlt.Length);
        }

        return key;
    }

    private bool IsPrimitiveType(Type type) => type.IsPrimitive || 
                                               type == typeof(string) || 
                                               type == typeof(decimal) || 
                                               type == typeof(DateTime) ||
                                               type == typeof(Guid) ||
                                               type.IsEnum;

    private string FormatErrorMessage(string fieldName, ModelError error)
    {
        if (!string.IsNullOrEmpty(error.ErrorMessage))
        {
            var msg = error.ErrorMessage;
            
            // Универсальное сообщение для ошибок конвертации
            if (msg.Contains("could not be converted", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("The input was not valid", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("не может быть пустым", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("is not valid", StringComparison.OrdinalIgnoreCase))
            {
                return $"Неверный формат поля '{fieldName}'.";
            }
            
            if (msg.Contains("Path:") && msg.Contains("LineNumber:"))
            {
                return msg.Split("Path:")[0].Trim();
            }
            
            return msg;
        }
        
        if (error.Exception != null)
        {
            return $"Неверный формат поля '{fieldName}'.";
        }
        
        return $"Неверный формат поля '{fieldName}'.";
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
