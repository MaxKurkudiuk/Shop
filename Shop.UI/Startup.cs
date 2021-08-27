using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using Stripe;

namespace Shop.UI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            _config = configuration;
        }

        private IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                _config.GetConnectionString("DefaultConnection")));

            //services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSession(options => {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromDays(365);
            });

            StripeConfiguration.ApiKey = _config.GetSection("Stripe")["SecretKey"];

            services.Configure<StripeSettings>(_config.GetSection("Stripe"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
