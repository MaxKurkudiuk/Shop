using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Cart {
    public class GetOrder {
        private ISessionManager _sessionManager;
        private ApplicationDbContext _context;

        public GetOrder(ISessionManager sessionManager, ApplicationDbContext context) {
            _sessionManager = sessionManager;
            _context = context;
        }

        public class Response {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }

            public int GetTotalCharge() => Products.Sum(x => x.Value * x.Qty);
        }

        public class Product {
            public int ProductId { get; set; }
            public int StockId { get; set; }
            public int Qty { get; set; }
            public int Value { get; set; }
        }

        public class CustomerInformation {
            public string FirstName { get; internal set; }
            public string LastName { get; internal set; }
            public string Email { get; internal set; }
            public string PhoneNumber { get; internal set; }

            public string Address1 { get; internal set; }
            public string Address2 { get; internal set; }
            public string City { get; internal set; }
            public string PostCode { get; internal set; }
        }

        public decimal GetTotalValue() {
            var cartList = _sessionManager.GetCart();

            if (cartList == null)
                return 0;

            return _context.Stock
                .Include(x => x.Product)
                .AsEnumerable()
                .Where(x => cartList.Any(y => y.StockId == x.Id))
                .Sum(x => x.Product.Value * cartList.FirstOrDefault(y => y.StockId == x.Id).Qty);
        }

        public Response Do() {
            var cart = _sessionManager.GetCart();

            if (cart == null)
                return null;

            var listOfProducts = _context.Stock
                .Include(x => x.Product)
                .AsEnumerable()
                .Where(x => cart.Any(y => y.StockId == x.Id))
                .Select(x => new Product() {
                    ProductId = x.ProductId,
                    StockId = x.Id,
                    Value = (int) (x.Product.Value * 100),    // Payment value style
                    Qty = cart.FirstOrDefault(y => y.StockId == x.Id).Qty
                }).ToList();

            var customerInformation = _sessionManager.GetCustomerInformation();

            return new Response {
                Products = listOfProducts,
                CustomerInformation = new CustomerInformation {
                    FirstName = customerInformation.FirstName,
                    LastName = customerInformation.LastName,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address1 = customerInformation.Address1,
                    Address2 = customerInformation.Address2,
                    City = customerInformation.City,
                    PostCode = customerInformation.PostCode
                }
            };
        }
    }
}
