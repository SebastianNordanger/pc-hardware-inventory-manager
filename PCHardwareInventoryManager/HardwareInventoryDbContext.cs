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

    // Fallback configuration used when no options are passed in (e.g. by the console app, which calls "new HardwareInventoryDBContext()" with no arguments)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PCHardwareInventoryManagerDB;Trusted_Connection=True;");
    }
}
