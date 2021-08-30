using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Shop.Application.Cart;

namespace Shop.UI.Pages.Checkout {
    public class CustomerInformationModel : PageModel {
        private IWebHostEnvironment _envoronment;

        public CustomerInformationModel(IWebHostEnvironment envoronment) {
            _envoronment = envoronment;
        }

        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }

        public IActionResult OnGet() {
            // Get CustomerInformation
            var information = new GetCustomerInformation(HttpContext.Session).Do();

            // If CustomerInformation exists go to paiment
            if (information == null) {
                if (_envoronment.IsDevelopment())
                    CustomerInformation = new AddCustomerInformation.Request() {
                        FirstName = "testFirstName",
                        LastName = "testLastName",
                        Email = "test@email.com",
                        PhoneNumber = "123123123",
                        Address1 = "Address1",
                        Address2 = "Address2",
                        City = "City",
                        PostCode = "PostCode"
                    };

                return Page();
            } else
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
