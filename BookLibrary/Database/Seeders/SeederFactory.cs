using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Seeders;

public static class SeederFactory
{
    public static void ApplySeeders(ModelBuilder modelBuilder)
    {
        var seeders = new IDataSeeder[]
        {
            new AuthorSeeder(),
            new CategorySeeder(),
            new BookSeeder()
        };

        foreach (var seeder in seeders)
        {
            seeder.Seed(modelBuilder);
        }
    }
}
