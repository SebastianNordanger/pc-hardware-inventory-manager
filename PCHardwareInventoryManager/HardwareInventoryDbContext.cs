using Microsoft.EntityFrameworkCore;

// Bridges the Part objects to the database (this generates the actual DB)
public class HardwareInventoryDBContext : DbContext
{
    public DbSet<Part> Parts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PCHardwareInventoryManagerDB;Trusted_Connection=True;");
    }
}
