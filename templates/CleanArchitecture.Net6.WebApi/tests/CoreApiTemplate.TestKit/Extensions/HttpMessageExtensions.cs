using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;

namespace CoreApiTemplate.IntegrationTest.Extensions
{
    public static class HttpMessageExtensions
    {

        public static async Task AssertIfBadRequestAsync(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage is null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await Task.CompletedTask;
        }

        public static StringContent GetRequestContent(this object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(this HttpResponseMessage httpResponse)
        {
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(stringResponse);

            return result;
        }

        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null && !p.GetValue(obj, null).GetType().Equals(typeof(string[]))
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        public static void SetContent<T>(this HttpRequestMessage httpRequestMessage, T content)
        {
            if (httpRequestMessage is null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

    }
}
