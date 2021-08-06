using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.Products;
using Shop.Database;
using System.Threading.Tasks;

namespace Shop.UI.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public ProductViewModel Product { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public void OnGet() {

        }

        public async Task<IActionResult> OnPost() {
            await new CreateProduct(_context).Do(Product);

            return RedirectToPage("Index");
        }
    }
}
