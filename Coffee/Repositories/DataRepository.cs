using Coffee.Data;
using Coffee.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Repositories
{
    public class DataRepository
    {
        private ApplicationDbContext _context;

        public DataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var result = await _context.Users.OrderByDescending(x => x.Created).ToListAsync();

            return result;
        }

        public async Task BlockUsersAsync(string userId)
        {
            var item = await _context.Users.FirstAsync(x => x.Id == userId);

            item.LockoutEnd = DateTime.UtcNow.AddYears(1000);
            await _context.SaveChangesAsync();
        }

        public async Task UnBlockUsersAsync(string userId)
        {
            var item = await _context.Users.FirstAsync(x => x.Id == userId);

            item.LockoutEnd = null;
            await _context.SaveChangesAsync();
        }
    }
}
