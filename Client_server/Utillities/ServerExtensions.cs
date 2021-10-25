using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Utillities;

namespace Utillities
{

    public class Request
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Date { get; set; } // timestamp??
        public string Body { get; set; } // category
    }

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


    public static class ServerExtensions
    {
        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static T FromJson<T>(this string element)
        {
            return JsonSerializer.Deserialize<T>(element, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static void SendRequest(this TcpClient client, string request)
        {
            var msg = Encoding.UTF8.GetBytes(request);
            client.GetStream().Write(msg, 0, msg.Length);
        }

        public static Response ReadResponse(this TcpClient client)
        {
            var strm = client.GetStream();
            //strm.ReadTimeout = 250;
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream())
            {
                int bytesread = 0;
                do
                {
                    bytesread = strm.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);

                } while (bytesread == 2048);

                var responseData = Encoding.UTF8.GetString(memStream.ToArray());
                return JsonSerializer.Deserialize<Response>(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }

    }
}
