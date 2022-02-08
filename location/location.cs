using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
namespace location
{
    class location
    {
        static void Main(string[] args)
        {
            if (args.Length != 0 && args.Length<=2)
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
                else if (result == "OK\r\n" && args.Length==2)
                {
                    Console.WriteLine(args[0] + " location changed to be " + args[1]);
                }
            }
            else if (args.Length==0)
            {
                Console.WriteLine("No arguments where supplied!");
            }
            else 
            {
                Console.WriteLine("Too many arguments where supplied!");
            }
        }
    }
}
