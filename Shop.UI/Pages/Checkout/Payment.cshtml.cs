using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;
using Stripe;

namespace Shop.UI.Pages.Checkout {
    public class PaymentModel : PageModel
    {
        private ApplicationDbContext _context;

        public PaymentModel(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            if (information == null)
                return RedirectToPage("/Checkout/CustomerInformation");

            return Page();
        }

        public IActionResult OnPost(string stripeEmail, string stripeToken) {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var cartOrder = new GetOrder(HttpContext.Session, _context).Do();

            var customer = customers.Create(new CustomerCreateOptions {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions {
                Amount = cartOrder.GetTotalCharge(),
                Description = "Shop Purchase",
                Currency = "usd",
                Customer = customer.Id
            });

            return RedirectToPage("/Index");
        }
    }
}
