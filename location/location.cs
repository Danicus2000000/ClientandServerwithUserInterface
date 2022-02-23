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
            query = query.Substring(0, query.Length - 1);
            try
            {
                personID = query.Split(" ")[0];
                locationID = query.Split(" ")[1];
            }
            catch (Exception) { }
            try
            {
                TcpClient client = new TcpClient();
                client.ReceiveTimeout = timeout;
                client.SendTimeout = timeout;
                client.Connect(host, port);
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                sw.WriteLine(query);
                sw.Flush();
                string result = "";
                while (sr.Peek()>=0) 
                {
                    result += sr.ReadLine();
                }
                //if (File.Exists(logfilelocation))
                //{
                //    File.Delete(logfilelocation);
                //}
                if (request == requestType.whois)
                {
                    if (result != "OK\r\n")
                    {
                        Console.WriteLine(personID + " is " + result+"\r\n");
                        File.AppendAllText(logfilelocation, personID + " is " + result+"\n");

                    }
                    else if (result == "OK\r\n")
                    {
                        Console.WriteLine(personID + " location changed to be " + locationID+"\r\n");
                        File.AppendAllText(logfilelocation, personID + " location changed to be " + locationID+"\n");
                    }
                }
                else if (request == requestType.HTTP09 || request == requestType.HTTP10 || request == requestType.HTTP11)//HERE
                {
                    Console.WriteLine(result+"\r\n");
                    File.AppendAllText(logfilelocation, result+"\n");
                }
            }
            catch (IOException) 
            {
                Console.WriteLine("The Client Timed Out");
            }
        }
    }
}
//        private static void whois(string[] args,TcpClient client)
//        {
//            if (args.Length != 0 && args.Length <= 2)
//            {
//                StreamWriter sw = new StreamWriter(client.GetStream());
//                StreamReader sr = new StreamReader(client.GetStream());
//                if (args.Length == 1)
//                {
//                    sw.WriteLine(args[0]);
//                }
//                else if (args.Length == 2)
//                {
//                    sw.WriteLine(args[0] + " " + args[1]);
//                }
//                sw.Flush();
//                string result = sr.ReadToEnd();
//                if (args.Length == 1)
//                {
//                    Console.WriteLine(args[0] + " is " + result);
//                }
//                else if (result == "OK\r\n" && args.Length == 2)
//                {
//                    Console.WriteLine(args[0] + " location changed to be " + args[1]);
//                }
//            }
//            else if (args.Length == 0)
//            {
//                Console.WriteLine("No arguments where supplied!");
//            }
//            else
//            {
//                Console.WriteLine("Too many arguments where supplied!");
//            }
//        }
//        private static void HTTP09(string[] args,TcpClient client)
//        {
//            if (args.Length != 0 && args.Length <= 3)
//            {
//                StreamWriter sw = new StreamWriter(client.GetStream());
//                StreamReader sr = new StreamReader(client.GetStream());
//                if (args.Length == 2)
//                {
//                    sw.WriteLine(args[0] + " " + args[1]);
//                }
//                else if (args.Length == 3)
//                {
//                    sw.WriteLine(args[0] + " " + args[1] + " " + args[2]);
//                }
//                sw.Flush();
//                string result = sr.ReadToEnd();
//                if (args.Length == 2)
//                {
//                    Console.WriteLine(args[1] + " is " + result);
//                }
//                else if (result == "OK\r\n" && args.Length == 3)
//                {
//                    Console.WriteLine(args[1] + " location changed to be " + args[2]);
//                }
//            }
//            else if (args.Length == 0)
//            {
//                Console.WriteLine("No arguments where supplied!");
//            }
//            else
//            {
//                Console.WriteLine("Too many arguments where supplied!");
//            }
//        }
//        private static void HTTP10(string[] args,TcpClient client)
//        {
//            if (args.Length != 0 && args.Length <= 3)
//            {
//                StreamWriter sw = new StreamWriter(client.GetStream());
//                StreamReader sr = new StreamReader(client.GetStream());
//                if (args.Length == 3)
//                {
//                    sw.WriteLine(args[0] + " " + args[1] + " " + args[2]);
//                }
//                else if (args.Length == 4)
//                {
//                    sw.WriteLine(args[0] + " " + args[1] + " " + args[2] + " " + args[3]);
//                }
//                sw.Flush();
//                string result = sr.ReadToEnd();
//                if (args.Length == 3)
//                {
//                    Console.WriteLine(args[1] + " is " + result);
//                }
//                else if (result == "OK\r\n" && args.Length == 4)
//                {
//                    Console.WriteLine(args[1] + " location changed to be " + args[3]);
//                }
//            }
//            else if (args.Length == 0)
//            {
//                Console.WriteLine("No arguments where supplied!");
//            }
//            else
//            {
//                Console.WriteLine("Too many arguments where supplied!");
//            }
//        }
//        private static void HTTP11(string[] args,TcpClient client)
//        {
//            if (args.Length != 0 && args.Length <= 5)
//            {
//                StreamWriter sw = new StreamWriter(client.GetStream());
//                StreamReader sr = new StreamReader(client.GetStream());
//                if (args.Length == 4)
//                {
//                    sw.WriteLine(args[0] + " " + args[1] + " " + args[2]+" "+args[3]);
//                }
//                else if (args.Length == 5)
//                {
//                    sw.WriteLine(args[0] + " " + args[1] + " " + args[2] + " " + args[3]+" "+args[4]);
//                }
//                sw.Flush();
//                string result = sr.ReadToEnd();
//                if (args.Length == 3)
//                {
//                    Console.WriteLine(args[1] + " is " + result);
//                }
//                else if (result == "OK\r\n" && args.Length == 4)
//                {
//                    Console.WriteLine(args[1] + " location changed to be " + args[3]);
//                }
//            }
//            else if (args.Length == 0)
//            {
//                Console.WriteLine("No arguments where supplied!");
//            }
//            else
//            {
//                Console.WriteLine("Too many arguments where supplied!");
//            }
//        }
//    }
//}
