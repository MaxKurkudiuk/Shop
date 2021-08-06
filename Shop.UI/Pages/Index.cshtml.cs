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

        public class ProductViewModel {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public void OnGet() {

        }

        public async Task<IActionResult> OnPost() {
            await new CreateProduct(_context).Do(Product.Name, Product.Description, Product.Value);

            return RedirectToPage("Index");
        }
    }
}
