using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ThanyaProject.BL.Service.IService;
using ThanyaProject.DAL.Data;
using ThanyaProject.Models.Model;

namespace ThanyaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        
        private readonly UserManager<User> _userManager;
        private readonly IDashBoardService _dashboardService;
        private readonly AppDbContext _context;

        public DashboardController(
            IDashBoardService dashboardService,
            AppDbContext context,
            UserManager<User> userManager)
        {
            _dashboardService = dashboardService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserDashboard()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _dashboardService.GetUserDashboardAsync(userId);

            return Ok(result);
        }
        [Authorize(Roles = "Paramedic")]
        [HttpGet("emergency-record")]
        public async Task<IActionResult> GetParamedicDashboard(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.EmergencyToken == token);

                if (user == null)
                    return NotFound("Invalid token");

                var medical = await _context.MedicalRecords
                    .Include(m => m.MedicalImages)
                    .FirstOrDefaultAsync(m => m.UserId == user.Id);

                return Ok(new
                {
                    fullName = $"{user.FirstName} {user.LastName}",
                    age = user.Age,
                    gender = user.Gender,

                    bloodType = medical?.BloodType,
                    allergies = medical?.Allergies,
                    chronicDiseases = medical?.ChronicDiseases,
                    currentMedication = medical?.CurrentMedication,
                    weight = medical?.Weight,
                    summery = medical?.Summery,

                    images = medical?.MedicalImages?
                        .Select(x => new
                        {
                            x.Id,
                            x.Url
                        })
                        .ToList()
                });
            }

            var result = await _dashboardService.GetAdminDashboardAsync();
            return Ok(result);
        }
    }
}