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
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public readonly AppDbContext _context;
        public DeviceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Device>> GetDevicesByUserIdAsync(int userId)
        {
            return await _context.Devices
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> GetDeviceCountByUserIdAsync(int userId)
        {
            return await _context.Devices
                .CountAsync(d => d.UserId == userId);
        }
    }
}
