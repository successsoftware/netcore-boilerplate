using RichardSzalay.MockHttp;

namespace CoreApiTemplate.Mock
{
    public class MockServer
    {
        public static HttpClient CreateHttpClient(string baseAddress)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp = PublicApisMock.Create(mockHttp, baseAddress);

            var client = mockHttp.ToHttpClient();

            client.BaseAddress = new Uri(baseAddress);

            return client;
        }
    }
}
