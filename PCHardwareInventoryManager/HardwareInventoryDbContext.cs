using Microsoft.EntityFrameworkCore;

// Bridges the Part objects to the database (this generates the actual DB)
public class HardwareInventoryDBContext : DbContext
{
    public DbSet<Part> Parts { get; set; }

    // Parameterless constructor used by the console app - relies on OnConfiguring() for setup
    public HardwareInventoryDBContext()
    {
    }

    // Constructor used by the Api project's Dependency Injection - accepts configurations (like the connection string) from PCHardwareInventoryManager.Api's Program.cs
    public HardwareInventoryDBContext(DbContextOptions<HardwareInventoryDBContext> options) : base(options)
    {
    }

    // Fallback used when no options are passed in (e.g. by the console app, which calls "new HardwareInventoryDBContext()" )
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Skip the fallback if the API already set up the database connection itself
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PCHardwareInventoryManagerDB;Trusted_Connection=True;");
        }
    }
}
