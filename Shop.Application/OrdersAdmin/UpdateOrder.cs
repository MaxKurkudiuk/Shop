using Shop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin {
    public class UpdateOrder {
        private ApplicationDbContext _context;

        public UpdateOrder(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<bool> DoAsync(int id) {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);

            order.Status++;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
