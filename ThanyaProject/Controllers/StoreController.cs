using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Enum;

namespace ThanyaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        #region Products

        [Authorize(Roles ="Admin")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            await _storeService.CreateProductAsync(dto);
            return Ok(new { message = "Product created successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
        {
            await _storeService.UpdateProductAsync(id, dto);
            return Ok(new { message = "Product updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _storeService.DeleteProductAsync(id);
            return Ok(new { message = "Product deleted successfully" });
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _storeService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _storeService.GetProductByIdAsync(id);
            if (product == null)
               return NotFound();

            return Ok(product);
        }

        #endregion
        #region Orders
        [Authorize]
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder(CreatOrderDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _storeService.CreateOrderAsync(userId, dto);

            return Ok(new { message = result });
        }

        [Authorize]
        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = await _storeService.GetUserOrdersAsync(userId);

            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _storeService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/orders/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            await _storeService.UpdateOrderStatusAsync(id, status);

            return Ok(new { message = "Order status updated" });
        } 

        [Authorize]
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            var order = await _storeService.GetOrderDetailsAsync(id, userId, isAdmin);

            return Ok(order);
        }
        #endregion 
    }
}
