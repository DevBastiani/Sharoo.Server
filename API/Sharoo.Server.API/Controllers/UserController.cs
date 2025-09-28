using Microsoft.AspNetCore.Mvc;
using Sharoo.Server.Application.DTOs.Users.Create;
using Sharoo.Server.Application.Services.Users;

namespace Sharoo.Server.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            await _service.CreateUserAsync(UserCreateRequest.FromRequestToEntity(request));
            return Created();
        }
    }
}
