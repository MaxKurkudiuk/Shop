using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.StockAdmin {
    public class GetStocks {
        private ApplicationDbContext _context;

        public GetStocks(ApplicationDbContext context) {
            _context = context;
        }

        public IEnumerable<StockViewModel> Do(int productId) 
            => _context.Stock
                .Where(x => x.ProductId == productId)
                .Select(x => new StockViewModel() {
                    Id = x.Id,
                    Description = x.Description,
                    Qty = x.Qty
                })
                .ToList();

        public class StockViewModel {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
