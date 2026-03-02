using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Repository.IRepository;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Enum;
using ThanyaProject.Models.Model;

namespace ThanyaProject.BL.Service
{
    public class StoreService : IStoreService
    {
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;

        public StoreService(IProductRepository productRepo,
                            IOrderRepository orderRepo)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
        }
        public async Task CreateProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description
            };

            await _productRepo.AddAsync(product);
        }
        public async Task UpdateProductAsync(int id, ProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            product.Price = dto.Price;

            await _productRepo.UpdateAsync(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            await _productRepo.DeleteAsync(product);
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
            => await _productRepo.GetAllAsync();

        public async Task<Product?> GetProductByIdAsync(int id)
            => await _productRepo.GetByIdAsync(id);

        public async Task<string> CreateOrderAsync(int userId, CreatOrderDto dto)
        {
            var order = new Order
            {
                UserId = userId,
                DeliveryAddress = dto.DeliveryAddress,
                PaymentMethod = dto.PaymentMethod,
                Status = OrderStatus.Pending
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                if (product.Stock < item.Quantity)
                    throw new Exception("Insufficient stock");

                product.Stock -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                total += product.Price * item.Quantity;
            }

            order.TotalPrice = total;

            await _orderRepo.AddAsync(order);

            return "Order Created Successfully";
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
            => await _orderRepo.GetUserOrdersAsync(userId);

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _orderRepo.GetAllAsync();

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = status;

            await _orderRepo.UpdateAsync(order);
        }
        public async Task<Order?> GetOrderDetailsAsync(int orderId, int userId, bool isAdmin)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (!isAdmin && order.UserId != userId)
                throw new Exception("Unauthorized access");

            return order;
        }
        public async Task CancelOrderAsync(int orderId, int userId, bool isAdmin)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (!isAdmin && order.UserId != userId)
                throw new Exception("Unauthorized");

            if (order.Status == OrderStatus.Paid ||order.Status == OrderStatus.Delivered)
                throw new Exception("Cannot cancel this order");

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
            }

           order.Status = OrderStatus.Cancelled;

            await _orderRepo.UpdateAsync(order);
        }
    }
}
