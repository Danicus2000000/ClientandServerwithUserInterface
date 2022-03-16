using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;

namespace locationserver
{
    class locationserver
    {
        private static string logFileLocation = Directory.GetCurrentDirectory()+"\\logfile.txt";
        private static string serverDatabaseLocation = "";
        private static int timeout = 2000;
        private static Dictionary<string, string> storeddata = new Dictionary<string, string>();
        private static ConcurrentQueue<string> logDataToWrite= new ConcurrentQueue<string>();
        /// <summary>
        /// Parses any arguments given to fill out required data
        /// </summary>
        /// <param name="args">The arguments handed either at the command line or via user input</param>
        static void parseArgs(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)//loops through all arguments
                {
                    switch (args[i])
                    {
                        case "-l":
                            logFileLocation = args[i + 1];
                            break;
                        case "-f":
                            serverDatabaseLocation = args[i + 1];
                            break;
                        case "-t":
                            timeout = Convert.ToInt32(args[i + 1]);
                            break;
                    }
                }
            }
            catch (Exception)//Exceptions can only be caused by invalid argument use so the program outputs an error and exits
            {
                Console.WriteLine("The request was in an invalid format!");
                Console.WriteLine("Will run with no log file and no database instead!");
                logFileLocation = "";
                serverDatabaseLocation = "";
            }
        }
        /// <summary>
        /// Checks to ensure that the log file and save file for interal server details are pointed at an accessable location
        /// </summary>
        static void validateFileLocations()
        {
            try
            {
                if (logFileLocation != "")
                {
                    if (!File.Exists(logFileLocation))//if the file does not exist at location attempt to create it
                    {
                        File.Create(logFileLocation);
                    }
                }
                if (serverDatabaseLocation != "")
                {
                    if (!File.Exists(serverDatabaseLocation))
                    {
                        File.Create(serverDatabaseLocation);
                    }
                }
            }
            catch (Exception)//if the creation fails the path is invalid
            {
                Console.WriteLine("The selected path for the source or log file was invalid or inaccessable");
                Console.WriteLine("Will run with no log file and no database instead!");
                logFileLocation = "";
                serverDatabaseLocation = "";
            }
        }
        public static void runServer()
        {
            validateFileLocations();
            TcpListener listener;
            if (serverDatabaseLocation != "")//reads in any available data to the datastore
            {
                try
                {
                    string[] dataToStore = File.ReadAllLines(serverDatabaseLocation);
                    foreach (string add in dataToStore)
                    {
                        if (add != "")
                        {
                            string[] parts = add.Split(" ");
                            string set = "";
                            for (int i = 1; i < parts.Length; i++)
                            {
                                set += parts[i] + " ";
                            }
                            storeddata[parts[0]] = set.Substring(0, set.Length - 1);
                        }
                    }
                }
                catch (Exception) 
                {
                    Console.WriteLine("Database file was not formated correctly, using null dataset instead");
                    storeddata = new Dictionary<string, string>();
                }
            }
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
                        Thread threadInitialise = new Thread(() => threadgen.run(connection,storeddata,logFileLocation,timeout,logDataToWrite));
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
            AppDomain.CurrentDomain.ProcessExit+=new EventHandler(OnProcessExit);
            parseArgs(args);
            runServer();
        }
        static void OnProcessExit(object sender,EventArgs e) 
        {
            if (serverDatabaseLocation != "" && storeddata.Count!=0) 
            {
                string result = "";
                foreach (KeyValuePair<string, string> set in storeddata)
                {
                    result+=set.Key + " " + set.Value + "\n";
                }
                result=result.Substring(0,result.Length-1);
                File.WriteAllText(serverDatabaseLocation,result);
            }
            if(logFileLocation != "" && logDataToWrite.Count!=0) 
            {
                File.AppendAllLines(logFileLocation, logDataToWrite);
            }
        }
    }
}
