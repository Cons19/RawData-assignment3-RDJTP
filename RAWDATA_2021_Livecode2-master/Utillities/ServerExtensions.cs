using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utillities
{
    public class ServerExtensions
    {

        public class Response
        {
            public string Status { get; set; }
            public string Body { get; set; }
        }

        public class Category
        {
            [JsonPropertyName("cid")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }

        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static T FromJson<T>(this string element)
        {
            return JsonSerializer.Deserialize<T>(element, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
