using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.DTOs;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CLNotificationController : ControllerBase
    {
        private readonly BLNotificationHandler _notificationHandler;

        Response response = new Response();

        public CLNotificationController(BLNotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrWhiteSpace(userIdClaim) ? 0 : Convert.ToInt32(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            int userId = GetCurrentUserId();
            response = await _notificationHandler.GetUserNotificationsAsync(userId);
            return Ok(response);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            var count = await _notificationHandler.GetUnreadCountAsync(userId);
            return Ok(new UnreadNotificationCountDTO { Count = count });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetCurrentUserId();
            response = await _notificationHandler.MarkAsReadAsync(id, userId);
            return Ok(response);
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            response = await _notificationHandler.MarkAllAsReadAsync(userId);
            return Ok(response);
        }
    }
}
