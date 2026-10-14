// Progress:
// Phase 1:
// Defined Part class (name, category, price, stock, low stock threshold, id), stored in a List<Part>, basic validation (no negative price/stock/low stock threshold), implemented CRUD (Create, Read, Update, Delete parts), and console menu for CRUD.

// Phase 2:
// Replace in-memory List<Part> with permanent storage using SQL Server + Entity Framework Core (EF Core) - set up DbContext, connection string, migrations, and update the CRUD methods to read/write to the database instead of the list. Setup of Git also performed.

// Phase 3 (Last phase):
// Add search filter for parts by name and category, low stock report, sorting display output, input validation polish (duplicate name checks),
// a REST API layer (Dependency Injection setup, connection string, MapControllers, CreatePart, GetAllParts, UpdatePart, DeletePart, SearchParts (with price filter), LowStockReport, and SortParts on PartsController), and Docker Compose (API + SQL Server as two containers).


// OOP pillars (Encapsulation -> Inheritance -> Abstraction)
// Phase 1:
// Encapsulation - constructor validates its own data before assigning it (rejects negative price/stock/threshold, empty name/category in Part class).

// Phase 2:
// Inheritance - HardwareInventoryDBContext inherits from EF Core's DbContext class, giving it built-in database functionality.

// Phase 3 (Last phase):
// Abstraction - inheriting from ControllerBase gives access to Ok(), which hides the details of building an HTTP response.

// Polymorphism - not used in this project - all four OOP pillars weren't required to naturally fit the scope, and forcing one in wouldn't add real value here


// Phase 2 - EF Core setup (Package Manager Console):
// Add-Migration <Name> -> generates migration files based on current model
// Update-Database -> applies migration, and creates/updates actual DB


// Phase 2 - Git setup (terminal commands used so far, ordered by typical workflow):
// git init -> creates a new Git repository in this folder (one-time only in this context, already done for this project)
// git checkout master -> switches back to an existing branch
// git pull -> gets the latest changes before branching
// git checkout -b <branch-name> -> creates a new branch and switches to it right away
// git add . -> marks files as ready to be saved
// git commit -m "<message>" -> actually saves the marked files, with a shot message describing what changed
// git commit --amend --no-edit -> adds any newly marked changes into the previous commit instead of creating a new one (only safe if that commit hasn't been pushed/shared yet)
// git status -> shows what's ready to save, and what's not yet included
// git log -> shows past saves (history)
// git merge <branch-name> -> merges another branch's changes into the current branch 
// git diff -> shows exact line-by-line changes that aren't staged/confirmed yet


// Phase 3 - API testing (PowerShell):
// Invoke-WebRequest -Uri <url> -> sends a GET request, returns status code + response content
// Invoke-WebRequest -Uri <url> -Method POST -ContentType "application/json" -Body '<json>' -UseBasicParsing -> sends a POST request with a JSON body
// Invoke-WebRequest -Uri <url> -Method PUT -ContentType "application/json" -Body '<json>' -UseBasicParsing -> sends a PUT request with a JSON body to update an existing resource
// Invoke-WebRequest -Uri <url> -Method DELETE -UseBasicParsing -> sends a DELETE request, removes the resource (returns 204 No Content on success)
// Invoke-WebRequest -Uri <url>?<param>=<value> -UseBasicParsing -> sends a GET request with query parameters, used for SearchParts (searchTerm, minPrice, maxPrice) and SortParts (sortChoice) - LowStockReport takes no parameters
// | Select-Object -ExpandProperty Content -> shows only the JSON body from the response, instead of the full status/headers output


// Phase 3 - Docker commands used (API + SQL Server as two containers via Docker Compose):
// docker compose up --build -d -> builds the API image and starts both containers in the background
// docker compose up -d db -> starts only the database container (used to test it on its own)
// docker ps -> lists running containers
// docker compose logs api -> shows the API container's output (used to find startup errors)
// docker compose start db -> starts the stopped database container (keeps its data)
// docker compose stop db -> stops the database container without removing it (keeps its data)
// docker compose down -> stops and removes the containers (and the test data inside them)


// Phase 3 - Docker notes:
// The API reaches the database by the service name "db" (defined in docker-compose.yml), not localhost
// OnConfiguring only uses the LocalDB fallback if nothing else was configured (IsConfigured), because it overrode the Docker connection string and crashed with "LocalDB is not supported on this platform"
// Removed the console project's unused appsettings.json (the console app uses a hardcoded connection string), it clashed with the API's appsettings.json when publishing
// The API runs Database.Migrate() on startup because the Docker database starts empty
// The console app still uses LocalDB, the Docker database is separate (test data only)
// The Dockerfile sets ASPNETCORE_ENVIRONMENT=Development so Swagger UI works in the container


// Endpoints:
// GET /api/parts -> returns all parts
// POST /api/parts -> creates a new part
// PUT /api/parts/{id} -> updates an existing part
// DELETE /api/parts/{id} -> deletes a part
// GET /api/parts/search?searchTerm=...&minPrice=...&maxPrice=... -> search by name/category, with optional price range
// GET /api/parts/low-stock -> returns parts at or below their stock threshold
// GET /api/parts/sorted?sortChoice=... -> sorts parts by a chosen field


// Also added Swagger UI (/swagger) afterward, for interactively browsing/testing endpoints in the browser as an alternative to PowerShell (in Docker: http://localhost:8080/swagger).
