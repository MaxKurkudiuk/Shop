using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Stripe;
using System.Linq;
using System.Threading.Tasks;
using Cart = Shop.Application.Cart;

namespace Shop.UI.Pages.Checkout {
    public class PaymentModel : PageModel
    {
        public IActionResult OnGet([FromServices] GetCustomerInformation getCustomerInformation, [FromServices] Cart.GetOrder getOrder)
        {
            var information = getCustomerInformation.Do();

            if (information == null)
                return RedirectToPage("/Checkout/CustomerInformation");

            return Page();
        }

        public async Task<IActionResult> OnPost(string stripeEmail, string stripeToken,
                [FromServices] Cart.GetOrder getOrder, [FromServices] CreateOrder createOrder) {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var cartOrder = getOrder.Do();

            var customer = customers.Create(new CustomerCreateOptions {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions {
                Amount = cartOrder.GetTotalCharge(), // Total payment amount
                Description = "Shop Purchase",      // Form title
                Currency = "usd",                   // currency type
                Customer = customer.Id              // customer in current session
            });

            var sessionId = HttpContext.Session.Id;

            // create order
            await createOrder.Do(new CreateOrder.Request() {
                StripeReference = charge.Id,
                SessionId = sessionId,

                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                City = cartOrder.CustomerInformation.City,
                PostCode = cartOrder.CustomerInformation.PostCode,

                Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock() {
                    StockId = x.StockId,
                    Qty = x.Qty
                }).ToList()
            });

            return RedirectToPage("/Index");
        }
    }
}
