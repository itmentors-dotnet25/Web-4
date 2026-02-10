using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookLibrary.Filters.ValidationFilters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // ПРОВЕРКА 1: Ошибки привязки модели (например, "Привет" вместо числа)
        // Это КРИТИЧЕСКИ ВАЖНО при использовании SuppressModelStateInvalidFilter = true
        if (!context.ModelState.IsValid)
        {
            var bindingErrors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? []
                );

            context.Result = ValidationResponseFormat.FromModelStateErrors(bindingErrors);
            return; // Валидатор НЕ вызывается
        }

        // ПРОВЕРКА 2: Получаем параметр из тела запроса
        var bodyParam = context.ActionArguments.Values
            .FirstOrDefault(v => v != null);

        if (bodyParam == null)
        {
            await next();
            return;
        }

        // Получаем валидатор для типа параметра
        var validatorType = typeof(IValidator<>).MakeGenericType(bodyParam.GetType());
        var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

        if (validator == null)
        {
            await next();
            return;
        }

        // Выполняем валидацию бизнес-правил через FluentValidation
        var validationResult = await validator.ValidateAsync(
            new ValidationContext<object>(bodyParam));

        if (!validationResult.IsValid)
        {
            context.Result = ValidationResponseFormat.FromValidationFailures(
                validationResult.Errors);
            return;
        }

        await next();
    }
}
