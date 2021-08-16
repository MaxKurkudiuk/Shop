using Shop.Database;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin {
    public class UpdateProduct {
        private ApplicationDbContext _context;

        public UpdateProduct(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Response> Do(Request request) {
            await _context.SaveChangesAsync();
            return new Response();
        }

        public class Request {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public class Response {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
