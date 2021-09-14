using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Linq;

namespace Shop.UI.ViewComponents {
    public class CartViewComponent : ViewComponent {
        private GetCart _getCart;

        public CartViewComponent(GetCart getCart) {
            _getCart = getCart;
        }
        public IViewComponentResult Invoke(string view = "Default") {
            var model = _getCart.Do();
            if (view == "Small") {
                var totalValue = model.Sum(x => x.RealValue * x.Qty);
                return View(view, $"${totalValue:N2}");
            }

            return View(view, model);
        }
    }
}
