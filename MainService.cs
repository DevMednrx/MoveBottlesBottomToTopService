using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MoveBottlesBottomToTopService
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        public void RunService()
        {
            WriteToLogFile("Service Started!!!", "");

            MoveBottlesBottomToTopAsync();

            WriteToLogFile("Service Stopped!!!", "");
        }

        private void MoveBottlesBottomToTopAsync()
        {
            WriteToLogFile("Move bottle bottom to top started.", "");

            using (var client = new HttpClient()) // Reuse HttpClient
            {
                client.Timeout = TimeSpan.FromHours(1);
                string inventoryUrl = $"{ConfigurationManager.AppSettings["APIBaseUrl"]}inventory/MoveBottlesBottomToTop";

                try
                {
                    var response = client.PostAsync(inventoryUrl, null).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        WriteToLogFile($"MoveBottlesBottomToTop Call Success", "");
                    }
                    else
                    {
                        WriteToLogFile($"MoveBottlesBottomToTop Call Failed - {response?.ReasonPhrase}", "");
                    }
                }
                catch (Exception ex)
                {
                    WriteToLogFile($"API Call failed. Error: {ex.Message}", "");
                }
            }

            WriteToLogFile("Move bottle bottom to top completed.", "");
        }

        public void WriteToLogFile(string logMessage, string logStackTrace)
        {
            try
            {
                if (!string.IsNullOrEmpty(logMessage))
                {
                    string logDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", DateTime.Now.ToString("MMddyyyy"));
                    if (!Directory.Exists(logDirectoryPath))
                        Directory.CreateDirectory(logDirectoryPath);

                    string logFilePath = Path.Combine(logDirectoryPath, "Log.txt");

                    // Ensure log file is created
                    if (!File.Exists(logFilePath))
                    {
                        using (FileStream objLogFileStream = new FileStream(logFilePath, FileMode.Create)) { }
                    }

                    // Append log
                    using (StreamWriter objLogStreamWriter = new StreamWriter(logFilePath, true))
                    {
                        objLogStreamWriter.WriteLine($"{DateTime.Now} - {logMessage}");
                        if (!string.IsNullOrEmpty(logStackTrace))
                            objLogStreamWriter.WriteLine(logStackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception if needed, but don't throw to avoid breaking execution
                Console.WriteLine($"Logging error: {ex.Message}");
            }
        }
    }    
}
