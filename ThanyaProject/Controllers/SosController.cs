using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThanyaProject.DAL.Data;
using ThanyaProject.Models.DTO;
using ThanyaProject.Models.Model;

[Route("api/sos")]
[ApiController]
public class SosController : ControllerBase
{
    private readonly AppDbContext _context;

    public SosController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost("alert")]
    public async Task<IActionResult> SendSOS(SOSRequestDTO request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var notification = new Notification
        {
            UserId = userId,
            Message = $"SOS Alert from device {request.DeviceId} " +
            $"Location: {request.Location.Lat},{request.Location.Long}",
            DateCreated = DateTime.UtcNow,
            IsRead = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            status = "success",
            message = "SOS alert sent successfully"
        });
    }
}