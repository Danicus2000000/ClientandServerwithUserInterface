﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace locationserver
{
    class serverstart
    {
        private static Thread threadInitialise;
        enum requestType//the enum used to represent which protocol is in use
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
        private static requestType getRequestType(string request) 
        {
            string[] requestlines = request.Split("\r\n");
            if (requestlines[0].Contains("HTTP/1.1")) 
            {
                return requestType.HTTP11;
            }
            else if (requestlines[0].Contains("HTTP/1.0")) 
            {
                return requestType.HTTP10;
            }
            else if(requestlines[0].Contains("GET") || requestlines[0].Contains("PUT")) 
            {
                return requestType.HTTP09;
            }
            return requestType.whois;
        }
        private static string handleRequest(Dictionary<string, string> storedData, requestType requestType, string data)
        {
            string[] splitData = data.Split(" ");
            switch (requestType) 
            {
                case requestType.whois:
                    if (splitData.Length == 1)
                    {
                        try
                        {
                            return storedData[splitData[0]] + "\r\n";
                        }
                        catch (Exception)
                        {
                            return "ERROR: no entries found\r\n";
                        }
                    }
                    else
                    {
                        string personID=splitData[0];
                        splitData[0] = "";
                        string locationID = string.Join("",splitData);
                        Console.WriteLine(personID);
                        Console.WriteLine(locationID);
                        storedData[personID] = locationID.Replace("\r\n", "");
                        return "OK\r\n";
                    }
                case requestType.HTTP09:
                    if (splitData[0] == "GET") 
                    {
                        try
                        {
                            string personID = splitData[1].Replace("\r\n", "");
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
                        string nameLocation = splitData[1].Substring(1);
                        string personID = nameLocation.Split("\r\n\r\n")[0];
                        splitData[0] = "";
                        string locationID = string.Join("", splitData).Split("\r\n\r\n")[1].Replace("\r\n","");//removes all newline operators and name from the output and ensures location can have spaces in
                        //string locationID = nameLocation.Split("\r\n\r\n")[1].Replace("\r\n", "");
                        storedData[personID] = locationID;
                        return "HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
                case requestType.HTTP10:
                    if (splitData[0] == "GET") 
                    {
                        try
                        {
                            string personID = splitData[1].Substring(2);
                            string locationID= storedData[personID];
                            return "HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n" + locationID + "\r\n";
                        }
                        catch (Exception) 
                        {
                            return "HTTP/1.0 404 Not Found\r\nContent-Type: text/plain\r\n\r\n";
                        }
                    }
                    else if (splitData[0] == "POST") 
                    {
                        string personID=splitData[1].Substring(1);
                        string locationID = data.Split("\r\n")[3];
                        storedData[personID] = locationID;
                        return "HTTP/1.0 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
                case requestType.HTTP11:
                    if(splitData[0] == "GET") 
                    {
                        try
                        {
                            string personID = splitData[1].Substring(7);
                            string locationID= storedData[personID];
                            return "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n" + locationID + "\r\n";
                        }
                        catch (Exception) 
                        {
                            return "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n";
                        }
                    }
                    else if (splitData[1] == "POST") 
                    {
                        string[] dataLines = data.Split("\r\n");
                        string[] nameAndLocation = dataLines[4].Split("&");
                        string personID = nameAndLocation[0].Substring(5);
                        string locationID = nameAndLocation[1].Substring(9);
                        storedData[personID] = locationID;
                        return "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                    }
                    break;
            }
            return "";
        }
        public void threadstart(NetworkStream request)
        {
            //threadInitialise = new Thread(new ThreadStart(run));
            //threadInitialise.Start();
        }

        /// <summary>
        /// Retrieves and Runs the request
        /// </summary>
        /// <param name="request">The network stream containing the request</param>
        /// <param name="storeddata">The Dictionary being used to store data</param>
        public void run(NetworkStream request,Dictionary<string,string> storedData) 
        {
            try
            {
                Byte[] bytes = new Byte[256];
                string data = "";
                StreamReader sr = new StreamReader(request);
                try
                {
                    while (!sr.EndOfStream)
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
                Console.WriteLine("Server recieved: \"" + data+"\"");
                requestType requestType= getRequestType(data);
                StreamWriter write = new StreamWriter(request);
                string responseMessage = handleRequest(storedData, requestType, data);
                write.Write(responseMessage);
                write.Flush();
                Console.WriteLine("server sent: \"" + responseMessage + "\"");
            }
            catch(IOException)
            {
                Console.WriteLine("The thread timed out!");
            }
            //threadgen.threadstart(socketStream);
        }
    }
}
