using SSS.EntityFrameworkCore.Extensions.Entities;

namespace CleanArchitecture.Net7.WebApi.Data.Models
{
    public class RefreshToken : IBaseEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
