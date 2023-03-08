using System.Reflection;
using System.Text.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SSS.CommonLib.Entensions;

namespace CleanArchitecture.Net7.WebApi.Extensions
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static string ConvertPropertyNamesToPascalCase(this string jsonString)
        {
            using var sr = new StringReader(jsonString);

            using var jr = new JsonTextReader(sr);

            var jo = JObject.Load(jr);

            foreach (JProperty jp in jo.Properties().ToList())
            {
                string name = jp.Name.ConvertCamelCaseToSenteceCase();
                jp.Replace(new JProperty(name, jp.Value));
            }

            return jo.ToString();
        }

        public static string GetFirstCharacter(this string value)
        {
            var strSplit = value.Split();

            return strSplit[0]?.Substring(0, 1)?.ToString()?.ToUpper();
        }

        public static TObject DumpToConsole<TObject>(this TObject @object)
        {
            var output = "NULL";
            if (@object != null)
            {
                output = System.Text.Json.JsonSerializer.Serialize(@object, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }

            Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
            return @object;
        }
    }
}
