using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;
using System.Collections.Generic;

namespace Shop.UI.Pages {
    public class CartModel : PageModel {
        private ApplicationDbContext _context;

        public CartModel(ApplicationDbContext context) {
            _context = context;
        }

        public IEnumerable<GetCart.Response> Cart { get; set; }

        public IActionResult OnGet() {
            Cart = new GetCart(HttpContext.Session, _context).Do();
            return Page();
        }
    }
}
