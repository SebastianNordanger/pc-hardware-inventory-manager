// Progress:
// Phase 1:
// Defined Part class (name, category, price, stock, low stock threshold, id), stored in a List<Part>, basic validation (no negative price/stock/low stock threshold), implemented CRUD (Create, Read, Update, Delete parts), and console menu for CRUD.

// Phase 2:
// Replace in-memory List<Part> with permanent storage using SQL Server + Entity Framework Core (EF Core) - set up DbContext, connection string, migrations, and update the CRUD methods to read/write to the database instead of the list. Setup of Git also performed.

// Phase 3 (Last phase):
// Add search filter for parts by name and category, low stock report, sorting display output, input validation polish (duplicate name checks),
// and a REST API layer (Dependency Injection setup, connection string, MapControllers, CreatePart, GetAllParts, UpdatePart, DeletePart, SearchParts (with price filter), LowStockReport, and SortParts on PartsController)


// OOP pillars (Encapsulation --> Inheritance --> Abstraction)
// Phase 1:
// Encapsulation - constructor validates its own data before assigning it (rejects negative price/stock/threshold, empty name/category in Part class).

// Phase 2:
// Inheritance - HardwareInventoryDBContext inherits from EF Core's DbContext class, giving it built-in database functionality.

// Phase 3 (Last phase):
// Abstraction - inheriting from ControllerBase gives access to Ok(), which hides the details of building an HTTP response.
// Polymorphism - not used in this project - all four OOP pillars weren't required to naturally fit the scope, and forcing one in wouldn't add real value here


// Phase 2 - EF Core setup (Package Manager Console):
// Add-Migration <Name> --> generates migration files based on current model
// Update-Database --> applies migration, and creates/updates actual DB


// Phase 2 - Git setup (terminal commands used so far, ordered by typical workflow):
// git init --> creates a new Git repository in this folder (one-time only in this context, already done for this project)
// git checkout master --> switches back to an existing branch
// git pull --> gets the latest changes before branching
// git checkout -b <branch-name> --> creates a new branch and switches to it right away
// git add . --> marks files as ready to be saved
// git commit -m "<message>" --> actually saves the marked files, with a shot message describing what changed
// git commit --amend --no-edit --> adds any newly marked changes into the previous commit instead of creating a new one (only safe if that commit hasn't been pushed/shared yet)
// git status --> shows what's ready to save, and what's not yet included
// git log --> shows past saves (history)
// git merge <branch-name> --> merges another branch's changes into the current branch 
// git diff --> shows exact line-by-line changes that aren't staged/confirmed yet


// Phase 3 - API testing (PowerShell):
// Invoke-WebRequest -Uri <url> --> sends a GET request, returns status code + response content
// Invoke-WebRequest -Uri <url> -Method POST -ContentType "application/json" -Body '<json>' -UseBasicParsing --> sends a POST request with a JSON body
// Invoke-WebRequest -Uri <url> -Method PUT -ContentType "application/json" -Body '<json>' -UseBasicParsing --> sends a PUT request with a JSON body to update an existing resource
// Invoke-WebRequest -Uri <url> -Method DELETE -UseBasicParsing --> sends a DELETE request, removes the resource (returns 204 No Content on success)
// Invoke-WebRequest -Uri <url>?<param>=<value> -UseBasicParsing --> sends a GET request with query parameters, used for SearchParts (searchTerm, minPrive, maxPrice) and SortParts (sortChoice) - LowStockReport takes no parameters
// | Select-Object -ExpandProperty Content --> shows only the JSON body from the response, instead of the full status/headers output



// Method definition to add a new part (C - Create)
// Using ?? "" on ReadLine() to avoid null warnings (ReadLine() could return null)
static Part? AddPart(HardwareInventoryDBContext db)
{

    Console.WriteLine("\nEnter part name:");
    string name = Console.ReadLine() ?? "";

    // .Any() checks if a part with this name already exists (case-insensitive)
    bool nameExists = db.Parts.Any(p => p.Name.ToLower() == name.ToLower());

    if (nameExists)
    {
        Console.WriteLine("\nA part with that name already exists.");
        return null;
    }

    Console.WriteLine("\nEnter part category:");
    string category = Console.ReadLine() ?? "" ;

    try // Convert numeric input - Could throw an error if input isn't a valid number
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
        Console.WriteLine("Part added successfully.");

        return newPart;
    }
    catch (FormatException)  // Bad number format (e.g., empty/non-numeric input)
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
    // .Any() checks if the table has at least one row
    // ! flips .Any() so that it checks if there are no matches
    // If it's empty, print a message and exit early - otherwise, continue down to the foreach loop
    if (!db.Parts.Any())
    {
        Console.WriteLine("No parts found.");
        return;
    }

    foreach (Part p in db.Parts)
    {
        Console.WriteLine($"{p.Id} | {p.Name} | {p.Category} | {p.Price:F2} | {p.Stock} | {p.LowStockThreshold}"); // :F2 shows price with exactly 2 decimal places
    }
}

