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
    public class MedicalRecordRepository:Repository<MedicalRecord>,IMedicalRecordRepository
    {
        private readonly AppDbContext _context;

        public MedicalRecordRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<MedicalRecord?> GetMedicalRecordAsync(int userId)
        {
            return await _context.MedicalRecords
                                 .Include(m => m.MedicalImages)
                                 .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}
