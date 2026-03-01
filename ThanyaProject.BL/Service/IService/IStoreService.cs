using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service.IService
{
    public interface IStoreService
    {
        Task CreateProductAsync(ProductDto dto);
        Task UpdateProductAsync(int id, ProductDto dto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<string> CreateOrderAsync(int userId, CreatOrderDto dto);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task<Order?> GetOrderDetailsAsync(int orderId, int userId, bool isAdmin);
        Task CancelOrderAsync(int orderId, int userId, bool isAdmin);

    }
}
