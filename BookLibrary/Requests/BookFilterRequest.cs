namespace BookLibrary.Requests
{
    /// <summary>
    /// Параметры фильтрации и сортировки книг
    /// </summary>
    public class BookFilterRequest
    {
        /// <summary>
        /// Фильтр по автору (частичное совпадение)
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Поле для сортировки (по умолчанию: title)
        /// </summary>
        public string? SortBy { get; set; } = "title";

        /// <summary>
        /// Направление сортировки: asc / desc
        /// </summary>
        public string? SortOrder { get; set; } = "asc";
    }
}