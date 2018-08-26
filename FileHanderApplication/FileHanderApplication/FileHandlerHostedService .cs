using System;
using System.Threading;
using System.Threading.Tasks;
using FileHandlerService;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FileHanderApplication
{
    public class FileHandlerHostedService : IHostedService, IDisposable
    {
        private FtpServer _ftpServer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ftpServer = new FtpServer();
            _ftpServer.Start();
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