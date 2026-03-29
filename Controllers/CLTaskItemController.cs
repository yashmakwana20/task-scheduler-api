using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TaskManagement.Hubs;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CLTaskItemController : ControllerBase
    {
        public BLTaskItemHandler _objHandler;
        private readonly int _userId;
        private readonly IHubContext<TaskNotificationHub> _hubContext;
        Response response = new Response();

        public CLTaskItemController(BLTaskItemHandler objHandler, IHttpContextAccessor contextAccessor, IHubContext<TaskNotificationHub> hubContext)
        {
            _objHandler = objHandler;
            _hubContext = hubContext;

            var identity = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userIdClaim))
            {
                _userId = Convert.ToInt32(userIdClaim);
            }

        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("GetTaskData")]
        public IActionResult GetTaskData(int userId = 0)
        {
            return Ok(_objHandler.GetTaskData(userId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveTaskItem(TaskItems objTaskITem)
        {
            _objHandler._userId = _userId;
            response = _objHandler.SaveTaskItem(objTaskITem);

            if (!response.IsError)
                await NotifyTaskChanged("task-created", objTaskITem.UserId);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItemTask(TaskItems objTaskITem)
        {
            _objHandler._userId = _userId;
            response = _objHandler.UpdateTaskItem(objTaskITem);

            if (!response.IsError)
                await NotifyTaskChanged("task-updated", objTaskITem.UserId);

            return Ok(response);
        }

        [HttpPut("AssignTasks")]
        public async Task<IActionResult> AssignTasks(TaskAssign objTaskAssign)
        {
            _objHandler._userId = _userId;
            response = _objHandler.AssignTasks(objTaskAssign);

            if (!response.IsError)
                await NotifyTaskChanged("task-assigned", objTaskAssign.userId);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTaskItem(int Id)
        {
            response = _objHandler.DeleteTaskItem(Id);

            if (!response.IsError)
            {
                await _hubContext.Clients.Groups("admins", "all-authenticated-users")
                .SendAsync("TaskChanged", new
                {
                    eventType = "task-deleted",
                    taskId = Id,
                    triggeredBy = _userId,
                    occurredAt = DateTime.UtcNow
                });
            }

            return Ok(response);
        }

        private async Task NotifyTaskChanged(string eventType, int? assignedUserId)
        {
            await _hubContext.Clients.Groups("admins", "all-authenticated- users").SendAsync("TaskChanged", new
            {
                eventType,
                userId = assignedUserId,
                triggeredBy = _userId,
                occuredAt = DateTime.UtcNow
            });

            if (assignedUserId.HasValue && assignedUserId.Value > 0)
            {
                await _hubContext.Clients.Groups($"user-{assignedUserId.Value}").SendAsync("TaskChanged", new
                {
                    eventType,
                    userId = assignedUserId,
                    triggeredBy = _userId,
                    occuredAt = DateTime.UtcNow
                });
            }
        }

        private async Task NotifyTaskAssigned(int assignedUserId)
        {
            await _hubContext.Clients.Groups("admins", "all-authenticated-users")
                .SendAsync("TaskChanged", new
                {
                    eventType = "task-assigned",
                    userId = assignedUserId,
                    triggeredBy = _userId,
                    occurredAt = DateTime.UtcNow
                });

            await _hubContext.Clients.Group($"user-{assignedUserId}")
                .SendAsync("TaskChanged", new
                {
                    eventType = "task-assigned",
                    userId = assignedUserId,
                    triggeredBy = _userId,
                    occurredAt = DateTime.UtcNow
                });
        }
    }
}
