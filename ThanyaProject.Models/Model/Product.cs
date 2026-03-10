using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }
        public string Description { get; set; } = null!;
        public string Currency { get; set; } = "EGP";
        //public string ImgUrl { get; set; }
        public int ImageId { get; set; }
        [AllowNull]
        public Image Image { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}

