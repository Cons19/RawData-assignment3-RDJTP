﻿using System;
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
                Method = "echo",
                Date = unixTimestamp,
                Body = "Hello World"
            };

            client.Write(req.ToJson());
            var response = client.Read();

            Console.WriteLine(response);
        }
    }
}
