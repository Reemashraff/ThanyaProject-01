using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Data;
using ThanyaProject.DAL.Repository;
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
        private readonly ICartItemRepository _cartRepo;
        private readonly IImageService _imageService;
        private readonly IImageReository _imageRepo;
        private readonly IStripeService _stripeService;
        private readonly AppDbContext _context;
        public StoreService(IProductRepository productRepo,
                            IOrderRepository orderRepo,
                            ICartItemRepository cartRepo,
                            IImageService imageService,
                            IImageReository imageReos,
                            IStripeService stripeService,
                            AppDbContext context)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _imageService = imageService;
            _imageRepo = imageReos;
            _stripeService = stripeService;
            _context = context;

        }
        public async Task<Product> CreateProductAsync(ProductDto dto)
        {
            Image image = null;

            if (dto.FormFile != null)
            {
                var imageUrl = await _imageService.UploadImageAsync(dto.FormFile);

                image = new Image
                {
                    Url = imageUrl.SecureUrl.ToString(),
                    PublicId = Guid.NewGuid().ToString()
                };

                await _imageRepo.AddAsync(image);
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Currency = dto.Currency,
                Stock = dto.Stock,
                Image = image
            };

            await _productRepo.AddAsync(product);

            return product;
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
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();

            return products.Select(p => new ProductDto
            {
               // Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Currency = "EGP",
                ImageUrl = p.Image != null ? p.Image.Url : null,
                Stock = p.Stock 
            });
        }
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
              //  Id = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Currency = "EGP",
                ImageUrl = product.Image != null ? product.Image.Url : null,
                Stock = product.Stock 
            };
        }

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
        public async Task<string> CreateOrderFromCartAsync(int userId)
        {
            var cartItems = await _cartRepo.GetUserCartAsync(userId);

            if (cartItems == null || !cartItems.Any())
                throw new Exception("Cart is empty");

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                PaymentMethod = PaymentMethod.Stripe,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cartItems)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"Insufficient stock for {item.Product.Name}");

                item.Product.Stock -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });

                total += item.Product.Price * item.Quantity;
            }

            order.TotalPrice = total;

            await _orderRepo.AddAsync(order);

            return order.OrderId.ToString();
        }
        public async Task ConfirmOrderAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = OrderStatus.Paid;

            await _orderRepo.UpdateAsync(order);

            await _cartRepo.ClearCartAsync(order.UserId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
            => await _orderRepo.GetUserOrdersAsync(userId);

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _orderRepo.GetAllOrdersWithDetailsAsync();

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
            var order = await _orderRepo.GetOrderWithDetailsAsync(orderId);

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
        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var existingItem = await _cartRepo.GetCartItemAsync(userId, productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _cartRepo.UpdateAsync(existingItem);
            }
            else
            {
                await _cartRepo.AddAsync(new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                });
            }
        }
        public async Task<IEnumerable<CartItemDto>> GetUserCartAsync(int userId)
        {
            var cartItems = await _cartRepo.GetUserCartAsync(userId);

            return cartItems.Select(c => new CartItemDto
            {
                ProductId = c.ProductId,
                Title = c.Product.Name,
                Price = c.Product.Price,
                Quantity = c.Quantity,
                Total = c.Product.Price * c.Quantity
            });
        }
        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cartItem = await _cartRepo.FirstOrDefaultAsync(
                c => c.UserId == userId && c.ProductId == productId);

            if (cartItem == null)
                throw new Exception("Item not found in cart");

            await _cartRepo.DeleteAsync(cartItem);
        }
        public async Task ClearCartAsync(int userId)
        {
            await _cartRepo.ClearCartAsync(userId);
        }

    }
}
