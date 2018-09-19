using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FileHanderApplication
{
    public class Program
    {


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();


        public static void Main(string[] args)
        {
            try
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);

                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                builder.SetBasePath(pathToContentRoot);
                string logPath = configuration.GetValue<string>("logPath");
                Log.Logger = new LoggerConfiguration().WriteTo.RollingFile(logPath + "\\" +"{Date}.log").CreateLogger();

                Log.Information("The service is starting");
                CreateWebHostBuilder(args).Build().RunAsService();
                //BuildWebHost(args).Run();
                Log.Information("The service has been started");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.Message);
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            builder.SetBasePath(pathToContentRoot);
            string url = configuration.GetValue<string>("url");

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Configure the app here.
                })
                .UseUrls(url)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>();
        }
    }
}
