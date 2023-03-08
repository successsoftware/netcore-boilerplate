using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ProfileMapper : ResponseMapper<ProfileResponse, IdentityUser>
    {
        public override ProfileResponse FromEntity(IdentityUser e) => new()
        {
            Email = e.Email
        };
    }
}