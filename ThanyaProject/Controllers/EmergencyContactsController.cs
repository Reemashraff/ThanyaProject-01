using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.Models.DTO;

namespace ThanyaProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactsController : ControllerBase
    {
        private readonly IEmergencyContactService _service;

        public EmergencyContactsController(IEmergencyContactService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add(EmergencyContactDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.AddContact(userId, dto);

            return Ok(result);
        }


        [HttpPut("update/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(int id, EmergencyContactDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.UpdateContact(userId, id, dto);

            if (result == null)
                return NotFound("Contact not found");

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.DeleteContact(userId, id);

            if (!result)
                return NotFound("Contact not found");

            return Ok("Deleted successfully");
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetAllContacts(userId);

            return Ok(result);
        }
    }
}
