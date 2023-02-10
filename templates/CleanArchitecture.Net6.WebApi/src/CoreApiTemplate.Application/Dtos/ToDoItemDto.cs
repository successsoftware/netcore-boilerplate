using AutoMapper;
using CoreApiTemplate.Application.Mapping;
using CoreApiTemplate.Domain.Entities;

namespace CoreApiTemplate.Application.Dtos
{
    public class ToDoItemDto : IMapFrom<ToDoItem>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ToDoItem, ToDoItemDto>();
        }
    }
}
