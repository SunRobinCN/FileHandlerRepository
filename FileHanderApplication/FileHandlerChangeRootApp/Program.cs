using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FileHandlerChangeRootApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileLog.Info("The program is starting", LogType.Info);
                Console.WriteLine("The program is starting");
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                builder.SetBasePath(pathToContentRoot);
                string filePath = configuration.GetValue<string>("filePath");
                string content = File.ReadAllText(pathToContentRoot + "\\" + "appContent.json");
                File.WriteAllText(filePath, content);
                FileLog.Info("The program is finished", LogType.Info);
                Console.WriteLine("The program is finished");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FileLog.Error("Error", e, LogType.Error);
            }
        }
    }
}
