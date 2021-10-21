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
            Int32 unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


            client.Connect("localhost", PORT);

            /*var req = new Request
            {
                Method = "dwadwa",
                Path = "dwad",
                Date = unixTimestamp,
                Body = new Category { Id = 1, Name = "NewName" }
            };*/
            var req = new
            {
                Method = "xxxx",
                Path = "testing",
                Date = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                Body = "{}"
            };


            client.Write(req.ToJson());


            var response = client.Read();
            var data = response.FromJson<Response>();

            Console.WriteLine(response);
        }
    }
}
