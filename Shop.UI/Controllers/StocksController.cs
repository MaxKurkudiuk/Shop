using Microsoft.AspNetCore.Mvc;
using Shop.Application.StockAdmin;
using Shop.Database;
using System.Threading.Tasks;

namespace Shop.UI.Controllers {
    [Route("[controller]")]
    public class StocksController : Controller {
        private ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult GetStock() => Ok(new GetStock(_context).Do());

        [HttpPost("")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request request) => Ok(await new CreateStock(_context).Do(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id) => Ok(await new DeleteStock(_context).Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request) => Ok(await new UpdateStock(_context).Do(request));
    }
}
