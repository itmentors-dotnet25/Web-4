namespace BookLibrary.Specifications.Book;

public class BookFilterParams
{
    /// <summary>
    /// Фильтр по названию (частичное совпадение)
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Фильтр по автору (частичное совпадение)
    /// </summary>
    public string? Author { get; set; }
    
    /// <summary>
    /// Фильтр по жанру (точное совпадение)
    /// </summary>
    public string? Genre { get; set; }
    
    /// <summary>
    /// Фильтр по году публикации
    /// </summary>
    public int? Year { get; set; }
    
    /// <summary>
    /// Фильтр по доступности
    /// </summary>
    public bool? IsAvailable { get; set; }
    
    /// <summary>
    /// Сортировка: title, author, year, createdAt
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Направление сортировки: asc, desc
    /// </summary>
    public string? SortDirection { get; set; } = "asc";
    
    /// <summary>
    /// Количество записей на страницу
    /// </summary>
    public int? PageSize { get; set; }
    
    /// <summary>
    /// Номер страницы
    /// </summary>
    public int? PageNumber { get; set; } = 1;
}
