using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Application.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shop.UI.Infrastructure {
    public class SessionManager : ISessionManager {
        private ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor) {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddCustomerInformation(CustomerInformation customer) {
            var stringObject = JsonConvert.SerializeObject(customer);

            _session.SetString("customer-info", stringObject);
        }

        public void AddProduct(int stockId, int qty) {
            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (cartList.Any(x => x.StockId == stockId))
                cartList.Find(x => x.StockId == stockId).Qty += qty;
            else
                cartList.Add(new CartProduct() {
                    StockId = stockId,
                    Qty = qty
                });

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }

        public List<CartProduct> GetCart() {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return null;    // ?

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            cartList = cartList.Where(x => x.Qty > 0).ToList();

            if (cartList.Count == 0)
                return null;    // ? new List<CartProduct>();

            return cartList;
        }

        public CustomerInformation GetCustomerInformation() {
            var stringObject = _session.GetString("customer-info");

            if (string.IsNullOrEmpty(stringObject))
                return null;

            return JsonConvert.DeserializeObject<CustomerInformation>(stringObject);
        }

        public string GetId() => _session.Id;

        public void RemoveProduct(int stockId, int qty, bool isAll) {
            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return;

            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockId == stockId))
                return;
            else if (cartList.Find(x => x.StockId == stockId).Qty <= 0 || isAll)
                cartList.FirstOrDefault(x => x.StockId == stockId).Qty = 0;
            else
                cartList.FirstOrDefault(x => x.StockId == stockId).Qty -= qty;

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);
        }
    }
}
