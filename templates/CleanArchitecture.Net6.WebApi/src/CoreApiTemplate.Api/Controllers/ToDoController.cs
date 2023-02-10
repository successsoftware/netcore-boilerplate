using CoreApiTemplate.Application.Dtos;
using CoreApiTemplate.Application.Services;
using Microsoft.AspNetCore.Mvc;
using SSS.AspNetCore.Extensions.Models;
using System.Threading.Tasks;

namespace CoreApiTemplate.Api.Controllers
{
    public class ToDoController : ApiBase
    {
        private readonly IToDoService _service;

        public ToDoController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BaseQuery @query)
            => Ok(await _service.SearchAsync(@query));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemDto dto)
            => Ok(await _service.CreateAsync(dto));
    }
}
