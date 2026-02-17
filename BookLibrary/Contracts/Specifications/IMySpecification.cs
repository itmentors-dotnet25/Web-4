using System.Linq.Expressions;
using Ardalis.Specification;

namespace BookLibrary.Contracts.Specifications;

/// <summary>
/// Интерфейс спецификации для фильтрации запросов
/// Паттерн Specification — каноничный подход в .NET для сложной фильтрации
/// </summary>
public interface IMySpecification<T>
{
    /// <summary>
    /// Критерий фильтрации
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }
    
    /// <summary>
    /// Включения связанных сущностей для загрузки (Include)
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }
    
    /// <summary>
    /// Строковые пути для загрузки связанных сущностей
    /// </summary>
    List<string> IncludeStrings { get; }
    
    /// <summary>
    /// Сортировка по возрастанию
    /// </summary>
    List<Expression<Func<T, object>>> OrderBy { get; }
    
    /// <summary>
    /// Сортировка по убыванию
    /// </summary>
    List<Expression<Func<T, object>>> OrderByDescending { get; }
    
    /// <summary>
    /// Пагинация — количество записей для пропуска
    /// </summary>
    int? Take { get; }
    
    /// <summary>
    /// Пагинация — количество записей для возврата
    /// </summary>
    int? Skip { get; }
    
    /// <summary>
    /// Применять ли пагинацию
    /// </summary>
    bool IsPagingEnabled { get; }
}
