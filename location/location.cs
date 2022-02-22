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
            if (args.Length != 0)
            {
                requestType request = requestType.whois;
                string test = args.ToString();
                if (test.Contains("HTTP/1.0"))
                {
                    request = requestType.HTTP10;
                }
                else if (test.Contains("HTTP/1.1"))
                {
                    request = requestType.HTTP11;
                }
                else if(!test.Contains("GET") && !test.Contains("PUT") && !test.Contains("POST")) 
                {
                    request = requestType.whois;
                }
                else 
                {
                    request = requestType.HTTP09;
                }
                if (request == requestType.whois)
                {
                    whois(args);
                }
                else if (request == requestType.HTTP09)
                {
                    HTTP09(args);
                }
                else if (request == requestType.HTTP10)
                {
                    HTTP10(args);
                }
                else if (request == requestType.HTTP11)
                {
                    HTTP11(args);
                }
            }
        }
        private static void whois(string[] args) 
        {
            if (args.Length != 0 && args.Length <= 2)
            {
                TcpClient client = new TcpClient();
                client.Connect("whois.net.dcs.hull.ac.uk", 43);
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                if (args.Length == 1)
                {
                    sw.WriteLine(args[0]);
                }
                else if (args.Length == 2)
                {
                    sw.WriteLine(args[0] + " " + args[1]);
                }
                sw.Flush();
                string result = sr.ReadToEnd();
                if (args.Length == 1)
                {
                    Console.WriteLine(args[0] + " is " + result);
                }
                else if (result == "OK\r\n" && args.Length == 2)
                {
                    Console.WriteLine(args[0] + " location changed to be " + args[1]);
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("No arguments where supplied!");
            }
            else
            {
                Console.WriteLine("Too many arguments where supplied!");
            }
        }
        private static void HTTP09(string[] args) 
        {
            //implement HTTP 0.9
        }
        private static void HTTP10(string[] args)
        {
            //implement HTTP 1.0
        }
        private static void HTTP11(string[] args)
        {
            //implement HTTP 1.1
        }
    }
}
