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

    [Authorize(Roles = "User")]
    [HttpPost("alert")]
    public async Task<IActionResult> SendSOS(SOSRequestDTO request)
    {
        if (request == null || request.Location == null)
            return BadRequest("Location is required");

        var device = _context.Devices
            .FirstOrDefault(d => d.DeviceId == request.DeviceId);

        if (device == null)
            return NotFound("Device not found");

        var notification = new Notification
        {
            UserId = device.UserId,
            DeviceId = device.Id,
            Message = "SOS Alert",
            DateCreated = DateTime.UtcNow,
            Type = "emergency",
            Latitude = request.Location.Lat,
            Longitude = request.Location.Long,
            IsRead = false,
            Resolved = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            status = "success",
            message = "SOS alert sent successfully"
        });
    }

    [Authorize(Roles = "Admin,Doctor,Paramedic")]
    [HttpGet("alerts")]
    public IActionResult GetSOSAlerts()
    {
        var alerts = _context.Notifications
            .Where(n => n.Type == "emergency")
            .OrderByDescending(n => n.DateCreated)
            .ToList();

        return Ok(alerts);
    }

    [Authorize]
    [HttpGet("notifications")]
    public IActionResult GetNotifications()
    {
        var notifications = _context.Notifications
            .OrderByDescending(n => n.DateCreated)
            .ToList();

        return Ok(notifications);
    }

    [Authorize]
    [HttpPut("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);

        if (notification == null)
            return NotFound();

        notification.IsRead = true;

        await _context.SaveChangesAsync();

        return Ok("Notification marked as read");
    }


    [Authorize(Roles = "User")]
    [HttpGet("history")]
    public IActionResult GetSOSHistory()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var history = _context.Notifications
    .Where(n => n.UserId == userId && n.Type == "emergency")
    .OrderByDescending(n => n.DateCreated)
    .Select(n => new SosHistoryDto
    {
        Id = "alert_" + n.NotificationId,
        DeviceName = n.Device != null ? n.Device.Name : "Unknown Device",
        Time = n.DateCreated.ToString("yyyy-MM-dd hh:mm tt"),
        Resolved = n.Resolved,
        Details = n.Message,
        Latitude = n.Latitude,
        Longitude = n.Longitude
    })
    .ToList();

        return Ok(new
        {
            status = "success",
            history = history
        });
    }

}