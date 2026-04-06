using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Enum;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service.IService
{
    public interface IStoreService
    {
        Task<Product> CreateProductAsync(ProductDto dto);
        Task UpdateProductAsync(int id, ProductDto dto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductItemResponse>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<string> CreateOrderAsync(int userId, CreatOrderDto dto);
        Task<int> CreateOrderFromCartAsync(int userId);
        Task ConfirmOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<Order?> GetOrderDetailsAsync(int orderId, int userId, bool isAdmin);
        Task CancelOrderAsync(int orderId, int userId, bool isAdmin);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
        Task<IEnumerable<CartItemDto>> GetUserCartAsync(int userId);
        Task ClearCartAsync(int userId);

    }
}