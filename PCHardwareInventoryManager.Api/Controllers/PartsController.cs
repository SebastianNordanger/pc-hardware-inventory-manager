using Microsoft.AspNetCore.Mvc;

namespace PCHardWareInventoryManager.Api.Controllers
{
    // [ApiController] enables built-in API behaviors, [Route] sets the base URL to api/parts
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        // Dependency Injection - ASP.Net Core provides the DbContext automatically
        private readonly HardwareInventoryDBContext _context;

        public PartsController(HardwareInventoryDBContext context)
        {
            _context = context;
        }

        // Method definition to handle POST requests to api/parts (create a new part) (C - Create)
        [HttpPost]
        public IActionResult CreatePart([FromBody] Part newPart)
        {
            _context.Parts.Add(newPart);
            _context.SaveChanges();

            return Ok(newPart);
        }

        // Method definition to handle GET requests to api/parts (returns all parts as JSON) (R - Read)
        [HttpGet]
        public IActionResult GetAllParts()
        {
            var allParts = _context.Parts.ToList();

            return Ok(allParts);
        }

        // Method definition to handle PUT requests to api/parts/{id} (updates an existing part) (U - Update)
        [HttpPut("{id}")]
        public IActionResult UpdatePart(int id, Part updatedPart)
        {
            var existingPart = _context.Parts.Find(id);

            if (existingPart == null)
            {
                return NotFound();
            }

            // Copy new values from updatedPart onto existingPart - EF Core only saves changes made to the tracked object, manual assignment of new values needed
            existingPart.Name = updatedPart.Name;
            existingPart.Category = updatedPart.Category;
            existingPart.Price = updatedPart.Price;
            existingPart.Stock = updatedPart.Stock;
            existingPart.LowStockThreshold = updatedPart.LowStockThreshold;

            _context.SaveChanges();

            return Ok(existingPart);
        }

        // Method defintion to handle DELETE requests to api/parts/{id} (deletes an existing part) (D - Delete)
        [HttpDelete("{id}")]
        public IActionResult DeletePart(int id)
        {
            var partToDelete = _context.Parts.Find(id);

            if (partToDelete == null)
            {
                return NotFound();
            }

            _context.Parts.Remove(partToDelete);
            _context.SaveChanges();

            return NoContent();

        }

        // Method defintion to handle GET requests for api/parts/search?searchTerm=... (search by name or category, partial/case-insenstive match)
        [HttpGet("search")]
        public IActionResult SearchParts([FromQuery] string searchTerm, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            // Matches happen on both Name and Category, using ToLower() for case-insensitive comparison
            var matches = _context.Parts.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.ToLower().Contains(searchTerm.ToLower()));

            if (minPrice.HasValue)
            {
                matches = matches.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                matches = matches.Where(p => p.Price <= maxPrice.Value);
            }

            var result = matches.ToList();

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        // Method definition to handle GET requests for api/parts/low-stock (returns parts at or below their low stock threshold)
        [HttpGet("low-stock")]
        public IActionResult LowStockReport()
        {

            // Parts where current stock is at or below their set threshold
            var lowStockParts = _context.Parts.Where(p => p.Stock <= p.LowStockThreshold).ToList();

            if (!lowStockParts.Any())
            {
                return NotFound();
            }

            return Ok(lowStockParts);
        }

        // Method definition to handle GET requests for api/parts/sorted?sortBy=... (sorts parts by a chosen field: id, name, category, price, stock, or lowstockthreshold)
        [HttpGet("sorted")]
        public IActionResult SortParts([FromQuery] string sortChoice)
        {
            IQueryable<Part> parts = _context.Parts;

            switch (sortChoice?.ToLower())
            {
                case "id":
                    parts = parts.OrderBy(p => p.Id);
                    break;
                case "name":
                    parts = parts.OrderBy(p => p.Name);
                    break;
                case "category":
                    parts = parts.OrderBy(p => p.Category);
                    break;
                case "price":
                    parts = parts.OrderBy(p => p.Price);
                    break;
                case "stock":
                    parts = parts.OrderBy(p => p.Stock);
                    break;
                case "lowstockthreshold":
                    parts = parts.OrderBy(p => p.LowStockThreshold);
                    break;
                default:
                    return BadRequest("Invalid sort field.");
            }

            return Ok(parts.ToList());
        }
    }
}
