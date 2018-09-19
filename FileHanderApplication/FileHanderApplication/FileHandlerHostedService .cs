using System;
using System.Threading;
using System.Threading.Tasks;
using FileHandlerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FileHanderApplication
{
    public class FileHandlerHostedService : IHostedService, IDisposable
    {
        private FtpServer _ftpServer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("The ftp server is starting");
                _ftpServer = new FtpServer();
                _ftpServer.Start();
                Log.Information("The ftp server has been started");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error(e.Message);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}