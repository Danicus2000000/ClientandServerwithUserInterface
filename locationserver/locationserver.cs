using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
using System.Windows.Forms;
namespace locationserver
{
    class locationserver
    {
        private static string logFileLocation = Directory.GetCurrentDirectory()+"\\logfile.txt";//filenames are all logical
        private static string serverDatabaseLocation = "";
        private static int timeout = 1000;
        private static ConcurrentDictionary<string, string> storeddata = new ConcurrentDictionary<string, string>();//stores name,location pairs
        private static ConcurrentQueue<string> logDataToWrite= new ConcurrentQueue<string>();
        private static bool useGUI=false;
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
                        case "-l"://if log file specified overwrite default path
                            logFileLocation = args[i + 1];
                            break;
                        case "-f"://if database file specified store the path
                            serverDatabaseLocation = args[i + 1];
                            break;
                        case "-t"://if timeout specified store it
                            timeout = Convert.ToInt32(args[i + 1]);
                            break;
                        case "-w"://if -w used run GUI
                            useGUI = true;
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
            if (useGUI) 
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new locationserverGUI(logFileLocation,serverDatabaseLocation,timeout));
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
            }
            catch (Exception)//if the creation fails the path is invalid default to null paths
            {
                Console.WriteLine("The selected path for the log file was invalid or inaccessable");
                Console.WriteLine("Will run with no log file instead!");
                logFileLocation = "";
            }
            try
            {
                if (serverDatabaseLocation != "")
                {
                    if (!File.Exists(serverDatabaseLocation))
                    {
                        File.Create(serverDatabaseLocation);
                    }
                }
            }
            catch (Exception)//if the creation fails the path is invalid default to null paths
            {
                Console.WriteLine("The selected path for the database was invalid or inaccessable");
                Console.WriteLine("Will run with no database instead!");
                serverDatabaseLocation = "";
            }
        }
        private static void readDatabase()
        {
            try
            {
                string[] dataToStore;//stores database as set of lines
                using (FileStream fileAppend = File.Open(serverDatabaseLocation, FileMode.Open))//read database file
                {
                    using (StreamReader output = new StreamReader(fileAppend))
                    {
                        dataToStore = output.ReadToEnd().Split("\n");
                    }
                }
                foreach (string add in dataToStore)//writes data to storage concurent dictionary
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
                storeddata = new ConcurrentDictionary<string, string>();
            }
        }

        private static void runServer()
        {
            validateFileLocations();//check file locations
            if (serverDatabaseLocation != "") 
            {
                readDatabase();//attempts to read database file
            }
            if (logFileLocation != "") //if there is a log file start a thread to deal with log file writting
            {
                Thread log = new Thread(() => logStuff());
                log.Start();
            }
            TcpListener listener;
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
        }
        static void logStuff() 
        {
            while (true) 
            {
                if (logDataToWrite.Count != 0) 
                {
                    foreach (string value in logDataToWrite)
                    {
                        try
                        {
                            using (FileStream fileAppend = File.Open(logFileLocation, FileMode.Append))//write to file
                            {
                                using (StreamWriter output = new StreamWriter(fileAppend))
                                {
                                    output.WriteLine(value);
                                }
                            }
                            logDataToWrite.TryDequeue(out string no);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}
