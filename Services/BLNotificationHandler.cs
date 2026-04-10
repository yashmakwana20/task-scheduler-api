using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.DTOs;
using TaskManagement.Models;
using TaskManagement.Services.Common;

namespace TaskManagement.Services
{
    public class BLNotificationHandler
    {
        private readonly AppDbContext _context;

        Response response = new Response();

        public BLNotificationHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response> CreateAsync(CreateNotificationDTO dto)
        {
            Notification notification = new Notification
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                RelatedTaskId = dto.RelatedTaskId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            response.DataModel = new NotificationDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                RelatedTaskId = notification.RelatedTaskId,
                CreatedAt = notification.CreatedAt
            };

            return response;
        }

        public async Task<Response> GetUserNotificationsAsync(int userId)
        {
            List<NotificationDTO> lstNotification = new List<NotificationDTO>();
            lstNotification = await _context.Notifications.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    Type = x.Type,
                    IsRead = x.IsRead,
                    RelatedTaskId = x.RelatedTaskId,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();

            if (lstNotification.Count > 0)
                response.Data = lstNotification.ToDataTable();
            else
                response.IsError = true;

            return response;

        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {

            return await _context.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead);
        }

        public async Task<Response> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

            if (notification == null)
            {
                response.IsError = true;
                response.Message = "Notification not found.";
                return response;
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            response.Message = "Notification marked as read.";
            return response;
        }

        public async Task<Response> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            foreach (var item in notifications)
            {
                item.IsRead = true;
                item.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            response.Count = notifications.Count;
            response.Message = "All notifications marked as read.";
            return response;
        }
    }
}
