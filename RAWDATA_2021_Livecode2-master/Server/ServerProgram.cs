using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using Utillities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

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
                    string serverTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

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
                    } else if (data.Date.Length != serverTimeStamp.Length || data.Date is not string)
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
                    } else if (data.Body != null)
                    { 
                        // conversion JSON to object
                        Category categoryFromJson = null;
                        try 
                        {
                             categoryFromJson = JsonSerializer.Deserialize<Category>(data.Body);
                             if (categoryFromJson.Id <= 0 || categoryFromJson.Name == null) 
                             { 
                                if (errorMessage != "")
                                    errorMessage += ",";
                                errorMessage += "Illegal body";
                            
                             }
                        } catch 
                        {
                            if (errorMessage != "")
                                    errorMessage += ",";
                            errorMessage += "Illegal body";
                        }
                    }
                    
                    if (errorMessage == "")
                    { 
                        checkAPI(data.Method, data.Path, client);
                    } else
                    { 
                        res.Status = errorMessage;
                        client.Write(res.ToJson());
                    }
                }
            }
        }
    
        private static void checkAPI (string method, string path, NetworkClient client)
        { 
            var categories = GetCategories();
            var res = new Response();
            string[] pathComponents = path.Split('/');
            int pathLength = pathComponents.Length - 1;

            if (pathLength == 1) 
            {
                res.Status = "4 Bad Request";
                client.Write(res.ToJson());
            } else if (pathLength == 2 || pathLength == 3)
            {
                if (pathComponents[1] == "api" && pathComponents[2] == "categories")
                { 
                    if (method == "read")
                    { 
                        if (pathLength == 3)
                        {
                            try 
                            { 
                                int id = Int32.Parse(pathComponents[3]);
                                bool found = false;
                                foreach (Category category in categories)
                                    { 
                                        if (category.Id == id)
                                        {
                                            res.Status = "1 Ok";
                                            res.Body = (new { cid = category.Id, Name = category.Name}).ToJson();
                                            client.Write(res.ToJson());
                                            found = true;
                                            break;
                                        }
                                    }
                                if (!found)
                                {
                                    res.Status = "5 Not Found";
                                    client.Write(res.ToJson());
                                }
                            } catch
                                {
                                    res.Status = "4 Bad Request";
                                    client.Write(res.ToJson());
                                }
                        } else
                        {
                            res.Status = "1 Ok";
                            res.Body = categories.ToJson();
                            client.Write(res.ToJson());
                        }

                    } else if (method == "create")
                    { 
                        
                    } else if (method == "update")
                    { 
        
                    } else if (method == "delete")
                    { 
        
                    } else if (method == "echo")
                    { 
            
                    }
                } else
                {
                    res.Status = "4 Bad Request";
                    client.Write(res.ToJson());
                }
            } 
        }

        public static List<Category> GetCategories()
        {
		    return new List<Category>
		    {
			    new Category { Id = 1, Name = "Beverages" },
			    new Category { Id = 2, Name = "Condiments" },
			    new Category { Id = 3, Name = "Confections"}
		    };
	    }
    }
}
