using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Shop.UI.Controllers {
    [Route("[controller]")]   // if use endpoints.MapDefaultControllerRoute();   instead endpoints.MapControllers(); you can remove it attribute
    public class AccountController : Controller {
        private SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager) {
            _signInManager = signInManager;
        }

        [HttpGet("Logout")]   // if use endpoints.MapDefaultControllerRoute();   instead endpoints.MapControllers(); you can remove it attribute
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();

            return RedirectToPage("/Index");
        }
    }
}
