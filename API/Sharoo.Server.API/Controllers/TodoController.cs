using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharoo.Server.Application.DTOs.Todos.Create.Request;
using Sharoo.Server.Application.DTOs.Todos.Filter;
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

        /// <summary>
        /// Returns all todos for the authenticated user.
        /// Optionally filter by creation or completion date using query parameters.
        /// </summary>
        /// <param name="filter">Optional date range filters (query string).</param>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TodoFilterRequest filter)
        {
            var userId = GetCurrentUserId();

            var hasFilter = filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue
                            || filter.CompletedFrom.HasValue || filter.CompletedTo.HasValue;

            var todos = hasFilter
                ? await _service.ReadByFilterAsync(userId, filter)
                : await _service.ReadAsync(userId);

            return Ok(TodoReadResponse.FromEntitiesToResponse(todos));
        }

        /// <summary>
        /// Returns todos for the authenticated user filtered by date ranges.
        /// All query parameters are optional; combine them to narrow results.
        /// </summary>
        /// <param name="filter">Date range filters (query string).</param>
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromQuery] TodoFilterRequest filter)
        {
            var userId = GetCurrentUserId();
            var todos = await _service.ReadByFilterAsync(userId, filter);
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
