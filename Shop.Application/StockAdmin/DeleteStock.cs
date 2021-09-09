using Shop.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin {
    public class DeleteStock {
        private ApplicationDbContext _context;

        public DeleteStock(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<bool> DoAsync(int id) {
            var stock = _context.Stock.FirstOrDefault(x => x.Id == id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
