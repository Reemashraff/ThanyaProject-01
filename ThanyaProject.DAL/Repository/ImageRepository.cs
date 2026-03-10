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
    public class ImageRepository : Repository<Image>, IImageReository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
