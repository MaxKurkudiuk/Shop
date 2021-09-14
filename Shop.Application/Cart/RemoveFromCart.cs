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
            public bool All { get; set; }
        }

        public async Task<bool> DoAsync(Request request) {
            _sessionManager.RemoveProduct(request.StockId, request.Qty, request.All);

            var stockOnHold = _context.StocksOnHold.FirstOrDefault(x => x.StockId == request.StockId && x.SessionId == _sessionManager.GetId());
            var stockToHold = _context.Stock.FirstOrDefault(x => x.Id == request.StockId);

            if (stockOnHold == null)
                return true; // return can't find any stock on hold by currect sessionId

            if (request.All) {
                stockToHold.Qty += stockOnHold.Qty;
                stockOnHold.Qty = 0;
            } else {
                stockToHold.Qty += request.Qty;
                stockOnHold.Qty -= request.Qty;
            }

            if (stockOnHold.Qty <= 0)
                _context.StocksOnHold.Remove(stockOnHold);
            else
                stockOnHold.ExpiryDate = DateTime.Now.AddMinutes(20);

            await _context.SaveChangesAsync();  // save new StockOnHold and update Stock => stockOnHold.Qty

            return true;
        }
    }
}
