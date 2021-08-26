using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;

namespace Shop.UI.Pages.Checkout {
    public class CustomerInformationModel : PageModel {
        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }

        public IActionResult OnGet() {
            // Get CustomerInformation
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            // If CustomerInformation exists go to paiment
            if (information == null)
                return Page();
            else
                return RedirectToPage("/Checkout/Payment");
        }

        public IActionResult OnPost() {
            // Post CustomerInformation
            if (!ModelState.IsValid)
                return Page();

            new AddCustomerInformation(HttpContext.Session).Do(CustomerInformation);
            return RedirectToPage("/Checkout/Payment");
        }
    }
}
