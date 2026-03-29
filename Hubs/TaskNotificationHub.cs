using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TaskManagement.Hubs
{
    public class TaskNotificationHub : Hub
    {
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var role = user.FindFirst(ClaimTypes.Role)?.Value;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrWhiteSpace(role) && role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, "all-authenticated-users");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
