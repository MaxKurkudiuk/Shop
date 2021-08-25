using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System.Linq;

namespace Shop.Application.Cart {
    public class GetCart {
        private ISession _session;
        private ApplicationDbContext _context;

        public GetCart(ISession session, ApplicationDbContext context) {
            _session = session;
            _context = context;
        }

        public class Response {
            public string Name { get; set; }
            public string Value { get; set; }
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public Response Do() {
            //TODO: account from multiple items in the cart
            var stringObject = _session.GetString("cart");

            var cartProduct = JsonConvert.DeserializeObject<CartProduct>(stringObject);

            var response = _context.Stock
                .Include(x => x.Product)
                .Where(x => x.Id == cartProduct.StockId)
                .Select(x => new Response() {
                    Name = x.Product.Name,
                    Value = $"$ {x.Product.Value:N2}",
                    StockId = x.Id,
                    Qty = cartProduct.Qty
                })
                .FirstOrDefault();

            return response;
        }
    }
}
