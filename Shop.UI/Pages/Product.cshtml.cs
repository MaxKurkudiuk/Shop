using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages {
    public class ProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProductModel(ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty]
        public Test ProductTest { get; set; }

        public class Test {
            public string Id { get; set; }
        }

        public GetProduct.ProductViewModel Product { get; set; }

        public IActionResult OnGet(string name)
        {
            Product = new GetProduct(_context).Do(name.Replace("-", " "));
            if (Product == null)
                return RedirectToPage("Index");
            else
                return Page();
        }

        public IActionResult OnPost() {
            var current_id = HttpContext.Session.GetString("id");

            HttpContext.Session.SetString("id", ProductTest.Id);

            return RedirectToPage("Index");
        }
    }
}
