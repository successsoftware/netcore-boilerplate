using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreApiTemplate.IntegrationTest.Extensions
{
    public static class ProblemDetailExtension
    {
        public static ProblemDetails AsProblemDetail(this string response)
        {
            return JsonConvert.DeserializeObject<ProblemDetails>(response);
        }
    }
}