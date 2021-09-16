using Shop.Domain.Infrastructure;
using Shop.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Cart {
    public class RemoveFromCart {
        private ISessionManager _sessionManager;
        private ApplicationDbContext _context;

        public RemoveFromCart(ISessionManager sessionManager, ApplicationDbContext context) {
            _sessionManager = sessionManager;
            _context = context;
        }

        public class Request {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public Task RemoveFromHold(int stockId, int qty, string sessionId) {
            var stockOnHold = _context.StocksOnHold.FirstOrDefault(x => x.StockId == stockId && x.SessionId == sessionId);
            var stockToHold = _context.Stock.FirstOrDefault(x => x.Id == stockId);

            if (qty == 0)     // All
                stockToHold.Qty += stockOnHold.Qty;
            else
                stockToHold.Qty += qty;

            stockOnHold.Qty -= qty;

            if (stockOnHold.Qty <= 0)
                _context.Remove(stockOnHold);

            return _context.SaveChangesAsync();  // save new StockOnHold and update Stock => stockOnHold.Qty
        }

        public async Task<bool> DoAsync(Request request) {
            await RemoveFromHold(request.StockId, request.Qty, _sessionManager.GetId());

            _sessionManager.RemoveProduct(request.StockId, request.Qty);

            return true;
        }
    }
}
