using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utillities;
using System.Text.Json;

namespace Server
{
    class ServerProgram
    {
        const int PORT = 5000;
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, PORT);
            server.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                var client = new NetworkClient(server.AcceptTcpClient());
                var serverExtension = new ServerExtensions();
                Console.WriteLine("Client accepted");

                var message = client.Read();
                serverExtension.FromJSON()
                if (message == string.Empty) { 
                }

                Console.WriteLine($"Client message '{message}'");

                client.Write(message.ToUpper());
            }

        }
    }
}
