using System;
using System.Net.Sockets;
using Utillities;

namespace Client
{

    class ClientProgram
    {
        const int PORT = 5000;
        static void Main(string[] args)
        {

            var client = new NetworkClient();
            string unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

            client.Connect("localhost", PORT);

            var req = new Request
            {
                Method = "create",
                Path = "/api/categories",
                Date = unixTimestamp,
                Body = (new { name = "Seafood" }).ToJson()
            };

            client.Write(req.ToJson());

            var response = client.Read();
            var data = response.FromJson<Response>();

            Console.WriteLine(response);
        }
    }
}
