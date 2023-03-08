using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class SignupMapper : RequestMapper<SignupRequest, IdentityUser>
    {
        public override IdentityUser ToEntity(SignupRequest r) => new()
        {
            Email = r.Email,
            UserName = r.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }
}