using System;
using System.Net.Sockets;
using System.Text;
using Utillities;

namespace Client
{
    
    class ClientProgram
    {
        const int PORT = 5000;
        static void Main(string[] args)
        {

            var client = new NetworkClient();

            client.Connect("localhost", PORT);

            var message = "hello";
            client.Write(message);

            var response = client.Read();

            Console.WriteLine($"Server response '{response}'");

        }
    }
}
