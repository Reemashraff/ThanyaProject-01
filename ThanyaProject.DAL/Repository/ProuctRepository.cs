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
    public class ProuctRepository : Repository<Product> ,IProductRepository
    {
        private readonly AppDbContext _context;

        public ProuctRepository(AppDbContext context) : base(context)
        {
            context = _context;
        }

    }
}
