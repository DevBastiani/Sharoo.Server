using Microsoft.AspNetCore.Mvc;
using Sharoo.Server.Application.DTOs.Todos.Create.Request;
using Sharoo.Server.Application.DTOs.Todos.Read;
using Sharoo.Server.Application.DTOs.Todos.ReadById;
using Sharoo.Server.Application.Services.Todos;

namespace Sharoo.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _service.ReadAsync();
            return Ok(TodoReadResponse.FromEntitiesToResponse(todos));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var todo = await _service.ReadByIdAsync(id);
            if (todo is null)
                return NotFound();

            return Ok(TodoReadByIdResponse.FromEntityToResponse(todo));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(TodoCreateRequest.FromRequestToEntity(request));
            return Created();
        }

        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.ChangeStatusAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todo = await _service.ReadByIdAsync(id);
            if (todo is null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
