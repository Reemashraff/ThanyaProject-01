using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThanyaProject.BL.Service;
using ThanyaProject.Models;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

namespace ThanyaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyDevices()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var devices = await _service.GetDevicesByUserIdAsync(userId);

            return Ok(new
            {
                status = "success",
                count = devices.Count,
                data = devices
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("sub")!.Value);

            var result = await _service.CreateAsync(dto, userId);

            return Ok(new
            {
                status = "success",
                message = "Device created successfully",
                data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Device device)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.UpdateAsync(id, device, userId);

            if (!result)
                return NotFound(new { status = "error", message = "Device not found" });

            return Ok(new
            {
                status = "success",
                message = "Device updated successfully"
            });
        }
    }
}