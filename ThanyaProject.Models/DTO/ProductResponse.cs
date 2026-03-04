using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class ProductResponse
    {
        public string Status { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
