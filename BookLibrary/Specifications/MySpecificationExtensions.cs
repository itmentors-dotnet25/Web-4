using BookLibrary.Contracts.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Specifications;

/// <summary>
/// Методы расширения для применения спецификаций к IQueryable
/// </summary>
public static class MySpecificationExtensions
{
    /// <summary>
    /// Применить спецификацию к запросу
    /// </summary>
    public static IQueryable<T> ApplyMySpecification<T>(
        this IQueryable<T> query, 
        IMySpecification<T> spec) where T : class
    {
        ArgumentNullException.ThrowIfNull(spec);

        // 1. Включения (СНАЧАЛА — критично для EF Core)
        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }
    
        foreach (var includeString in spec.IncludeStrings)
        {
            query = query.Include(includeString);
        }
    
        // 2. Фильтрация
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }
    
        // 3. Сортировка
        if (spec.OrderBy.Count != 0) // ← CA1860: Count вместо Any()
        {
            query = query.OrderBy(spec.OrderBy[0]);
            for (int i = 1; i < spec.OrderBy.Count; i++)
            {
                query = ((IOrderedQueryable<T>)query).ThenBy(spec.OrderBy[i]);
            }
        }
        else if (spec.OrderByDescending.Count != 0)
        {
            query = query.OrderByDescending(spec.OrderByDescending[0]);
            for (int i = 1; i < spec.OrderByDescending.Count; i++)
            {
                query = ((IOrderedQueryable<T>)query).ThenByDescending(spec.OrderByDescending[i]);
            }
        }
    
        // 4. Пагинация
        if (spec.IsPagingEnabled)
        {
            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);
            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);
        }
    
        return query;
    }
}
