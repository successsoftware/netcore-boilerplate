using CleanArchitecture.Net7.WebApi.Features.UserEndpoint;

using FastEndpoints.Security;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests;

public class LoginEndpointTests : IClassFixture<BaseWebApplicationFactory>
{
    private const string Url = "/api/user/auth/login";
    private readonly BaseWebApplicationFactory _fixture;

    public LoginEndpointTests(BaseWebApplicationFactory fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_AccessToken_Successfully()
    {
        var loginReq = new LoginRequest
        {
            Username = "admin",
            Password = "admin@123"
        };

        var response = await _fixture.Client.PostAsJsonAsync(Url, loginReq);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Should_Throw_BadRequest_Error()
    {
        var loginReq = new LoginRequest
        {
            Username = "admin",
            Password = "admin@1234"
        };

        var response = await _fixture.Client.PostAsJsonAsync(Url, loginReq);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
