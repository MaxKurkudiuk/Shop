using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System.Linq;

namespace Shop.UI.ViewComponents {
    public class CartViewComponent : ViewComponent {
        private ApplicationDbContext _context;

        public CartViewComponent(ApplicationDbContext context) {
            _context = context;
        }

        public IViewComponentResult Invoke(string view = "Default") {
            var model = new GetCart(HttpContext.Session, _context).Do();
            if (view == "Small") {
                var totalValue = model.Sum(x => x.RealValue * x.Qty);
                return View(view, $"${totalValue:N2}");
            }

            return View(view, model);
        }
    }
}
