using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CardId { get; set; }
        public Cards Cards { get; set; }
        public int PackageId { get; set; }
        public Pachage Package { get; set; }
    }
}
