using CoreApiTemplate.Application.Proxies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreApiTemplate.Api.Controllers
{
    public class PublicApiController : ApiBase
    {
        private readonly PublicApiProxy _publicApi;

        public PublicApiController(PublicApiProxy publicApi)
        {
            _publicApi = publicApi;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntries()
            => Ok(await _publicApi.GetEntriesAsync());
    }
}
