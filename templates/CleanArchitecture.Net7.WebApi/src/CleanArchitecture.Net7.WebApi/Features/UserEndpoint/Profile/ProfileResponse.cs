using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ProfileResponse : BaseResponse
    {
        public string Email { get; set; }
    }
}