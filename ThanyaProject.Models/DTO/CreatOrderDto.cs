using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class CreatOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
        public string DeliveryAddress { get; set; } = null!;
        public string PaymentMethod { get; set; } = "Stripe";
    }

}
