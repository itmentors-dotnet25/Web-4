using Ardalis.Specification;
using System.Linq.Expressions;

namespace BookLibrary.Specifications.Author;

public class ArdalisAuthorSpecification : Specification<Models.Author>
{
    // Поля разрешенные для сортировки
    private static readonly Dictionary<string, Expression<Func<Models.Author, object>>> SortSelectors = 
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = a => a.Name,
            ["country"] = a => a.Country ?? string.Empty,
            ["birthyear"] = a => a.BirthYear ?? 0,
            ["createdat"] = a => a.CreatedAt,
            ["id"] = a => a.Id
        };

    public ArdalisAuthorSpecification(AuthorFilterParams filterParams)
    {
        // Фильтрация
        if (!string.IsNullOrWhiteSpace(filterParams.Name))
        {
            Query.Where(a => a.Name.Contains(filterParams.Name!, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrWhiteSpace(filterParams.Country))
        {
            Query.Where(a => a.Country == filterParams.Country);
        }
        
        if (filterParams.BirthYear.HasValue)
        {
            Query.Where(a => a.BirthYear == filterParams.BirthYear.Value);
        }
        
        if (filterParams.IsActive.HasValue)
        {
            Query.Where(a => a.IsActive == filterParams.IsActive.Value);
        }
        
        // Множественная сортировка
        ApplyMultipleSorting(filterParams.SortBy, filterParams.SortByDesc);
        
        // Пагинация
        if (filterParams.PageSize.HasValue && filterParams.PageNumber.HasValue)
        {
            Query.Skip((filterParams.PageNumber.Value - 1) * filterParams.PageSize.Value)
                 .Take(filterParams.PageSize.Value);
        }
    }
    
    private void ApplyMultipleSorting(string? sortBy, string? sortByDesc)
    {
        var hasPrimarySort = false;
        
        // Сортировка по возрастанию (первое поле = основная, остальные = дополнительные)
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var fields = sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var field in fields)
            {
                if (SortSelectors.TryGetValue(field, out var selector))
                {
                    Query.OrderBy(selector!);
                    hasPrimarySort = true;
                }
            }
        }
        
        // Сортировка по убыванию
        if (!string.IsNullOrWhiteSpace(sortByDesc))
        {
            var fields = sortByDesc.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var field in fields)
            {
                if (SortSelectors.TryGetValue(field, out var selector))
                {
                    Query.OrderByDescending(selector!);
                    hasPrimarySort = true;
                }
            }
        }
        
        // Если сортировка не указана, сортируем по Id по умолчанию
        if (!hasPrimarySort)
        {
            Query.OrderBy(a => a.Id);
        }
    }
}
