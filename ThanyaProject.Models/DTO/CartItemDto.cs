using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Model;

namespace ThanyaProject.Models.DTO
{
    public class CartItemDto
    {
        public int Quantity { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
