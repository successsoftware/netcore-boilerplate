using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreApiTemplate.Application.Proxies
{
    /// <summary>
    /// This use for demo Mock Server
    /// </summary>
    public class PublicApiProxy
    {
        public readonly HttpClient HttpClient;

        public record EntryData(int Count, object Entries);

        public PublicApiProxy(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public static PublicApiProxy GetInstance(HttpClient httpClient)
        {
            return new PublicApiProxy(httpClient);
        }

        public async Task<EntryData> GetEntriesAsync()
        {
            var response = await HttpClient.GetAsync("/entries");

            response.EnsureSuccessStatusCode();

            var strResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<EntryData>(strResponse);
        }
    }
}
