using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Threading.Tasks;

namespace Shop.UI.Controllers {
    [Route("[controller]/[action]")]
    public class CartController : Controller {
        [HttpPost("{stockId}")]
        public async Task<IActionResult> AddOne(int stockId, [FromServices] AddToCart addToCart) {
            var request = new AddToCart.Request() {
                StockId = stockId,
                Qty = 1
            };
            var success = await addToCart.DoAsync(request);
            if (success) {
                return Ok("Item added to cart");
            }
            return BadRequest("Failed to add to cart");
        }

        [HttpPost("{stockId}/{qty}")]
        public async Task<IActionResult> Remove(int stockId, int qty, [FromServices] RemoveFromCart removeFromCart) {
            var request = new RemoveFromCart.Request() {
                StockId = stockId,
                Qty = qty,
            };
            var success = await removeFromCart.DoAsync(request);
            if (success) {
                return Ok("Item removed from cart");
            }
            return BadRequest("Failed to remove item from cart");
        }

        //[HttpPost("{stockId}")]
        //public async Task<IActionResult> RemoveAll(int stockId, [FromServices] RemoveFromCart removeAllFromCart) {
        //    var request = new RemoveFromCart.Request() {
        //        StockId = stockId,
        //        Qty = 0
        //    };
        //    var success = await removeAllFromCart.DoAsync(request);
        //    if (success) {
        //        return Ok("Item removed all items from cart");
        //    }
        //    return BadRequest("Failed to remove all items from cart");
        //}
    }
}
