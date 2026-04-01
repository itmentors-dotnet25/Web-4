namespace BookLibrary.Contracts;

public class CategoryStatsDto
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public double AveragePublicationYear { get; set; }
}