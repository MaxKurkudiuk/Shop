using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Products {
    public class GetProduct {
        private ApplicationDbContext _context;

        public GetProduct(ApplicationDbContext context) {
            _context = context;
        }

        public ProductViewModel Do(string name) =>
            _context.Products
            .Include(x => x.Stock)
            .Where(x => x.Name == name)
            .Select(x => new ProductViewModel() {
                Name = x.Name,
                Description = x.Description,
                Value = $"$ {x.Value:N2}",  // 1100.50 => 1,100.50 => $ 1,100.50

                Stock = x.Stock.Select(y => new StockViewModel() {
                    Id = y.Id,
                    Description = y.Description,
                    InStock = y.Qty > 0
                })
            })
            .FirstOrDefault();

        public class ProductViewModel {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel {
            public int Id { get; set; }
            public string Description { get; set; }
            public bool InStock { get; set; }
        }
    }
}
