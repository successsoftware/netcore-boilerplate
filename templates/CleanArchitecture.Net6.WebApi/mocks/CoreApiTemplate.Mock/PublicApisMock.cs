using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace CoreApiTemplate.Mock
{
    public static class PublicApisMock
    {
        private static string _baseUrl = string.Empty;

        public static MockHttpMessageHandler Create(MockHttpMessageHandler mockHttp,
            string baseAddress)
        {
            _baseUrl = $"{baseAddress}";

            return mockHttp
                .GetList();
        }

        private static MockHttpMessageHandler GetList(this MockHttpMessageHandler mockHttp)
        {
            var result = new
            {
                Count = 2,
                Entries = new object[]
                {
                    new
                    {
                        API = "AdoptAPet",
                        Description = "Resource to help get pets adopted",
                    },
                    new
                    {
                        API = "Axolotl",
                        Description = "Collection of axolotl pictures and facts",
                    }
                }
            };

            mockHttp.When(HttpMethod.Get, $"{_baseUrl}/entries")
                    .Respond("application/json", JsonConvert.SerializeObject(result));

            return mockHttp;
        }
    }
}
