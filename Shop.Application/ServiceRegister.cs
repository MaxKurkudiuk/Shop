using Shop.Application.OrdersAdmin;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;
using Shop.Application.UsersAdmin;

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

            return @this;
        }
    }
}