// Method definition to update a part (U - Update)
static void UpdatePart(HardwareInventoryDBContext db, int id)
{
    // Finds the first part matching this ID, or null if none found - safer than looping + modifying the mid-iteration
    // => means here "for each part p, check this condition" (lambda)
    Part? p = db.Parts.FirstOrDefault(p => p.Id == id);

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

        db.SaveChanges();
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

// Method definition to delete a part (D - Delete)
static void DeletePart(HardwareInventoryDBContext db, int id)
{
    Part? p = db.Parts.FirstOrDefault(p => p.Id == id);

    if (p == null)
    {
        Console.WriteLine("Part with the specified Id not found.");
        return;
    }

    db.Parts.Remove(p);  // marks the part to be deleted
    db.SaveChanges();
    Console.WriteLine($"Part with Id {id} has been successfully deleted.");
}

// Method definition to search parts by name or category (partial text and case-insensitive match)
static void SearchParts(HardwareInventoryDBContext db, string searchTerm)
{
    // .Where() filters parts, keeping only ones where the condition is true
    // || means "or" - matches if name OR category OR both contains the search term
    // Using .Contains() instead of == so partial matches work (e.g. "409" finds "4090")
    var matches = db.Parts.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.ToLower().Contains(searchTerm.ToLower()));

    // Ask for optional price range - leave blank to skip either filter
    Console.Write("Min price (leave blank to skip): ");
    string minInput = (Console.ReadLine() ?? "").Trim();
    if (decimal.TryParse(minInput, out decimal minPrice))
    {
        matches = matches.Where(p => p.Price >= minPrice);
    }

    Console.Write("Max price (leave blank to skip): ");
    string maxInput = (Console.ReadLine() ?? "").Trim();
    if (decimal.TryParse(maxInput, out decimal maxPrice))
    {
        matches = matches.Where(p => p.Price <= maxPrice);
    }

    var result = matches.ToList();

    if (!result.Any())
    {
        Console.WriteLine("No parts found with that name/category.");
        return;
    }

    foreach(Part p in result)
    {
        Console.WriteLine($"{p.Name} | {p.Category} | {p.Price:F2} | {p.Stock} | {p.LowStockThreshold}");
    }
}

// Method definition to report parts at or below stock threshold
static void LowStockReport(HardwareInventoryDBContext db)
{
    var lowStockParts = db.Parts.Where(p => p.Stock <= p.LowStockThreshold).ToList();

    if (!lowStockParts.Any())
    {
        Console.WriteLine("No parts found below the low stock threshold");
        return;
    }

    foreach(Part p in lowStockParts)
    {
        Console.WriteLine($"{p.Name} | {p.Category} | {p.Price:F2} | {p.Stock} | {p.LowStockThreshold} ");
    }
}

// Method definition to sort and dispaly parts by a chosen field
static void SortParts(HardwareInventoryDBContext db)
{
    Console.WriteLine("Sort by:\n1. Id \n2. Name \n3. Category \n4. Price \n5. Stock \n6. Low Stock Threshold");
    Console.Write("Enter: ");
    string sortChoice = (Console.ReadLine() ?? "").Trim();

    List<Part> parts;

    switch (sortChoice)
    {
        case "1":
            parts = db.Parts.OrderBy(p => p.Id).ToList();
            break;

        case "2":
            parts = db.Parts.OrderBy(p => p.Name).ToList();
            break;

        case "3":
            parts = db.Parts.OrderBy(p => p.Category).ToList();
            break;

        case "4":
            parts = db.Parts.OrderBy(p => p.Price).ToList();
            break;

        case "5":
            parts = db.Parts.OrderBy(p => p.Stock).ToList();
            break;

        case "6":
            parts = db.Parts.OrderBy(p => p.LowStockThreshold).ToList();
            break;

        default:
            Console.WriteLine("Invalid choice.");
            return;
    }

    foreach (Part p in parts)
    {
        Console.WriteLine($"{p.Id} | {p.Name} | {p.Category} | {p.Price:F2} | {p.Stock} | {p.LowStockThreshold}");
    }
}

// Create one database connection to use for the whole program - closes automatically when the program ends
using HardwareInventoryDBContext db = new HardwareInventoryDBContext();

// Interactive menu loop to allow the user to choose which action to perform - runs until the user selectes Exit (8)
while (true)
{
    Console.WriteLine("Choose an option:\n1. Add Part \n2. Display Parts \n3. Update Parts \n4. Delete Part \n5. Search Part \n6. Low Stock Report \n7. Sort Parts \n8. Exit Menu");
    Console.WriteLine();
    Console.Write("Enter: ");
    string inputChoice = (Console.ReadLine() ?? "").Trim(); // Included .Trim() to remove any leading/trailing whitespaces from the input

    switch (inputChoice)
    {
        case "1":
            AddPart(db);
            Console.WriteLine();  // add blank line for readability
            break;

        case "2":
            Console.WriteLine();
            DisplayParts(db);
            Console.WriteLine();
            break;

        case "3":
            try
            {
                Console.Write("\nEnter the Id of the part to update: ");
                int updatePartID = int.Parse(Console.ReadLine() ?? "");
                Console.WriteLine();
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
                Console.Write("\nEnter the Id of the part to delete: ");
                int deletePartID = int.Parse(Console.ReadLine() ?? "");
                Console.WriteLine();
                DeletePart(db, deletePartID);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input: Id must be a number.");
            }
            Console.WriteLine();
            break;

        case "5":
            Console.Write("\nEnter the name/category of the part (price range prompts will follow): ");
            string searchTerm = Console.ReadLine() ?? "";
            Console.WriteLine();
            SearchParts(db, searchTerm);
            Console.WriteLine();
            break;

        case "6":
            Console.WriteLine();
            LowStockReport(db);
            Console.WriteLine();
            break;

        case "7":
            Console.WriteLine();
            SortParts(db);
            Console.WriteLine();
            break;

        case "8":
            return;

     default:
            Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 8.");
            break;
    }
}
