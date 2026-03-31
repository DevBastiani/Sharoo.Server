using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharoo.Server.Application.DTOs.Todos.Create.Request;
using Sharoo.Server.Application.DTOs.Todos.Read;
using Sharoo.Server.Application.DTOs.Todos.ReadById;
using Sharoo.Server.Application.Services.Todos;

namespace Sharoo.Server.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            var todos = await _service.ReadAsync(userId);
            return Ok(TodoReadResponse.FromEntitiesToResponse(todos));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var todo = await _service.ReadByIdAsync(id);
            return Ok(TodoReadByIdResponse.FromEntityToResponse(todo));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            await _service.CreateAsync(TodoCreateRequest.FromRequestToEntity(request, userId));
            return Created();
        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id)
        {
            await _service.ChangeStatusAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
