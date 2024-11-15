using System;
using System.IO;
using System.Net.Sockets;
namespace location
{
    class Location
    {
        enum RequestType//the enum used to represent which protocol is in use
        {
            whois,
            HTTP09,
            HTTP10,
            HTTP11
        }
        private static RequestType request = RequestType.whois;//all changable variable used throughout names are self explanitory
        private static string host = "whois.net.dcs.hull.ac.uk";
        private static int timeout = 1000;
        private static int port = 43;
        private static string personID = "";
        private static string locationID = "";
        private static bool debugMode = false;

        /// <summary>
        /// Parses any arguments given to fill out required data
        /// </summary>
        /// <param name="args">The arguments handed either at the command line or via user input</param>
        static void ParseArgs(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)//loops through all arguments
                {
                    switch (args[i])
                    {
                        case "-h1"://if a HTTP protocol is specified
                            request = RequestType.HTTP11;
                            break;
                        case "-h0":
                            request = RequestType.HTTP10;
                            break;
                        case "-h9":
                            request = RequestType.HTTP09;
                            break;
                        case "-h"://if alternate host is specified
                            host = args[i + 1];
                            break;
                        case "-t"://if alternate timeout is specified
                            timeout = int.Parse(args[i + 1]);
                            break;
                        case "-p"://if alternate port is specified
                            port = int.Parse(args[i + 1]);
                            break;
                        case "-d":
                            debugMode = true;
                            break;
                        default://all that remains in a valid string is the name and location
                            if (i != 0)//if this argument is not the first
                            {
                                if (args[i - 1] != "-h" && args[i - 1] != "-t" && args[i - 1] != "-p")//check its predecessor does not require this as data
                                {
                                    if (personID == "")//if person id is empty fill it
                                    {
                                        personID = args[i];
                                    }
                                    else//if location id is empty fill it
                                    {
                                        locationID += args[i] + " ";
                                    }
                                }
                            }
                            else//if we are on the first and all other flags have by this point been checked
                            {
                                if (personID == "")//if person id is empty fill it
                                {
                                    personID = args[i];
                                }
                                else//if location id is empty fill it
                                {
                                    locationID += args[i] + " ";
                                }
                            }
                            break;
                    }
                }
                if (locationID != "") //if location id is not empty remove the last space
                {
                    locationID = locationID[..^1];
                }
            }
            catch (Exception)//Exceptions can only be caused by invalid argument use so the program outputs an error and exits
            {
                Console.WriteLine("The request was in an invalid format!");
                Environment.Exit(-69);
            }
        }
        /// <summary>
        /// Takes in the context of the request and returns it in a format the server will be able to manage
        /// </summary>
        /// <returns>A string formatted based on the set protocol</returns>
        static string FormatRequest()
        {
            if (locationID == "")//if there is a no location ID we are requesting data
            {
                switch (request)//based on request type structure as required
                {
                    case RequestType.whois:
                        return personID + "\r\n";
                    case RequestType.HTTP09:
                        return "GET /" + personID + "\r\n";
                    case RequestType.HTTP10:
                        return "GET /?" + personID + " HTTP/1.0\r\n\r\n";
                    case RequestType.HTTP11:
                        return "GET /?name=" + personID + " HTTP/1.1\r\nHost: " + host + "\r\n\r\n";
                }
            }
            else//if there is we are modifying data
            {
                switch (request)//based on request type structure as required
                {
                    case RequestType.whois:
                        return personID + " " + locationID + "\r\n";
                    case RequestType.HTTP09:
                        return "PUT /" + personID + "\r\n\r\n" + locationID + "\r\n";
                    case RequestType.HTTP10:
                        return "POST /" + personID + " HTTP/1.0\r\nContent-Length: " + locationID.Length + "\r\n\r\n" + locationID;
                    case RequestType.HTTP11:
                        string body = "name=" + personID + "&location=" + locationID;
                        return "POST / HTTP/1.1\r\nHost: " + host + "\r\nContent-Length: " + body.Length + "\r\n\r\n" + body;
                }
            }
            return "Error: Not Found";//in case of serious error there is a breakout clause
        }

        /// <summary>
        /// Handles the server request and structures the response the user sees into a logical form
        /// </summary>
        /// <param name="requestFullFormat">The fully formatted request to send the server</param>
        /// <returns>The response from the server formatted as required</returns>
        static string HandleRequest(string requestFullFormat)
        {
            TcpClient client = new()
            {
                ReceiveTimeout = timeout,
                SendTimeout = timeout
            };//intialise client and timeout and attempt to connect
            try
            {
                client.Connect(host, port);
            }
            catch (Exception)//if connection fails alert the user
            {
                Console.WriteLine("The Client was unable to connect");
                Environment.Exit(-69);
            }
            StreamWriter sw = new(client.GetStream());
            sw.Write(requestFullFormat);
            sw.Flush();//writes the request to the server and then sends it
            if (debugMode)//if debug show full input
            {
                Console.WriteLine("Request sent: " + requestFullFormat.Replace("\r", "\\r").Replace("\n", "\\n"));
                Console.WriteLine("Request Format: " + request.ToString());
            }
            StreamReader sr = new(client.GetStream());//Get the response from the server
            string response = "";
            try
            {
                while (!sr.EndOfStream)
                {
                    response += (char)sr.Read();
                }
            }
            catch (Exception)//if an error occured then the request was not furfilled therefore it timed out
            {
                if (response.Length == 0)
                {
                    Console.WriteLine("The request timed out!");
                    Environment.Exit(-69);
                }
            }
            if (debugMode)//if debug show true output
            {
                Console.WriteLine("True Request response: " + response.Replace("\r", "\\r").Replace("\n", "\\n"));
                Console.WriteLine();
            }
            string result;
            if (request == RequestType.whois)//if we are dealing with a whois request
            {
                if (locationID == "")//if it is a get request
                {
                    result = personID + " is " + response + "\r\n";//structure request result in get style for whois

                }
                else//if it is a modify request
                {
                    result = personID + " location changed to be " + locationID + "\r\n";//structure in that style
                    //if (personID == "GET" && locationID == "/636948") Lab 4 issue workaround
                    //{
                    //    result = "Error: Not Found";
                    //}
                }
            }
            else//if it is a HTTP style request
            {
                string[] lineresponse = response.Split("\r\n");//split response into lines
                if (lineresponse[0] == "HTTP/0.9 200 OK" || lineresponse[0] == "HTTP/1.0 200 OK" || lineresponse[0] == "HTTP/1.1 200 OK")//check header to see if request succeeded
                {
                    if (locationID == "")//if it was a get request you can split by \r\n\r\n to find the data requested
                    {
                        result = personID + " is " + response.Split("\r\n\r\n")[1] + "\r\n";//structure in that style
                    }
                    else//if not then the server will not send a response only okay so we must use our data stored earlier to signal completion
                    {
                        result = personID + " location changed to be " + locationID + "\r\n";//structure in that style
                    }
                }
                else if (lineresponse[0] == "HTTP/0.9 404 Not Found" || lineresponse[0] == "HTTP/1.0 404 Not Found" || lineresponse[0] == "HTTP/1.1 404 Not Found")//if the data is not found
                {
                    result = "Error: Not Found";
                }
                else
                {
                    string trueresponse = response.Split("\r\n\r\n")[1];//in order to structure correctly must remove some unrequired data from the response
                    result = string.Concat(personID, " is ", trueresponse.AsSpan(0, trueresponse.Length - 2));//structure in that style
                }
            }
            return result;//return result to be outputted by main program

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)//if no arguments are given ask for them
            {
                //make winform
                while (args.Length == 0)
                {
                    Console.Write("No arguments given please enter request: ");
                    args = Console.ReadLine().Split(" ");
                }
            }
            ParseArgs(args);//parse the arguments given
            string requestFullFormat = FormatRequest();//build requests
            try
            {
                Console.WriteLine(HandleRequest(requestFullFormat));//sends and outputs result of request
            }
            catch (IOException)
            {
                Console.WriteLine("The Client Timed out!");
            }
        }
    }
}