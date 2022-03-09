using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace locationserver
{
    class locationserver
    {
        public static void runServer()
        {

            TcpListener listener;
            Dictionary<string, string> storeddata = new Dictionary<string, string>();
            while (true)
            {
                try
                {
                    listener = new TcpListener(IPAddress.Any,43);
                    listener.Start();
                    while (true)
                    {
                        Socket connection = listener.AcceptSocket();
                        serverstart threadgen = new serverstart();
                        Thread threadInitialise = new Thread(() => threadgen.run(connection,storeddata));
                        threadInitialise.Start();
                    }
                }
                catch
                {
                    Console.WriteLine("An error Occured");
                }
            }
        }
        static void Main(string[] args)
        {
            runServer();
        }
    }
}
