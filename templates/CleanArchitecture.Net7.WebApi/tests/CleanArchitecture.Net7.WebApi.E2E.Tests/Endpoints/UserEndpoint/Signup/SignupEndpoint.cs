using AutoFixture;

using CleanArchitecture.Net7.WebApi.Features.UserEndpoint;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests
{
    public class SignupEndpoint : IClassFixture<BaseWebApplicationFactory>
    {
        private const string Url = "/api/user/auth/signup";
        private readonly BaseWebApplicationFactory _fixture;
        private readonly Fixture _autoFixture;

        public SignupEndpoint(BaseWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _autoFixture = new Fixture();
        }

        [Fact]
        public async Task Create_Account_Successfully()
        {
            _autoFixture.Customize<SignupRequest>(
                c => c.With(x => x.Email, "test@yopmail.com"));

            var signupReq = _autoFixture.Create<SignupRequest>();

            var response = await _fixture.Client.PostAsJsonAsync(Url, signupReq);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_Account_Failed_With_Invalid_Data()
        {
            var signupReq = _autoFixture.Create<SignupRequest>();
            signupReq.Email = string.Empty;

            var response = await _fixture.Client.PostAsJsonAsync(Url, signupReq);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
