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
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllOrdersWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }
        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Order?> GetOrderWithDetailsAsync(int id)
        {
            return await _context.Orders
            // .AsNoTracking()
             .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Product)
                 .ThenInclude(p => p.Image)
             .FirstOrDefaultAsync(o => o.OrderId == id);
        }

    }
}
