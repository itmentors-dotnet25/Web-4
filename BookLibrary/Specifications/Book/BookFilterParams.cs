namespace BookLibrary.Specifications.Book;

public class BookFilterParams
{
    /// <summary>
    /// Фильтр по названию (частичное совпадение)
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Фильтр по автору
    /// </summary>
    public int? AuthorId { get; set; }
    
    /// <summary>
    /// Фильтр по имени автора
    /// </summary>
    public string? Author { get; set; }
    
    /// <summary>
    /// Фильтр по категории
    /// </summary>
    public int? CategoryId { get; set; }
    
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
    /// Сортировка: title, author, year, createdAt и т.д.
    /// </summary>
    public string? SortBy { get; set; }       // Поля по возрастанию
    public string? SortByDesc { get; set; }   // Поля по убыванию
    
    /// <summary>
    /// Включать ли информацию об авторах в ответ
    /// </summary>
    public bool WithAuthors { get; set; } = false;

    /// <summary>
    /// Включать ли информацию о категориях в ответ
    /// </summary>
    public bool WithCategories { get; set; } = false;
    
    /// <summary>
    /// Количество записей на страницу
    /// </summary>
    public int? PageSize { get; set; }
    
    /// <summary>
    /// Номер страницы
    /// </summary>
    public int? PageNumber { get; set; } = 1;
}
