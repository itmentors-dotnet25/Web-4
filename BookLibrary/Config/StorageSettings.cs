namespace BookLibrary.Config;

public class StorageSettings
{
    /// <summary>
    /// Провайдер хранилища: "InMemory" или "PostgreSQL"
    /// </summary>
    public string Provider { get; set; } = "PostgreSQL";

    /// <summary>
    /// Определяет, используется ли in-memory хранилище
    /// </summary>
    public bool UseInMemory => Provider.Equals("InMemory", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Определяет, используется ли PostgreSQL
    /// </summary>
    public bool UsePostgreSQL => Provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase);
}
