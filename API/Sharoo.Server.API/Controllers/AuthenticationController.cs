using Microsoft.AspNetCore.Mvc;
using Sharoo.Server.API.Authentication;
using Sharoo.Server.Application.DTOs.Authentications.GenerateToken;
using Sharoo.Server.Application.Services.Users;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _service;

        public AuthenticationController(IUserService userService, IAuthenticationService service)
        {
            _userService = userService;
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken([FromBody] AuthenticationGenerateTokenRequest request)
        {
            User user = await _userService.ReadUserByEmailAndPasswordAsync(request.Email, request.Password);
            var token = _service.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
