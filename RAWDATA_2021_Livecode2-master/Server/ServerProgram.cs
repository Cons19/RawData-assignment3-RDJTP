using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
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
                if (req == "{}")
                {
                    var r = new Response { Body = "4", Status = "Illegal method, Missing method, Illegal path, Missing path, Illegal date, Missing date, Missing body" };
                    client.Write(r.ToJson());
                }
                else
                {
                    var data = req.FromJson<Request>();
                    var res = new Response();
                    string errorMessage = "";
                    string[] methods = {"read", "create", "update", "delete", "echo"};

                    switch (data.Method)
                    {
                        case "":
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal method";
                            break;
                        case null:
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing method";
                            break;
                        default:
                            if(!methods.Contains(data.Method))
                            {
                                res.Body = "4";
                                if (errorMessage != "")
                                    errorMessage += ",";
                                errorMessage += "Illegal method";
                            }
                            break;
                    }

                    switch (data.Path)
                    {
                        case "":
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal resource";
                            break;
                        case null:
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing resource";
                            break;
                        default:
                            break;
                    }

                    switch (data.Date)
                    {
                        case "":
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Illegal date";
                            break;
                        case null:
                            res.Body = "4";
                            if (errorMessage != "")
                                errorMessage += ",";
                            errorMessage += "Missing date";
                            break;
                        default:
                            if (DateTimeOffset.Now.ToUnixTimeSeconds().ToString() != data.Date )
                            {
                                if (errorMessage != "")
                                    errorMessage += ",";
                                errorMessage += "Illegal date";
                            }
                            break;
                    }

                    switch (data.Body)
                    {
                        case null:
                            res.Body = "4";
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
