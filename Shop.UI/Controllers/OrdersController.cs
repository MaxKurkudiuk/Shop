using Microsoft.AspNetCore.Mvc;
using Shop.Application.OrdersAdmin;
using Shop.Database;
using System.Threading.Tasks;

namespace Shop.UI.Controllers {
    [Route("[controller]")]
    public class OrdersController : Controller {
        private ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult GetOrders(int status) => Ok(new GetOrders(_context).Do(status));
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id) => Ok(new GetOrder(_context).Do(id));
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id) => Ok(await new UpdateOrder(_context).Do(id));
    }
}
