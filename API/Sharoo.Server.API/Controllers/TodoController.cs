using Microsoft.AspNetCore.Mvc;

namespace Sharoo.Server.API.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public IActionResult HelloWorld()
        {
            return Ok(new { Message = "Coming soon..." });
        }
    }
}
