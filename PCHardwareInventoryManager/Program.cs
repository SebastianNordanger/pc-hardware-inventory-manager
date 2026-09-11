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


// !!! * I AM HERE (NEXT SESSION: Set up GIT properly (commits, branches) before continuing. After that: update CRUD methods (AddPart, DisplayParts UpdatePart, DeletePart) to use the database HarwareInventoryDBContext instead of the in-memory List<Part>.) * !!!
// Git setup, first command: git init (run in terminal, inside the project folder)!

// Add a new instance
Part checkpartOne = new Part("RTX 4090", "GPU", 400, 5, 2);
Part checkpartTwo = new Part("RTX 4080", "GPU", 300, 10, 5);
Part checkpartThree = new Part("RTX 4070", "GPU", 200, 15, 5);

// Add a list to store the hardware parts and loop through it to display the part information
List<Part> hardwareParts = new List<Part>();
hardwareParts.Add(checkpartOne);
hardwareParts.Add(checkpartTwo);
hardwareParts.Add(checkpartThree);


// Method definition to add a new part (C - Create)
// Using ?? "" on ReadLine() to avoid null warnings (ReadLine() could return null)
static Part? AddPart()
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

        return new Part(name, category, price, stock, lowstockthreshold);
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
static void DisplayParts(List<Part> parts)
{
    foreach (Part p in parts)
    {
        Console.WriteLine($"{p.Id } {p.Name} {p.Category} {p.Price} {p.Stock} {p.LowStockThreshold}");
    }
}


// Method defintion to update a part (U - Update)
static void UpdatePart(List<Part> parts, int id)
{
    foreach (Part p in parts)
    {
        if (p.Id == id)
        {
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

                // If it gets there, validation passed - which means it's safe to update the real part
                p.Name = validated.Name;
                p.Category = validated.Category;
                p.Price = validated.Price;
                p.Stock = validated.Stock;
                p.LowStockThreshold = validated.LowStockThreshold;
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
    }
}


static void DeletePart(List<Part> parts, int id)
{
    foreach (Part p in parts)
    {
        if (p.Id == id)
        {
            parts.Remove(p);
            Console.WriteLine($"Part with Id {id} has ben succsesfully deleeted.");
            return; // Exit the method loop after deleting part
        }
    }

    // only reached if the loop finished without finding a match
    Console.WriteLine("Part with the specified Id not found.");
}

// Interactive menu loop to allow the user to choose which action to perform - runs until the user selectes Exit (5)
while (true)
{
    Console.WriteLine("1. Add Parts\n2. Display Parts \n3. Update Parts \n4. Delete Parts \n5. Exit Menu\n");
    string inputChoice = (Console.ReadLine() ?? "").Trim(); // Included .Trim() to remove any leading/trailing whitespaces from the input

    switch (inputChoice)
    {
        case "1":
            Part? newPart = AddPart();
            if (newPart != null)
            {
                hardwareParts.Add(newPart);
            }
            Console.WriteLine();
            break;

        case "2":
            DisplayParts(hardwareParts);
            Console.WriteLine();
            break;

        case "3":
            try
            {
                Console.WriteLine("\nEnter the Id of the part to update:");
                int updatePartID = int.Parse(Console.ReadLine() ?? "");
                UpdatePart(hardwareParts, updatePartID);
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
                DeletePart(hardwareParts, deletePartID);
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
