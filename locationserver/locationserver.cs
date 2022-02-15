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
            Socket connection;
            NetworkStream socketStream;
            Dictionary<string, string> storeddata = new Dictionary<string, string>();
            serverstart threadgen = new serverstart();
            while (true)
            {
                try
                {
                    listener = new TcpListener(43);
                    listener.Start();
                    while (true)
                    {
                        connection = listener.AcceptSocket();
                        Console.WriteLine("Conenction Recieved");
                        socketStream = new NetworkStream(connection);
                        socketStream.ReadTimeout = 1000;
                        socketStream.WriteTimeout = 1000;
                        threadgen.run(socketStream, storeddata);
                        socketStream.Close();
                        connection.Close();
                        Console.WriteLine("Connection Disposed");
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
