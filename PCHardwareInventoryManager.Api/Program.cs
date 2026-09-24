using Microsoft.EntityFrameworkCore;
using PCHardwareInventoryManager;  // Namespace where Part and HardwareInventoryDBContext live - update if console project's namespace is different

var builder = WebApplication.CreateBuilder(args); // Creates the object used to configure the app before it's built

// Everything below here configures the app before it starts 
// Register controllers so ASP.NET Core knows PartsController exists
builder.Services.AddControllers();

// Register HardwareInventoryDBContext with Dependency Injection so controllers can just ask for it in their constructor instead of doing "new HardwareInventoryDBContext()" themselves
builder.Services.AddDbContext<HardwareInventoryDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build(); // Configuration is done - this builds the actual app object

// Maps controller [HttpPost]/[HttpGet]/[HttpPut]/[HttpDelete] attributes to real routes
// Without this, PartsController exists in code but isn't reachable via HTTP
app.MapControllers();

app.Run();  // Starts the server, app is now listening for requests
