using Microsoft.AspNetCore.Identity;
using Shop.Database;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.Application.UsersAdmin {
    public class CreateUser {
        private UserManager<IdentityUser> _userManager;

        public CreateUser(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }

        public class Request {
            public string UserName { get; set; }
        }

        public async Task<bool> DoAsync(Request request) {
            var managerUser = new IdentityUser(request.UserName);
            await _userManager.CreateAsync(managerUser, "password");

            var managerClaim = new Claim("Role", "Manager");
            await _userManager.AddClaimAsync(managerUser, managerClaim);

            return true;
        }
    }
}
