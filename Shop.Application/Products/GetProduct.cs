using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products {
    public class GetProduct {
        private ApplicationDbContext _context;

        public GetProduct(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<ProductViewModel> Do(string name) {
            var stocksOnHold = _context.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0) {
                var stockToReturn = _context.Stock.AsEnumerable().Where(x => stocksOnHold.Any(y => y.StockId == x.Id)).ToList();

                foreach (var stock in stockToReturn)
                    stock.Qty += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;

                _context.StocksOnHold.RemoveRange(stocksOnHold);

                await _context.SaveChangesAsync();
            }

            return _context.Products
                .Include(x => x.Stock)
                .Where(x => x.Name == name)
                .Select(x => new ProductViewModel() {
                    Name = x.Name,
                    Description = x.Description,
                    Value = $"${x.Value:N2}",  // 1100.50 => 1,100.50 => $ 1,100.50

                    Stock = x.Stock.Select(y => new StockViewModel() {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    })
                })
                .FirstOrDefault();
        }

        public class ProductViewModel {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
