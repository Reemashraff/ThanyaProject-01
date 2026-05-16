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
        int GetIdFromToken()
        {
            var idString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idString is null)
                return -1;
            if (!int.TryParse(idString.Value, out int id))
                return -1;
            return id;
        }
        #region Devices
        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("GETDevices")]
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
        [HttpPost("CreateDevice")]
        public async Task<IActionResult> Create([FromBody] DeviceCreateDto dto)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
           var Id = GetIdFromToken();

            var result = await _service.CreateAsync(dto, Id);

            return Ok(new
            {
                status = "success",
                message = "Device created successfully",
                data = result
            });
        }

        [Authorize]
        [HttpPut("band-update/{id}")]
        public async Task<IActionResult> UpdateFromBand(int id, [FromBody] DeviceBandUpdateDto dto)
        {

            var userId = GetIdFromToken();
            if (userId == -1)
                return Unauthorized(new { status = "error", message = "Unauthorized User" });

            var result = await _service.UpdateFromBandAsync(id, dto, userId);

            if (!result)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "Device not found or does not belong to this user!"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Band data updated successfully"
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _service.DeleteAsync(id, userId);

            if (!result)
                return NotFound(new { status = "error", message = "Device not found" });

            return Ok(new
            {
                status = "success",
                message = "Device deleted successfully"
            });
        }
        
        #endregion

    }
}