using Newtonsoft.Json;
using System.Text;

namespace CoreApiTemplate.Mock
{
    public static class HttpExtensions
    {
        public static StringContent ObjToHttpContent(this object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
