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
            if (args.Length != 0)
            {
                int c;
                TcpClient client = new TcpClient();
                client.Connect("whois.net.dcs.hull.ac.uk", 43);
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                sw.WriteLine(args[0]);
                sw.Flush();
                Console.WriteLine(sr.ReadToEnd());
            }
            else
            {
                Console.WriteLine("No arguments where supplied!");
            }
        }
    }
}
