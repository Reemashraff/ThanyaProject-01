using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ThanyaProject.BL.Service;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Enum;
using ThanyaProject.Models.Model;
using Microsoft.Extensions.Options;
using ThanyaProject.Setting;

namespace ThanyaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IStripeService _stripeService;
        private readonly StripeSetting _stripeOptions;
        private readonly UserManager<User> _userManager;

        public StoreController(
            IStoreService storeService,
            IStripeService stripeService,
            IOptions<StripeSetting> stripeOptions,
            UserManager<User> userManager)
        {
           _storeService = storeService;
           _stripeService = stripeService;
           _stripeOptions = stripeOptions.Value;  
           _userManager = userManager;
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _storeService.GetAllProductsAsync();

            var response = new ProductResponse
            {
                Status = "success",
                Products = products
            };

            return Ok(response);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _storeService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new
                {
                    status = "fail",
                    message = "Product not found"
                });

            return Ok(new
            {
                status = "success",
                product = product
            });
        }

        #endregion
        #region Orders
        //[Authorize]
        //[HttpPost("orders")]
        //public async Task<IActionResult> CreateOrder(CreatOrderDto dto)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        //    var result = await _storeService.CreateOrderAsync(userId, dto);

        //    return Ok(new { message = result });
        //}

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

        [Authorize]
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            var order = await _storeService.GetOrderDetailsAsync(id, userId, isAdmin);

            return Ok(order);
        }
        [HttpPost]
        [HttpPost("Checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orderId = await _storeService.CreateOrderFromCartAsync(userId);

            var order = await _storeService.GetOrderDetailsAsync(
                int.Parse(orderId),
                userId,
                User.IsInRole("Admin")
            );

            var successUrl = Url.Action("ConfirmOrder", "Store",
                new { orderId = orderId },
                Request.Scheme);

            var sessionUrl = await _stripeService.CreateCheckoutSession(order.OrderItems, successUrl);

            return Ok(new
            {
                status = "success",
                sessionUrl = sessionUrl
            });
        }

        [HttpGet("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            await _storeService.ConfirmOrderAsync(orderId);

            return Ok(new
            {
                status = "success",
                message = "Payment confirmed and order completed"
            });
        }
        #endregion
        #region CartItem
        [Authorize]
        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _storeService.RemoveFromCartAsync(userId, productId);

            return Ok(new
            {
                status = "success",
                message = "Product removed from cart"
            });
        }

        [Authorize]
        [HttpDelete("cart")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _storeService.ClearCartAsync(userId);

            return Ok(new
            {
                status = "success",
                message = "Cart cleared successfully"
            });
        }
        #endregion 
    }
}
