using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Orders;
using Shop.Database;

namespace Shop.UI.Pages {
    public class OrderModel : PageModel {
        private ApplicationDbContext _context;

        public OrderModel(ApplicationDbContext context) {
            _context = context;
        }

        public GetOrder.Response Order { get; set; }

        public void OnGet(string reference) {
            Order = new GetOrder(_context).Do(reference);
        }
    }
}
