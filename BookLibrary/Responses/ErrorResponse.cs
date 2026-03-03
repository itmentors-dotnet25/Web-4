namespace BookLibrary.Models
{
    /// <summary>
    /// Стандартизированный ответ об ошибке
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP статус код
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Уникальный идентификатор ошибки (для логирования)
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Детали ошибки (только в режиме разработки)
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Дополнительные поля для валидации
        /// </summary>
        public Dictionary<string, string[]>? Errors { get; set; }

        public override string ToString() =>
            $"[{StatusCode}] {Message} {(TraceId != null ? $"(Trace: {TraceId})" : "")}";
    }
}