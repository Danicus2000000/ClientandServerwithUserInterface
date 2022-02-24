using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
namespace location
{
    class location
    {
        enum requestType
        {
            whois,
            HTTP09,
            HTTP10,
            HTTP11
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                while (args.Length == 0)
                {
                    Console.Write("No arguments given please enter request: ");
                    args = Console.ReadLine().Split(" ");
                }
            }
            string query = "";
            requestType request = requestType.whois;
            int timeout = 1000;
            string host = "whois.net.dcs.hull.ac.uk";
            int port = 43;
            string personID = "";
            string locationID = "";
            string logfilelocation = Directory.GetCurrentDirectory() + "\\log.txt";
            string serverdatabaselocation = Directory.GetCurrentDirectory() + "\\locations.txt";
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-h1":
                            request = requestType.HTTP11;
                            break;
                        case "-h0":
                            request = requestType.HTTP10;
                            break;
                        case "-h9":
                            request = requestType.HTTP09;
                            break;
                        case "-h":
                            host = args[i + 1];
                            break;
                        case "-t":
                            timeout = int.Parse(args[i + 1]);
                            break;
                        case "-p":
                            port = int.Parse(args[i + 1]);
                            break;
                        case "-l":
                            logfilelocation = args[i + 1];
                            break;
                        case "-f":
                            serverdatabaselocation = args[i + 1];
                            break;
                        default:
                            if (i != 0)
                            {
                                if (args[i - 1] != "-h" && args[i - 1] != "-t" && args[i - 1] != "-p" && args[i - 1] != "-l" && args[i - 1] != "-f")
                                {
                                    query += args[i] + " ";
                                }
                            }
                            else
                            {
                                query += args[i] + " ";
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The request was in an invalid format!");
                Environment.Exit(0);
            }
            try 
            {
                if (!File.Exists(logfilelocation)) 
                {
                    File.Create(logfilelocation);
                }
                else if (!File.Exists(serverdatabaselocation)) 
                {
                    File.Create(serverdatabaselocation);
                }
            }
            catch (Exception) 
            {
                Console.WriteLine("The selected path for the source or log file was invalid or inaccessable");
                Environment.Exit(-69);
            }
            query = query.Substring(0, query.Length - 1);
            try
            {
                string[] temp = query.Split(" ");
                personID = temp[0];
                locationID = string.Join(" ",temp[1..temp.Length]);
            }
            catch (Exception) { }
            try
            {
                string requestFullFormat = "";//build requests
                if (locationID == "")
                {
                    switch (request)
                    {
                        case requestType.whois:
                            requestFullFormat = personID+"\r\n";
                            break;
                        case requestType.HTTP09:
                            requestFullFormat = "GET /" + personID + "\r\n";
                            break;
                        case requestType.HTTP10:
                            requestFullFormat = "GET /?" + personID + " HTTP/1.0\r\n\r\n";
                            break;
                        case requestType.HTTP11:
                            requestFullFormat = "GET /?name=" + personID + " HTTP/1.1\r\nHost: " + host + "\r\n\r\n";
                            break;
                    }
                }
                else 
                {
                    switch (request)
                    {
                        case requestType.whois:
                            requestFullFormat = personID + " " + locationID + "\r\n";
                            break;
                        case requestType.HTTP09:
                            requestFullFormat = "PUT /" + personID + "\r\n\r\n" + locationID + "\r\n";
                            break;
                        case requestType.HTTP10:
                            requestFullFormat = "POST /" + personID + " HTTP/1.0\r\nContent-Length: " + locationID.Length + "\r\n\r\n" + locationID;
                            break;
                        case requestType.HTTP11:
                            string body = "name=" + personID + "&location=" + locationID;
                            requestFullFormat = "POST / HTTP/1.1\r\nHost: "+host+"\r\nContent-Length: "+body.Length+"\r\n\r\n"+body;
                            break;
                    }
                }
                Console.WriteLine(sendAndRespond(requestFullFormat,timeout,host,port,request,personID,locationID,logfilelocation));
            }
            catch (IOException) 
            {
                Console.WriteLine("The Client Timed Out");
            }
        }
        static string sendAndRespond(string requestFullFormat,int timeout, string host, int port,requestType request,string personID,string locationID,string logFileLocation) 
        {
            TcpClient client = new TcpClient();
            client.ReceiveTimeout = timeout;
            client.SendTimeout = timeout;
            client.Connect(host, port);
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());
            sw.Write(requestFullFormat);//need to modify query for HTTP requests as server is only recieving them in whois format therefore not returning corrrectly
            sw.Flush();
            string response = "";
            try
            {
                while (!sr.EndOfStream)
                {
                    response += (char)sr.Read();
                }
            }
            catch (Exception)
            {
                if (response.Length == 0)
                {
                    Console.WriteLine("The request timed out!");
                    Environment.Exit(-69);
                }
            }
            string result = "";
            if (request == requestType.whois)
            {
                if (locationID == "")
                {
                    result = personID + " is " + response + "\r\n";
                    File.AppendAllText(logFileLocation, personID + " is " + response + "\n");

                }
                else
                {
                    result = personID + " location changed to be " + locationID + "\r\n";
                    File.AppendAllText(logFileLocation, personID + " location changed to be " + locationID + "\n");
                }
            }
            else if (request == requestType.HTTP09 || request==requestType.HTTP10 || request==requestType.HTTP11)
            {
                string[] lineresponse = response.Split("\r\n");
                if (lineresponse[0] == "HTTP/0.9 200 OK" || lineresponse[0]=="HTTP/1.0 200 OK" || lineresponse[0]== "HTTP/1.1 200 OK")
                {
                    if (locationID == "")
                    {
                        result = personID + " is " + response.Split("\r\n\r\n")[1] + "\r\n";
                    }
                    else
                    {
                        result = personID + " location changed to be " + locationID + "\r\n";
                        File.AppendAllText(logFileLocation, personID + " location changed to be " + locationID + "\n");
                    }
                }
                else if (lineresponse[0] == "HTTP/0.9 200 OK" || lineresponse[0] == "HTTP/1.0 200 OK" || lineresponse[0] == "HTTP/1.1 200 OK")
                {
                    Console.WriteLine("Error: Not Found");
                }
                else 
                {
                    string trueresponse = response.Split("\r\n\r\n")[1];
                    Console.WriteLine(personID+" is "+trueresponse.Substring(0,trueresponse.Length-2));
                }
                File.AppendAllText(logFileLocation, response + "\n");
            }
            return result;

        }
    }
}