namespace BookLibrary.Contracts;

public class CategoryStatsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}