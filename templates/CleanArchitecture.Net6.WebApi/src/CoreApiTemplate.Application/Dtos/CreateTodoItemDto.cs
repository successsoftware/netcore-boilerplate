using AutoMapper;
using CoreApiTemplate.Application.Mapping;
using CoreApiTemplate.Domain.Entities;
using FluentValidation;

namespace CoreApiTemplate.Application.Dtos
{
    public class CreateTodoItemDto : IMapTo<ToDoItem>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTodoItemDto, ToDoItem>();
        }
    }

    public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
