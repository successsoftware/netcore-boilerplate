using SSS.EntityFrameworkCore.Extensions.Entities;

namespace CoreApiTemplate.Domain.Entities
{
    public class ToDoItem : AuditEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
