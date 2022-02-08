using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
namespace locationserver
{
    class serverstart
    {
        Thread threadInitialise;
        public void threadstart()
        {
            threadInitialise = new Thread(new ThreadStart(run));
            threadInitialise.Start();
        }

        public void run() 
        {

        }
    }
}
