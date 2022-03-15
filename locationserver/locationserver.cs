using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.IO;
namespace locationserver
{
    class locationserver
    {
        private static string logFileLocation = Directory.GetCurrentDirectory()+"\\logfile.txt";
        private static string serverDatabaseLocation = "";

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
            Dictionary<string, string> storeddata = new Dictionary<string, string>();
            if (serverDatabaseLocation != "")//reads in any available data to the datastore
            {
                try
                {
                    string[] dataToStore = File.ReadAllLines(serverDatabaseLocation);
                    foreach (string add in dataToStore)
                    {
                        string[] parts = add.Split("!,");
                        storeddata[parts[0]] = parts[1];
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
                        Thread threadInitialise = new Thread(() => threadgen.run(connection,storeddata,logFileLocation));
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
            parseArgs(args);
            runServer();
        }
    }
}
