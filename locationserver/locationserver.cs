using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
namespace locationserver
{
    class locationserver
    {
        public void runServer()
        {
            TcpListener listener;
            Socket connection;
            NetworkStream socketStream;
            serverstart threadgen = new serverstart();
            try
            {
                listener = new TcpListener(43);
                listener.Start();
                while (true)
                {
                    connection = listener.AcceptSocket();
                    socketStream = new NetworkStream(connection);
                    threadgen.threadstart();//do request on thread
                    socketStream.Close();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                log.log(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(43);
        }
    }
}
