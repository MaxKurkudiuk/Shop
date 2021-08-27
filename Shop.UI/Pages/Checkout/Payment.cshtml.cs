using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Stripe;

namespace Shop.UI.Pages.Checkout {
    public class PaymentModel : PageModel
    {
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

            var customer = customers.Create(new CustomerCreateOptions {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions {
                Amount = 500,
                Description = "Sample Charge",
                Currency = "usd",
                Customer = customer.Id
            });

            return RedirectToPage("/Index");
        }
    }
}
