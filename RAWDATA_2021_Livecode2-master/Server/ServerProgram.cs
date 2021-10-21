using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utillities;

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
                Console.WriteLine("Client accepted");

                var req = client.Read();

                if (string.Compare(req, "") == 1)
                {
                    System.Console.WriteLine("here");
                    var r = new Response { Body = 4, Status = "Illegal method, Missing method, Illegal path, Missing path, Illegal date, Missing body" };
                    client.Write(r.ToJson());
                }
                else
                {
                    var data = req.FromJson<Request>();
                    var res = new Response();
                    string errorMessage = "";

                    switch (data.Method)
                    {
                        case "":
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal method";
                            break;
                        case null:
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing method";
                            break;
                        default:
                            break;
                    }

                    switch (data.Path)
                    {
                        case "":
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal path";
                            break;
                        case null:
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing path";
                            break;
                        default:
                            break;
                    }

                    switch (data.Date)
                    {
                        case 0:
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal date";
                            break;
                        default:
                            break;
                    }


                    switch (data.Body)
                    {
                        case null:
                            res.Body = 4;
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing body";
                            break;
                        default:
                            break;
                    }

                    res.Status = errorMessage;
                    client.Write(res.ToJson());
                }
            }
        }
    }
}
