using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service.IService
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSession(IEnumerable<OrderItem> orderItems, string successUrl);
    }
}
