using Microsoft.AspNetCore.Mvc;

namespace CoreApiTemplate.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiBase : ControllerBase
    {
        protected OkObjectResult Success()
        {
            return Ok(new { success = true });
        }
    }
}
