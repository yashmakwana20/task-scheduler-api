using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public CLTaskItemController(BLTaskItemHandler objHandler, IHttpContextAccessor contextAccessor)
        {
            _objHandler = objHandler;

            var identity = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
                _userId = Convert.ToInt32(identity.Claims.FirstOrDefault().Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetTaskData")]
        public IActionResult GetTaskData()
        {
            return Ok(_objHandler.GetTaskData());
        }

        [HttpPost]
        public IActionResult SaveTaskItem(TaskItems objTaskITem)
        {
            _objHandler._userId = _userId;
            return Ok(_objHandler.SaveTaskItem(objTaskITem));
        }

        [HttpPut]
        public IActionResult UpdateItemTask(TaskItems objTaskITem)
        {
            _objHandler._userId = _userId;
            return Ok(_objHandler.UpdateTaskItem(objTaskITem));
        }

        [HttpDelete]
        public IActionResult DeleteTaskItem(int Id)
        {
            return Ok(_objHandler.DeleteTaskItem(Id));
        }
    }
}
