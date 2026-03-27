using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CLUserController : ControllerBase
    {
        public BLUserHandler _objHandler;

        public CLUserController(BLUserHandler objHandler)
        {
            _objHandler = objHandler;
        }

        [HttpGet("GetUserData")]
        public IActionResult GetUserData()
        {
            return Ok(_objHandler.GetUserData());
        }
    }
}
