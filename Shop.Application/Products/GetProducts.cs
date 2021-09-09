using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Products {
    public class GetProducts {
        private ApplicationDbContext _context;

        public GetProducts(ApplicationDbContext context) {
            _context = context;
        }

        public IEnumerable<ProductViewModel> Do() => _context.Products
            .Include(x => x.Stock)
            .Select(x => new ProductViewModel {
                Name = x.Name,
                Description = x.Description,
                Value = $"${x.Value:N2}",  // 1100.50 => 1,100.50 => $ 1,100.50

                StockCount = x.Stock.Sum(y => y.Qty)
            }).ToList();

        public class ProductViewModel {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int StockCount { get; set; }
        }
    }
}
