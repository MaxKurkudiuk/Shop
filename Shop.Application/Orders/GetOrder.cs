using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Orders {
    public class GetOrder {
        private ApplicationDbContext _context;

        public GetOrder(ApplicationDbContext context) {
            _context = context;
        }

        public class Response {
            public string OrderRef { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
            public IEnumerable<Product> Products { get; set; }
            public string TotalValue { get; set; }
        }

        public class Product {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int Qty { get; set; }
            public string StockDescription { get; set; }
        }
        
        public Response Do(string reference) {
            var response = _context.Orders
                .Where(x => x.OrderRef == reference)
                .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .Select(x => new Response() {
                    OrderRef = x.OrderRef,

                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    PostCode = x.PostCode,

                    Products = x.OrderStocks.Select(y => new Product() {
                        Name = y.Stock.Product.Name,
                        Description = y.Stock.Product.Description,
                        Value = $"$ {y.Stock.Product.Value:N2}",  // 1100.50 => 1,100.50 => $ 1,100.50
                        StockDescription = y.Stock.Description,
                        Qty = y.Qty,
                    }),

                    TotalValue = $"$ {x.OrderStocks.Sum(y => y.Stock.Product.Value):N2}",  // 1100.50 => 1,100.50 => $ 1,100.50
                })
                .FirstOrDefault();

            return response;
        }
    }
}
