using Shop.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;

namespace Shop.Database {
    public class StockManager : IStockManager {
        private ApplicationDbContext _context;

        public StockManager(ApplicationDbContext context) {
            _context = context;
        }

        public bool EnoughStock(int stockId, int qty) =>
            _context.Stock.FirstOrDefault(x => x.Id == stockId).Qty >= qty;

        public Stock GetStockWithProduct(int stockId) =>
            _context.Stock.Include(x => x.Product).FirstOrDefault(x => x.Id == stockId);

        public Task PutStockOnHold(int stockId, int qty, string sessionId) {
            // begin transaction

            // update Stock set qty = qty + {0} where Id = {1}
            _context.Stock.FirstOrDefault(x => x.Id == stockId).Qty -= qty;

            var stockOnHold = _context.StocksOnHold.Where(x => x.SessionId == sessionId).ToList();
            // select count(*) from StockOnHold where StockId = {0} and sessionId = {1}
            if (stockOnHold.Any(x => x.StockId == stockId)) {
                // update StockOnHold set qty = qty + {0} where StockId = {1} and sessionId = {2}
                stockOnHold.FirstOrDefault(x => x.StockId == stockId).Qty += qty;
            } else {
                // insert into StockOnHold (StockId, SessionId, Qty, ExpiryDate) values ({0}, {1}, {2}, {3})
                _context.StocksOnHold.Add(new StockOnHold() {
                    StockId = stockId,
                    SessionId = sessionId,
                    Qty = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }
            // update StockOnHold set ExpiryDate = {0} where SessionId = {1}
            foreach (var stock in stockOnHold)
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);

            // commit transaction
            return _context.SaveChangesAsync();  // save new StockOnHold and update Stock => stockOnHold.Qty
        }

        public Task RemoveStockFromHold(int stockId, int qty, string sessionId) {
            var stockOnHold = _context.StocksOnHold.FirstOrDefault(x => x.StockId == stockId && x.SessionId == sessionId);
            var stockToHold = _context.Stock.FirstOrDefault(x => x.Id == stockId);

            stockToHold.Qty += qty;
            stockOnHold.Qty -= qty;

            if (stockOnHold.Qty <= 0)
                _context.Remove(stockOnHold);

            return _context.SaveChangesAsync();  // save new StockOnHold and update Stock => stockOnHold.Qty
        }
    }
}
