using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.Models.DTO;

namespace ThanyaProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _service;

        public ContactUsController(IContactUsService service)
        {
            _service = service;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(ContactUsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.SendAsync(dto);

            return Ok(new { message = "Message sent successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Message not found" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}