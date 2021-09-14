using Shop.Application.Infrastructure;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Cart {
    public class AddToCart {
        private ISessionManager _sessionManager;
        private ApplicationDbContext _context;

        public AddToCart(ISessionManager sessionManager, ApplicationDbContext context) {
            _sessionManager = sessionManager;
            _context = context;
        }

        public class Request {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> DoAsync(Request request) {
            var stockOnHold = _context.StocksOnHold.Where(x => x.SessionId == _sessionManager.GetId()).ToList();
            var stockToHold = _context.Stock.Where(x => x.Id == request.StockId).FirstOrDefault();

            if (stockToHold.Qty < request.Qty)
                return false; // return not enough stock

            if (stockOnHold.Any(x => x.StockId == request.StockId)) {
                stockOnHold.FirstOrDefault(x => x.StockId == request.StockId).Qty += request.Qty;
            } else {
                _context.StocksOnHold.Add(new StockOnHold() {
                    StockId = stockToHold.Id,
                    SessionId = _sessionManager.GetId(),
                    Qty = request.Qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            stockToHold.Qty -= request.Qty;

            foreach (var stock in stockOnHold) {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            await _context.SaveChangesAsync();  // save new StockOnHold and update Stock => stockOnHold.Qty

            _sessionManager.AddProduct(request.StockId, request.Qty);

            return true;
        }
    }
}
