using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace locationserver
{
    class ServerStart
    {
        enum RequestType//the enum used to represent which protocol is in use
        {
            whois,
            HTTP09,
            HTTP10,
            HTTP11
        }
        /// <summary>
        /// Using the context of the request the type of request is figured out
        /// </summary>
        /// <param name="request">The Request sent by the client</param>
        /// <returns>The Type of request to be handled</returns>
        private static RequestType GetRequestType(string request) //searches for key identifiers that signify each request type
        {
            if (request.Contains("HTTP/1.1") && (request.Contains("GET /?name=") || request.Contains("POST / HTTP/1.1\r\nHost:")))
            {
                return RequestType.HTTP11;
            }
            else if (request.Contains("HTTP/1.0") && (request.Contains("GET /?") || request.Contains("POST /")))
            {
                return RequestType.HTTP10;
            }
            else if ((request.Contains("GET /") || (request.Contains("PUT /") && request.Contains("\r\n\r\n"))) && !request.Contains("GET /?"))
            {
                return RequestType.HTTP09;
            }
            return RequestType.whois;
        }
        private static string HandleRequest(ConcurrentDictionary<string, string> storedData, RequestType requestType, string data)
        {
            data = data.Replace("  ", " ");//handle data based on request type
            string[] splitData = data.Split(" ");
            switch (requestType)
            {
                case RequestType.whois:
                    Console.WriteLine("Request Type: whois");
                    if (splitData.Length == 1)
                    {
                        try
                        {
                            return storedData[data.Replace("\r\n", "")] + "\r\n";
                        }
                        catch (Exception)
                        {
                            return "ERROR: no entries found\r\n";
                        }
                    }
                    else
                    {
                        string personID = splitData[0];
                        string locationID = data[(personID.Length + 1)..];
                        storedData[personID] = locationID.Replace("\r\n", "");
                        return "OK\r\n";
                    }
                case RequestType.HTTP09:
                    Console.WriteLine("Request Type: HTTP 0.9");
                    if (splitData[0] == "GET")
                    {
                        try
                        {
                            string personID = splitData[1][1..].Replace("\r\n", "");
                            string locationID = storedData[personID];
                            return "HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n" + locationID + "\r\n";
                        }
                        catch (Exception)
                        {
                            return "HTTP/0.9 404 Not Found\r\nContent-Type: text/plain\r\n\r\n";
                        }
                    }
                    else if (splitData[0] == "PUT")
                    {
                        string personID = data.Split("\r\n\r\n")[0][5..];
                        string locationID = data.Split("\r\n\r\n")[1].Replace("\r\n", "");//removes all newline operators and name from the output and ensures location can have spaces in
                        storedData[personID] = locationID;
                        return "HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
                case RequestType.HTTP10:
                    Console.WriteLine("Request Type: HTTP 1.0");
                    if (splitData[0] == "GET")
                    {
                        try
                        {
                            string personID = splitData[1][2..].Replace("\r\n", "");
                            string locationID = storedData[personID];
                            return "HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n" + locationID + "\r\n";
                        }
                        catch (Exception)
                        {
                            return "HTTP/1.0 404 Not Found\r\nContent-Type: text/plain\r\n\r\n";
                        }
                    }
                    else if (splitData[0] == "POST")
                    {
                        string personID = splitData[1][1..].Replace("\r\n", "");
                        string locationID = data.Split("\r\n")[3];
                        storedData[personID] = locationID;
                        return "HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
                case RequestType.HTTP11:
                    Console.WriteLine("Request Type: HTTP 1.1");
                    if (splitData[0] == "GET")
                    {
                        try
                        {
                            string personID = splitData[1][7..];
                            string locationID = storedData[personID];
                            return "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n" + locationID + "\r\n";
                        }
                        catch (Exception)
                        {
                            return "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n";//string formatting allows for complience with standards
                        }
                    }
                    else if (splitData[0] == "POST")
                    {
                        string[] dataLines = data.Split("\r\n");
                        string personID = dataLines[4][5..].Split("&")[0];
                        string locationID = dataLines[4][(15 + personID.Length)..];
                        storedData[personID] = locationID;
                        return "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
            }
            return "";
        }
        /// <summary>
        /// Retrieves and Runs the request
        /// </summary>
        /// <param name="socket">The network socket being used for the request</param>
        /// <param name="storedData">The Dictionary being used to store data</param>
        /// <param name="log">The log file being used to log server use</param>
        /// <param name="database">The database being used for storedata</param>
        /// <param name="timeout">The timeout to be used</param>
        public void Run(Socket socket, ConcurrentDictionary<string, string> storedData, string log, int timeout, ConcurrentQueue<string> loglist)
        {
            NetworkStream socketStream = new(socket);
            Console.WriteLine("Connection Recieved");
            try
            {
                socketStream.ReadTimeout = timeout;
                socketStream.WriteTimeout = timeout;
                Byte[] bytes = new Byte[256];//buffer for data
                string data = "";
                StreamReader sr = new(socketStream);//recieve data
                try
                {
                    while (sr.Peek() >= 0)
                    {
                        data += (char)sr.Read();
                    }
                }
                catch (Exception)//if an error occured then the request that was sent corrupted
                {
                    if (data.Length == 0)
                    {
                        Console.WriteLine("The request timed out!");
                    }
                }
                Console.WriteLine("Server recieved: \"" + data.Replace("\r", "\\r").Replace("\n", "\\n") + "\"");//respond to request
                RequestType requestType = GetRequestType(data);
                StreamWriter write = new(socketStream);
                string responseMessage = HandleRequest(storedData, requestType, data);
                write.Write(responseMessage);
                write.Flush();
                sr.Dispose();
                write.Dispose();
                Console.WriteLine("server sent: \"" + responseMessage.Replace("\r", "\\r").Replace("\n", "\\n") + "\"");//output to server console
                string formatData = data.Replace("\n", "\\n").Replace("\r", "\\r");
                string formatResponse = responseMessage.Replace("\n", "\\n").Replace("\r", "\\r");
                loglist.Enqueue((socket.RemoteEndPoint as IPEndPoint).Address.ToString() + " - - [" + DateTime.Now + "] \"" + formatData + "\" " + formatResponse);//data to write
            }
            catch (Exception)//if timeout occurs
            {
                Console.WriteLine("The thread timed out!");
            }
            finally//close socket on request complete
            {
                socketStream.Close();
                socket.Close();
                Console.WriteLine("Connection Disposed");
            }

        }
    }
}
