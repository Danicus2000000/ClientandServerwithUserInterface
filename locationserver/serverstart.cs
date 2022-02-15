using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace locationserver
{
    class serverstart
    {
        Thread threadInitialise;
        public void threadstart(NetworkStream request)
        {
            //threadInitialise = new Thread(new ThreadStart(run));
            //threadInitialise.Start();
        }

        public void run(NetworkStream request,Dictionary<string,string> storeddata) 
        {
            try
            {
                Byte[] bytes = new Byte[256];
                string data = null;
                StreamReader sr = new StreamReader(request);
                data = sr.ReadLine();
                Console.WriteLine("Server recieved " + data);
                string[] split = data.Split(" ");
                StreamWriter write = new StreamWriter(request);
                if (split.Length == 1)
                {
                    try
                    {
                        write.WriteLine(storeddata[split[0]]);
                        write.Flush();
                        Console.WriteLine("Server Sent " + storeddata[split[0]]);
                    }
                    catch (Exception)
                    {
                        write.WriteLine("ERROR: no entries found\r\n");
                        write.Flush();
                        Console.WriteLine("Server Sent ERROR: no enteries found");
                    }
                }
                else
                {
                    string build = "";
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (i != 0 && i != split.Length - 1)
                        {
                            build += split[i] + " ";
                        }
                        else if (i == split.Length - 1)
                        {
                            build += split[i];
                        }
                    }
                    storeddata[split[0]] = build;
                    write.WriteLine("OK\r\n");
                    write.Flush();
                    Console.WriteLine("Server Sent OK");
                }
            }
            catch(IOException)
            {
                Console.WriteLine("The thread timed out!");
            }
            //threadgen.threadstart(socketStream);
        }
    }
}
