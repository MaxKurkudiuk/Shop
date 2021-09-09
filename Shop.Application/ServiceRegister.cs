﻿using Shop.Application.OrdersAdmin;
using Shop.Application.UsersAdmin;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceRegister {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this) {
            @this.AddTransient<CreateUser>();   // (Transient) Injection a new instance is provided to every controller and every service.

            @this.AddTransient<GetOrder>();
            @this.AddTransient<GetOrders>();
            @this.AddTransient<UpdateOrder>();

            return @this;
        }
    }
}
