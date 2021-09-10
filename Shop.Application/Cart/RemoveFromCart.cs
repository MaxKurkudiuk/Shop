using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Cart {
    public class RemoveFromCart {
        private ISession _session;
        private ApplicationDbContext _context;

        public RemoveFromCart(ISession session, ApplicationDbContext context) {
            _session = session;
            _context = context;
        }

        public class Request {
            public int StockId { get; set; }
            public int Qty { get; set; }
            public bool All { get; set; }
        }

        public async Task<bool> DoAsync(Request request) {
            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return true;

            cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockId == request.StockId))
                return true;
            else if (cartList.Find(x => x.StockId == request.StockId).Qty <= 0 || request.All)
                cartList.FirstOrDefault(x => x.StockId == request.StockId).Qty = 0;
            else
                cartList.FirstOrDefault(x => x.StockId == request.StockId).Qty -= request.Qty;

            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);

            //if (stockOnHold.Qty < request.Qty)
            //    return false; // return not enough stocks on hold

            var stockOnHold = _context.StocksOnHold.FirstOrDefault(x => x.StockId == request.StockId && x.SessionId == _session.Id);
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
