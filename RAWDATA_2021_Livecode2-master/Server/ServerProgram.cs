using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using Utillities;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                    string[] bodyMethods = {"create", "update", "echo"};

                    // Checking Data Method
                    if (data.Method == "")
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Illegal method";
                    } else if (data.Method == null)
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Missing method";
                    } else if(!methods.Contains(data.Method))
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Illegal method";
                    }

                    // Checking Data Path
                    if (data.Path == "")
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Illegal resource";
                    } else if (data.Path == null)
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Missing resource";
                    }

                    // Checking Data Date
                    if (data.Date == "")
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Illegal date";
                    } else if (data.Date == null)
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Missing date";
                    } else if (DateTimeOffset.Now.ToUnixTimeSeconds().ToString() != data.Date )
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Illegal date";
                    }
                    // checking Data Body
                    if (data.Body == null && bodyMethods.Contains(data.Method))
                    {
                        if (errorMessage != "")
                            errorMessage += ",";
                        errorMessage += "Missing body";
                    } else if (data.Body != null && data.Method == "echo")
                    {
                        res.Body = data.Body;
                    }

                    // conversion JSON to object
                    try 
                    { 
                        var categoryFromJson = JsonSerializer.Deserialize<Category>(data.Body);                    
                    }
                    catch 
                    { 
                         if (errorMessage != "")
                            errorMessage += ",";
                         errorMessage += "Illegal body";
                    }

                    res.Status = errorMessage;
                    client.Write(res.ToJson());
                }
            }
        }
    }
}
