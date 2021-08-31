using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shop.Domain.Models;

namespace Shop.Database {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }
        public DbSet<StockOnHold> StocksOnHolds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<OrderStock>()
                .HasKey(x => new { x.StockId, x.OrderId });
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.LogTo(System.Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
