using System.Net.Http.Headers;
using System.Web;

using CleanArchitecture.Net7.WebApi.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MimeKit;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests.Extensions
{
    public static class IntegrationTestHelper
    {
        public static StringContent GetRequestContent(this object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(this HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(stringResponse);

            return result;
        }

        public static MultipartFormDataContent GetRequestFormData<T>(string fileName, T data = null) where T : class
        {
            MultipartFormDataContent multiContent = new();

            if (!string.IsNullOrEmpty(fileName))
            {
                // File
                var filePath = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}IntegrationTestFiles{Path.DirectorySeparatorChar}{fileName}";

                byte[] bytes = File.ReadAllBytes(filePath);

                var content = new StreamContent(new MemoryStream(bytes));
                var mimeType = MimeTypes.GetMimeType(fileName);
                content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                multiContent.Add(content, "file", fileName);
            }

            //Data
            if (data is not null)
            {
                var dic = data.AsDictionary();

                foreach (var kv in dic)
                {
                    if (kv.Key != "File")
                    {
                        if (kv.Value != null)
                        {
                            Type tp = kv.Value.GetType();
                            if (tp.IsArray)
                            {
                                var index = 0;
                                var values = kv.Value as object[];
                                foreach (var val in values)
                                {
                                    multiContent.Add(new StringContent(val.ToString()), $"{kv.Key}[{index}]");
                                    index++;
                                }
                            }
                            else
                            {
                                var stringContent = (tp.Equals(typeof(int)) || tp.Equals(typeof(bool))) ? new StringContent(kv.Value.ToString()) : new StringContent((string)kv.Value);
                                multiContent.Add(stringContent, kv.Key);
                            }
                        }
                    }
                }
            }

            return multiContent;
        }

        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null && !p.GetValue(obj, null).GetType().Equals(typeof(string[]))
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        public static async Task AssertCorrectFromatProblemDetails(this HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var objResponse = JObject.Parse(stringResponse);

            Assert.False(objResponse["errorCode"] == null);

            Assert.False(objResponse["errorMessage"] == null);

            Assert.False(objResponse["traceId"] == null);
        }

        public static async Task AssertBadRequest(this HttpResponseMessage response)
        {
            await Task.CompletedTask;

            response.Should().NotBeNull();
            HttpStatusCode.BadRequest.Should().Be(response.StatusCode);
        }

        public static async Task AssertNotFound(this Task<HttpResponseMessage> response)
        {
            try
            {
                await response;
            }
            catch (Exception ex)
            {
                var statusCode = ex.HResult;
                statusCode.Should().Be(400);
            }
        }

        public static IServiceCollection Override<TSource>(this IServiceCollection services,
            Func<IServiceCollection, IServiceCollection> register = null)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(TSource));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            register(services);

            return services;
        }

        public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
            if (descriptor != null) services.Remove(descriptor);
        }
    }
}
