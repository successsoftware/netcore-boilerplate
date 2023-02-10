using AutoMapper;
using CoreApiTemplate.Application.Dtos;
using CoreApiTemplate.Application.Interfaces;
using CoreApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.AspNetCore.Extensions.Exceptions;
using SSS.AspNetCore.Extensions.Models;
using SSS.AspNetCore.Extensions.ServiceProfiling;
using SSS.Common.Entensions;
using SSS.Common.Models;
using SSS.EntityFrameworkCore.Extensions.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiTemplate.Application.Services
{
    public interface IToDoService
    {
        Task<Pageable<ToDoItemDto>> SearchAsync(BaseQuery @param);

        Task<ToDoItemDto> CreateAsync(CreateTodoItemDto dto);

        Task<ToDoItemDto> GetByIdAsync(string id);
    }


    public class ToDoService : BaseService<ToDoService>, IToDoService
    {
        private readonly IAppDbContext _context;

        public ToDoService(IMapper mapper, ILoggerFactory loggerFactory, IAppDbContext context) : base(mapper, loggerFactory)
        {
            _context = context;
        }

        public async Task<ToDoItemDto> CreateAsync(CreateTodoItemDto dto)
        {
            var entity = MapToEntity<ToDoItem, CreateTodoItemDto>(dto);

            _context.ToDoItems.Add(entity);

            await _context.SaveChangesAsync();

            return MapToDto<ToDoItem, ToDoItemDto>(entity);
        }

        public async Task<ToDoItemDto> GetByIdAsync(string id)
        {
            var todoItem = await _context.ToDoItems.FindAsync(id);

            if (todoItem is null) throw new NotFoundException($"Not found record with id {id}");

            return MapToDto<ToDoItem, ToDoItemDto>(todoItem);
        }

        public async Task<Pageable<ToDoItemDto>> SearchAsync(BaseQuery @param)
        {

            var query = _context.ToDoItems.ApplySort(@param.OrderBy, @param.Direction).AsQueryable();

            if (!string.IsNullOrEmpty(@param.SearchText))
            {
                if (!string.IsNullOrEmpty(@param.SearchFields))
                {
                    query = query.ApplyLikeSearch(@param.SearchText, @param.GetFilteredFields());
                }
                else
                {
                    query = query.Where(x => EF.Functions.Like(x.Title, $"%{@param.SearchText}%"));
                }
            }

            return await query.Select(x => _mapper.Map<ToDoItemDto>(x)).PaginatedListAsync(@param.PageIndex, param.PageSize);
        }
    }
}
