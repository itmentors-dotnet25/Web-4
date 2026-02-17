using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Seeders;

public interface IDataSeeder
{
    void Seed(ModelBuilder modelBuilder);
}
