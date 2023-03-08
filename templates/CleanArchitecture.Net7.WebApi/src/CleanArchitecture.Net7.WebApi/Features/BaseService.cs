using System.Security.Claims;

namespace CleanArchitecture.Net7.WebApi.Features
{
    public abstract class BaseService
    {
        protected readonly string _userId;

        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is not null)
            {
                _userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
