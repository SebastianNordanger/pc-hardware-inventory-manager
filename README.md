# PC Hardware Inventory Manager

A C# console app and REST API for managing PC hardware inventory, using Entity Framework Core and SQL Server for permanent storage.

## Features

- Full CRUD operations (Create, Read, Update, Delete) on inventory parts
- Search parts by name or category, with optional price range filtering
- Low stock report - flags parts at or below their set threshold
- Sort parts by any field (Id, Name, Category, Price, Stock, Low Stock Threshold)
- REST API built with ASP.NET Core (Dependency Injection, EF Core integration)
- Input validation (rejects negative values, empty fields, duplicate part names)

## How to run

**Console app:**
1. Open the solution in Visual Studio
2. Right click `PCHardwareInventoryManager` in Solution Explorer -> "Set as Startup Project"
3. Run (F5 or the green Run button) - the console menu will guide you through all operations

**REST API:**
1. Right click `PCHardwareInventoryManager.Api` in Solution Explorer -> "Set as Startup Project"
2. Run (F5 or the green Run button) - the API will start on `http://localhost:5089` (check your launch settings for the exact port)
3. Available endpoints:
   - `GET /api/parts` - get all parts
   - `POST /api/parts` - create a new part
   - `PUT /api/parts/{id}` - update a part
   - `DELETE /api/parts/{id}` - delete a part
   - `GET /api/parts/search?searchTerm=...&minPrice=...&maxPrice=...` - search by name/category, with optional price range
   - `GET /api/parts/low-stock` - list parts at or below their stock threshold
   - `GET /api/parts/sorted?sortChoice=...` - sort by a chosen field

**Requirements:** SQL Server (LocalDB is fine) - update the connection string in `appsettings.json` / `OnConfiguring` if needed.

## Built with
C#, .NET, Entity Framework Core, ASP.NET Core, SQL Server
