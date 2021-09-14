﻿using Shop.Domain.Models;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Infrastructure {
    public interface ISessionManager {
        string GetId();
        void AddProduct(CartProduct cartProduct);
        void RemoveProduct(int stockId, int qty, bool isAll);
        IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector);

        void AddCustomerInformation(CustomerInformation customer);
        CustomerInformation GetCustomerInformation();
    }
}
