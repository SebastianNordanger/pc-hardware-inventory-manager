// Progress:
// Phase 1: Defined Part class (name, category, price, stock, low stock threhold, id), stored in a List<Part>, basic validation (no negative price/stock/low stock threshold), implemented CRUD (Create, Read, Update, Delete parts), and console menu for CRUD.
// Phase 2: Replace in-memory List<Part> with permanent storage using SQL Server + Entity Framework Core (EF Core) - set up DbContext, connection string, migrations, and update the CRUD methods to read/write to the database instead of the list. Setup of Git also performed.
// * !!! I AM HERE (*might need to modify later!) !!! * 
// Phase 3:


// OOP pillars (Encapsulation --> Inheritance --> ...?) (*might not need to add more here, cause not needed it seems in this project):
// Phase 1: Encapsulation - constructor validates its own data before assigning it (rejects negative price/stock/threshold, empty name/category in Part class).
// Phase 2: Inheritance - HardwareInventoryDBContext inherits from EF Core's DbContext class, giving it built-in database functionality. !!! * I AM HERE (*missing if used here or later: Polymorphism and Abstraction) !!! *
// Phase 3:

// Phase 2 - EF Core setup (Package Manager Console):
// Add-Migration <Name> --> generates migration files based on current model
// Update-Database --> applies migration, and creattes/updates actual DB

// Phase 2 - Git setup (terminal commands used so far):
// git init --> creates a new Git repository in this folder
// git add . --> marks files as ready to be saved
// git commit -m "<message>" --> actually saves the marked files, with a shot message describing what changed
// git status --> shows what's ready to save, and what's not yet included
// git log --> shows past saves (history)
// git checkout -b <branch-name> --> creates a new branch and switches to it right away


// Method definition to add a new part (C - Create)
// Using ?? "" on ReadLine() to avoid null warnings (ReadLine() could return null)
static Part? AddPart(HardwareInventoryDBContext db)
{
    Console.WriteLine("\nEnter  part name:");
    string name = Console.ReadLine() ?? "";

    Console.WriteLine("\nEnter part category:");
    string category = Console.ReadLine() ?? "" ;

    try // Parse numeric input - Could throw an error if input isn't a valid number
    {
        Console.WriteLine("\nEnter price:");
        decimal price = decimal.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("\nEnter stock:");
        int stock = int.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("\nEnter low stock threshold:");
        int lowstockthreshold = int.Parse(Console.ReadLine() ?? "");

        Part newPart = new Part(name, category, price, stock, lowstockthreshold);

        db.Parts.Add(newPart); // marks the new part for saving
        db.SaveChanges(); // actually writes it to the database
        Console.WriteLine();  // blank line before success message
        Console.WriteLine("Part added successfully.");

        return newPart;
    }
    catch (FormatException)  // Bad number format (e.g., emtpy/non-numeric input)
    {
        Console.WriteLine("Invalid input: price/stock/low stock threshold must be numbers.");
            return null;
    }
    catch (ArgumentException ex)  // Part constructor rejected invalid values
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
        return null;
    }
}

// Method definition to display the parts in the list (R - Read)
static void DisplayParts(HardwareInventoryDBContext db)
{
    // .Any() checks if the table as at least one row
    // If it's empty, print a message and exit early - otherwise, continue down to the foreach loop
    if (!db.Parts.Any())
    {
        Console.WriteLine("No parts found.");
        return;
    }

    foreach (Part p in db.Parts)
    {
        Console.WriteLine($"{p.Id} {p.Name} {p.Category} {p.Price:F2} {p.Stock} {p.LowStockThreshold}"); // :F2 shows price with exactly 2 decimal places
    }
}

// Method defintion to update a part (U - Update)
static void UpdatePart(HardwareInventoryDBContext db, int id)
{
    // Finds the first part matching this ID, or null if none found - safer than looping + modifying the mid-iteration
    Part? p = db.Parts.FirstOrDefault(x => x.Id == id);

    if (p == null)
    {
        Console.WriteLine("Part with the specified ID not found.");
        return;
    }

    Console.WriteLine("\nEnter new part name:");
    string name = Console.ReadLine() ?? "";

    Console.WriteLine("\nEnter new part category:");
    string category = Console.ReadLine() ?? "";

    try
    {
        Console.WriteLine("\nEnter new price:");
        decimal price = decimal.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("\nEnter new stock:");
        int stock = int.Parse(Console.ReadLine() ?? "");

        Console.WriteLine("\nEnter new low stock threshold:");
        int lowstockthreshold = int.Parse(Console.ReadLine() ?? "");

        // Create a temporary Part reusing Part's constructor to check the new values are valid
        Part validated = new Part(name, category, price, stock, lowstockthreshold);

        // Constructor validated the new values (no empty name/category, no negative price/stock/threshold) - safe to copy them onto the real part
        p.Name = validated.Name;
        p.Category = validated.Category;
        p.Price = validated.Price;
        p.Stock = validated.Stock;
        p.LowStockThreshold = validated.LowStockThreshold;

        db.SaveChanges();  // saves the changes made to p above
        Console.WriteLine("Part updated sucessfully.");
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid input: price/stock/low stock threshold must be numbers.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
    }
}

static void DeletePart(HardwareInventoryDBContext db, int id)
{
    Part? p = db.Parts.FirstOrDefault(x => x.Id == id);

    if (p == null)
    {
        Console.WriteLine("Part with the specified Id not found.");
        return;
    }

    db.Parts.Remove(p);  // marks the part to be deleted
    db.SaveChanges();
    Console.WriteLine($"Part with Id {id} has ben successfully deleted.");
}

// Create one database connection to use for the whole program - closes automatically when the program ends
using HardwareInventoryDBContext db = new HardwareInventoryDBContext();

// Interactive menu loop to allow the user to choose which action to perform - runs until the user selectes Exit (5)
while (true)
{
    Console.WriteLine("1. Add Parts\n2. Display Parts \n3. Update Parts \n4. Delete Parts \n5. Exit Menu\n");
    string inputChoice = (Console.ReadLine() ?? "").Trim(); // Included .Trim() to remove any leading/trailing whitespaces from the input

    switch (inputChoice)
    {
        case "1":
            AddPart(db);
            Console.WriteLine();
            break;

        case "2":
            DisplayParts(db);
            Console.WriteLine();
            break;

        case "3":
            try
            {
                Console.WriteLine("\nEnter the Id of the part to update:");
                int updatePartID = int.Parse(Console.ReadLine() ?? "");
                UpdatePart(db, updatePartID);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input: Id must be a number.");
            }
            Console.WriteLine();
            break;

        case "4":
            try
            {
                Console.WriteLine("\nEnter the Id of the part to delete:");
                int deletePartID = int.Parse(Console.ReadLine() ?? "");
                DeletePart(db, deletePartID);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input: Id must be a number.");
            }
            Console.WriteLine();
            break;

        case "5":
            return;

     default:
            Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 5,");
            break;
    }
}
