// The {get; set;} syntax is used to let other code read and change these values (standard way to write class properties in C#).

// Define the class:
public class Part
{
    // Define the properties:
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    // Keeps track of the next ID to give a new Part (shared by all Part objects)
    private static int idCounter = 0;
    public int Id { get; private set; } // Private set - only this class (and EF Core) can set it, external code can never set it directly

    // Define constructor to initialize the properties:
    public Part(string name, string category, decimal price, int stock, int lowStockThreshold)
    {
        // Check for valid values (non negative/empty) before assigning (Encapsulation)
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.");
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative.");
        if (lowStockThreshold < 0)
            throw new ArgumentException("Low stock threshold cannot be negative.");
     
        Name = name;
        Category = category;
        Price = price;
        Stock = stock;
        LowStockThreshold = lowStockThreshold;
        Id = idCounter++; // Assigns current idCounter value as this part's Id (e.g. RTX 4090), then increases idCounter by 1 so the next part created gets the next Id
    }
}
