using Shop.Application.OrdersAdmin;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;
using Shop.Application.UsersAdmin;
using Cart = Shop.Application.Cart;
using Orders = Shop.Application.Orders;
using Products = Shop.Application.Products;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceRegister {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this) {
            @this.AddTransient<CreateUser>();   // (Transient) Injection a new instance is provided to every controller and every service.

            @this.AddTransient<GetOrder>();
            @this.AddTransient<GetOrders>();
            @this.AddTransient<UpdateOrder>();

            @this.AddTransient<CreateProduct>();
            @this.AddTransient<DeleteProduct>();
            @this.AddTransient<GetProduct>();
            @this.AddTransient<GetProducts>();
            @this.AddTransient<UpdateProduct>();

            @this.AddTransient<CreateStock>();
            @this.AddTransient<DeleteStock>();
            @this.AddTransient<GetStock>();
            @this.AddTransient<UpdateStock>();

            @this.AddTransient<Cart.AddCustomerInformation>();
            @this.AddTransient<Cart.GetCustomerInformation>();
            @this.AddTransient<Cart.AddToCart>();
            @this.AddTransient<Cart.GetCart>();
            @this.AddTransient<Cart.RemoveFromCart>();
            @this.AddTransient<Cart.GetOrder>();

            @this.AddTransient<Orders.CreateOrder>();
            @this.AddTransient<Orders.GetOrder>();

            @this.AddTransient<Products.GetProduct>();
            @this.AddTransient<Products.GetProducts>();

            return @this;
        }
    }
}
