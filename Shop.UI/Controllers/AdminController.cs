using Microsoft.AspNetCore.Mvc;
using Shop.Database;

namespace Shop.UI.Controllers {
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context) {
            _context = context;
        }

    }
}
