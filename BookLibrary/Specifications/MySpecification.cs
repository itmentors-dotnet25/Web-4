using System.Linq.Expressions;
using BookLibrary.Contracts.Specifications;

namespace BookLibrary.Specifications;

public abstract class MySpecification<T> : IMySpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];
    private readonly List<string> _includeStrings = [];
    private readonly List<Expression<Func<T, object>>> _orderBy = [];
    private readonly List<Expression<Func<T, object>>> _orderByDescending = [];

    public virtual Expression<Func<T, bool>>? Criteria => null;
    public virtual List<Expression<Func<T, object>>> Includes => _includes;
    public virtual List<string> IncludeStrings => _includeStrings;
    public virtual List<Expression<Func<T, object>>> OrderBy => _orderBy;
    public virtual List<Expression<Func<T, object>>> OrderByDescending => _orderByDescending;
    public virtual int? Take => null;
    public virtual int? Skip => null;
    public virtual bool IsPagingEnabled => false;

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        _includeStrings.Add(includeString);
    }

    protected virtual void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        _orderBy.Add(orderByExpression);
    }

    protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        _orderByDescending.Add(orderByDescendingExpression);
    }
    
        // ===== СОРТИРОВКА ПО СТРОКОВЫМ ПАРАМЕТРАМ =====
    /// <summary>
    /// Применяет сортировку на основе строковых параметров sortBy (ASC) и sortByDesc (DESC).
    /// Поддерживает множественные поля через запятую. Добавляет сортировку по умолчанию, если не задана.
    /// </summary>
    /// <param name="sortBy">Поля для сортировки по возрастанию (через запятую)</param>
    /// <param name="sortByDesc">Поля для сортировки по убыванию (через запятую)</param>
    /// <param name="defaultSort">Выражение для сортировки по умолчанию (если не заданы параметры)</param>
    protected void ApplySorting(
        string? sortBy,
        string? sortByDesc,
        Expression<Func<T, object>>? defaultSort = null)
    {
        // Применяем сортировку по возрастанию
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            foreach (var field in ParseSortFields(sortBy))
            {
                if (GetSortExpression(field) is { } expr)
                    AddOrderBy(expr);
            }
        }

        // Применяем сортировку по убыванию
        if (!string.IsNullOrWhiteSpace(sortByDesc))
        {
            foreach (var field in ParseSortFields(sortByDesc))
            {
                if (GetSortExpression(field) is { } expr)
                    AddOrderByDescending(expr);
            }
        }

        // Добавляем сортировку по умолчанию, если ничего не применено
        if (OrderBy.Count == 0 && OrderByDescending.Count == 0 && defaultSort != null)
        {
            AddOrderBy(defaultSort);
        }
    }

    /// <summary>
    /// Возвращает выражение сортировки по имени поля. Должен быть переопределён в дочерних классах.
    /// </summary>
    /// <param name="field">Имя поля в нижнем регистре (без учёта регистра)</param>
    /// <returns>Выражение для сортировки или null, если поле не поддерживается</returns>
    protected virtual Expression<Func<T, object>>? GetSortExpression(string field) => null;

    // ===== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====
    private static IEnumerable<string> ParseSortFields(string input) =>
        input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
             .Select(f => f.Trim().ToLowerInvariant());
}
