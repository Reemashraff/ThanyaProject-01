using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.DAL.Data;
using ThanyaProject.DAL.Repository.IRepository;
using ThanyaProject.Models.Model;

namespace ThanyaProject.DAL.Repository
{
    public class CartRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetUserCartAsync(int userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .ThenInclude(p => p.Image)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .ThenInclude(p => p.Image)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
