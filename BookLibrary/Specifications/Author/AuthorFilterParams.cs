using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Specifications.Author;

/// <summary>
/// Параметры фильтрации для авторов
/// </summary>
public class AuthorFilterParams
{
    /// <summary>
    /// Фильтр по имени (частичное совпадение)
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Фильтр по стране (точное совпадение)
    /// </summary>
    public string? Country { get; set; }
    
    /// <summary>
    /// Фильтр по году рождения
    /// </summary>
    public int? BirthYear { get; set; }
    
    /// <summary>
    /// Фильтр по статусу активности
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Сортировка: name, country, birthYear, createdAt и т.д.
    /// </summary>
    public string? SortBy { get; set; }       // Поля по возрастанию
    public string? SortByDesc { get; set; }   // Поля по убыванию
    
    /// <summary>
    /// Включать ли информацию об авторах в ответ
    /// </summary>
    public bool WithBooks { get; set; } = false;

    
    /// <summary>
    /// Количество записей на страницу
    /// </summary>
    public int? PageSize { get; set; }
    
    /// <summary>
    /// Номер страницы
    /// </summary>
    public int? PageNumber { get; set; } = 1;
}
